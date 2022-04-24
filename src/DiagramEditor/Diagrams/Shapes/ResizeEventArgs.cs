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

namespace NClass.DiagramEditor.Diagrams.Shapes
{
    public delegate void ResizeEventHandler(object sender, ResizeEventArgs e);

    public class ResizeEventArgs : EventArgs
    {
        SizeF positionChange;
        SizeF sizeChange;

        public ResizeEventArgs(SizeF sizeChange)
        {
            this.positionChange = Size.Empty;
            this.sizeChange = sizeChange;
        }

        public ResizeEventArgs(SizeF positionChange, SizeF sizeChange)
        {
            this.positionChange = positionChange;
            this.sizeChange = sizeChange;
        }

        public SizeF PositionChange
        {
            get { return positionChange; }
            set { positionChange = value; }
        }

        public SizeF SizeChange
        {
            get { return sizeChange; }
            set { sizeChange = value; }
        }
    }
}
