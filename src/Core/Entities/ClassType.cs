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
using System.Xml;
using NClass.Translations;

namespace NClass.Core
{
	public abstract class ClassType : SingleInharitanceType
	{
		ClassModifier modifier = ClassModifier.None;
		ClassType baseClass = null;
		int derivedClassCount = 0;

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="name"/> does not fit to the syntax.
		/// </exception>
		protected ClassType(String name) : base(name)
		{
		}

		public sealed override EntityType EntityType
		{
			get { return EntityType.Class; }
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="value"/> does not fit to the syntax.
		/// </exception>
		public virtual ClassModifier Modifier
		{
			get
			{
				return modifier;
			}
			set
			{
				if (modifier != value) {
					if (value == ClassModifier.Static && (IsSuperClass || HasExplicitBase))
						throw new BadSyntaxException(Strings.ErrorInvalidModifier);
					if (value == ClassModifier.Sealed && IsSuperClass)
						throw new BadSyntaxException(Strings.ErrorInvalidModifier);

					modifier = value;
					Changed();
				}
			}
		}

		public override bool SupportsFields
		{
			get { return true; }
		}

		public override bool SupportsMethods
		{
			get { return true; }
		}

		public override bool SupportsConstuctors
		{
			get { return true; }
		}

		public override bool SupportsNesting
		{
			get { return true; }
		}

		public override bool IsAllowedParent
		{
			get
			{
				return (
					Modifier != ClassModifier.Sealed &&
					Modifier != ClassModifier.Static
				);
			}
		}

		public override bool IsAllowedChild
		{
			get
			{
				return (Modifier != ClassModifier.Static);
			}
		}

		public override bool HasExplicitBase
		{
			get
			{
				return (baseClass != null);
			}
		}

		public bool IsSuperClass
		{
			get { return (derivedClassCount > 0); }
		}

		public sealed override string Signature
		{
			get
			{
				string accessString = Language.GetAccessString(Access, false);
				string modifierString = Language.GetClassModifierString(Modifier, false);

				if (Modifier == ClassModifier.None)
					return string.Format("{0} Class", accessString);
				else
					return string.Format("{0} {1} Class", accessString, modifierString);
			}
		}

		public override string Stereotype
		{
			get { return null; }
		}

		/// <exception cref="RelationshipException">
		/// The base and derived types do not equal.-or-
		/// The <paramref name="value"/> is descendant of the type.
		/// </exception>
		public override SingleInharitanceType Base
		{
			get
			{
				return BaseClass;
			}
			set
			{
				if (value != null && !(value is ClassType))
					throw new RelationshipException(Strings.ErrorInvalidBaseType);

				BaseClass = (ClassType) value;
			}
		}

		/// <exception cref="RelationshipException">
		/// The language of <paramref name="value"/> does not equal.-or-
		/// <paramref name="value"/> is static or sealed class.-or-
		/// The <paramref name="value"/> is descendant of the class.
		/// </exception>
		public virtual ClassType BaseClass
		{
			get
			{
				return baseClass;
			}
			set
			{
				if (value == baseClass)
					return;

				if (value == null) {
					baseClass.derivedClassCount--;
					baseClass = null;
					Changed();
					return;
				}

				if (value == this)
					throw new RelationshipException(Strings.ErrorInvalidBaseType);

				if (value.Modifier == ClassModifier.Sealed ||
					value.Modifier == ClassModifier.Static)
				{
					throw new RelationshipException(Strings.ErrorCannotInherit);
				}
				if (value.IsAncestor(this)) {
					throw new RelationshipException(string.Format(Strings.ErrorCyclicBase,
						Strings.Class));
				}
				if (value.Language != this.Language)
					throw new RelationshipException(Strings.ErrorLanguagesDoNotEqual);

				baseClass = value;
				baseClass.derivedClassCount++;
				Changed();
			}
		}

		public override IEnumerable<Operation> OverridableOperations
		{
			get
			{
				for (int i = 0; i < OperationList.Count; i++) {
					if (OperationList[i].Overridable)
						yield return OperationList[i];
				}
			}
		}

		private bool IsAncestor(ClassType classType)
		{
			if (BaseClass != null && BaseClass.IsAncestor(classType))
				return true;
			else
				return (classType == this);
		}

		protected override void CopyFrom(TypeBase type)
		{
			base.CopyFrom(type);
			ClassType classType = (ClassType) type;
			modifier = classType.modifier;
		}

		public abstract ClassType Clone();

		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		protected internal override void Serialize(XmlElement node)
		{
			base.Serialize(node);

			XmlElement child = node.OwnerDocument.CreateElement("Modifier");
			child.InnerText = Modifier.ToString();
			node.AppendChild(child);
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

			XmlElement child = node["Modifier"];
			if (child != null)
				Modifier = Language.TryParseClassModifier(child.InnerText);

			base.Deserialize(node);
			RaiseChangedEvent = true;
		}
	}
}
