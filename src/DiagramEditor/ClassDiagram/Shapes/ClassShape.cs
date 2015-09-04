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
	public sealed class ClassShape : CompositeTypeShape
	{
		ClassType _class;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="classType"/> is null.
		/// </exception>
		internal ClassShape(ClassType classType)
			: base(classType)
		{
			_class = classType;
			UpdateMinSize();
		}

		public override CompositeType CompositeType
		{
			get { return _class; }
		}

		public ClassType ClassType
		{
			get { return _class; }
		}

		protected override bool CloneEntity(Diagram diagram)
		{
			return diagram.InsertClass(ClassType.Clone());
		}

		protected override Color GetBackgroundColor(Style style)
		{
			return style.ClassBackgroundColor;
		}

		protected override Color GetBorderColor(Style style)
		{
			return style.ClassBorderColor;
		}

		protected override int GetBorderWidth(Style style)
		{
			switch (_class.Modifier)
			{
				case ClassModifier.Abstract:
					return style.AbstractClassBorderWidth;

				case ClassModifier.Sealed:
					return style.SealedClassBorderWidth;

				case ClassModifier.Static:
					return style.StaticClassBorderWidth;

				case ClassModifier.None:
				default:
					return style.ClassBorderWidth;
			}
		}

		protected override bool IsBorderDashed(Style style)
		{
			switch (_class.Modifier)
			{
				case ClassModifier.Abstract:
					return style.IsAbstractClassBorderDashed;

				case ClassModifier.Sealed:
					return style.IsSealedClassBorderDashed;

				case ClassModifier.Static:
					return style.IsStaticClassBorderDashed;

				case ClassModifier.None:
				default:
					return style.IsClassBorderDashed;
			}
		}

		protected override Color GetHeaderColor(Style style)
		{
			return style.ClassHeaderColor;
		}

		protected override Font GetNameFont(Style style)
		{
			if (_class.Modifier == ClassModifier.Abstract)
				return style.AbstractNameFont;
			else
				return base.GetNameFont(style);
		}

		protected override int GetRoundingSize(Style style)
		{
			return style.ClassRoundingSize;
		}

		protected override GradientStyle GetGradientHeaderStyle(Style style)
		{
			return style.ClassGradientHeaderStyle;
		}
	}
}
