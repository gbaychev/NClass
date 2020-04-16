using System;
using NClass.Core;
using NClass.Core.UndoRedo;

namespace NClass.DiagramEditor.Commands

{
    public class RemoveMemberCommand : ICommand
    {
        readonly TypeBase shape;

        public RemoveMemberCommand(TypeBase shape, Member member)
        {
            throw new NotImplementedException();
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public CommandId CommandId
        {
            get { throw new NotImplementedException(); }
        }
    }
}