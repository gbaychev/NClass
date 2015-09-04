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
using System.Xml;

namespace NClass.Core
{
	public abstract class Relationship : Element, ISerializableElement
	{
		string label = string.Empty;
		bool attached = false;

		public event EventHandler Attaching;
		public event EventHandler Detaching;
		public event SerializeEventHandler Serializing;
		public event SerializeEventHandler Deserializing;

		public abstract IEntity First
		{
			get;
			protected set;
		}

		public abstract IEntity Second
		{
			get;
			protected set;
		}

		public abstract RelationshipType RelationshipType
		{
			get;
		}

		public virtual string Label
		{
			get
			{
				return label;
			}
			set
			{
				if (value == "")
					value = null;
				
				if (label != value && SupportsLabel)
				{
					label = value;
					Changed();
				}
			}
		}

		public virtual bool SupportsLabel
		{
			get { return false; }
		}

		/// <exception cref="RelationshipException">
		/// Cannot finalize relationship.
		/// </exception>
		internal void Attach()
		{
			if (!attached)
				OnAttaching(EventArgs.Empty);
			attached = true;
		}

		internal void Detach()
		{
			if (attached)
				OnDetaching(EventArgs.Empty);
			attached = false;
		}

		protected virtual void CopyFrom(Relationship relationship)
		{
			label = relationship.label;
		}

		void ISerializableElement.Serialize(XmlElement node)
		{
			Serialize(node);
		}

		void ISerializableElement.Deserialize(XmlElement node)
		{
			Deserialize(node);
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		protected internal virtual void Serialize(XmlElement node)
		{
			if (node == null)
				throw new ArgumentNullException("node");

			if (SupportsLabel && Label != null)
			{
				XmlElement labelNode = node.OwnerDocument.CreateElement("Label");
				labelNode.InnerText = Label.ToString();
				node.AppendChild(labelNode);
			}
			OnSerializing(new SerializeEventArgs(node));
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		protected internal virtual void Deserialize(XmlElement node)
		{
			if (node == null)
				throw new ArgumentNullException("node");

			if (SupportsLabel)
			{
				XmlElement labelNode = node["Label"];
				if (labelNode != null)
					Label = labelNode.InnerText;
			}
			OnDeserializing(new SerializeEventArgs(node));
		}

		protected virtual void OnAttaching(EventArgs e)
		{
			if (Attaching != null)
				Attaching(this, e);
		}

		protected virtual void OnDetaching(EventArgs e)
		{
			if (Detaching != null)
				Detaching(this, e);
		}

		private void OnSerializing(SerializeEventArgs e)
		{
			if (Serializing != null)
				Serializing(this, e);
		}

		private void OnDeserializing(SerializeEventArgs e)
		{
			if (Deserializing != null)
				Deserializing(this, e);
		}

		public abstract override string ToString();
	}
}
