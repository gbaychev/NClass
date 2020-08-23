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
using System.Windows.Forms;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using NClass.DiagramEditor.Commands;
using NClass.DiagramEditor.Diagrams.Shapes;
using NClass.DiagramEditor.UseCaseDiagram.Shapes;

namespace NClass.DiagramEditor.Diagrams.Editors
{
    public partial class ShapeNameEditor : PopupWindow
    {
        private Shape shape = null;

        public ShapeNameEditor()
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
        }

        internal void Init(Shape element, Color background, Color backGradient, Color foreground, Font textFont)
        {
            shape = element;
            txtName.BackColor = Color.FromArgb((background.R + backGradient.R) / 2, (background.G + backGradient.G) / 2, (background.B + backGradient.B) / 2);
            txtName.ForeColor = foreground;
            txtName.Text = shape.Entity.Name;

            txtName.Font = new Font(textFont.FontFamily, textFont.SizeInPoints * shape.Diagram.Zoom, textFont.Style);
        }

        internal void Relocate(Shape element, Rectangle absoluteBounds)
        {
            IDiagram diagram = shape.Diagram;

            if (diagram == null) return;

            // The following lines are required because of a .NET bug:
            // http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=380085
            if (!MonoHelper.IsRunningOnMono)
            {
                absoluteBounds.X -= 3;
                absoluteBounds.Width += 3;
            }

            this.SetBounds(
                (int)(absoluteBounds.X * diagram.Zoom) - diagram.Offset.X + ParentLocation.X,
                (int)(absoluteBounds.Y * diagram.Zoom) - diagram.Offset.Y + ParentLocation.Y,
                (int)(absoluteBounds.Width * diagram.Zoom),
                (int)(absoluteBounds.Height * diagram.Zoom));
        }

        private bool TryValidateData()
        {
            try
            {
                var newName = txtName.Text;
                var changeNameCommand = new ChangePropertyCommand<Shape, string>(shape, s => s.Entity.Name, (s, newValue) => s.Entity.Name = newValue, newName);
                changeNameCommand.Execute();
                shape.Diagram.TrackCommand(changeNameCommand);
                errorProvider.SetError(this, null);
                return true;
            }
            catch (Exception e)
            {
                errorProvider.SetError(this, e.Message);
                return false;
            }
        }

        private void txtComment_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (e.Modifiers == Keys.None || !txtName.Multiline)
                    {
                        shape.HideEditor();
                    }
                    else if (e.Modifiers == Keys.Control && txtName.Multiline)
                        this.txtName.AppendText(Environment.NewLine);
                    break;
                case Keys.Escape:
                    shape.HideEditor();
                    break;
            }
        }

        public override void Closing()
        {
            TryValidateData();
        }
    }
}
