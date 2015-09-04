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
using NClass.Translations;

namespace NClass.Core
{
	public sealed class GeneralizationRelationship : TypeRelationship
	{
		/// <exception cref="RelationshipException">
		/// Cannot create generalization.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="derivedType"/> is null.-or-
		/// <paramref name="baseType"/> is null.
		/// </exception>
		internal GeneralizationRelationship(CompositeType derivedType, CompositeType baseType)
			: base(derivedType, baseType)
		{
			Attach();
		}

		public override RelationshipType RelationshipType
		{
			get { return RelationshipType.Generalization; }
		}

		private CompositeType DerivedType
		{
			get { return (CompositeType) First; }
		}

		private CompositeType BaseType
		{
			get { return (CompositeType) Second; }
		}

		public GeneralizationRelationship Clone(CompositeType derivedType, CompositeType baseType)
		{
			GeneralizationRelationship generalization = 
				new GeneralizationRelationship(derivedType, baseType);
			generalization.CopyFrom(this);
			return generalization;
		}

		/// <exception cref="RelationshipException">
		/// Cannot finalize relationship.
		/// </exception>
		protected override void OnAttaching(EventArgs e)
		{
			base.OnAttaching(e);

			if (!DerivedType.IsAllowedChild)
				throw new RelationshipException(Strings.ErrorNotAllowedChild);
			if (!BaseType.IsAllowedParent)
				throw new RelationshipException(Strings.ErrorNotAllowedParent);
			if (First is SingleInharitanceType && ((SingleInharitanceType) First).HasExplicitBase)
				throw new RelationshipException(Strings.ErrorMultipleBases);
			if (First is SingleInharitanceType ^ Second is SingleInharitanceType ||
				First is InterfaceType ^ Second is InterfaceType)
				throw new RelationshipException(Strings.ErrorInvalidBaseType);

			if (First is SingleInharitanceType && Second is SingleInharitanceType) {
				((SingleInharitanceType) First).Base = (SingleInharitanceType) Second;
			}
			else if (First is InterfaceType && Second is InterfaceType) {
				((InterfaceType) First).AddBase((InterfaceType) Second);
			}
		}

		protected override void OnDetaching(EventArgs e)
		{
			base.OnDetaching(e);

			if (First is SingleInharitanceType)
				((SingleInharitanceType) First).Base = null;
			else if (First is InterfaceType)
				((InterfaceType) First).RemoveBase(Second as InterfaceType);
		}

		public override string ToString()
		{
			return string.Format("{0}: {1} --> {2}",
				Strings.Generalization, First.Name, Second.Name);
		}
	}
}
