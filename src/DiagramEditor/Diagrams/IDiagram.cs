// NClass - Free class diagram editor
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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Connections;
using NClass.DiagramEditor.Diagrams.Connections;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.Diagrams
{
    public interface IDiagram : IDocument
    {
        IEnumerable<Shape> Shapes { get; }
        IEnumerable<Connection> Connections { get; }
        DiagramType DiagramType { get; }
        int SelectedElementCount { get; }
        DiagramElement TopSelectedElement { get; }
        void ShowWindow(PopupWindow window);
        void HideWindow(PopupWindow window);
        IEnumerable<Connection> GetSelectedConnections();
        void CreateShape(EntityType type, Point? where = null);
        void CreateShapeAt(EntityType type, Point where);
        void CreateConnection(RelationshipType type);
        void SaveAsImage();
        void SaveAsImage(bool selectedOnly);
        void CopyAsImage();
        void CopyAsImage(bool selectedOnly);
        void AlignTop();
        void AlignLeft();
        void AlignBottom();
        void AlignRight();
        void AlignHorizontal();
        void AlignVertical();
        void AdjustToSameWidth();
        void AdjustToSameHeight();
        void AdjustToSameSize();
        void CollapseAll();
        void CollapseAll(bool selectedOnly);
        void ExpandAll();
        void ExpandAll(bool selectedOnly);
        void AutoSizeOfSelectedShapes();
        void AutoWidthOfSelectedShapes();
        void AutoHeightOfSelectedShapes();
        int SelectedShapeCount { get; }
        event EventHandler SelectionChanged;
        CommentRelationship AddCommentRelationship(Comment comment, IEntity entity);
        IEnumerable<Shape> GetShapesInDisplayOrder();
    }
}