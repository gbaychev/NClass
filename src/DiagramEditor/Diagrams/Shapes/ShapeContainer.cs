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
using System.Linq;
using NClass.Core;

namespace NClass.DiagramEditor.Diagrams.Shapes
{
    /// <summary>
    /// Simple interface for shapes, in which you can drop (nest)
    /// other shapes
    /// </summary>
    public abstract class ShapeContainer : Shape
    {
        protected bool areShapesHovering;

        protected INestable containerEntity;

        /// <summary>
        /// The contained shapes
        /// </summary>
        public List<Shape> ChildrenShapes { get; protected set; }
        /// <summary>
        /// When the user is hovering on this shapes
        /// </summary>
        public void EnterHover()
        {
            areShapesHovering = true;
            OnEnterHover?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// When the user stops hovering
        /// </summary>
        public void ExitHover()
        {
            areShapesHovering = false;
        }
        /// <summary>
        /// When the user drops the dragged shapes on the container
        /// </summary>
        /// <param name="shapes">The currently selected and dropped shapes</param>
        public void AttachShapes(List<Shape> shapes)
        {
            areShapesHovering = false;
            foreach (var shape in shapes)
            {
                if (shape == this)
                    continue;
                shape.ParentShape = this;
                if (ChildrenShapes.Contains(shape))
                    continue;
                ChildrenShapes.Add(shape);
                if (shape is ShapeContainer container)
                    container.SortOrder = this.SortOrder + 1;
                this.containerEntity.AddNestedChild(shape.Entity as INestableChild);

                UpdateSize();
            }
            NeedsRedraw = true;
        }

        protected virtual void UpdateSize()
        {

        }

        /// <summary>
        /// When dragging shapes away from this element
        /// </summary>
        /// <param name="shapes">The list of the dragged away shapes</param>
        public void DetachShapes(List<Shape> shapes)
        {
            foreach (var shape in shapes)
            {
                if (!ChildrenShapes.Contains(shape))
                    continue;

                shape.ParentShape = null;
                if (shape is ShapeContainer container)
                    container.SortOrder = 0;
                ChildrenShapes.Remove(shape);
                this.containerEntity.RemoveNestedChild(shape.Entity as INestableChild);
            }
        }
        /// <summary>
        /// Whether or not there are shapes hovering over this container
        /// </summary>
        public bool HasHoveringShapes => areShapesHovering;

        /// <summary>
        /// On hover enter
        /// </summary>
        public event EventHandler OnEnterHover;
        /// <summary>
        /// The sort order of child containers
        /// </summary>
        public int SortOrder { get; protected set; }

        protected ShapeContainer(IEntity entity) : base(entity)
        {
            this.containerEntity = (INestable)entity;
            this.ChildrenShapes = new List<Shape>();
        }

        public bool IsTopmostContainer()
        {
            return this.ParentShape == null;
        }

        public bool IsContainerSelected()
        {
            return this.IsSelected || ChildrenShapes.Any(cs => cs.IsSelected);
        }

        public IEnumerable<Shape> Flatten()
        {
            foreach (var children in FlattenChildrenShapes(this.ChildrenShapes))
            {
                yield return children;
            }

            yield return this;
        }

        private IEnumerable<Shape> FlattenChildrenShapes(IEnumerable<Shape> childrenShapes)
        {
            foreach (var child in childrenShapes)
            {
                if (child is ShapeContainer container)
                {
                    foreach (var containerChild in FlattenChildrenShapes(container.ChildrenShapes))
                    {
                        yield return containerChild;
                    }

                    yield return container;
                }
                else
                {
                    yield return child;
                }
            }
        }

        public IEnumerable<Shape> FlattenReverse(/*Predicate<Shape> match*/)
        {
            yield return this;

            foreach (var child in FlattenChildrenShapesReverse(this.ChildrenShapes))
            {
                //if(match(child))
                    yield return child;
            }
        }

        private IEnumerable<Shape> FlattenChildrenShapesReverse(IEnumerable<Shape> childrenShapes)
        {
            foreach (var child in childrenShapes)
            {
                if (child is ShapeContainer container)
                {
                    if(container.IsContainerSelected())
                        continue;

                    yield return container;
                    foreach (var containerChild in FlattenChildrenShapesReverse(container.ChildrenShapes))
                    {
                        yield return containerChild;
                    }
                }
                else if(!child.IsSelected)
                    yield return child;
            }
        }
    }
}