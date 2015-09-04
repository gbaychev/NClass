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
using NClass.Translations;

namespace NClass.Core
{
	public abstract class InterfaceType : CompositeType
	{
		List<InterfaceType> baseList;

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="name"/> does not fit to the syntax.
		/// </exception>
		protected InterfaceType(string name) : base(name)
		{
			baseList = new List<InterfaceType>();
		}

		public sealed override EntityType EntityType
		{
			get { return EntityType.Interface; }
		}

		protected List<InterfaceType> BaseList
		{
			get { return baseList; }
		}

		public IEnumerable<InterfaceType> Bases
		{
			get { return baseList; }
		}

		public override bool SupportsFields
		{
			get { return false; }
		}

		public override bool SupportsMethods
		{
			get { return true; }
		}

		public override bool SupportsConstuctors
		{
			get { return false; }
		}

		public override bool SupportsDestructors
		{
			get { return false; }
		}

		public override bool SupportsNesting
		{
			get { return false; }
		}

		public override bool HasExplicitBase
		{
			get { return (baseList.Count > 0); }
		}

		public override bool IsAllowedParent
		{
			get { return true; }
		}

		public override bool IsAllowedChild
		{
			get { return true; }
		}

		public sealed override string Signature
		{
			get
			{
				return (Language.GetAccessString(Access, false) + " Interface");
			}
		}

		public override string Stereotype
		{
			get { return "«interface»"; }
		}

		private bool IsAncestor(InterfaceType _interface)
		{
			foreach (InterfaceType baseInterface in baseList) {
				if (baseInterface.IsAncestor(_interface))
					return true;
			}
			return (_interface == this);
		}

		/// <exception cref="RelationshipException">
		/// The language of <paramref name="_base"/> does not equal.-or-
		/// <paramref name="_base"/> is earlier added base.-or-
		/// <paramref name="_base"/> is descendant of the interface.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="_base"/> is null.
		/// </exception>
		internal void AddBase(InterfaceType _base)
		{
			if (_base == null)
				throw new ArgumentNullException("_base");

			if (BaseList.Contains(_base)) {
				throw new RelationshipException(
					Strings.ErrorCannotAddSameBaseInterface);
			}
			if (_base.IsAncestor(this)) {
					throw new RelationshipException(string.Format(Strings.ErrorCyclicBase,
						Strings.Interface));
			}

			if (_base.Language != this.Language)
				throw new RelationshipException(Strings.ErrorLanguagesDoNotEqual);

			BaseList.Add(_base);
			Changed();
		}

		internal bool RemoveBase(InterfaceType _base)
		{
			if (BaseList.Remove(_base)) {
				Changed();
				return true;
			}
			else {
				return false;
			}
		}

		/// <exception cref="InvalidOperationException">
		/// The type does not support fields.
		/// </exception>
		public override Field AddField()
		{
			throw new InvalidOperationException("Interfaces do not support fields.");
		}

		/// <exception cref="InvalidOperationException">
		/// The type does not support constructors.
		/// </exception>
		public sealed override Constructor AddConstructor()
		{
			throw new InvalidOperationException("Interfaces do not support constructors.");
		}

		/// <exception cref="InvalidOperationException">
		/// The type does not support destructors.
		/// </exception>
		public sealed override Destructor AddDestructor()
		{
			throw new InvalidOperationException("Interfaces do not support destructors.");
		}

		public abstract InterfaceType Clone();
	}
}
