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

namespace NClass.Core
{
	[Flags]
	public enum OperationModifier
	{
		/// <summary>
		/// The operation has no modifiers.
		/// </summary>
		None = 0,

		/// <summary>
		/// A static operation does not operate on a specific instance,
		/// it belongs to the type.
		/// </summary>
		Static = 1,

		/// <summary>
		/// Declares a method whose implementation can be changed by an 
		/// overriding member in a derived class.
		/// </summary>
		Virtual = 2,

		/// <summary>
		/// Abstract members must be implemented by classes
		/// that derive from the abstract class.
		/// </summary>
		Abstract = 4,

		/// <summary>
		/// Provides a new implementation of a virtual member
		/// inherited from a base class.
		/// </summary>
		Override = 8,

		/// <summary>
		/// A sealed method overrides a method in a base class,
		/// but itself cannot be overridden further in any derived class.
		/// </summary>
		Sealed = 16,

		/// <summary>
		/// The derived class member hides the base class member.
		/// </summary>
		Hider = 32
	}
}
