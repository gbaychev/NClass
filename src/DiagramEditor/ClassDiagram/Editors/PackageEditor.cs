// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// Copyright (C) 2016 Georgi Baychev
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
using System.Windows.Forms;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using NClass.DiagramEditor.Diagrams;

namespace NClass.DiagramEditor.ClassDiagram.Editors
{
	public sealed partial class PackageEditor : EditorWindow
	{
		PackageShape shape = null;

		public PackageEditor()
		{
			InitializeComponent();
		}

		protected override void OnLocationChanged(EventArgs e)
		{
			base.OnLocationChanged(e);
		}

		internal override void Init(DiagramElement element)
		{
			shape = (PackageShape) element;
			
			txtName.BackColor = Style.CurrentStyle.PackageBackColor;
			txtName.ForeColor = Style.CurrentStyle.PackageTextColor;
			txtName.Text = shape.Name;

			Font font = Style.CurrentStyle.PackageFont;
			txtName.Font = new Font(font.FontFamily,
				font.SizeInPoints * shape.Diagram.Zoom, font.Style);
		}

		internal override void Relocate(DiagramElement element)
		{
			Relocate((PackageShape) element);
		}

		internal void Relocate(PackageShape shape)
		{
			IDiagram diagram = shape.Diagram;
			if (diagram != null)
			{
				Rectangle absolute = shape.GetNameRectangle();
	
				this.SetBounds(
					(int) (absolute.X * diagram.Zoom) - diagram.Offset.X + ParentLocation.X,
					(int) (absolute.Y * diagram.Zoom) - diagram.Offset.Y + ParentLocation.Y,
					(int) (absolute.Width * diagram.Zoom),
					(int) (absolute.Height * diagram.Zoom));

                this.txtName.Width = (int)(absolute.Width * diagram.Zoom);
                this.txtName.Height = (int)(absolute.Height * diagram.Zoom);
            }
		}

		public override void ValidateData()
		{
			shape.Name = txtName.Text;
		}

		private void txtPackage_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter && e.Modifiers != Keys.None ||
				e.KeyCode == Keys.Escape)
			{
				shape.HideEditor();
			}
		}
	}
}
