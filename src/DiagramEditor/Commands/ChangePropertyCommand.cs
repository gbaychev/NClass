using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NClass.Core.UndoRedo;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.Commands
{
    public class ChangePropertyCommand<T> : ICommand
    {
        private readonly Shape shape;
        private readonly PropertyInfo propertyHandler;
        private T newValue;
        private T oldValue;

        public ChangePropertyCommand(Shape shape, Expression<Func<Shape, T>> propertySelector, T newValue)
        {
            this.shape = shape;
            var body = (MemberExpression)propertySelector.Body;
            propertyHandler = (PropertyInfo)body.Member;
            this.newValue = newValue;
        }

        public void Execute()
        {
            oldValue = (T)propertyHandler.GetValue(shape);
            propertyHandler.SetValue(shape, newValue);
        }

        public void Undo()
        {
            shape.RaiseChangedEvent = false;
            propertyHandler.SetValue(shape, oldValue);
            shape.RaiseChangedEvent = true;
        }

        public CommandId CommandId
        {
            get { throw new NotImplementedException(); }
        }
    }
}