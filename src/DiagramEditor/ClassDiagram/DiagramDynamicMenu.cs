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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NClass.Translations;
using NClass.Core;

namespace NClass.DiagramEditor.ClassDiagram
{
	public sealed partial class DiagramDynamicMenu : DynamicMenu
	{
		static DiagramDynamicMenu _default = new DiagramDynamicMenu();

		ToolStripMenuItem[] menuItems;
		Diagram diagram = null;

		private DiagramDynamicMenu()
		{
			InitializeComponent();
			UpdateTexts();
			
			menuItems = new ToolStripMenuItem[2] { mnuDiagram, mnuFormat };
		}

		public static DiagramDynamicMenu Default
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
				diagram = document as Diagram;
				diagram.SelectionChanged += new EventHandler(diagram_SelectionChanged);

				mnuNewStructure.Visible = diagram.Language.SupportsStructures;
				mnuNewDelegate.Visible = diagram.Language.SupportsDelegates;
				toolNewStructure.Visible = diagram.Language.SupportsStructures;
				toolNewDelegate.Visible = diagram.Language.SupportsDelegates;
				toolDelete.Enabled = diagram.HasSelectedElement;
			}
		}

		private void diagram_SelectionChanged(object sender, EventArgs e)
		{
			toolDelete.Enabled = (diagram != null && diagram.HasSelectedElement);
		}

		private void UpdateTexts()
		{
			// Diagram menu
			mnuDiagram.Text = Strings.MenuDiagram;
			mnuAddNewElement.Text = Strings.MenuNew;
			mnuNewClass.Text = Strings.MenuClass;
			mnuNewStructure.Text = Strings.MenuStruct;
			mnuNewInterface.Text = Strings.MenuInterface;
			mnuNewEnum.Text = Strings.MenuEnum;
			mnuNewDelegate.Text = Strings.MenuDelegate;
			mnuNewComment.Text = Strings.MenuComment;
			mnuNewAssociation.Text = Strings.MenuAssociation;
			mnuNewComposition.Text = Strings.MenuComposition;
			mnuNewAggregation.Text = Strings.MenuAggregation;
			mnuNewGeneralization.Text = Strings.MenuGeneralization;
			mnuNewRealization.Text = Strings.MenuRealization;
			mnuNewDependency.Text = Strings.MenuDependency;
			mnuNewNesting.Text = Strings.MenuNesting;
			mnuNewCommentRelationship.Text = Strings.MenuCommentRelationship;
			mnuMembersFormat.Text = Strings.MenuMembersFormat;
			mnuShowType.Text = Strings.MenuType;
			mnuShowParameters.Text = Strings.MenuParameters;
			mnuShowParameterNames.Text = Strings.MenuParameterNames;
			mnuShowInitialValue.Text = Strings.MenuInitialValue;
			mnuGenerateCode.Text = Strings.MenuGenerateCode;
			mnuSaveAsImage.Text = Strings.MenuSaveAsImage;

			// Format menu
			mnuFormat.Text = Strings.MenuFormat;
			mnuAlign.Text = Strings.MenuAlign;
			mnuAlignTop.Text = Strings.MenuAlignTop;
			mnuAlignLeft.Text = Strings.MenuAlignLeft;
			mnuAlignBottom.Text = Strings.MenuAlignBottom;
			mnuAlignRight.Text = Strings.MenuAlignRight;
			mnuAlignHorizontal.Text = Strings.MenuAlignHorizontal;
			mnuAlignVertical.Text = Strings.MenuAlignVertical;
			mnuMakeSameSize.Text = Strings.MenuMakeSameSize;
			mnuSameWidth.Text = Strings.MenuSameWidth;
			mnuSameHeight.Text = Strings.MenuSameHeight;
			mnuSameSize.Text = Strings.MenuSameSize;
			mnuAutoSize.Text = Strings.MenuAutoSize;
			mnuAutoWidth.Text = Strings.MenuAutoWidth;
			mnuAutoHeight.Text = Strings.MenuAutoHeight;
			mnuCollapseAll.Text = Strings.MenuCollapseAll;
			mnuExpandAll.Text = Strings.MenuExpandAll;

			// Toolbar
			toolNewClass.Text = Strings.AddNewClass;
			toolNewStructure.Text = Strings.AddNewStructure;
			toolNewInterface.Text = Strings.AddNewInterface;
			toolNewEnum.Text = Strings.AddNewEnum;
			toolNewDelegate.Text = Strings.AddNewDelegate;
			toolNewComment.Text = Strings.AddNewComment;
			toolNewAssociation.Text = Strings.AddNewAssociation;
			toolNewComposition.Text = Strings.AddNewComposition;
			toolNewAggregation.Text = Strings.AddNewAggregation;
			toolNewGeneralization.Text = Strings.AddNewGeneralization;
			toolNewRealization.Text = Strings.AddNewRealization;
			toolNewDependency.Text = Strings.AddNewDependency;
			toolNewNesting.Text = Strings.AddNewNesting;
			toolNewCommentRelationship.Text = Strings.AddNewCommentRelationship;
			toolDelete.Text = Strings.DeleteSelectedItems;
		}

		#region Event handlers

		private void mnuDiagram_DropDownOpening(object sender, EventArgs e)
		{
			bool hasContent = (diagram != null && !diagram.IsEmpty);
			mnuGenerateCode.Enabled = hasContent;
			mnuSaveAsImage.Enabled = hasContent;
		}

		private void mnuNewClass_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.CreateShape(EntityType.Class);
		}

		private void mnuNewStructure_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.CreateShape(EntityType.Structure);
		}

		private void mnuNewInterface_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.CreateShape(EntityType.Interface);
		}

		private void mnuNewEnum_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.CreateShape(EntityType.Enum);
		}

		private void mnuNewDelegate_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.CreateShape(EntityType.Delegate);
		}

		private void mnuNewComment_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.CreateShape(EntityType.Comment);
		}

		private void mnuNewAssociation_Click(object sender, EventArgs e)
		{
			diagram.CreateConnection(RelationshipType.Association);
			//TODO: toolNewAssociation.Checked = true;
		}

		private void mnuNewComposition_Click(object sender, EventArgs e)
		{
			diagram.CreateConnection(RelationshipType.Composition);
			//toolNewComposition.Checked = true;
		}

		private void mnuNewAggregation_Click(object sender, EventArgs e)
		{
			diagram.CreateConnection(RelationshipType.Aggregation);
			//toolNewAggregation.Checked = true;
		}

		private void mnuNewGeneralization_Click(object sender, EventArgs e)
		{
			diagram.CreateConnection(RelationshipType.Generalization);
			//toolNewGeneralization.Checked = true;
		}

		private void mnuNewRealization_Click(object sender, EventArgs e)
		{
			diagram.CreateConnection(RelationshipType.Realization);
			//toolNewRealization.Checked = true;
		}

		private void mnuNewDependency_Click(object sender, EventArgs e)
		{
			diagram.CreateConnection(RelationshipType.Dependency);
			//toolNewDependency.Checked = true;
		}

		private void mnuNewNesting_Click(object sender, EventArgs e)
		{
			diagram.CreateConnection(RelationshipType.Nesting);
			//toolNewNesting.Checked = true;
		}

		private void mnuNewCommentRelationship_Click(object sender, EventArgs e)
		{
			diagram.CreateConnection(RelationshipType.Comment);
			//toolNewCommentRelationship.Checked = true;
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
			DiagramEditor.Settings.Default.ShowType = ((ToolStripMenuItem) sender).Checked;
			if (diagram != null)
				diagram.Redraw();
		}

		private void mnuShowParameters_Click(object sender, EventArgs e)
		{
			DiagramEditor.Settings.Default.ShowParameters = ((ToolStripMenuItem) sender).Checked;
			if (diagram != null)
				diagram.Redraw();
		}

		private void mnuShowParameterNames_Click(object sender, EventArgs e)
		{
			DiagramEditor.Settings.Default.ShowParameterNames = ((ToolStripMenuItem) sender).Checked;
			if (diagram != null)
				diagram.Redraw();
		}

		private void mnuShowInitialValue_Click(object sender, EventArgs e)
		{
			DiagramEditor.Settings.Default.ShowInitialValue = ((ToolStripMenuItem) sender).Checked;
			if (diagram != null)
				diagram.Redraw();
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

		private void mnuSaveAsImage_Click(object sender, EventArgs e)
		{
			if (diagram != null && !diagram.IsEmpty)
				diagram.SaveAsImage();
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

		private void mnuAlignTop_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AlignTop();
		}

		private void mnuAlignLeft_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AlignLeft();
		}

		private void mnuAlignBottom_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AlignBottom();
		}

		private void mnuAlignRight_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AlignRight();
		}

		private void mnuAlignHorizontal_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AlignHorizontal();
		}

		private void mnuAlignVertical_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AlignVertical();
		}

		private void mnuSameWidth_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AdjustToSameWidth();
		}

		private void mnuSameHeight_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AdjustToSameHeight();
		}

		private void mnuSameSize_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AdjustToSameSize();
		}

		private void mnuAutoSize_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AutoSizeOfSelectedShapes();
		}

		private void mnuAutoWidth_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AutoWidthOfSelectedShapes();
		}

		private void mnuAutoHeight_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AutoHeightOfSelectedShapes();
		}

		private void mnuCollapseAll_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.CollapseAll();
		}

		private void mnuExpandAll_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.ExpandAll();
		}

		private void toolDelete_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.DeleteSelectedElements();
		}

		#endregion
	}
}