using System;
using NClass.Core;
using NClass.Core.UndoRedo;

namespace NClass.DiagramEditor.Commands
{
    public class DeleteMemberCommand : ICommand
    {
        readonly TypeBase shape;

        public DeleteMemberCommand(TypeBase shape, Member member)
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

        public CommandId CommandId => CommandId.DeleteMember;
        public string DisplayText => CommandIdToString.GetString(CommandId);
    }
}