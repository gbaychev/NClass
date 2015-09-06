using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using NClass.Translations;

namespace NClass.Core
{
	//TODO: átdolgozni
	public class Model : IProjectItem
	{
		string name;
		Language language;
		List<IEntity> entities = new List<IEntity>();
		List<Relationship> relationships = new List<Relationship>();
		Project project = null;
		bool isDirty = false;
		bool loading = false;

		public event EventHandler Modified;
		public event EventHandler Renamed;
		public event EventHandler Closing;
		public event EntityEventHandler EntityAdded;
		public event EntityEventHandler EntityRemoved;
		public event RelationshipEventHandler RelationAdded;
		public event RelationshipEventHandler RelationRemoved;
		public event SerializeEventHandler Serializing;
		public event SerializeEventHandler Deserializing;

		protected Model()
		{
			name = Strings.Untitled;
			language = null;
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="language"/> is null.
		/// </exception>
		public Model(Language language) : this(null, language)
		{
		}

        /// <exception cref="ArgumentException">
		/// <paramref name="name"/> cannot be empty string.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="language"/> is null.
		/// </exception>
		public Model(string name, Language language)
		{
			if (language == null)
				throw new ArgumentNullException("language");
			if (name != null && name.Length == 0)
				throw new ArgumentException("Name cannot empty string.");

			this.name = name;
			this.language = language;
		}

		public string Name
		{
			get
			{
				if (name == null)
					return Strings.Untitled;
				else
					return name;
			}
			set
			{
				if (name != value && value != null)
				{
					name = value;
					OnRenamed(EventArgs.Empty);
					OnModified(EventArgs.Empty);
				}
			}
		}

		public Language Language
		{
			get { return language; }
		}

		public Project Project
		{
			get { return project; }
			set { project = value; }
		}

		public bool IsUntitled
		{
			get
			{
				return (name == null);
			}
		}

		public bool IsDirty
		{
			get { return isDirty; }
		}
        
		public bool IsEmpty
		{
			get
			{
				return (entities.Count == 0 && relationships.Count == 0);
			}
		}

		void IModifiable.Clean()
		{
			isDirty = false;
			//TODO: tagokat is tisztítani!
		}

		public void Close()
		{
			OnClosing(EventArgs.Empty);
		}

		public IEnumerable<IEntity> Entities
		{
			get { return entities; }
		}

		public IEnumerable<Relationship> Relationships
		{
			get { return relationships; }
		}

		private void ElementChanged(object sender, EventArgs e)
		{
			OnModified(e);
		}

		private void AddEntity(IEntity entity)
		{
			entities.Add(entity);
			entity.Modified += new EventHandler(ElementChanged);
			OnEntityAdded(new EntityEventArgs(entity));
		}

		public ClassType AddClass()
		{
			ClassType newClass = Language.CreateClass();
			AddEntity(newClass);
			return newClass;
		}

	    /// <exception cref="InvalidOperationException">
		/// The language does not support structures.
		/// </exception>
		public StructureType AddStructure()
		{
			StructureType structure = Language.CreateStructure();
			AddEntity(structure);
			return structure;
		}

		public InterfaceType AddInterface()
		{
			InterfaceType newInterface = Language.CreateInterface();
			AddEntity(newInterface);
			return newInterface;
		}
		
		public EnumType AddEnum()
		{
			EnumType newEnum = Language.CreateEnum();
			AddEntity(newEnum);
			return newEnum;
		}


		/// <exception cref="InvalidOperationException">
		/// The language does not support delegates.
		/// </exception>
		public DelegateType AddDelegate()
		{
			DelegateType newDelegate = Language.CreateDelegate();
			AddEntity(newDelegate);
			return newDelegate;
		}


		public Comment AddComment()
		{
			Comment comment = new Comment();
            AddEntity(comment);
            return comment;
		}

		private void AddRelationship(Relationship relationship)
		{
			relationships.Add(relationship);
			relationship.Modified += new EventHandler(ElementChanged);
			OnRelationAdded(new RelationshipEventArgs(relationship));
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="first"/> or <paramref name="second"/> is null.
		/// </exception>
		public AssociationRelationship AddAssociation(TypeBase first, TypeBase second)
		{
			AssociationRelationship association = new AssociationRelationship(first, second);
            AddRelationship(association);
            return association;
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="first"/> or <paramref name="second"/> is null.
		/// </exception>
		public AssociationRelationship AddComposition(TypeBase first, TypeBase second)
		{
			AssociationRelationship composition = new AssociationRelationship(
				first, second, AssociationType.Composition);

            AddRelationship(composition);
            return composition;
		}

		/// <exception cref="RelationshipException">
		/// Cannot create relationship between the two types.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="derivedType"/> or <paramref name="baseType"/> is null.
		/// </exception>
		public GeneralizationRelationship AddGeneralization(CompositeType derivedType,
			CompositeType baseType)
		{
			GeneralizationRelationship generalization =
				new GeneralizationRelationship(derivedType, baseType);

            AddRelationship(generalization);
            return generalization;
		}

		/// <exception cref="RelationshipException">
		/// Cannot create relationship between the two types.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="implementer"/> or <paramref name="baseType"/> is null.
		/// </exception>
		public RealizationRelationship AddRealization(TypeBase implementer,
			InterfaceType baseType)
		{
			RealizationRelationship realization = new RealizationRelationship(
				implementer, baseType);

            AddRelationship(realization);
            return realization;
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="first"/> or <paramref name="second"/> is null.
		/// </exception>
		public DependencyRelationship AddDependency(TypeBase first, TypeBase second)
		{
			DependencyRelationship dependency = new DependencyRelationship(first, second);

            AddRelationship(dependency);
            return dependency;
		}

	    /// <exception cref="RelationshipException">
		/// Cannot create relationship between the two types.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="parentType"/> or <paramref name="innerType"/> is null.
		/// </exception>
		public NestingRelationship AddNesting(CompositeType parentType, TypeBase innerType)
		{
			NestingRelationship nesting = new NestingRelationship(parentType, innerType);

			AddRelationship(nesting);
			return nesting;
		}
        
		/// <exception cref="ArgumentNullException">
		/// <paramref name="comment"/> or <paramref name="entity"/> is null.
		/// </exception>
		public CommentRelationship AddCommentRelationship(Comment comment, IEntity entity)
		{
			CommentRelationship commentRelationship = new CommentRelationship(comment, entity);

			AddRelationship(commentRelationship);
			return commentRelationship;
		}

	    public void RemoveEntity(IEntity entity)
		{
			if (entities.Remove(entity))
			{
				entity.Modified -= new EventHandler(ElementChanged);
				RemoveRelationships(entity);
				OnEntityRemoved(new EntityEventArgs(entity));
			}
		}

		private void RemoveRelationships(IEntity entity)
		{
			for (int i = 0; i < relationships.Count; i++)
			{
				Relationship relationship = relationships[i];
				if (relationship.First == entity || relationship.Second == entity)
				{
					relationship.Detach();
					relationship.Modified -= new EventHandler(ElementChanged);
					relationships.RemoveAt(i--);
					OnRelationRemoved(new RelationshipEventArgs(relationship));
				}
			}
		}

		public void RemoveRelationship(Relationship relationship)
		{
			if (relationships.Contains(relationship))
			{
				relationship.Detach();
				relationship.Modified -= new EventHandler(ElementChanged);
				relationships.Remove(relationship);
				OnRelationRemoved(new RelationshipEventArgs(relationship));
			}
		}

		public void Serialize(XmlElement node)
		{
            if (node == null)
                throw new ArgumentNullException("root");

            XmlElement nameElement = node.OwnerDocument.CreateElement("Name");
            nameElement.InnerText = Name;
            node.AppendChild(nameElement);

            XmlElement languageElement = node.OwnerDocument.CreateElement("Language");
            languageElement.InnerText = Language.AssemblyName;
            node.AppendChild(languageElement);

            SaveEntitites(node);
            SaveRelationships(node);

            OnSerializing(new SerializeEventArgs(node));
        }

		public void Deserialize(XmlElement node)
		{
            if (node == null)
                throw new ArgumentNullException("root");
            loading = true;

            XmlElement nameElement = node["Name"];
            if (nameElement == null || nameElement.InnerText == "")
                name = null;
            else
                name = nameElement.InnerText;

            XmlElement languageElement = node["Language"];
            try
            {
                Language language = Language.GetLanguage(languageElement.InnerText);
                if (language == null)
                    throw new InvalidDataException("Invalid project language.");

                this.language = language;
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("Invalid project language.", ex);
            }

            LoadEntitites(node);
            LoadRelationships(node);

            OnDeserializing(new SerializeEventArgs(node));
            loading = false;
        }

		/// <exception cref="InvalidDataException">
		/// The save format is corrupt and could not be loaded.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="root"/> is null.
		/// </exception>
		private void LoadEntitites(XmlNode root)
		{
			if (root == null)
				throw new ArgumentNullException("root");

			XmlNodeList nodeList = root.SelectNodes("Entities/Entity");

			foreach (XmlElement node in nodeList)
			{
				try
				{
					string type = node.GetAttribute("type");

					IEntity entity = GetEntity(type);
					entity.Deserialize(node);
				}
				catch (BadSyntaxException ex)
				{
					throw new InvalidDataException("Invalid entity.", ex);
				}
			}
		}

		private IEntity GetEntity(string type)
		{
			switch (type)
			{
				case "Class":
				case "CSharpClass":     // Old file format
				case "JavaClass":       // Old file format
					return AddClass();

				case "Structure":
				case "StructType":      // Old file format
					return AddStructure();

				case "Interface":
				case "CSharpInterface": // Old file format
				case "JavaInterface":   // Old file format
					return AddInterface();

				case "Enum":
				case "CSharpEnum":      // Old file format
				case "JavaEnum":        // Old file format
					return AddEnum();

				case "Delegate":
				case "DelegateType":    // Old file format
					return AddDelegate();

				case "Comment":
					return AddComment();

				default:
					throw new InvalidDataException("Invalid entity type: " + type);
			}
		}

		/// <exception cref="InvalidDataException">
		/// The save format is corrupt and could not be loaded.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="root"/> is null.
		/// </exception>
		private void LoadRelationships(XmlNode root)
		{
			if (root == null)
				throw new ArgumentNullException("root");

			XmlNodeList nodeList = root.SelectNodes(
				"Relationships/Relationship|Relations/Relation"); // old file format

			foreach (XmlElement node in nodeList)
			{
				string type = node.GetAttribute("type");
				string firstString = node.GetAttribute("first");
				string secondString = node.GetAttribute("second");
				int firstIndex, secondIndex;

				if (!int.TryParse(firstString, out firstIndex) ||
					!int.TryParse(secondString, out secondIndex))
				{
					throw new InvalidDataException(Strings.ErrorCorruptSaveFormat);
				}
				if (firstIndex < 0 || firstIndex >= entities.Count ||
					secondIndex < 0 || secondIndex >= entities.Count)
				{
					throw new InvalidDataException(Strings.ErrorCorruptSaveFormat);
				}

				try
				{
					IEntity first = entities[firstIndex];
					IEntity second = entities[secondIndex];
					Relationship relationship;

					switch (type)
					{
						case "Association":
							relationship = AddAssociation(first as TypeBase, second as TypeBase);
							break;

						case "Generalization":
							relationship = AddGeneralization(
								first as CompositeType, second as CompositeType);
							break;

						case "Realization":
							relationship = AddRealization(first as TypeBase, second as InterfaceType);
							break;

						case "Dependency":
							relationship = AddDependency(first as TypeBase, second as TypeBase);
							break;

						case "Nesting":
							relationship = AddNesting(first as CompositeType, second as TypeBase);
							break;

						case "Comment":
						case "CommentRelationship": // Old file format
							if (first is Comment)
								relationship = AddCommentRelationship(first as Comment, second);
							else
								relationship = AddCommentRelationship(second as Comment, first);
							break;

						default:
							throw new InvalidDataException(
								Strings.ErrorCorruptSaveFormat);
					}
					relationship.Deserialize(node);
				}
				catch (ArgumentNullException ex)
				{
					throw new InvalidDataException("Invalid relationship.", ex);
				}
				catch (RelationshipException ex)
				{
					throw new InvalidDataException("Invalid relationship.", ex);
				}
			}
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		private void SaveEntitites(XmlElement node)
		{
			if (node == null)
				throw new ArgumentNullException("root");

			XmlElement entitiesChild = node.OwnerDocument.CreateElement("Entities");

			foreach (IEntity entity in entities)
			{
				XmlElement child = node.OwnerDocument.CreateElement("Entity");

				entity.Serialize(child);
				child.SetAttribute("type", entity.EntityType.ToString());
				entitiesChild.AppendChild(child);
			}
			node.AppendChild(entitiesChild);
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="root"/> is null.
		/// </exception>
		private void SaveRelationships(XmlNode root)
		{
			if (root == null)
				throw new ArgumentNullException("root");

			XmlElement relationsChild = root.OwnerDocument.CreateElement("Relationships");

			foreach (Relationship relationship in relationships)
			{
				XmlElement child = root.OwnerDocument.CreateElement("Relationship");

				int firstIndex = entities.IndexOf(relationship.First);
				int secondIndex = entities.IndexOf(relationship.Second);

				relationship.Serialize(child);
				child.SetAttribute("type", relationship.RelationshipType.ToString());
				child.SetAttribute("first", firstIndex.ToString());
				child.SetAttribute("second", secondIndex.ToString());
				relationsChild.AppendChild(child);
			}
			root.AppendChild(relationsChild);
		}

		protected virtual void OnEntityAdded(EntityEventArgs e)
		{
			if (EntityAdded != null)
				EntityAdded(this, e);
			OnModified(EventArgs.Empty);
		}

		protected virtual void OnEntityRemoved(EntityEventArgs e)
		{
			if (EntityRemoved != null)
				EntityRemoved(this, e);
			OnModified(EventArgs.Empty);
		}

		protected virtual void OnRelationAdded(RelationshipEventArgs e)
		{
			if (RelationAdded != null)
				RelationAdded(this, e);
			OnModified(EventArgs.Empty);
		}

		protected virtual void OnRelationRemoved(RelationshipEventArgs e)
		{
			if (RelationRemoved != null)
				RelationRemoved(this, e);
			OnModified(EventArgs.Empty);
		}

		protected virtual void OnSerializing(SerializeEventArgs e)
		{
			if (Serializing != null)
				Serializing(this, e);
		}

		protected virtual void OnDeserializing(SerializeEventArgs e)
		{
			if (Deserializing != null)
				Deserializing(this, e);
			OnModified(EventArgs.Empty);
		}

		protected virtual void OnModified(EventArgs e)
		{
			isDirty = true;
			if (Modified != null)
				Modified(this, e);
		}

		protected virtual void OnRenamed(EventArgs e)
		{
			if (Renamed != null)
				Renamed(this, e);
		}

		protected virtual void OnClosing(EventArgs e)
		{
			if (Closing != null)
				Closing(this, e);
		}

		public override string ToString()
		{
			if (IsDirty)
				return Name + "*";
			else
				return Name;
		}
	}
}
