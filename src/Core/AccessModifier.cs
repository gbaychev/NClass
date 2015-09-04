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
	public enum AccessModifier
	{
		/// <summary>
		/// The default access in the context of the element's scope.
		/// </summary>
		Default,

		/// <summary>
		/// Access is not restricted.
		/// </summary>
		Public,

		/// <summary>
		/// Access is limited to the current assembly or types 
		/// derived from the containing class.
		/// </summary>
		ProtectedInternal,

		/// <summary>
		/// Access is limited to the current assembly.
		/// </summary>
		Internal,

		/// <summary>
		/// Access is limited to the containing class or types 
		/// derived from the containing class.
		/// </summary>
		Protected,

		/// <summary>
		/// Access is limited to the containing type.
		/// </summary>
		Private
	}
}
