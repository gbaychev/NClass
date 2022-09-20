// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// Copyright (C) 2020 Georgi Baychev
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
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Linq;
using NClass.Core.UndoRedo;
using NClass.Translations;

namespace NClass.Core
{
    public abstract class Model : IModifiable
    {
        protected List<IEntity> entities = new List<IEntity>();
        protected List<Relationship> relationships = new List<Relationship>();
        protected bool isDirty = false;
        protected bool loading = false;
        protected int dontRaiseRequestCount = 0;


        public event ModifiedEventHandler Modified;
        public event EntityEventHandler EntityAdded;
        public event EntityEventHandler EntityRemoved;
        public event EntityEventHandler EntityNested;
        public event RelationshipEventHandler RelationAdded;
        public event RelationshipEventHandler RelationRemoved;
        public event SerializeEventHandler Serializing;
        public event SerializeEventHandler Deserializing;

        public Project Project { get; set; }
        private string name;
        public string Name
        {
            get => name ?? Strings.Untitled;
            set => name = value;
        }

        public bool RaiseChangedEvent
        {
            get => (dontRaiseRequestCount == 0);
            set
            {
                if (!value)
                    dontRaiseRequestCount++;
                else if (dontRaiseRequestCount > 0)
                    dontRaiseRequestCount--;
            }
        }

        public bool IsDirty => isDirty;

        public bool IsEmpty => (entities.Count == 0 && relationships.Count == 0);

        void IModifiable.Clean()
        {
            isDirty = false;
            //TODO: tagokat is tisztítani!
        }

        public IEnumerable<IEntity> Entities => entities;

        public IEnumerable<Relationship> Relationships => relationships;

        protected void ElementChanged(object sender, EventArgs e)
        {
            // TODO
            OnModified((ModificationEventArgs)e);
        }

        protected void AddEntity(IEntity entity, bool notify = true)
        {
            entities.Add(entity);
            entity.Modified += ElementChanged;
            if(notify)
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

        protected void AddRelationship(Relationship relationship, bool notify = true)
        {
            relationships.Add(relationship);
            relationship.Modified += ElementChanged;
            if(notify)
                OnRelationAdded(new RelationshipEventArgs(relationship));
        }

        public virtual void Serialize(XmlElement node)
        {
            SaveEntities(node);
            SaveEntityContainers(node);
            SaveRelationships(node);

            OnSerializing(new SerializeEventArgs(node));
        }

        public void Deserialize(XmlElement node)
        {
            if (node == null)
                throw new ArgumentNullException("root");
            loading = true;

            LoadEntitites(node);
            LoadEntityContainers(node);
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

            var nodeList = root.SelectNodes("Entities/Entity");

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

        protected void LoadEntityContainers(XmlNode root)
        {
            if (root == null)
                throw new ArgumentNullException("root");

            var nodeList = root.SelectNodes("Containers/Container");

            foreach (XmlElement node in nodeList)
            {
                int.TryParse(node.Attributes["entityIndex"].InnerText, out var containerIndex);
                var container = entities[containerIndex] as INestable;
                foreach (XmlElement childNode in node.ChildNodes)
                {
                    if (int.TryParse(childNode.InnerText, out var childIndex) && childIndex >= 0) // to un-corrupt old "corrupted" files
                    {
                        var childEntity = entities[childIndex] as INestableChild;
                        container.AddNestedChild(childEntity);
                        EntityNested?.Invoke(container, new EntityEventArgs(childEntity));
                    }
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
        protected abstract void LoadRelationships(XmlNode root);

        /// <exception cref="ArgumentNullException">
        /// <paramref name="node"/> is null.
        /// </exception>
        private void SaveEntities(XmlElement node)
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

        protected void SaveEntityContainers(XmlElement node)
        {
            if (node == null)
                throw new ArgumentNullException("root");

            XmlElement containersNode = node.OwnerDocument.CreateElement("Containers");

            foreach (IEntity entity in entities.Where(e => e is INestable con && con.NestedChilds.Any()))
            {
                XmlElement child = node.OwnerDocument.CreateElement("Container");

                int index = entities.IndexOf(entity);
                child.SetAttribute("entityIndex", index.ToString());

                foreach (var childEntity in ((INestable)entity).NestedChilds)
                {
                    int childEntityIndexNum = entities.IndexOf(childEntity);

                    if (childEntityIndexNum >= 0) // made to rule out any cases where the child entity has an index of -1
                    {
                        var childEntityNode = node.OwnerDocument.CreateElement("ChildEntity");
                        childEntityNode.InnerText = childEntityIndexNum.ToString();
                        child.AppendChild(childEntityNode);
                    }
                }

                containersNode.AppendChild(child);
            }

            node.AppendChild(containersNode);
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
            EntityAdded?.Invoke(this, e);
            OnModified(ModificationEventArgs.Empty);
        }

        protected virtual void OnEntityRemoved(EntityEventArgs e)
        {
            EntityRemoved?.Invoke(this, e);
            OnModified(ModificationEventArgs.Empty);
        }

        protected virtual void OnRelationAdded(RelationshipEventArgs e)
        {
            RelationAdded?.Invoke(this, e);
            OnModified(ModificationEventArgs.Empty);
        }

        protected virtual void OnRelationRemoved(RelationshipEventArgs e)
        {
            RelationRemoved?.Invoke(this, e);
            OnModified(ModificationEventArgs.Empty);
        }

        protected virtual void OnSerializing(SerializeEventArgs e)
        {
            Serializing?.Invoke(this, e);
        }

        protected virtual void OnDeserializing(SerializeEventArgs e)
        {
            Deserializing?.Invoke(this, e);
            OnModified(ModificationEventArgs.Empty);
        }

        protected virtual void OnModified(ModificationEventArgs e)
        {
            isDirty = true;
            Modified?.Invoke(this, e);
        }

        public bool InsertRelationship(Relationship relationship)
        {
            if (relationship != null && !Relationships.Contains(relationship) &&
                Entities.Contains(relationship.First) && Entities.Contains(relationship.Second))
            {
                AddRelationship(relationship);
                return true;
            }

            return false;
        }

        public bool InsertComment(Comment comment)
        {
            if (comment != null && !Entities.Contains(comment))
            {
                AddEntity(comment);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reinsert an entity to the model, without notifying the diagram.
        /// Used by undo/redo when undoing the deletion of elements
        /// </summary>
        public void ReinsertEntity(IEntity entity)
        {
            Debug.Assert(entity != null);
            Debug.Assert(!Entities.Contains(entity));

            AddEntity(entity, false);
        }

        /// <summary>
        /// Reinsert a relationship to the model, without notifying the diagram.
        /// Used by undo/redo when undoing the deletion of elements
        /// </summary>
        public void ReinsertRelationship(Relationship relationship)
        {
            Debug.Assert(relationship != null);
            Debug.Assert(!Relationships.Contains(relationship));
            Debug.Assert(Entities.Contains(relationship.First));
            Debug.Assert(Entities.Contains(relationship.Second));

            AddRelationship(relationship, false);
        }
    }
}
