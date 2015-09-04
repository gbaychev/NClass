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
	public sealed class RealizationRelationship : TypeRelationship
	{
		/// <exception cref="RelationshipException">
		/// Cannot create realization.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="implementer"/> is null.-or-
		/// <paramref name="baseType"/> is null.
		/// </exception>
		internal RealizationRelationship(TypeBase implementer, InterfaceType baseType)
			: base(implementer, baseType)
		{
			if (!(implementer is IInterfaceImplementer))
				throw new RelationshipException(Strings.ErrorNotInterfaceImplementer);
			Attach();
		}

		public override RelationshipType RelationshipType
		{
			get { return RelationshipType.Realization; }
		}

		private IInterfaceImplementer Implementer
		{
			get { return (IInterfaceImplementer) First; }
		}

		private InterfaceType BaseType
		{
			get { return (InterfaceType) Second; }
		}

		public RealizationRelationship Clone(TypeBase implementer, InterfaceType baseType)
		{
			RealizationRelationship realization = new RealizationRelationship(implementer, baseType);
			realization.CopyFrom(this);
			return realization;
		}

		/// <exception cref="RelationshipException">
		/// Cannot finalize relationship.
		/// </exception>
		protected override void OnAttaching(EventArgs e)
		{
			Implementer.AddInterface(BaseType);
		}

		protected override void OnDetaching(EventArgs e)
		{
			Implementer.RemoveInterface(BaseType);
		}

		public override string ToString()
		{
			return string.Format("{0}: {1} --> {2}",
				Strings.Realization, First.Name, Second.Name);
		}
	}
}
