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

using System;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using System.Windows.Forms;
using NClass.Core.Entities;
using NClass.DiagramEditor.Commands;
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
            if (first is TypeShape shape1 && second is TypeShape shape2)
            {
                Func<Relationship> _connectionFactory = () => diagram.AddAssociation(shape1.TypeBase, shape2.TypeBase);
                var command = new AddConnectionCommand(diagram, _connectionFactory);
                command.Execute();
                diagram.TrackCommand(command);
            }
            else
            {
                MessageBox.Show(Strings.ErrorCannotCreateRelationship);
            }
        }

        private void CreateComposition()
        {
            if (first is TypeShape shape1 && second is TypeShape shape2)
            {
                Func<Relationship> _connectionFactory = () => diagram.AddComposition(shape1.TypeBase, shape2.TypeBase);
                var command = new AddConnectionCommand(diagram, _connectionFactory);
                command.Execute();
                diagram.TrackCommand(command);
            }
            else
            {
                MessageBox.Show(Strings.ErrorCannotCreateRelationship);
            }
        }

        private void CreateAggregation()
        {
            if (first is TypeShape shape1 && second is TypeShape shape2)
            {
                Func<Relationship> _connectionFactory = () => diagram.AddAggregation(shape1.TypeBase, shape2.TypeBase);
                var command = new AddConnectionCommand(diagram, _connectionFactory);
                command.Execute();
                diagram.TrackCommand(command);
            }
            else
            {
                MessageBox.Show(Strings.ErrorCannotCreateRelationship);
            }
        }

        private void CreateGeneralization()
        {
            if (first is CompositeTypeShape shape1 && second is CompositeTypeShape shape2)
            {
                try
                {
                    Func<Relationship> _connectionFactory = () => diagram.AddGeneralization(shape1.CompositeType, shape2.CompositeType);
                    var command = new AddConnectionCommand(diagram, _connectionFactory);
                    command.Execute();
                    diagram.TrackCommand(command);
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
            if (first is TypeShape shape1 && second is InterfaceShape shape2)
            {
                try
                {
                    Func<Relationship> _connectionFactory = () => diagram.AddRealization(shape1.TypeBase, shape2.InterfaceType);
                    var command = new AddConnectionCommand(diagram, _connectionFactory);
                    command.Execute();
                    diagram.TrackCommand(command);
                    return;
                }
                catch (RelationshipException)
                {
                    MessageBox.Show(Strings.ErrorCannotCreateRelationship);
                    return;
                } 
            }

            // Allow the Dart mixin class to be realized as well as interfaces as it can be used either as
            // a class or an interface.
            if (first is TypeShape shape3 && 
                ((ClassShape)second).ClassType.Modifier == ClassModifier.Mixin)
            {
                try
                {
                    Func<Relationship> _connectionFactory = () =>
                            diagram.AddRealization(shape3.TypeBase, ((ClassShape)second).ClassType);
                    var command = new AddConnectionCommand(diagram, _connectionFactory);
                    command.Execute();
                    diagram.TrackCommand(command);
                    return;
                }
                catch (RelationshipException)
                {
                    MessageBox.Show(Strings.ErrorCannotCreateRelationship);
                    return;
                }
            }

            MessageBox.Show(Strings.ErrorCannotCreateRelationship);
        }

        private void CreateDependency()
        {
            if (first is TypeShape shape1 && second is TypeShape shape2)
            {
                Func<Relationship> _connectionFactory = () => diagram.AddDependency(shape1.TypeBase, shape2.TypeBase);
                var command = new AddConnectionCommand(diagram, _connectionFactory);
                command.Execute();
                diagram.TrackCommand(command);
            }
            else
            {
                MessageBox.Show(Strings.ErrorCannotCreateRelationship);
            }
        }

        private void CreateNesting()
        {
            var parent = first.Entity as INestable;
            var child = second.Entity as INestableChild;

            try
            {
                if (parent == null)
                    throw new RelationshipException(Strings.ErrorParentNestingNotSupported);

                if (child == null)
                    throw new RelationshipException(Strings.ErrorChildNestingNotSupported);

                Func<Relationship> _connectionFactory = () => diagram.AddNesting(parent, child);
                var command = new AddConnectionCommand(diagram, _connectionFactory);
                command.Execute();
                diagram.TrackCommand(command);
            }
            catch (RelationshipException ex)
            {
                MessageBox.Show($"{Strings.ErrorCannotCreateRelationship} {ex.Message}");
            }
        }
    }
}