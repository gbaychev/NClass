// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// Copyright (C) 2020 Georgi Baychev

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
using System.Windows.Forms;
using NClass.Core;
using NClass.DiagramEditor.Commands;
using NClass.DiagramEditor.Diagrams;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram.Dialogs
{
    public class EnumDialog : ListDialog
    {
        private IDiagram diagram;
        private EnumType parent = null;

        protected override void FillList()
        {
            lstItems.Items.Clear();
            foreach (EnumValue value in parent.Values)
            {
                ListViewItem item = lstItems.Items.Add(value.ToString());

                item.Tag = value;
                item.ImageIndex = Icons.EnumItemImageIndex;
            }
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="text"/> does not fit to the syntax.
        /// </exception>
        /// <exception cref="ReservedNameException">
        /// The <paramref name="text"/> contains a reserved name.
        /// </exception>
        protected override void AddToList(string text)
        {
            var command = new AddEnumMemberCommand(parent, text);
            command.Execute();
            diagram.TrackCommand(command);

            var value = command.EnumValue;
            var item = lstItems.Items.Add(value.ToString());

            item.Tag = value;
            item.ImageIndex = Icons.EnumItemImageIndex;
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="text"/> does not fit to the syntax.
        /// </exception>
        /// <exception cref="ReservedNameException">
        /// The <paramref name="text"/> contains a reserved name.
        /// </exception>
        protected override void Modify(ListViewItem item, string text)
        {
            if (!(item.Tag is EnumValue tag)) return;

            var command = new RenameEnumMemberCommand(tag, parent, text);
            command.Execute();
            diagram.TrackCommand(command);

            item.Tag = command.EnumValue;
            item.Text = command.EnumValue.ToString();
        }

        protected override void MoveUpItem(ListViewItem item)
        {
            if (item != null)
                parent.MoveUpItem(item.Tag);
            base.MoveUpItem(item);
        }

        protected override void MoveDownItem(ListViewItem item)
        {
            if (item != null)
                parent.MoveDownItem(item.Tag);
            base.MoveDownItem(item);
        }

        protected override void Remove(ListViewItem item)
        {
            if (item != null && item.Tag is EnumValue tag)
            {
                var command = new DeleteFromEnumTypeCommand(parent, tag);
                command.Execute();
                diagram.TrackCommand(command);
            }
            base.Remove(item);
        }

        public void ShowDialog(IDiagram diagram, EnumType parent)
        {
            if (parent == null || diagram == null) return;

            this.diagram = diagram;
            this.parent = parent;
            this.Text = string.Format(Strings.ItemsOfType, parent.Name);
            FillList();

            base.ShowDialog();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Z when e.Modifiers == Keys.Control:
                    diagram.Undo();
                    FillList();
                    break;
                case Keys.Y when e.Modifiers == Keys.Control:
                    diagram.Redo();
                    FillList();
                    break;
            }

            base.OnKeyDown(e);
        }
    }
}
