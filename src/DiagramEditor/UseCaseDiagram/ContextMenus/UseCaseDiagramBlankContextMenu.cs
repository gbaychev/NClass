// NClass - Free class diagram editor
// Copyright (C) 2006-2007 Balazs Tihanyi
// Copyright (C) 2016 - 2018 Georgi Baychev
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
        ToolStripMenuItem mnuNewActor;
        ToolStripMenuItem mnuNewUseCase;
        ToolStripMenuItem mnuNewComment;
        ToolStripMenuItem mnuNewAssociation;
        ToolStripMenuItem mnuNewExtends;
        ToolStripMenuItem mnuNewIncludes;
        ToolStripMenuItem mnuNewGeneralization;
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

            mnuNewActor = new ToolStripMenuItem(Strings.AddNewActor, Resources.Actor, (s, e) => Diagram?.CreateShape(EntityType.Actor));
            mnuNewUseCase = new ToolStripMenuItem(Strings.AddNewUseCase, Resources.UseCase, (s, e) => Diagram?.CreateShape(EntityType.UseCase));
            mnuNewComment = new ToolStripMenuItem(Strings.MenuComment, Resources.Comment, (s, e) => Diagram?.CreateShape(EntityType.Comment));
            mnuNewAssociation = new ToolStripMenuItem(Strings.MenuAssociation, Resources.Association, (s, e) => Diagram?.CreateConnection(RelationshipType.UseCaseAssociation));
            mnuNewIncludes = new ToolStripMenuItem(Strings.AddNewIncludes, Resources.Includes, (s, e) => Diagram?.CreateConnection(RelationshipType.Inclusion));
            mnuNewExtends = new ToolStripMenuItem(Strings.AddNewExtends, Resources.Extends, (s, e) => Diagram?.CreateConnection(RelationshipType.Extension));
            mnuNewGeneralization = new ToolStripMenuItem(Strings.MenuGeneralization, Resources.Generalization, (s, e) => Diagram?.CreateConnection(RelationshipType.UseCaseGeneralization));
            mnuNewCommentRelationship = new ToolStripMenuItem(Strings.MenuCommentRelationship, Resources.CommentRel, (s, e) => Diagram?.CreateConnection(RelationshipType.Comment));

            mnuPaste = new ToolStripMenuItem(Strings.MenuPaste, Resources.Paste, (s, e) => Diagram?.Paste());
            mnuSaveAsImage = new ToolStripMenuItem(Strings.MenuSaveAsImage, Resources.Image, (s, e) => Diagram?.SaveAsImage());
            mnuSelectAll = new ToolStripMenuItem(Strings.MenuSelectAll, null, (s, e) => Diagram?.SelectAll());

            mnuAddNewElement.DropDownItems.AddRange(new ToolStripItem[] {
                mnuNewActor,
                mnuNewUseCase,
                mnuNewComment,
                new ToolStripSeparator(),
                mnuNewAssociation,
                mnuNewIncludes,
                mnuNewExtends,
                mnuNewGeneralization,
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
    }
}