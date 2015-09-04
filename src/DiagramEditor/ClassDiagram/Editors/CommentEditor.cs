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
using System.Windows.Forms;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Shapes;

namespace NClass.DiagramEditor.ClassDiagram.Editors
{
	public sealed partial class CommentEditor : EditorWindow
	{
		CommentShape shape = null;

		public CommentEditor()
		{
			InitializeComponent();
		}

		protected override void OnLocationChanged(EventArgs e)
		{
			base.OnLocationChanged(e);
		}

		internal override void Init(DiagramElement element)
		{
			shape = (CommentShape) element;
			
			txtComment.BackColor = Style.CurrentStyle.CommentBackColor;
			txtComment.ForeColor = Style.CurrentStyle.CommentTextColor;
			txtComment.Text = shape.Comment.Text;

			Font font = Style.CurrentStyle.CommentFont;
			txtComment.Font = new Font(font.FontFamily,
				font.SizeInPoints * shape.Diagram.Zoom, font.Style);
		}

		internal override void Relocate(DiagramElement element)
		{
			Relocate((CommentShape) element);
		}

		internal void Relocate(CommentShape shape)
		{
			Diagram diagram = shape.Diagram;
			if (diagram != null)
			{
				Rectangle absolute = shape.GetTextRectangle();
				// The following lines are required because of a .NET bug:
				// http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=380085
				if (!MonoHelper.IsRunningOnMono)
				{
					absolute.X -= 3;
					absolute.Width += 3;
				}
				
				this.SetBounds(
					(int) (absolute.X * diagram.Zoom) - diagram.Offset.X + ParentLocation.X,
					(int) (absolute.Y * diagram.Zoom) - diagram.Offset.Y + ParentLocation.Y,
					(int) (absolute.Width * diagram.Zoom),
					(int) (absolute.Height * diagram.Zoom));
			}
		}

		public override void ValidateData()
		{
			shape.Comment.Text = txtComment.Text;
		}

		private void txtComment_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter && e.Modifiers != Keys.None ||
				e.KeyCode == Keys.Escape)
			{
				shape.HideEditor();
			}
		}
	}
}
