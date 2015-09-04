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

		protected bool Loading
		{
			get { return loading; }
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

		void IProjectItem.Close()
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
			AddClass(newClass);
			return newClass;
		}

		protected virtual void AddClass(ClassType newClass)
		{
			AddEntity(newClass);
		}

		public bool InsertClass(ClassType newClass)
		{
			if (newClass != null && !entities.Contains(newClass) && newClass.Language == language)
			{
				AddClass(newClass);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <exception cref="InvalidOperationException">
		/// The language does not support structures.
		/// </exception>
		public StructureType AddStructure()
		{
			StructureType structure = Language.CreateStructure();
			AddStructure(structure);
			return structure;
		}

		protected virtual void AddStructure(StructureType structure)
		{
			AddEntity(structure);
		}

		public bool InsertStructure(StructureType structure)
		{
			if (structure != null && !entities.Contains(structure) &&
				structure.Language == language)
			{
				AddStructure(structure);
				return true;
			}
			else
			{
				return false;
			}
		}

		public InterfaceType AddInterface()
		{
			InterfaceType newInterface = Language.CreateInterface();
			AddInterface(newInterface);
			return newInterface;
		}

		protected virtual void AddInterface(InterfaceType newInterface)
		{
			AddEntity(newInterface);
		}

		public bool InsertInterface(InterfaceType newInterface)
		{
			if (newInterface != null && !entities.Contains(newInterface) &&
				newInterface.Language == language)
			{
				AddInterface(newInterface);
				return true;
			}
			else
			{
				return false;
			}
		}

		public EnumType AddEnum()
		{
			EnumType newEnum = Language.CreateEnum();
			AddEnum(newEnum);
			return newEnum;
		}

		protected virtual void AddEnum(EnumType newEnum)
		{
			AddEntity(newEnum);
		}

		public bool InsertEnum(EnumType newEnum)
		{
			if (newEnum != null && !entities.Contains(newEnum) &&
				newEnum.Language == language)
			{
				AddEnum(newEnum);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <exception cref="InvalidOperationException">
		/// The language does not support delegates.
		/// </exception>
		public DelegateType AddDelegate()
		{
			DelegateType newDelegate = Language.CreateDelegate();
			AddDelegate(newDelegate);
			return newDelegate;
		}

		protected virtual void AddDelegate(DelegateType newDelegate)
		{
			AddEntity(newDelegate);
		}

		public bool InsertDelegate(DelegateType newDelegate)
		{
			if (newDelegate != null && !entities.Contains(newDelegate) &&
				newDelegate.Language == language)
			{
				AddDelegate(newDelegate);
				return true;
			}
			else
			{
				return false;
			}
		}

		public Comment AddComment()
		{
			Comment comment = new Comment();
			AddComment(comment);
			return comment;
		}

		protected virtual void AddComment(Comment comment)
		{
			AddEntity(comment);
		}

		public bool InsertComment(Comment comment)
		{
			if (comment != null && !entities.Contains(comment))
			{
				AddComment(comment);
				return true;
			}
			else
			{
				return false;
			}
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
			AddAssociation(association);
			return association;
		}

		protected virtual void AddAssociation(AssociationRelationship association)
		{
			AddRelationship(association);
		}

		public bool InsertAssociation(AssociationRelationship associaton)
		{
			if (associaton != null && !relationships.Contains(associaton) &&
				entities.Contains(associaton.First) && entities.Contains(associaton.Second))
			{
				AddAssociation(associaton);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="first"/> or <paramref name="second"/> is null.
		/// </exception>
		public AssociationRelationship AddComposition(TypeBase first, TypeBase second)
		{
			AssociationRelationship composition = new AssociationRelationship(
				first, second, AssociationType.Composition);

			AddAssociation(composition);
			return composition;
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="first"/> or <paramref name="second"/> is null.
		/// </exception>
		public AssociationRelationship AddAggregation(TypeBase first, TypeBase second)
		{
			AssociationRelationship aggregation = new AssociationRelationship(
				first, second, AssociationType.Aggregation);

			AddAssociation(aggregation);
			return aggregation;
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

			AddGeneralization(generalization);
			return generalization;
		}

		protected virtual void AddGeneralization(GeneralizationRelationship generalization)
		{
			AddRelationship(generalization);
		}

		public bool InsertGeneralization(GeneralizationRelationship generalization)
		{
			if (generalization != null && !relationships.Contains(generalization) &&
				entities.Contains(generalization.First) && entities.Contains(generalization.Second))
			{
				AddGeneralization(generalization);
				return true;
			}
			else
			{
				return false;
			}
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

			AddRealization(realization);
			return realization;
		}

		protected virtual void AddRealization(RealizationRelationship realization)
		{
			AddRelationship(realization);
		}

		public bool InsertRealization(RealizationRelationship realization)
		{
			if (realization != null && !relationships.Contains(realization) &&
				entities.Contains(realization.First) && entities.Contains(realization.Second))
			{
				AddRealization(realization);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="first"/> or <paramref name="second"/> is null.
		/// </exception>
		public DependencyRelationship AddDependency(TypeBase first, TypeBase second)
		{
			DependencyRelationship dependency = new DependencyRelationship(first, second);

			AddDependency(dependency);
			return dependency;
		}

		protected virtual void AddDependency(DependencyRelationship dependency)
		{
			AddRelationship(dependency);
		}

		public bool InsertDependency(DependencyRelationship dependency)
		{
			if (dependency != null && !relationships.Contains(dependency) &&
				entities.Contains(dependency.First) && entities.Contains(dependency.Second))
			{
				AddDependency(dependency);
				return true;
			}
			else
			{
				return false;
			}
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

			AddNesting(nesting);
			return nesting;
		}

		protected virtual void AddNesting(NestingRelationship nesting)
		{
			AddRelationship(nesting);
		}

		public bool InsertNesting(NestingRelationship nesting)
		{
			if (nesting != null && !relationships.Contains(nesting) &&
				entities.Contains(nesting.First) && entities.Contains(nesting.Second))
			{
				AddNesting(nesting);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="comment"/> or <paramref name="entity"/> is null.
		/// </exception>
		public virtual CommentRelationship AddCommentRelationship(Comment comment, IEntity entity)
		{
			CommentRelationship commentRelationship = new CommentRelationship(comment, entity);

			AddCommentRelationship(commentRelationship);
			return commentRelationship;
		}

		protected virtual void AddCommentRelationship(CommentRelationship commentRelationship)
		{
			AddRelationship(commentRelationship);
		}

		public bool InsertCommentRelationship(CommentRelationship commentRelationship)
		{
			if (commentRelationship != null && !relationships.Contains(commentRelationship) &&
				entities.Contains(commentRelationship.First) && entities.Contains(commentRelationship.Second))
			{
				AddCommentRelationship(commentRelationship);
				return true;
			}
			else
			{
				return false;
			}
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

		void IProjectItem.Serialize(XmlElement node)
		{
			Serialize(node);
		}

		void IProjectItem.Deserialize(XmlElement node)
		{
			Deserialize(node);
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		private void Serialize(XmlElement node)
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

		/// <exception cref="InvalidDataException">
		/// The save format is corrupt and could not be loaded.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		private void Deserialize(XmlElement node)
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
