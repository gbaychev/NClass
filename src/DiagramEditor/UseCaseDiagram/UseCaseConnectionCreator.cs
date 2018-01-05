// NClass - Free class diagram editor
// Copyright (C) 2017 Georgi Baychev
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
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.UseCaseDiagram.Shapes;
using NClass.Translations;

namespace NClass.DiagramEditor.UseCaseDiagram
{
    internal class UseCaseConnectionCreator : ConnectionCreator<UseCaseDiagram>
    {
        public UseCaseConnectionCreator(UseCaseDiagram diagram, RelationshipType type) :
            base(diagram, type)
        {
        }

        protected override void CreateConnection()
        {
            switch (type)
            {
                case RelationshipType.Extension:
                    CreateExtends();
                    break;
                case RelationshipType.Inclusion:
                    CreateIncludes();
                    break;
                case RelationshipType.UseCaseAssocation:
                    CreateAssocation();
                    break;
                case RelationshipType.UseCaseGeneralization:
                    CreateGeneralization();
                    break;
            }

            created = true;
            diagram.Redraw();
        }

        private void CreateExtends()
        {
            if (first is UseCaseShape shape1 && second is UseCaseShape shape2)
            {
                diagram.AddExtends(shape1.UseCase, shape2.UseCase);
            }
            else
            {
                MessageBox.Show(Strings.ErrorCannotCreateRelationship);
            }
        }

        private void CreateIncludes()
        {
            if (first is UseCaseShape shape1 && second is UseCaseShape shape2)
            {
                diagram.AddIncludes(shape1.UseCase, shape2.UseCase);
            }
            else
            {
                MessageBox.Show(Strings.ErrorCannotCreateRelationship);
            }
        }

        private void CreateAssocation()
        {
            if (first is UseCaseShapeBase shape1 && second is UseCaseShapeBase shape2)
            {
                diagram.AddAssociation(shape1.UseCaseEntity, shape2.UseCaseEntity);
            }
            else
            {
                MessageBox.Show(Strings.ErrorCannotCreateRelationship);
            }
        }

        private void CreateGeneralization()
        {
            if (first is UseCaseShapeBase shape1 && second is UseCaseShapeBase shape2)
            {
                diagram.AddGeneralization(shape1.UseCaseEntity, shape2.UseCaseEntity);
            }
            else
            {
                MessageBox.Show(Strings.ErrorCannotCreateRelationship);
            }
        }
    }
}