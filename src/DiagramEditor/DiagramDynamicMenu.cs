// // NClass - Free class diagram editor
// // Copyright (C) 2016 Georgi Baychev
// // 
// // This program is free software; you can redistribute it and/or modify it under 
// // the terms of the GNU General Public License as published by the Free Software 
// // Foundation; either version 3 of the License, or (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful, but WITHOUT 
// // ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// // FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
// //
// // You should have received a copy of the GNU General Public License along with 
// // this program; if not, write to the Free Software Foundation, Inc., 
// // 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NClass.Core;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Properties;
using NClass.Translations;

namespace NClass.DiagramEditor
{
    public abstract class DiagramDynamicMenu : DynamicMenu
    {
        protected IDiagram diagram;
        protected ToolStripMenuItem[] menuItems;
        protected ToolStrip elementsToolStrip;
        

        public override IEnumerable<ToolStripMenuItem> GetMenuItems()
        {
            return this.menuItems;
        }

        public override ToolStrip GetToolStrip()
        {
            return this.elementsToolStrip;
        }

        protected virtual void InitComponents()
        {
            #region The align submenu
            this.mnuAlignTop = new ToolStripMenuItem(Strings.MenuAlignTop, Resources.AlignTop, (s, e) => diagram?.AlignTop());
            this.mnuAlignLeft = new ToolStripMenuItem(Strings.MenuAlignLeft, Resources.AlignLeft, (s, e) => diagram?.AlignLeft());
            this.mnuAlignBottom = new ToolStripMenuItem(Strings.MenuAlignBottom, Resources.AlignBottom, (s, e) => diagram?.AlignBottom());
            this.mnuAlignRight = new ToolStripMenuItem(Strings.MenuAlignRight, Resources.AlignRight, (s, e) => diagram?.AlignRight());
            this.mnuAlignHorizontal = new ToolStripMenuItem(Strings.MenuAlignHorizontal, Resources.AlignHorizontal, (s, e) => diagram?.AlignHorizontal());
            this.mnuAlignVertical = new ToolStripMenuItem(Strings.MenuAlignVertical, Resources.AlignVertical, (s, e) => diagram?.AlignVertical());
            this.mnuNewComment = new ToolStripMenuItem(Strings.AddNewComment, Resources.Comment, (o, e) => diagram?.CreateShape(EntityType.Comment));
            this.mnuNewCommentRelationship = new ToolStripMenuItem(Strings.AddNewCommentRelationship, Resources.CommentRel, (o, e) => diagram?.CreateConnection(RelationshipType.Comment));

            this.mnuAlign = new ToolStripMenuItem(Strings.MenuAlign, null, this.mnuAlignTop,
                                                                           this.mnuAlignLeft,
                                                                           this.mnuAlignBottom,
                                                                           this.mnuAlignRight,
                                                                           new ToolStripSeparator(),
                                                                           this.mnuAlignHorizontal,
                                                                           this.mnuAlignVertical);
            #endregion
            #region The 'make same size' submenu
            this.mnuSameWidth = new ToolStripMenuItem(Strings.MenuSameWidth, null, (s, e) => diagram?.AdjustToSameWidth());
            this.mnuSameHeight = new ToolStripMenuItem(Strings.MenuSameHeight, null, (s, e) => diagram?.AdjustToSameHeight());
            this.mnuSameSize = new ToolStripMenuItem(Strings.MenuSize, null, (s, e) => diagram?.AdjustToSameSize());
            this.mnuMakeSameSize = new ToolStripMenuItem(Strings.MenuMakeSameSize, null, mnuSameSize, mnuSameWidth, mnuSameHeight);
            #endregion
            #region The 'Auto size region'
            this.mnuAutoSize = new ToolStripMenuItem(Strings.MenuAutoSize, null, (s, e) => diagram?.AutoSizeOfSelectedShapes());
            this.mnuAutoWidth = new ToolStripMenuItem(Strings.MenuAutoWidth, null, (s, e) => diagram?.AutoWidthOfSelectedShapes());
            this.mnuAutoHeight = new ToolStripMenuItem(Strings.MenuAutoHeight, null, (s, e) => diagram?.AutoHeightOfSelectedShapes());
            #endregion
            #region Diagram submenus 
            this.mnuNewElement = new ToolStripMenuItem(Strings.MenuNew, Resources.NewElement);
            this.mnuSaveAsImage = new ToolStripMenuItem(Strings.MenuSaveAsImage, Resources.Image, (o, e) =>
            { 
                if (diagram != null && !diagram.IsEmpty)
                    diagram.SaveAsImage();
            });
            #endregion
            #region Top Menu Items
            this.mnuDiagram = new ToolStripMenuItem(Strings.MenuDiagram, null, this.mnuNewElement,
                                                                               new ToolStripSeparator(),
                                                                               this.mnuSaveAsImage);

            this.mnuFormat = new ToolStripMenuItem(Strings.MenuFormat, null, this.mnuAlign, 
                                                                             this.mnuMakeSameSize,
                                                                             new ToolStripSeparator(),
                                                                             this.mnuAutoSize,
                                                                             this.mnuAutoWidth,
                                                                             this.mnuAutoHeight);
            #endregion
            #region ToolStrip items
            this.elementsToolStrip = new ToolStrip();
            this.toolDelete = new ToolStripButton(Strings.Delete, Resources.Delete, (o, e) => diagram?.DeleteSelectedElements());
            this.toolNewComment = new ToolStripButton(Strings.AddNewComment, Resources.Comment, (o, e) => diagram?.CreateShape(EntityType.Comment)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            this.toolNewCommentRelationship = new ToolStripButton(Strings.AddNewCommentRelationship, Resources.CommentRel, (o, e) => diagram?.CreateConnection(RelationshipType.Comment)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            #endregion
        }

        protected virtual void diagram_SelectionChanged(object sender, EventArgs e)
        {
            toolDelete.Enabled = (diagram != null && diagram.HasSelectedElement);
        }

        #region Menu Items Declarations
        protected ToolStripMenuItem mnuAlign;
        protected ToolStripMenuItem mnuAlignTop;
        protected ToolStripMenuItem mnuAlignLeft;
        protected ToolStripMenuItem mnuAlignBottom;
        protected ToolStripMenuItem mnuAlignRight;
        protected ToolStripMenuItem mnuAlignHorizontal;
        protected ToolStripMenuItem mnuAlignVertical;
        protected ToolStripMenuItem mnuSameSize;
        protected ToolStripMenuItem mnuSameWidth;
        protected ToolStripMenuItem mnuSameHeight;
        protected ToolStripMenuItem mnuMakeSameSize;
        protected ToolStripMenuItem mnuAutoSize;
        protected ToolStripMenuItem mnuAutoWidth;
        protected ToolStripMenuItem mnuAutoHeight;
        protected ToolStripMenuItem mnuFormat;
        protected ToolStripMenuItem mnuNewElement;
        protected ToolStripMenuItem mnuSaveAsImage;
        protected ToolStripMenuItem mnuDiagram;
        protected ToolStripMenuItem mnuNewCommentRelationship;
        protected ToolStripMenuItem mnuNewComment;
        #endregion
        #region Toolstrip Items Declaration
        protected ToolStripButton toolNewComment;
        protected ToolStripButton toolNewCommentRelationship;
        protected ToolStripButton toolDelete;
#endregion
    }
}