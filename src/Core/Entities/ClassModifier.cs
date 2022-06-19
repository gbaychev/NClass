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

namespace NClass.Core.Entities
{
    public enum ClassModifier
    {
        None,

        /// <summary>
        /// Indicates that a class is intended only to be a base class of other classes.
        /// </summary>
        Abstract,

        /// <summary>
        /// Specifies that a class cannot be inherited.
        /// </summary>
        Sealed,

        /// <summary>
        /// Indicates that a class contains only static members.
        /// </summary>
        Static,


        /// <summary>
        /// Indicates that a class is a Mixin(Dart only).
        /// </summary>
        Mixin
    }
}
