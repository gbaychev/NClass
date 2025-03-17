﻿// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// Copyright (C) 2025 Georgi Baychev
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
using System.Windows.Forms;
using NClass.Translations;
using NClass.Core;
using NClass.DiagramEditor.Properties;

namespace NClass.DiagramEditor.ClassDiagram
{
    public sealed class ClassDiagramDynamicMenu : DiagramDynamicMenu
    {
        static ClassDiagramDynamicMenu _default = new ClassDiagramDynamicMenu();

        #region 'New' Submenu Items
        private ToolStripMenuItem mnuNewClass;
        private ToolStripMenuItem mnuNewPackage;
        private ToolStripMenuItem mnuNewStructure;
        private ToolStripMenuItem mnuNewInterface;
        private ToolStripMenuItem mnuNewEnum;
        private ToolStripMenuItem mnuNewDelegate;
        private ToolStripMenuItem mnuNewAssociation;
        private ToolStripMenuItem mnuNewComposition;
        private ToolStripMenuItem mnuNewAggregation;
        private ToolStripMenuItem mnuNewGeneralization;
        private ToolStripMenuItem mnuNewRealization;
        private ToolStripMenuItem mnuNewDependency;
        private ToolStripMenuItem mnuNewNesting;

        private ToolStripMenuItem mnuMembersFormat;
        private ToolStripMenuItem mnuShowType;
        private ToolStripMenuItem mnuShowParameters;
        private ToolStripMenuItem mnuShowParameterNames;
        private ToolStripMenuItem mnuShowInitialValue;

        private ToolStripMenuItem mnuGenerateCode;

        private ToolStripMenuItem mnuCollapseAll;
        private ToolStripMenuItem mnuExpandAll;

        private ToolStripButton toolNewClass;
        private ToolStripButton toolNewPackage;
        private ToolStripButton toolNewStructure;
        private ToolStripButton toolNewInterface;
        private ToolStripButton toolNewEnum;
        private ToolStripButton toolNewDelegate;
        private ToolStripButton toolNewAssociation;
        private ToolStripButton toolNewComposition;
        private ToolStripButton toolNewAggregation;
        private ToolStripButton toolNewGeneralization;
        private ToolStripButton toolNewRealization;
        private ToolStripButton toolNewDependency;
        private ToolStripButton toolNewNesting;
        #endregion

        #region Toolstrip Items
        #endregion

        private ClassDiagramDynamicMenu()
        {
            this.InitComponents();

            this.menuItems = new[] { mnuDiagram, mnuFormat };
        }

        public static ClassDiagramDynamicMenu Default
        {
            get { return _default; }
        }

        public override IEnumerable<ToolStripMenuItem> GetMenuItems()
        {
            return menuItems;
        }

        public override ToolStrip GetToolStrip()
        {
            return elementsToolStrip;
        }

        public override void SetReference(IDocument document)
        {
            if (diagram != null)
            {
                diagram.SelectionChanged -= new EventHandler(diagram_SelectionChanged);
            }

            if (document == null)
            {
                diagram = null;
            }
            else
            {
                diagram = (ClassDiagram)document;
                diagram.SelectionChanged += new EventHandler(diagram_SelectionChanged);

                mnuNewStructure.Visible = ((ClassDiagram)diagram).Language.SupportsStructures;
                mnuNewDelegate.Visible = ((ClassDiagram)diagram).Language.SupportsDelegates;
                toolNewStructure.Visible = ((ClassDiagram)diagram).Language.SupportsStructures;
                toolNewDelegate.Visible = ((ClassDiagram)diagram).Language.SupportsDelegates;
                toolDelete.Enabled = diagram.HasSelectedElement;
            }
        }

        protected override void InitComponents()
        {
            base.InitComponents();

            this.mnuNewClass = new ToolStripMenuItem(Strings.AddNewClass, Resources.Class, (o, e) => diagram?.CreateShape(EntityType.Class));
            this.mnuNewPackage = new ToolStripMenuItem(Strings.AddNewPackage, Resources.Package, (o, e) => diagram?.CreateShape(EntityType.Package));
            this.mnuNewStructure = new ToolStripMenuItem(Strings.AddNewStructure, Resources.Structure, (o, e) => diagram?.CreateShape(EntityType.Structure));
            this.mnuNewInterface = new ToolStripMenuItem(Strings.AddNewInterface, Resources.Interface32, (o, e) => diagram?.CreateShape(EntityType.Interface));
            this.mnuNewEnum = new ToolStripMenuItem(Strings.AddNewEnum, Resources.Enum, (o, e) => diagram?.CreateShape(EntityType.Enum));
            this.mnuNewDelegate = new ToolStripMenuItem(Strings.AddNewDelegate, Resources.Delegate, (o, e) => diagram?.CreateShape(EntityType.Delegate));
            this.mnuNewAssociation = new ToolStripMenuItem(Strings.AddNewAssociation, Resources.Association, (o, e) => diagram?.CreateConnection(RelationshipType.Association));
            this.mnuNewComposition = new ToolStripMenuItem(Strings.AddNewComposition, Resources.Composition, (o, e) => diagram?.CreateConnection(RelationshipType.Composition));
            this.mnuNewAggregation = new ToolStripMenuItem(Strings.AddNewAggregation, Resources.Aggregation, (o, e) => diagram?.CreateConnection(RelationshipType.Aggregation));
            this.mnuNewGeneralization = new ToolStripMenuItem(Strings.AddNewGeneralization, Resources.Generalization, (o, e) => diagram?.CreateConnection(RelationshipType.Generalization));
            this.mnuNewRealization = new ToolStripMenuItem(Strings.AddNewRealization, Resources.Realization, (o, e) => diagram?.CreateConnection(RelationshipType.Realization)); ;
            this.mnuNewDependency = new ToolStripMenuItem(Strings.AddNewDependency, Resources.Dependency, (o, e) => diagram?.CreateConnection(RelationshipType.Dependency));
            this.mnuNewNesting = new ToolStripMenuItem(Strings.AddNewNesting, Resources.Nesting, (o, e) => diagram?.CreateConnection(RelationshipType.Nesting));


            this.mnuShowType = new ToolStripMenuItem(Strings.MenuType, null, mnuShowType_Click);
            this.mnuShowParameters = new ToolStripMenuItem(Strings.MenuParameters, null, mnuShowParameters_Click);
            this.mnuShowParameterNames = new ToolStripMenuItem(Strings.MenuParameterNames, null, mnuShowParameterNames_Click);
            this.mnuShowInitialValue = new ToolStripMenuItem(Strings.InitialValue, null, mnuShowInitialValue_Click);
            this.mnuMembersFormat = new ToolStripMenuItem(Strings.MenuMembersFormat, Resources.Format,
                                                                                     this.mnuShowType,
                                                                                     this.mnuShowParameters,
                                                                                     this.mnuShowParameterNames,
                                                                                     this.mnuShowInitialValue);
            this.mnuMembersFormat.DropDownOpening += mnuMembersFormat_DropDownOpening;

            this.mnuGenerateCode = new ToolStripMenuItem(Strings.MenuGenerateCode, Resources.CodeGenerator, mnuGenerateCode_Click);

            this.mnuCollapseAll = new ToolStripMenuItem(Strings.MenuCollapseAll, Resources.CollapseAll, (o, e) => diagram?.CollapseAll());
            this.mnuExpandAll = new ToolStripMenuItem(Strings.MenuExpandAll, Resources.ExpandAll, (o, e) => diagram?.ExpandAll());

            this.mnuNewElement.DropDownItems.Add(mnuNewClass);
            this.mnuNewElement.DropDownItems.Add(mnuNewPackage);
            this.mnuNewElement.DropDownItems.Add(mnuNewStructure);
            this.mnuNewElement.DropDownItems.Add(mnuNewInterface);
            this.mnuNewElement.DropDownItems.Add(mnuNewEnum);
            this.mnuNewElement.DropDownItems.Add(mnuNewDelegate);
            this.mnuNewElement.DropDownItems.Add(mnuNewComment);
            this.mnuNewElement.DropDownItems.Add(new ToolStripSeparator());
            this.mnuNewElement.DropDownItems.Add(mnuNewAssociation);
            this.mnuNewElement.DropDownItems.Add(mnuNewComposition);
            this.mnuNewElement.DropDownItems.Add(mnuNewAggregation);
            this.mnuNewElement.DropDownItems.Add(mnuNewGeneralization);
            this.mnuNewElement.DropDownItems.Add(mnuNewRealization);
            this.mnuNewElement.DropDownItems.Add(mnuNewDependency);
            this.mnuNewElement.DropDownItems.Add(mnuNewNesting);
            this.mnuNewElement.DropDownItems.Add(mnuNewCommentRelationship);


            this.mnuDiagram.DropDownItems.Insert(1, mnuMembersFormat);
            this.mnuDiagram.DropDownItems.Insert(2, new ToolStripSeparator());
            this.mnuDiagram.DropDownItems.Insert(3, mnuGenerateCode);
            this.mnuDiagram.DropDownOpening += mnuDiagram_DropDownOpening;

            this.mnuFormat.DropDownItems.Add(new ToolStripSeparator());
            this.mnuFormat.DropDownItems.Add(mnuCollapseAll);
            this.mnuFormat.DropDownItems.Add(mnuExpandAll);
            this.mnuFormat.DropDownOpening += mnuFormat_DropDownOpening;

            this.toolNewClass = new ToolStripButton(Strings.AddNewClass, Resources.Class, (o, e) => diagram?.CreateShape(EntityType.Class)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            this.toolNewPackage = new ToolStripButton(Strings.AddNewPackage, Resources.Package, (o, e) => diagram?.CreateShape(EntityType.Package)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            this.toolNewStructure = new ToolStripButton(Strings.AddNewStructure, Resources.Structure, (o, e) => diagram?.CreateShape(EntityType.Structure)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            this.toolNewInterface = new ToolStripButton(Strings.AddNewInterface, Resources.Interface32, (o, e) => diagram?.CreateShape(EntityType.Interface)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            this.toolNewEnum = new ToolStripButton(Strings.AddNewEnum, Resources.Enum, (o, e) => diagram?.CreateShape(EntityType.Enum)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            this.toolNewDelegate = new ToolStripButton(Strings.AddNewDelegate, Resources.Delegate, (o, e) => diagram?.CreateShape(EntityType.Delegate)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            this.toolNewAssociation = new ToolStripButton(Strings.AddNewAssociation, Resources.Association, (o, e) => diagram?.CreateConnection(RelationshipType.Association)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            this.toolNewComposition = new ToolStripButton(Strings.AddNewComposition, Resources.Composition, (o, e) => diagram?.CreateConnection(RelationshipType.Composition)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            this.toolNewAggregation = new ToolStripButton(Strings.AddNewAggregation, Resources.Aggregation, (o, e) => diagram?.CreateConnection(RelationshipType.Aggregation)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            this.toolNewGeneralization = new ToolStripButton(Strings.AddNewGeneralization, Resources.Generalization, (o, e) => diagram?.CreateConnection(RelationshipType.Generalization)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            this.toolNewRealization = new ToolStripButton(Strings.AddNewRealization, Resources.Realization, (o, e) => diagram?.CreateConnection(RelationshipType.Realization)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            this.toolNewDependency = new ToolStripButton(Strings.AddNewDependency, Resources.Dependency, (o, e) => diagram?.CreateConnection(RelationshipType.Dependency)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            this.toolNewNesting = new ToolStripButton(Strings.AddNewNesting, Resources.Nesting, (o, e) => diagram?.CreateConnection(RelationshipType.Nesting)) { DisplayStyle = ToolStripItemDisplayStyle.Image };

            this.elementsToolStrip.Items.Add(this.toolNewClass);
            this.elementsToolStrip.Items.Add(this.toolNewPackage);
            this.elementsToolStrip.Items.Add(this.toolNewStructure);
            this.elementsToolStrip.Items.Add(this.toolNewInterface);
            this.elementsToolStrip.Items.Add(this.toolNewEnum);
            this.elementsToolStrip.Items.Add(this.toolNewDelegate);
            this.elementsToolStrip.Items.Add(this.toolNewComment);
            this.elementsToolStrip.Items.Add(this.toolNewAssociation);
            this.elementsToolStrip.Items.Add(this.toolNewComposition);
            this.elementsToolStrip.Items.Add(this.toolNewAggregation);
            this.elementsToolStrip.Items.Add(this.toolNewGeneralization);
            this.elementsToolStrip.Items.Add(this.toolNewRealization);
            this.elementsToolStrip.Items.Add(this.toolNewDependency);
            this.elementsToolStrip.Items.Add(this.toolNewNesting);
            this.elementsToolStrip.Items.Add(this.toolNewCommentRelationship);
        }

        #region Event handlers

        private void mnuDiagram_DropDownOpening(object sender, EventArgs e)
        {
            bool hasContent = (diagram != null && !diagram.IsEmpty && diagram is ClassDiagram);
            mnuGenerateCode.Enabled = hasContent;
            mnuSaveAsImage.Enabled = hasContent;
        }

        private void mnuMembersFormat_DropDownOpening(object sender, EventArgs e)
        {
            mnuShowType.Checked = DiagramEditor.Settings.Default.ShowType;
            mnuShowParameters.Checked = DiagramEditor.Settings.Default.ShowParameters;
            mnuShowParameterNames.Checked = DiagramEditor.Settings.Default.ShowParameterNames;
            mnuShowInitialValue.Checked = DiagramEditor.Settings.Default.ShowInitialValue;
        }

        private void mnuShowType_Click(object sender, EventArgs e)
        {
            DiagramEditor.Settings.Default.ShowType = ((ToolStripMenuItem)sender).Checked;
            diagram?.Redraw();
        }

        private void mnuShowParameters_Click(object sender, EventArgs e)
        {
            DiagramEditor.Settings.Default.ShowParameters = ((ToolStripMenuItem)sender).Checked;
            diagram?.Redraw();
        }

        private void mnuShowParameterNames_Click(object sender, EventArgs e)
        {
            DiagramEditor.Settings.Default.ShowParameterNames = ((ToolStripMenuItem)sender).Checked;
            diagram?.Redraw();
        }

        private void mnuShowInitialValue_Click(object sender, EventArgs e)
        {
            DiagramEditor.Settings.Default.ShowInitialValue = ((ToolStripMenuItem)sender).Checked;
            diagram?.Redraw();
        }

        private void mnuGenerateCode_Click(object sender, EventArgs e)
        {
            if (diagram != null && diagram.Project != null)
            {
                using (CodeGenerator.Dialog dialog = new CodeGenerator.Dialog())
                {
                    try
                    {
                        dialog.ShowDialog(diagram.Project);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Strings.UnknownError,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void mnuFormat_DropDownOpening(object sender, EventArgs e)
        {
            bool shapeSelected = (diagram != null && diagram.SelectedShapeCount >= 1);
            bool multiselection = (diagram != null && diagram.SelectedShapeCount >= 2);

            mnuAutoWidth.Enabled = shapeSelected;
            mnuAutoHeight.Enabled = shapeSelected;
            mnuAlign.Enabled = multiselection;
            mnuMakeSameSize.Enabled = multiselection;
        }
        #endregion
    }
}
