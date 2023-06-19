// NClass - Free class diagram editor
// Copyright (C) 2023 Georgi Baychev
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

using System.ComponentModel;
using System.Windows.Forms;

namespace NClass.DiagramEditor.ClassDiagram.Editors
{
    public class TabTextBox : TextBox
    {
        [DefaultValue(true)]
        public bool AllowSelectAll { get; set; }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && AcceptsReturn)
            {
                OnKeyDown(new KeyEventArgs(keyData));
                return true;
            }
            else if (keyData == Keys.Tab && AcceptsTab)
            {
                OnKeyDown(new KeyEventArgs(keyData));
                return true;
            }
            else if (keyData == (Keys.A | Keys.Control) && AllowSelectAll)
            {
                SelectAll();
                return true;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }
    }
}