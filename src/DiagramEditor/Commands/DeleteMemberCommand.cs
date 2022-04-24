using System;
using NClass.Core;
using NClass.Core.UndoRedo;

namespace NClass.DiagramEditor.Commands
{
    public class DeleteMemberCommand : ICommand
    {
        private readonly CompositeType compositeType;
        private readonly Member member;
        private int index;

        public DeleteMemberCommand(CompositeType compositeType, Member member)
        {
            this.compositeType = compositeType;
            this.member = member;
        }

        public void Execute()
        {
            index = compositeType.RemoveMember(member);
        }

        public void Undo()
        {
            if (index != -1)
            {
                compositeType.ReinsertMember(member, index);
            }
        }

        public CommandId CommandId => CommandId.DeleteMember;
        public string DisplayText => CommandIdToString.GetString(CommandId);
    }
}
