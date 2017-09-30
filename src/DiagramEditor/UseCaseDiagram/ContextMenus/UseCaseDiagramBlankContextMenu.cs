// NClass - Free class diagram editor
// Copyright (C) 2006-2007 Balazs Tihanyi
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
using System.Windows.Forms;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.ContextMenus;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Properties;
using NClass.Translations;

namespace NClass.DiagramEditor.UseCaseDiagram.ContextMenus
{
    public sealed class UseCaseDiagramBlankContextMenu : DiagramContextMenu
    {
        static UseCaseDiagramBlankContextMenu _default = new UseCaseDiagramBlankContextMenu();

        #region MenuItem fields
        ToolStripMenuItem mnuAddNewElement;
        ToolStripMenuItem mnuAddNewActor;
        ToolStripMenuItem mnuAddNewUseCase;
        ToolStripMenuItem mnuNewCommentRelationship;

        ToolStripMenuItem mnuPaste;
        ToolStripMenuItem mnuSaveAsImage;
        ToolStripMenuItem mnuSelectAll;
        #endregion

        private UseCaseDiagramBlankContextMenu()
        {
            InitMenuItems();
        }

        public static UseCaseDiagramBlankContextMenu Default
        {
            get { return _default; }
        }

        public override void ValidateMenuItems(IDiagram diagram)
        {
            if (diagram.DiagramType != DiagramType.UseCaseDiagram)
            {
                // TODO do this in a sane way
                throw new Exception("This is not a use case diagram");
            }

            base.ValidateMenuItems(diagram);
            mnuPaste.Enabled = diagram.CanPasteFromClipboard;

            mnuSaveAsImage.Enabled = !diagram.IsEmpty;
        }

        private void InitMenuItems()
        {
            mnuAddNewElement = new ToolStripMenuItem(Strings.MenuNew, Resources.NewElement);

            mnuAddNewActor = new ToolStripMenuItem(Strings.AddNewActor, Resources.Actor, mnuAddNewActor_Click);
            mnuAddNewUseCase = new ToolStripMenuItem(Strings.AddNewUseCase, Resources.UseCase, mnuAddNewUseCase_Click);
            mnuNewCommentRelationship = new ToolStripMenuItem(Strings.MenuCommentRelationship, Resources.CommentRel, mnuNewCommentRelationship_Click);

            mnuPaste = new ToolStripMenuItem(Strings.MenuPaste, Resources.Paste, mnuPaste_Click);
            mnuSaveAsImage = new ToolStripMenuItem(Strings.MenuSaveAsImage, Resources.Image, mnuSaveAsImage_Click);
            mnuSelectAll = new ToolStripMenuItem(Strings.MenuSelectAll, null, mnuSelectAll_Click);

            mnuAddNewElement.DropDownItems.AddRange(new ToolStripItem[] {
                mnuAddNewActor,
                mnuAddNewUseCase,
                new ToolStripSeparator(),
                mnuNewCommentRelationship
            });
            
            MenuList.AddRange(new ToolStripItem[] {
                mnuAddNewElement,
                new ToolStripSeparator(),
                mnuPaste,
                mnuSaveAsImage,
                mnuSelectAll
            });
        }

        private void mnuAddNewActor_Click(object sender, EventArgs e)
        {
            if (Diagram != null)
                Diagram.CreateShape(EntityType.Actor);
        }

        private void mnuAddNewUseCase_Click(object sender, EventArgs e)
        {
            if (Diagram != null)
                Diagram.CreateShape(EntityType.UseCase);
        }

        private void mnuNewCommentRelationship_Click(object sender, EventArgs e)
        {
            if (Diagram != null)
                Diagram.CreateConnection(RelationshipType.Comment);
        }

        private void mnuPaste_Click(object sender, EventArgs e)
        {
            if (Diagram != null)
                Diagram.Paste();
        }

        private void mnuSaveAsImage_Click(object sender, EventArgs e)
        {
            if (Diagram != null && !Diagram.IsEmpty)
                Diagram.SaveAsImage();
        }

        private void mnuSelectAll_Click(object sender, EventArgs e)
        {
            if (Diagram != null)
                Diagram.SelectAll();
        }
    }
}