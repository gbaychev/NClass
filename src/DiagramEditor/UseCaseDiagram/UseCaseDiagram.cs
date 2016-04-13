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

using System.Windows.Forms;
using NClass.Core;
using NClass.Core.Models;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.UseCaseDiagram
{
    public class UseCaseDiagram : Diagram<UseCaseModel>
    {
        public UseCaseDiagram()
        {
            this.model = new UseCaseModel();
            this.diagramDynamicMenu = UseCaseDiagramDynamicMenu.Default;
        }

        public override void KeyDown(KeyEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public override void CreateShape(EntityType type)
        {
            throw new System.NotImplementedException();
        }

        public override Shape AddShape(EntityType type)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnEntityAdded(object sender, EntityEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnRelationAdded(object sender, RelationshipEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}