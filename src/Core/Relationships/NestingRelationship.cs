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
	public sealed class NestingRelationship : TypeRelationship
	{
		/// <exception cref="RelationshipException">
		/// Cannot create nesting relationship.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="parentClass"/> is null.-or-
		/// <paramref name="innerClass"/> is null.
		/// </exception>
		internal NestingRelationship(CompositeType parentType, TypeBase innerType)
			: base(parentType, innerType)
		{
			Attach();
		}

		public override RelationshipType RelationshipType
		{
			get { return RelationshipType.Nesting; }
		}

		private CompositeType ParentType
		{
			get { return (CompositeType) First; }
		}

		private TypeBase InnerType
		{
			get { return (TypeBase) Second; }
		}

		public NestingRelationship Clone(CompositeType parentType, TypeBase innerType)
		{
			NestingRelationship nesting = new NestingRelationship(parentType, innerType);
			nesting.CopyFrom(this);
			return nesting;
		}

		/// <exception cref="RelationshipException">
		/// Cannot finalize relationship.
		/// </exception>
		protected override void OnAttaching(EventArgs e)
		{
			if (InnerType.IsNested)
				throw new RelationshipException(Strings.ErrorInnerTypeAlreadyNested);

			InnerType.NestingParent = ParentType;
		}

		protected override void OnDetaching(EventArgs e)
		{
			InnerType.NestingParent = null;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1} (+)--> {2}",
				Strings.Nesting, First.Name, Second.Name);
		}
	}
}
