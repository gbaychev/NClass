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
using NClass.Translations;

namespace NClass.Core
{
	public sealed class Comment : Element, IEntity
	{
		string text = string.Empty;

		public event SerializeEventHandler Serializing;
		public event SerializeEventHandler Deserializing;

		internal Comment()
		{
		}
	
		internal Comment(string text)
		{
			this.text = text;
		}

		public EntityType EntityType
		{
			get { return EntityType.Comment; }
		}

		public string Name
		{
			get { return Strings.Comment; }
		}

		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				if (value == null)
					value = string.Empty;

				if (text != value) {
					text = value;
					Changed();
				}
			}
		}

		public Comment Clone()
		{
			return new Comment(this.text);
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
		internal void Serialize(XmlElement node)
		{
			if (node == null)
				throw new ArgumentNullException("node");

			XmlElement child = node.OwnerDocument.CreateElement("Text");
			child.InnerText = Text;
			node.AppendChild(child);

			OnSerializing(new SerializeEventArgs(node));
		}

		/// <exception cref="BadSyntaxException">
		/// An error occured while deserializing.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// The XML document is corrupt.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		internal void Deserialize(XmlElement node)
		{
			if (node == null)
				throw new ArgumentNullException("node");

			XmlElement textNode = node["Text"];

			if (textNode != null)
				Text = textNode.InnerText;
			else
				Text = null;

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
			const int MaxLength = 50;

			if (Text == null) {
				return Strings.Comment;
			}
			else if (Text.Length > MaxLength) {
				return '"' + Text.Substring(0, MaxLength) + "...\"";
			}
			else {
				return '"' + Text + '"';
			}
		}
	}
}
