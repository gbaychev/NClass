﻿// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// Copyright (C) 2020 Georgi Baychev

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

namespace NClass.Core.Entities
{
    public interface IInterfaceImplementer
    {
        IEnumerable<CompositeType> Interfaces
        {
            get;        
        }

        string Name
        {
            get;
        }

        Language Language
        {
            get;
        }

        bool ImplementsInterface
        {
            get;
        }

        Operation GetDefinedOperation(Operation operation);

        /// <exception cref="ArgumentException">
        /// The language of <paramref name="operation"/> does not equal.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="operation"/> is null.
        /// </exception>
        Operation Implement(Operation operation, bool isExplicit);

        /// <exception cref="RelationshipException">
        /// The language of <paramref name="interfaceType"/> does not equal.-or-
        /// <paramref name="interfaceType"/> is earlier implemented interface.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="interfaceType"/> is null.
        /// </exception>
        void AddInterface(InterfaceType interfaceType);

        void RemoveInterface(InterfaceType interfaceType);

        /// <exception cref="RelationshipException">
        /// The language of <paramref name="interfaceType"/> does not equal.-or-
        /// <paramref name="interfaceType"/> is earlier implemented interface.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="interfaceType"/> is null.
        /// </exception>
        /// For Dart support the mixin class can alos be used as an interface.
        void AddInterface(ClassType interfaceType);

        void RemoveInterface(ClassType interfaceType);
    }
}
