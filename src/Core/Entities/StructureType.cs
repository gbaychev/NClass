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
using System.Collections.Generic;

namespace NClass.Core
{
	public abstract class StructureType : SingleInharitanceType
	{
		/// <exception cref="BadSyntaxException">
		/// The <paramref name="name"/> does not fit to the syntax.
		/// </exception>
		protected StructureType(string name) : base(name)
		{
		}

		public sealed override EntityType EntityType
		{
			get { return EntityType.Structure; }
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

		public override bool SupportsDestructors
		{
			get { return false; }
		}

		public override bool SupportsNesting
		{
			get { return true; }
		}

		public override bool HasExplicitBase
		{
			get { return false; }
		}

		public override bool IsAllowedParent
		{
			get { return false; }
		}

		public override bool IsAllowedChild
		{
			get { return false; }
		}

		public override IEnumerable<Operation> OverridableOperations
		{
			get { return new Operation[] { } ; }
		}

		public sealed override string Signature
		{
			get
			{
				return (Language.GetAccessString(Access, false) + " Structure");
			}
		}

		public override string Stereotype
		{
			get { return "«structure»"; }
		}

		/// <exception cref="InvalidOperationException">
		/// The type does not support destructors.
		/// </exception>
		public override Destructor AddDestructor()
		{
			throw new InvalidOperationException("Structures do not support destructors.");
		}

		public abstract StructureType Clone();
	}
}
