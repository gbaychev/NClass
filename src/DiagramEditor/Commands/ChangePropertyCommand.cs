using System;
using System.Linq.Expressions;
using System.Reflection;
using NClass.Core.UndoRedo;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.Commands
{
    public class ChangePropertyCommand<T, U> : ICommand where T : Shape
    {
        private readonly T shape;
        private readonly Func<T, U> getter;
        private readonly Action<T, U> setter;
        private readonly U newValue;
        private U oldValue;

        public ChangePropertyCommand(T shape, Func<T, U> getter, Action<T, U> setter, U newValue)
        {
            this.shape = shape;
            this.newValue = newValue;
            this.getter = getter;
            this.setter = setter;
        }

        public void Execute()
        {
            oldValue = getter(shape);
            setter(shape, newValue);
        }

        public void Undo()
        {
            shape.RaiseChangedEvent = false;
            setter(shape, oldValue);
            shape.RaiseChangedEvent = true;
        }

        public CommandId CommandId => CommandId.ChangeProperty;
        public string DisplayText => CommandIdToString.GetString(CommandId);
    }
}
