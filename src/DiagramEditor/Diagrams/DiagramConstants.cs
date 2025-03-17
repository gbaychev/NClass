// NClass - Free class diagram editor
// Copyright (C) 2025 Georgi Baychev
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

using System.Drawing;

namespace NClass.DiagramEditor.Diagrams
{
    internal class DiagramConstants
    {
        public const int DiagramPadding = 10;
        public const int PrecisionSize = 10;
        public const int MaximalPrecisionDistance = 500;
        public const float DashSize = 3;
        public static readonly Size MinSize = new Size(3000, 2000);
        public static readonly Pen SelectionPen = new Pen (Color.Black) { DashPattern = new[] { DashSize, DashSize } };
    }
}