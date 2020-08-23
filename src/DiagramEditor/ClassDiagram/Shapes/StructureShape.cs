// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// Copyright (C) 2020 Georgi Baychev
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
using System.Drawing;
using NClass.Core;
using NClass.DiagramEditor.Diagrams;

namespace NClass.DiagramEditor.ClassDiagram.Shapes
{
    public sealed class StructureShape : CompositeTypeShape
    {
        StructureType structure;

        /// <exception cref="ArgumentNullException">
        /// <paramref name="structType"/> is null.
        /// </exception>
        internal StructureShape(StructureType structure) : base(structure)
        {
            this.structure = structure;
            UpdateMinSize();
        }

        public override CompositeType CompositeType
        {
            get { return structure; }
        }

        public StructureType StructureType
        {
            get { return structure; }
        }

        protected override bool CloneEntity(IDiagram diagram)
        {
            if (diagram.DiagramType != DiagramType.ClassDiagram)
                return false;

            return ((ClassDiagram)diagram).InsertStructure(StructureType.Clone());
        }

        protected override Color GetBackgroundColor(Style style)
        {
            return style.StructureBackgroundColor;
        }

        protected override Color GetBorderColor(Style style)
        {
            return style.StructureBorderColor;
        }

        protected override int GetBorderWidth(Style style)
        {
            return style.StructureBorderWidth;
        }

        protected override bool IsBorderDashed(Style style)
        {
            return style.IsStructureBorderDashed;
        }

        protected override Color GetHeaderColor(Style style)
        {
            return style.StructureHeaderColor;
        }

        protected override int GetRoundingSize(Style style)
        {
            return style.StructureRoundingSize;
        }

        protected override GradientStyle GetGradientHeaderStyle(Style style)
        {
            return style.StructureGradientHeaderStyle;
        }
    }
}
