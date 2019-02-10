// NClass - Free class diagram editor
// Copyright (C) 2019 Georgi Baychev
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
using System.Drawing;

namespace NClass.DiagramEditor.Diagrams.Shapes
{
    /// <summary>
    /// Simple interface for shapes, in which you can drop (nest)
    /// other shapes
    /// </summary>
    public interface IShapeContainer
    {
        /// <summary>
        /// The contained shapes
        /// </summary>
        List<Shape> ChildrenShapes { get; set; }
        /// <summary>
        /// When the user is hovering on this shapes
        /// </summary>
        void EnterHover();
        /// <summary>
        /// When the user stops hovering
        /// </summary>
        void ExitHover();
        /// <summary>
        /// When the user drops the dragged shapes on the container
        /// </summary>
        /// <param name="shapes">The currently selected and dropped shapes</param>
        void AttachShapes(List<Shape> shapes);
        /// <summary>
        /// When dragging shapes away from this element
        /// </summary>
        /// <param name="shapes">The list of the dragged away shapes</param>
        void DetachShapes(List<Shape> shapes);
        /// <summary>
        /// Whether or not there are shapes hovering over this container
        /// </summary>
        bool HasHoveringShapes { get; }
        /// <summary>
        /// Whether or not a shape is hovering over this container
        /// </summary>
        /// <param name="location">The location of the other shape</param>
        /// <returns>True, if another shape is hovering over</returns>
        bool ContainsShape(PointF location);

        /// <summary>
        /// On hover enter
        /// </summary>
        event EventHandler OnEnterHover;
        /// <summary>
        /// The sort order of child containers
        /// </summary>
        int SortOrder { get; set; }
    }
}