// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// 
// This program is free software; you can redistribute it and/or modify it under 
// the terms of the GNU General Public License as published by the Free Software 
// Foundation; either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with 
// this program; if not, write to the Free Software Foundation, Inc., 
// 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using NClass.Translations;

namespace NClass.Core
{
	public sealed class Project : IModifiable
	{
		string name;
		FileInfo projectFile = null;
		List<IProjectItem> items = new List<IProjectItem>();
		bool isDirty = false;
		bool isUntitled = true;
		bool isReadOnly = false;
		bool loading = false;

		public event EventHandler Modified;
		public event EventHandler Renamed;
		public event EventHandler FileStateChanged;
		public event ProjectItemEventHandler ItemAdded;
		public event ProjectItemEventHandler ItemRemoved;

		public Project()
		{
			name = Strings.Untitled;
		}

		/// <exception cref="ArgumentException">
		/// <paramref name="name"/> cannot be empty string.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="name"/> is null.
		/// </exception>
		public Project(string name)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (name.Length == 0)
				throw new ArgumentException("Name cannot empty string.");

			this.name = name;
		}

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (name != value && value != null && value.Length > 0)
				{
					name = value;
					isUntitled = false;
					OnRenamed(EventArgs.Empty);
					OnModified(EventArgs.Empty);
				}
			}
		}

		public bool IsUntitled
		{
			get { return isUntitled; }
		}

		public bool IsReadOnly
		{
			get { return isReadOnly; }
		}

		public string FilePath
		{
			get
			{
				if (projectFile != null)
					return projectFile.FullName;
				else
					return null;
			}
			private set
			{
				if (value != null)
				{
					try
					{
						FileInfo file = new FileInfo(value);

						if (projectFile == null || projectFile.FullName != file.FullName)
						{
							projectFile = file;
							OnFileStateChanged(EventArgs.Empty);
						}
					}
					catch
					{
						if (projectFile != null)
						{
							projectFile = null;
							OnFileStateChanged(EventArgs.Empty);
						}
					}
				}
				else if (projectFile != null) // value == null
				{
					projectFile = null;
					OnFileStateChanged(EventArgs.Empty);
				}
			}
		}

		public string FileName
		{
			get
			{
				if (projectFile != null)
					return projectFile.Name;
				else
					return Name + ".ncp";
			}
		}

		public bool IsDirty
		{
			get { return isDirty; }
		}

		public IEnumerable<IProjectItem> Items
		{
			get { return items; }
		}

		public int ItemCount
		{
			get { return items.Count; }
		}

		public bool IsEmpty
		{
			get { return ItemCount == 0; }
		}

		public void Clean()
		{
			foreach (IProjectItem item in Items)
			{
				item.Clean();
			}
			if (isDirty)
			{
				isDirty = false;
				OnFileStateChanged(EventArgs.Empty);
			}
		}

		public void CloseItems()
		{
			foreach (IProjectItem item in Items)
			{
				item.Close();
			}
		}

		/// <exception cref="ArgumentException">
		/// <paramref name="item"/> has been already added to the project.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="item"/> is null.
		/// </exception>
		public void Add(IProjectItem item)
		{
			if (item == null)
				throw new ArgumentNullException("item");
			if (items.Contains(item))
				throw new ArgumentException("The project already contains this item.");

			item.Project = this;
			item.Modified += new EventHandler(item_Modified);
			items.Add(item);

			OnItemAdded(new ProjectItemEventArgs(item));
			OnModified(EventArgs.Empty);
		}

		public void Remove(IProjectItem item)
		{
			if (items.Remove(item))
			{
				item.Close();
				item.Modified -= new EventHandler(item_Modified);
				OnItemRemoved(new ProjectItemEventArgs(item));
				OnModified(EventArgs.Empty);
			}
		}

		private void item_Modified(object sender, EventArgs e)
		{
			isDirty = true;
			OnModified(EventArgs.Empty);
		}

		public string GetProjectDirectory()
		{
			if (projectFile != null)
				return projectFile.DirectoryName;
			else
				return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		}

		/// <exception cref="IOException">
		/// Could not load the project.
		/// </exception>
		/// <exception cref="InvalidDataException">
		/// The save file is corrupt and could not be loaded.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="fileName"/> is empty string.
		/// </exception>
		public static Project Load(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
				throw new ArgumentException(Strings.ErrorBlankFilename, "fileName");

			if (!File.Exists(fileName))
				throw new FileNotFoundException(Strings.ErrorFileNotFound);

			XmlDocument document = new XmlDocument();
			try
			{
				document.Load(fileName);
			}
			catch (Exception ex)
			{
				throw new IOException(Strings.ErrorCouldNotLoadFile, ex);
			}

			XmlElement root = document["Project"];
			if (root == null)
			{
				root = document["ClassProject"]; // Old file format
				if (root == null)
				{
					throw new InvalidDataException(Strings.ErrorCorruptSaveFile);
				}
				else
				{
					Project oldProject = LoadWithPreviousFormat(root);
					oldProject.FilePath = fileName;
					oldProject.name = Path.GetFileNameWithoutExtension(fileName);
					oldProject.isUntitled = false;
					return oldProject;
				}
			}

			Project project = new Project();
			project.loading = true;
			try
			{
				project.Deserialize(root);
			}
			catch (Exception ex)
			{
				throw new InvalidDataException(Strings.ErrorCorruptSaveFile, ex);
			}
			project.loading = false;
			project.FilePath = fileName;
			project.isReadOnly = project.projectFile.IsReadOnly;

			return project;
		}

		private static Project LoadWithPreviousFormat(XmlElement root)
		{
			Project project = new Project();
			project.loading = true;

			Assembly assembly = Assembly.Load("NClass.DiagramEditor");
			IProjectItem projectItem = (IProjectItem) assembly.CreateInstance(
				"NClass.DiagramEditor.ClassDiagram.Diagram", false,
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
				null, null, null, null);

			try
			{
				projectItem.Deserialize(root);
			}
			catch (Exception ex)
			{
				throw new InvalidDataException(Strings.ErrorCorruptSaveFile, ex);
			}
			project.Add(projectItem);
			project.loading = false;
			project.isReadOnly = true;
			return project;
		}

		/// <exception cref="IOException">
		/// Could not save the project.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// The project was not saved before by the <see cref="Save(string)"/> method.
		/// </exception>
		public void Save()
		{
			if (projectFile == null)
				throw new InvalidOperationException(Strings.ErrorCannotSaveFile);

			Save(FilePath);
		}

		/// <exception cref="IOException">
		/// Could not save the project.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="fileName"/> is null or empty string.
		/// </exception>
		public void Save(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
				throw new ArgumentException(Strings.ErrorBlankFilename, "fileName");

			XmlDocument document = new XmlDocument();
			XmlElement root = document.CreateElement("Project");
			document.AppendChild(root);

			Serialize(root);
			try
			{
				document.Save(fileName);
			}
			catch (Exception ex)
			{
				throw new IOException(Strings.ErrorCouldNotSaveFile, ex);
			}

			isReadOnly = false;
			FilePath = fileName;
			Clean();
		}

		private void Serialize(XmlElement node)
		{
			XmlElement nameElement = node.OwnerDocument.CreateElement("Name");
			nameElement.InnerText = this.Name;
			node.AppendChild(nameElement);

			foreach (IProjectItem item in Items)
			{
				XmlElement itemElement = node.OwnerDocument.CreateElement("ProjectItem");
				item.Serialize(itemElement);

				Type type = item.GetType();
				XmlAttribute typeAttribute = node.OwnerDocument.CreateAttribute("type");
				typeAttribute.InnerText = type.FullName;
				itemElement.Attributes.Append(typeAttribute);

				XmlAttribute assemblyAttribute = node.OwnerDocument.CreateAttribute("assembly");
				assemblyAttribute.InnerText = type.Assembly.FullName;
				itemElement.Attributes.Append(assemblyAttribute);

				node.AppendChild(itemElement);
			}
		}

		/// <exception cref="InvalidDataException">
		/// The save format is corrupt and could not be loaded.
		/// </exception>
		private void Deserialize(XmlElement node)
		{
			isUntitled = false;

			XmlElement nameElement = node["Name"];
			if (nameElement == null || nameElement.InnerText == "")
				throw new InvalidDataException("Project's name cannot be empty.");
			name = nameElement.InnerText;

			foreach (XmlElement itemElement in node.GetElementsByTagName("ProjectItem"))
			{
				XmlAttribute typeAttribute = itemElement.Attributes["type"];
				XmlAttribute assemblyAttribute = itemElement.Attributes["assembly"];

				if (typeAttribute == null || assemblyAttribute == null)
					throw new InvalidDataException("ProjectItem's type or assembly name is missing.");

				string typeName = typeAttribute.InnerText;
				string assemblyName = assemblyAttribute.InnerText;

				try
				{
					Assembly assembly = Assembly.Load(assemblyName);
					IProjectItem projectItem = (IProjectItem) assembly.CreateInstance(
						typeName, false,
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
						null, null, null, null);

					projectItem.Deserialize(itemElement);
					projectItem.Clean();
					Add(projectItem);
				}
				catch (InvalidDataException)
				{
					throw;
				}
				catch (Exception ex)
				{
					throw new InvalidDataException("Invalid type or assembly of ProjectItem.", ex);
				}
			}
		}

		private void OnModified(EventArgs e)
		{
			if (!loading)
			{
				isDirty = true;
				if (Modified != null)
					Modified(this, e);
			}
		}

		private void OnRenamed(EventArgs e)
		{
			if (Renamed != null)
				Renamed(this, e);
		}

		private void OnItemAdded(ProjectItemEventArgs e)
		{
			if (ItemAdded != null)
				ItemAdded(this, e);
		}

		private void OnItemRemoved(ProjectItemEventArgs e)
		{
			if (ItemRemoved != null)
				ItemRemoved(this, e);
		}

		private void OnFileStateChanged(EventArgs e)
		{
			if (FileStateChanged != null)
				FileStateChanged(this, e);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if (this.GetType() != obj.GetType())
				return false;

			Project project = (Project) obj;

			if (this.projectFile == null && project.projectFile == null)
				return object.ReferenceEquals(this, obj);

			return (
				this.projectFile != null && project.projectFile != null &&
				this.projectFile.FullName == project.projectFile.FullName
			);
		}

		public override int GetHashCode()
		{
			if (projectFile != null)
				return projectFile.GetHashCode();
			else
				return Name.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("{0} [{1}]", Name, FilePath);
		}
	}
}