using NClass.Core;
using NClass.Core.UndoRedo;
using NClass.DiagramEditor.ClassDiagram.Shapes;

namespace NClass.DiagramEditor.Commands
{
    public class InsertMemberCommand : ICommand
    {
        private readonly CompositeTypeShape shape;
        private readonly MemberType memberType;
        private Member member;

        public InsertMemberCommand(CompositeTypeShape shape, MemberType memberType)
        {
            this.shape = shape;
            this.memberType = memberType;
        }

        public void Execute()
        {
            member = shape.InsertNewMember(memberType);
        }

        public void Undo()
        {
            shape.CompositeType.RemoveMember(member);
        }

        public CommandId CommandId => CommandId.AddMember;
        public string DisplayText => CommandIdToString.GetString(CommandId);
    }
}