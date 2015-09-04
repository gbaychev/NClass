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
using System.Collections;
using System.Text.RegularExpressions;
using NClass.Translations;

namespace NClass.Core
{
	public abstract class TypeBase : LanguageElement, IEntity
	{
		string name;
		AccessModifier access = AccessModifier.Public;
		CompositeType nestingParent = null;

		public event SerializeEventHandler Serializing;
		public event SerializeEventHandler Deserializing;

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="name"/> does not fit to the syntax.
		/// </exception>
		protected TypeBase(string name)
		{
			Initializing = true;
			Name = name;
			Initializing = false;
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="value"/> does not fit to the syntax.
		/// </exception>
		public virtual string Name
		{
			get
			{
				return name;
			}
			set
			{
				string newName = Language.GetValidName(value, true);

				if (newName != name) {
					name = newName;
					Changed();
				}
			}
		}

		public abstract EntityType EntityType
		{
			get;
		}

		/// <exception cref="BadSyntaxException">
		/// The type visibility is not valid in the current context.
		/// </exception>
		public virtual AccessModifier AccessModifier
		{
			get
			{
				return access;
			}
			set
			{
				if (!Language.IsValidModifier(value))
					throw new BadSyntaxException(Strings.ErrorInvalidModifier);

				if (access != value) {
					access = value;
					Changed();
				}
			}
		}

		public abstract AccessModifier DefaultAccess
		{
			get;
		}

		public AccessModifier Access
		{
			get
			{
				if (AccessModifier == AccessModifier.Default)
					return DefaultAccess;
				else
					return AccessModifier;
			}
		}

		/// <exception cref="RelationshipException">
		/// Parent type does not support nesting.-or-
		/// The inner type is already nested.-or-
		/// The parent type is already a child member of the type.
		/// </exception>
		public virtual CompositeType NestingParent
		{
			get
			{
				return nestingParent;
			}
			protected internal set
			{
				if (nestingParent != value) {
					if (value == this) {
						throw new RelationshipException(Strings.ErrorRecursiveNesting);
					}
					if (value != null && !value.SupportsNesting) {
						throw new RelationshipException(Strings.ErrorNestingNotSupported);
					}
					if (value != null && value.IsNestedAncestor(this)) {
						throw new RelationshipException(Strings.ErrorCyclicNesting);
					}

					if (nestingParent != null)
						nestingParent.RemoveNestedChild(this);
					nestingParent = value;
					if (nestingParent != null)
						nestingParent.AddNestedChild(this);
					Changed();
				}
			}
		}

		public bool IsNested
		{
			get { return (NestingParent != null); }
		}

		public abstract Language Language
		{
			get;
		}

		public abstract string Stereotype
		{
			get;
		}

		public abstract string Signature
		{
			get;
		}

		private bool IsNestedAncestor(TypeBase type)
		{
			if (NestingParent != null && NestingParent.IsNestedAncestor(type))
				return true;
			else
				return (type == this);
		}

		public abstract bool MoveUpItem(object item);

		public abstract bool MoveDownItem(object item);

		protected static bool MoveUp(IList list, object item)
		{
			if (item == null)
				return false;

			int index = list.IndexOf(item);
			if (index > 0) {
				object temp = list[index - 1];
				list[index - 1] = list[index];
				list[index] = temp;
				return true;
			}
			else {
				return false;
			}
		}

		protected static bool MoveDown(IList list, object item)
		{
			if (item == null)
				return false;

			int index = list.IndexOf(item);
			if (index >= 0 && index < list.Count - 1) {
				object temp = list[index + 1];
				list[index + 1] = list[index];
				list[index] = temp;
				return true;
			}
			else {
				return false;
			}
		}

		protected virtual void CopyFrom(TypeBase type)
		{
			name = type.name;
			access = type.access;
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

			XmlElement child;

			child = node.OwnerDocument.CreateElement("Name");
			child.InnerText = Name;
			node.AppendChild(child);

			child = node.OwnerDocument.CreateElement("Access");
			child.InnerText = AccessModifier.ToString();
			node.AppendChild(child);

			OnSerializing(new SerializeEventArgs(node));
		}

		/// <exception cref="BadSyntaxException">
		/// An error occured whiledeserializing.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// The XML document is corrupt.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		protected internal virtual void Deserialize(XmlElement node)
		{
			if (node == null)
				throw new ArgumentNullException("node");

			RaiseChangedEvent = false;
			XmlElement nameChild = node["Name"];
			if (nameChild != null)
				Name = nameChild.InnerText;

			XmlElement accessChild = node["Access"];
			if (accessChild != null)
				AccessModifier = Language.TryParseAccessModifier(accessChild.InnerText);

			RaiseChangedEvent = true;
			OnDeserializing(new SerializeEventArgs(node));
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

		public override string ToString()
		{
			return Name + ": " + Signature;
		}
	}
}
