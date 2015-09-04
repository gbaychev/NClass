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
using System.Text;
using System.Xml;
using NClass.Translations;

namespace NClass.Core
{
	public sealed class AssociationRelationship : TypeRelationship
	{
		AssociationType associationType = AssociationType.Association;
		Direction direction = Direction.Unidirectional;
		string startRole, endRole;
		string startMultiplicity, endMultiplicity;

		public event EventHandler Reversed;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="first"/> or <paramref name="second"/> is null.
		/// </exception>
		internal AssociationRelationship(TypeBase first, TypeBase second)
			: base(first, second)
		{
			Attach();
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="first"/> is null.-or-
		/// <paramref name="second"/> is null.
		/// </exception>
		internal AssociationRelationship(TypeBase first, TypeBase second, AssociationType type)
			: base(first, second)
		{
			this.associationType = type;
			Attach();
		}

		public override RelationshipType RelationshipType
		{
			get { return RelationshipType.Association; }
		}

		public override bool SupportsLabel
		{
			get { return true; }
		}

		public Direction Direction
		{
			get
			{
				return direction;
			}
			set
			{
				if (direction != value)
				{
					direction = value;
					Changed();
				}
			}
		}

		public AssociationType AssociationType
		{
			get
			{
				return associationType;
			}
			set
			{
				if (associationType != value)
				{
					associationType = value;
					Changed();
				}
			}
		}

		public bool IsAggregation
		{
			get
			{
				return (associationType == AssociationType.Aggregation);
			}
		}

		public bool IsComposition
		{
			get
			{
				return (associationType == AssociationType.Composition);
			}
		}

		public string StartRole
		{
			get
			{
				return startRole;
			}
			set
			{
				if (value == "")
					value = null;

				if (startRole != value)
				{
					startRole = value;
					Changed();
				}
			}
		}

		public string EndRole
		{
			get
			{
				return endRole;
			}
			set
			{
				if (value == "")
					value = null;

				if (endRole != value)
				{
					endRole = value;
					Changed();
				}
			}
		}

		public string StartMultiplicity
		{
			get
			{
				return startMultiplicity;
			}
			set
			{
				if (value == "")
					value = null;

				if (startMultiplicity != value)
				{
					startMultiplicity = value;
					Changed();
				}
			}
		}

		public string EndMultiplicity
		{
			get
			{
				return endMultiplicity;
			}
			set
			{
				if (value == "")
					value = null;

				if (endMultiplicity != value)
				{
					endMultiplicity = value;
					Changed();
				}
			}
		}

		public void Reverse()
		{
			//TODO: ne az õsosztályon kereszül érje el, egyszerûbb lenne saját taggal
			IEntity first = First;
			First = Second;
			Second = first;

			OnReversed(EventArgs.Empty);
			Changed();
		}

		protected override void CopyFrom(Relationship relationship)
		{
			base.CopyFrom(relationship);

			AssociationRelationship association = (AssociationRelationship) relationship;
			associationType = association.associationType;
			direction = association.direction;
			startRole = association.startRole;
			endRole = association.endRole;
			startMultiplicity = association.startMultiplicity;
			endMultiplicity = association.endMultiplicity;
		}

		public AssociationRelationship Clone(TypeBase first, TypeBase second)
		{
			AssociationRelationship association = new AssociationRelationship(first, second);
			association.CopyFrom(this);
			return association;
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		protected internal override void Serialize(XmlElement node)
		{
			base.Serialize(node);

			XmlElement directionNode = node.OwnerDocument.CreateElement("Direction");
			directionNode.InnerText = Direction.ToString();
			node.AppendChild(directionNode);

			XmlElement aggregationNode = node.OwnerDocument.CreateElement("AssociationType");
			aggregationNode.InnerText = AssociationType.ToString();
			node.AppendChild(aggregationNode);

			if (StartRole != null)
			{
				XmlElement roleNode = node.OwnerDocument.CreateElement("StartRole");
				roleNode.InnerText = StartRole.ToString();
				node.AppendChild(roleNode);
			}
			if (EndRole != null)
			{
				XmlElement roleNode = node.OwnerDocument.CreateElement("EndRole");
				roleNode.InnerText = EndRole.ToString();
				node.AppendChild(roleNode);
			}
			if (StartMultiplicity != null)
			{
				XmlElement multiplicityNode = node.OwnerDocument.CreateElement("StartMultiplicity");
				multiplicityNode.InnerText = StartMultiplicity.ToString();
				node.AppendChild(multiplicityNode);
			}
			if (EndMultiplicity != null)
			{
				XmlElement multiplicityNode = node.OwnerDocument.CreateElement("EndMultiplicity");
				multiplicityNode.InnerText = EndMultiplicity.ToString();
				node.AppendChild(multiplicityNode);
			}
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		protected internal override void Deserialize(XmlElement node)
		{
			base.Deserialize(node);

			XmlElement child = node["Direction"];

			RaiseChangedEvent = false;
			if (child != null)
			{                                              // Old file format
				if (child.InnerText == "Unidirectional" || child.InnerText == "SourceDestination")
					Direction = Direction.Unidirectional;
				else
					Direction = Direction.Bidirectional;
			}

			try
			{
				// Old file format
				{
					child = node["IsAggregation"];
					if (child != null && bool.Parse(child.InnerText))
						associationType = AssociationType.Aggregation;

					child = node["IsComposition"];
					if (child != null && bool.Parse(child.InnerText))
						associationType = AssociationType.Composition;
				}

				child = node["AssociationType"];
				if (child != null)
				{
					if (child.InnerText == "Aggregation")
						associationType = AssociationType.Aggregation;
					else if (child.InnerText == "Composition")
						associationType = AssociationType.Composition;
					else
						associationType = AssociationType.Association;
				}

				child = node["StartRole"];
				if (child != null)
					startRole = child.InnerText;

				child = node["EndRole"];
				if (child != null)
					endRole = child.InnerText;

				child = node["StartMultiplicity"];
				if (child != null)
					startMultiplicity = child.InnerText;

				child = node["EndMultiplicity"];
				if (child != null)
					endMultiplicity = child.InnerText;
			}
			catch (ArgumentException)
			{
				// Wrong format
			}
			RaiseChangedEvent = true;
		}

		private void OnReversed(EventArgs e)
		{
			if (Reversed != null)
				Reversed(this, e);
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder(50);

			if (IsAggregation)
				builder.Append(Strings.Aggregation);
			else if (IsComposition)
				builder.Append(Strings.Composition);
			else
				builder.Append(Strings.Association);
			builder.Append(": ");
			builder.Append(First.Name);

			switch (Direction)
			{
				case Direction.Bidirectional:
					if (AssociationType == AssociationType.Association)
						builder.Append(" --- ");
					else
						builder.Append(" <>-- ");
					break;
				case Direction.Unidirectional:
					if (AssociationType == AssociationType.Association)
						builder.Append(" --> ");
					else
						builder.Append(" <>-> ");
					break;
				default:
					builder.Append(", ");
					break;
			}
			builder.Append(Second.Name);

			return builder.ToString();
		}
	}
}