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
	public abstract class SingleInharitanceType : CompositeType, IInterfaceImplementer
	{
		List<InterfaceType> interfaceList = new List<InterfaceType>();

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="name"/> does not fit to the syntax.
		/// </exception>
		protected SingleInharitanceType(string name) : base(name)
		{
		}

		/// <exception cref="RelationshipException">
		/// The base and derived types do not equal.-or-
		/// The <paramref name="value"/> is descendant of the type.
		/// </exception>
		public abstract SingleInharitanceType Base
		{
			get;
			set;
		}

		public abstract IEnumerable<Operation> OverridableOperations
		{
			get;
		}

		protected List<InterfaceType> InterfaceList
		{
			get { return interfaceList; }
		}

		public IEnumerable<InterfaceType> Interfaces
		{
			get { return interfaceList; }
		}

		public bool ImplementsInterface
		{
			get
			{
				return (interfaceList.Count > 0);
			}
		}

		/// <exception cref="RelationshipException">
		/// The language of <paramref name="interfaceType"/> does not equal.-or-
		/// <paramref name="interfaceType"/> is earlier implemented interface.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="interfaceType"/> is null.
		/// </exception>
		public virtual void AddInterface(InterfaceType interfaceType)
		{
			if (interfaceType == null)
				throw new ArgumentNullException("interfaceType");

			foreach (InterfaceType implementedInterface in InterfaceList) {
				if (interfaceType == implementedInterface)
					throw new RelationshipException(Strings.ErrorCannotAddSameInterface);
			}
			
			InterfaceList.Add(interfaceType);
			Changed();
		}

		public void RemoveInterface(InterfaceType interfaceType)
		{
			if (InterfaceList.Remove(interfaceType))
				Changed();
		}

		/// <exception cref="ArgumentException">
		/// The language of <paramref name="operation"/> does not equal.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="operation"/> is null.
		/// </exception>
		public Operation Implement(Operation operation, bool explicitly)
		{
			if (operation == null)
				throw new ArgumentNullException("operation");

			if (operation.Language != this.Language)
				throw new ArgumentException(Strings.ErrorLanguagesDoNotEqual);

			if (!(operation.Parent is InterfaceType)) {
				throw new ArgumentException("The operation is not a member of an interface.");
			}

			if (explicitly && !operation.Language.SupportsExplicitImplementation) {
				throw new ArgumentException(
					Strings.ErrorExplicitImplementation, "explicitly");
			}

			Operation newOperation = Language.Implement(operation, this, explicitly);
			newOperation.Parent = this;

			AddOperation(newOperation);
			return newOperation;
		}

		/// <exception cref="ArgumentException">
		/// <paramref name="operation"/> cannot be overridden.-or-
		/// The language of <paramref name="operation"/> does not equal.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="operation"/> is null.
		/// </exception>
		public Operation Override(Operation operation)
		{
			if (operation == null)
				throw new ArgumentNullException("operation");

			if (operation.Language != this.Language)
				throw new ArgumentException(Strings.ErrorLanguagesDoNotEqual);

			Operation newOperation = Language.Override(operation, this);

			AddOperation(newOperation);
			return newOperation;
		}
	}
}
