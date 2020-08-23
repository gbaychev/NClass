// NClass - Free class diagram editor
// Copyright (C) 2006-2007 Balazs Tihanyi
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
using NClass.Core;
using NClass.DiagramEditor.Properties;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using NClass.DiagramEditor.Diagrams;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram.ContextMenus
{
    internal sealed class PackageShapeContextMenu : DiagramContextMenu
    {
        ToolStripMenuItem mnuAddNewElement;
        ToolStripMenuItem mnuNewPackage;
        ToolStripMenuItem mnuNewClass;
        ToolStripMenuItem mnuNewStructure;
        ToolStripMenuItem mnuNewInterface;
        ToolStripMenuItem mnuNewEnum;
        ToolStripMenuItem mnuNewDelegate;
        ToolStripMenuItem mnuNewComment;
        ToolStripMenuItem mnuEditPackage;

        private PackageShapeContextMenu()
        {
            InitMenuItems();
        }

        public static PackageShapeContextMenu Default { get; } = new PackageShapeContextMenu();

        public override void ValidateMenuItems(IDiagram diagram)
        {
            base.ValidateMenuItems(diagram);
            var classDiagram = (ClassDiagram) diagram;
            ShapeContextMenu.Default.ValidateMenuItems(diagram);
            mnuNewDelegate.Visible = classDiagram.Language.SupportsDelegates;
            mnuNewInterface.Visible = classDiagram.Language.SupportsInterfaces;
            mnuEditPackage.Enabled = (diagram.SelectedElementCount == 1);
        }

        private void InitMenuItems()
        {
            mnuAddNewElement = new ToolStripMenuItem(Strings.MenuNew, Resources.NewElement);
            mnuNewPackage = new ToolStripMenuItem(Strings.MenuPackage, Resources.Package, mnuNewPackage_Click);
            mnuNewClass = new ToolStripMenuItem(Strings.MenuClass, Resources.Class, mnuNewClass_Click);
            mnuNewStructure = new ToolStripMenuItem(Strings.MenuStruct, Resources.Structure, mnuNewStructure_Click);
            mnuNewInterface = new ToolStripMenuItem(Strings.MenuInterface, Resources.Interface32, mnuNewInterface_Click);
            mnuNewEnum = new ToolStripMenuItem(Strings.MenuEnum, Resources.Enum, mnuNewEnum_Click);
            mnuNewDelegate = new ToolStripMenuItem(Strings.MenuDelegate, Resources.Delegate, mnuNewDelegate_Click);
            mnuNewComment = new ToolStripMenuItem(Strings.MenuComment, Resources.Comment, mnuNewComment_Click);

            mnuAddNewElement.DropDownItems.AddRange(new ToolStripItem[] {
                mnuNewPackage,
                mnuNewClass,
                mnuNewStructure,
                mnuNewInterface,
                mnuNewEnum,
                mnuNewDelegate,
                mnuNewComment});

            mnuEditPackage = new ToolStripMenuItem(
                Strings.MenuEditPackage,
                Resources.EditComment, mnuEditPackage_Click);

            MenuList.Add(mnuAddNewElement);
            MenuList.Add(new ToolStripSeparator());
            MenuList.AddRange(ShapeContextMenu.Default.MenuItems);
            MenuList.AddRange(new ToolStripItem[] {
                new ToolStripSeparator(),
                mnuEditPackage,
            });
        }
        
        private void mnuEditPackage_Click(object sender, EventArgs e)
        {
            if (Diagram != null)
            {
                PackageShape packageShape = Diagram.TopSelectedElement as PackageShape;
                packageShape?.EditText();
            }
        }

        private void mnuNewPackage_Click(object sender, EventArgs e)
        {
            Diagram?.CreateShapeAt(EntityType.Package, new Point((int)menuPosition.Value.X, (int)menuPosition.Value.Y));
        }

        private void mnuNewClass_Click(object sender, EventArgs e)
        {
            Diagram?.CreateShapeAt(EntityType.Class, new Point((int)menuPosition.Value.X, (int)menuPosition.Value.Y));
        }

        private void mnuNewStructure_Click(object sender, EventArgs e)
        {
            Diagram?.CreateShapeAt(EntityType.Structure, new Point((int)menuPosition.Value.X, (int)menuPosition.Value.Y));
        }

        private void mnuNewInterface_Click(object sender, EventArgs e)
        {
            Diagram?.CreateShapeAt(EntityType.Interface, new Point((int)menuPosition.Value.X, (int)menuPosition.Value.Y));
        }

        private void mnuNewEnum_Click(object sender, EventArgs e)
        {
            Diagram?.CreateShapeAt(EntityType.Enum, new Point((int)menuPosition.Value.X, (int)menuPosition.Value.Y));
        }

        private void mnuNewDelegate_Click(object sender, EventArgs e)
        {
            Diagram?.CreateShapeAt(EntityType.Delegate, new Point((int)menuPosition.Value.X, (int)menuPosition.Value.Y));
        }

        private void mnuNewComment_Click(object sender, EventArgs e)
        {
            Diagram?.CreateShapeAt(EntityType.Comment, new Point((int)menuPosition.Value.X, (int)menuPosition.Value.Y));
        }
    }
}