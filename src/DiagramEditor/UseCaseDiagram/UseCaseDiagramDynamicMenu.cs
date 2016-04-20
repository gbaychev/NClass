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
using System.Windows.Forms;
using NClass.Core;
using NClass.DiagramEditor.Properties;
using NClass.Translations;

namespace NClass.DiagramEditor.UseCaseDiagram
{
    public partial class UseCaseDiagramDynamicMenu : DiagramDynamicMenu
    {
        static UseCaseDiagramDynamicMenu _default = new UseCaseDiagramDynamicMenu();

        #region 'New' Submenu Items
        private ToolStripMenuItem mnuNewUseCase;
        private ToolStripMenuItem mnuAddNewActor;
        private ToolStripMenuItem mnuAddNewAssociation;
        private ToolStripMenuItem mnuAddNewExtends;
        private ToolStripMenuItem mnuAddNewIncludes;
        #endregion

        #region Toolstrip Items
        private ToolStripButton toolNewUseCase;
        private ToolStripButton toolNewActor;
        private ToolStripButton toolNewAssociation;
        private ToolStripButton toolNewExtends;
        private ToolStripButton toolNewIncludes;
        #endregion

        private UseCaseDiagramDynamicMenu()
        {
            InitComponents();

            this.menuItems = new[] { mnuDiagram, mnuFormat };
        }

        public static UseCaseDiagramDynamicMenu Default
        {
            get
            {
                if(_default == null)
                    _default = new UseCaseDiagramDynamicMenu();
                return _default;
            }
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
                // TODO do this in a sane way
                diagram = document as UseCaseDiagram;
                if (diagram == null)
                {
                    throw new Exception("This is not a use case diagram");
                }
                diagram.SelectionChanged += new EventHandler(diagram_SelectionChanged);

            };
        }

        protected sealed override void InitComponents()
        {
            base.InitComponents();

            this.mnuNewUseCase = new ToolStripMenuItem(Strings.AddNewUseCase, Resources.UseCase, (o,e) => diagram?.CreateShape(EntityType.UseCase));
            this.mnuAddNewActor = new ToolStripMenuItem(Strings.AddNewActor, Resources.Actor, (o, e) => diagram?.CreateShape(EntityType.Actor));
            this.mnuAddNewAssociation = new ToolStripMenuItem(Strings.AddNewAssociation, Resources.Association, (o, e) => diagram?.CreateConnection(RelationshipType.Association));
            this.mnuAddNewExtends = new ToolStripMenuItem(Strings.AddNewExtends, Resources.Extends, (o, e) => diagram?.CreateConnection(RelationshipType.Extension));
            this.mnuAddNewIncludes = new ToolStripMenuItem(Strings.AddNewIncludes, Resources.Includes, (o, e) => diagram?.CreateConnection(RelationshipType.Inclusion));

            this.mnuNewElement.DropDownItems.Add(this.mnuNewUseCase);
            this.mnuNewElement.DropDownItems.Add(this.mnuAddNewActor);
            this.mnuNewElement.DropDownItems.Add(new ToolStripSeparator());
            this.mnuNewElement.DropDownItems.Add(this.mnuAddNewAssociation);
            this.mnuNewElement.DropDownItems.Add(this.mnuAddNewExtends);
            this.mnuNewElement.DropDownItems.Add(this.mnuAddNewIncludes);

            this.toolNewUseCase = new ToolStripButton(Strings.AddNewUseCase, Resources.UseCase, (o, e) => diagram?.CreateShape(EntityType.UseCase)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            this.toolNewActor = new ToolStripButton(Strings.AddNewActor, Resources.Actor, (o, e) => diagram?.CreateShape(EntityType.Actor)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            this.toolNewAssociation = new ToolStripButton(Strings.Association, Resources.Association, (o, e) => diagram?.CreateConnection(RelationshipType.Association)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            this.toolNewExtends = new ToolStripButton(Strings.AddNewExtends, Resources.Extends, (o, e) => diagram?.CreateConnection(RelationshipType.Extension)) { DisplayStyle = ToolStripItemDisplayStyle.Image };
            this.toolNewIncludes = new ToolStripButton(Strings.AddNewIncludes, Resources.Includes, (o, e) => diagram?.CreateConnection(RelationshipType.Inclusion)) { DisplayStyle = ToolStripItemDisplayStyle.Image };

            this.elementsToolStrip.Items.Add(this.toolNewUseCase);
            this.elementsToolStrip.Items.Add(this.toolNewActor);
            this.elementsToolStrip.Items.Add(new ToolStripSeparator());
            this.elementsToolStrip.Items.Add(this.toolNewAssociation);
            this.elementsToolStrip.Items.Add(this.toolNewExtends);
            this.elementsToolStrip.Items.Add(this.toolNewIncludes);
            this.elementsToolStrip.Items.Add(new ToolStripSeparator());
            this.elementsToolStrip.Items.Add(this.toolDelete);
        }   

        private void diagram_SelectionChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuFormat_DropDownOpening(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

    }
}