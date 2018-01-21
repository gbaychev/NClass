// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
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

using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using System.Windows.Forms;
using NClass.Translations;
using NClass.DiagramEditor.Diagrams;

namespace NClass.DiagramEditor.ClassDiagram
{
    internal class ClassConnectionCreator : ConnectionCreator<ClassDiagram>
    {
        public ClassConnectionCreator(ClassDiagram diagram, RelationshipType type) 
            : base(diagram, type)
        {
        }

        protected override void CreateConnection()
        {
            switch (type)
            {
                case RelationshipType.Association:
                    CreateAssociation();
                    break;

                case RelationshipType.Composition:
                    CreateComposition();
                    break;

                case RelationshipType.Aggregation:
                    CreateAggregation();
                    break;

                case RelationshipType.Generalization:
                    CreateGeneralization();
                    break;

                case RelationshipType.Realization:
                    CreateRealization();
                    break;

                case RelationshipType.Dependency:
                    CreateDependency();
                    break;

                case RelationshipType.Nesting:
                    CreateNesting();
                    break;
            }

            base.CreateConnection();
        }

        private void CreateAssociation()
        {
            TypeShape shape1 = first as TypeShape;
            TypeShape shape2 = second as TypeShape;

            if (shape1 != null && shape2 != null)
            {
                diagram.AddAssociation(shape1.TypeBase, shape2.TypeBase);
            }
            else
            {
                MessageBox.Show(Strings.ErrorCannotCreateRelationship);
            }
        }

        private void CreateComposition()
        {
            TypeShape shape1 = first as TypeShape;
            TypeShape shape2 = second as TypeShape;

            if (shape1 != null && shape2 != null)
            {
                diagram.AddComposition(shape1.TypeBase, shape2.TypeBase);
            }
            else
            {
                MessageBox.Show(Strings.ErrorCannotCreateRelationship);
            }
        }

        private void CreateAggregation()
        {
            TypeShape shape1 = first as TypeShape;
            TypeShape shape2 = second as TypeShape;

            if (shape1 != null && shape2 != null)
            {
                diagram.AddAggregation(shape1.TypeBase, shape2.TypeBase);
            }
            else
            {
                MessageBox.Show(Strings.ErrorCannotCreateRelationship);
            }
        }

        private void CreateGeneralization()
        {
            CompositeTypeShape shape1 = first as CompositeTypeShape;
            CompositeTypeShape shape2 = second as CompositeTypeShape;

            if (shape1 != null && shape2 != null)
            {
                try
                {
                    diagram.AddGeneralization(shape1.CompositeType, shape2.CompositeType);
                }
                catch (RelationshipException)
                {
                    MessageBox.Show(Strings.ErrorCannotCreateRelationship);
                }
            }
            else
            {
                MessageBox.Show(Strings.ErrorCannotCreateRelationship);
            }
        }

        private void CreateRealization()
        {
            TypeShape shape1 = first as TypeShape;
            InterfaceShape shape2 = second as InterfaceShape;

            if (shape1 != null && shape2 != null)
            {
                try
                {
                    diagram.AddRealization(shape1.TypeBase, shape2.InterfaceType);
                }
                catch (RelationshipException)
                {
                    MessageBox.Show(Strings.ErrorCannotCreateRelationship);
                }
            }
            else
            {
                MessageBox.Show(Strings.ErrorCannotCreateRelationship);
            }
        }

        private void CreateDependency()
        {
            TypeShape shape1 = first as TypeShape;
            TypeShape shape2 = second as TypeShape;

            if (shape1 != null && shape2 != null)
            {
                diagram.AddDependency(shape1.TypeBase, shape2.TypeBase);
            }
            else
            {
                MessageBox.Show(Strings.ErrorCannotCreateRelationship);
            }
        }

        private void CreateNesting()
        {
            CompositeTypeShape shape1 = first as CompositeTypeShape;
            TypeShape shape2 = second as TypeShape;

            if (shape1 != null && shape2 != null)
            {
                try
                {
                    diagram.AddNesting(shape1.CompositeType, shape2.TypeBase);
                }
                catch (RelationshipException)
                {
                    MessageBox.Show(Strings.ErrorCannotCreateRelationship);
                }
            }
            else
            {
                MessageBox.Show(Strings.ErrorCannotCreateRelationship);
            }
        }
    }
}