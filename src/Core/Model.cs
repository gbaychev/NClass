// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// Copyright (C) 2016 Georgi Baychev
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
using System.Collections.Generic;
using System.Xml;
using System.IO;
using NClass.Translations;

namespace NClass.Core
{
	public abstract class Model : IModifiable
	{
		protected List<IEntity> entities = new List<IEntity>();
		protected List<Relationship> relationships = new List<Relationship>();
		protected bool isDirty = false;
		protected bool loading = false;

		public event EventHandler Modified;
		public event EntityEventHandler EntityAdded;
		public event EntityEventHandler EntityRemoved;
		public event RelationshipEventHandler RelationAdded;
		public event RelationshipEventHandler RelationRemoved;
		public event SerializeEventHandler Serializing;
		public event SerializeEventHandler Deserializing;

	    public Project Project { get; set; }
	    public string Name { get; set; }

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

		public IEnumerable<IEntity> Entities
		{
			get { return entities; }
		}

		public IEnumerable<Relationship> Relationships
		{
			get { return relationships; }
		}
		protected void ElementChanged(object sender, EventArgs e)
		{
			OnModified(e);
		}

		protected void AddEntity(IEntity entity)
		{
			entities.Add(entity);
			entity.Modified += ElementChanged;
			OnEntityAdded(new EntityEventArgs(entity));
		}

	    public void RemoveEntity(IEntity entity)
		{
			if (entities.Remove(entity))
			{
				entity.Modified -= ElementChanged;
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
					relationship.Modified -= ElementChanged;
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
				relationship.Modified -= ElementChanged;
				relationships.Remove(relationship);
				OnRelationRemoved(new RelationshipEventArgs(relationship));
			}
		}

        public IEntity AddComment()
        {
            Comment comment = new Comment();
            AddEntity(comment);
            return comment;
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

        protected void AddRelationship(Relationship relationship)
        {
            relationships.Add(relationship);
            relationship.Modified += ElementChanged;
            OnRelationAdded(new RelationshipEventArgs(relationship));
        }

        public virtual void Serialize(XmlElement node)
		{
            SaveEntitites(node);
            SaveRelationships(node);

            OnSerializing(new SerializeEventArgs(node));
        }

		public void Deserialize(XmlElement node)
		{
            if (node == null)
                throw new ArgumentNullException("root");
            loading = true;

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
		protected void LoadEntitites(XmlNode root)
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

	    protected abstract IEntity GetEntity(string type);

	    /// <exception cref="InvalidDataException">
	    /// The save format is corrupt and could not be loaded.
	    /// </exception>
	    /// <exception cref="ArgumentNullException">
	    /// <paramref name="root"/> is null.
	    /// </exception>
	    protected  abstract void LoadRelationships(XmlNode root);
		
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
	}
}
