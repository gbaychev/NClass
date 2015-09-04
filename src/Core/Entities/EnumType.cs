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
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NClass.Core
{
	public abstract class EnumType : TypeBase
	{
		List<EnumValue> values = new List<EnumValue>();

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="name"/> does not fit to the syntax.
		/// </exception>
		protected EnumType(string name) : base(name)
		{
		}

		public sealed override EntityType EntityType
		{
			get { return EntityType.Enum; }
		}

		public IEnumerable<EnumValue> Values
		{
			get { return values; }
		}

		public int ValueCount
		{
			get { return values.Count; }
		}

		public sealed override string Signature
		{
			get
			{
				return (Language.GetAccessString(Access, false) + " Enum");
			}
		}

		public override string Stereotype
		{
			get { return "«enumeration»"; }
		}

		/// <exception cref="BadSyntaxException">
		/// The name does not fit to the syntax.
		/// </exception>
		/// <exception cref="ReservedNameException">
		/// The name is a reserved name.
		/// </exception>
		public abstract EnumValue AddValue(string declaration);

		/// <exception cref="ReservedNameException">
		/// The name is a reserved name.
		/// </exception>
		protected void AddValue(EnumValue newValue)
		{
			if (newValue != null) {
				foreach (EnumValue value in Values) {
					if (value.Name == newValue.Name)
						throw new ReservedNameException(newValue.Name);
				}

				values.Add(newValue);
				newValue.Modified += delegate { Changed(); };
				Changed();
			}
		}

		public EnumValue GetValue(int index)
		{
			if (index >= 0 && index < values.Count)
				return values[index];
			else
				return null;
		}

		/// <exception cref="BadSyntaxException">
		/// The name does not fit to the syntax.
		/// </exception>
		/// <exception cref="ReservedNameException">
		/// The name is a reserved name.
		/// </exception>
		public abstract EnumValue ModifyValue(EnumValue value, string declaration);

		/// <exception cref="ReservedNameException">
		/// The new name is a reserved name.
		/// </exception>
		protected bool ChangeValue(EnumValue oldValue, EnumValue newValue)
		{
			if (oldValue == null || newValue == null)
				return false;

			int index = -1;
			for (int i = 0; i < values.Count; i++) {
				if (values[i] == oldValue)
					index = i;
				else if (values[i].Name == newValue.Name)
					throw new ReservedNameException(newValue.Name);
			}

			if (index == -1) {
				return false;
			}
			else {
				values[index] = newValue;
				Changed();
				return true;
			}
		}

		public void RemoveValue(EnumValue value)
		{
			if (values.Remove(value))
				Changed();
		}

		public override bool MoveUpItem(object item)
		{
			if (item is EnumValue && MoveUp(values, item))
			{
				Changed();
				return true;
			}
			else
			{
				return false;
			}
		}

		public override bool MoveDownItem(object item)
		{
			if (item is EnumValue && MoveDown(values, item))
			{
				Changed();
				return true;
			}
			else
			{
				return false;
			}
		}

		protected override void CopyFrom(TypeBase type)
		{
			base.CopyFrom(type);

			EnumType enumType = (EnumType) type;
			values.Clear();
			values.Capacity = enumType.values.Capacity;
			foreach (EnumValue value in enumType.values)
			{
				values.Add(value.Clone());
			}
		}

		public abstract EnumType Clone();

		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		protected internal override void Serialize(XmlElement node)
		{
			base.Serialize(node);

			foreach (EnumValue value in values) {
				XmlElement child = node.OwnerDocument.CreateElement("Value");
				child.InnerText = value.ToString();
				node.AppendChild(child);
			}
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
		protected internal override void Deserialize(XmlElement node)
		{
			RaiseChangedEvent = false;

			XmlNodeList nodeList = node.SelectNodes("Value");
			foreach (XmlNode valueNode in nodeList)
				AddValue(valueNode.InnerText);

			base.Deserialize(node);
			RaiseChangedEvent = true;
		}
	}
}
