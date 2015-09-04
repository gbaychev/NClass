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
using System.Drawing;
using System.Drawing.Drawing2D;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Editors;

namespace NClass.DiagramEditor.ClassDiagram.Shapes
{
	public sealed class InterfaceShape : CompositeTypeShape
	{
		InterfaceType _interface;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="interfaceType"/> is null.
		/// </exception>
		internal InterfaceShape(InterfaceType interfaceType)
			: base(interfaceType)
		{
			_interface = interfaceType;
			UpdateMinSize();
		}

		public override CompositeType CompositeType
		{
			get { return _interface; }
		}

		public InterfaceType InterfaceType
		{
			get { return _interface; }
		}

		protected override bool CloneEntity(Diagram diagram)
		{
			return diagram.InsertInterface(InterfaceType.Clone());
		}

		protected override Color GetBackgroundColor(Style style)
		{
			return style.InterfaceBackgroundColor;
		}

		protected override Color GetBorderColor(Style style)
		{
			return style.InterfaceBorderColor;
		}

		protected override int GetBorderWidth(Style style)
		{
			return style.InterfaceBorderWidth;
		}

		protected override bool IsBorderDashed(Style style)
		{
			return style.IsInterfaceBorderDashed;
		}

		protected override Color GetHeaderColor(Style style)
		{
			return style.InterfaceHeaderColor;
		}

		protected override int GetRoundingSize(Style style)
		{
			return style.InterfaceRoundingSize;
		}

		protected override GradientStyle GetGradientHeaderStyle(Style style)
		{
			return style.InterfaceGradientHeaderStyle;
		}
	}
}
