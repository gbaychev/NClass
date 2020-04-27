using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NClass.Core;
using NClass.Core.UndoRedo;
using NClass.DiagramEditor.ClassDiagram.Shapes;

namespace NClass.DiagramEditor.Commands
{
    public class AddMemberCommand : ICommand
    {
        private readonly CompositeTypeShape shape;
        private readonly MemberType memberType;
        private Member member;

        public AddMemberCommand(CompositeTypeShape shape, MemberType memberType)
        {
            this.shape = shape;
            this.memberType = memberType;
        }

        public void Execute()
        {
            shape.InsertNewMember(memberType);
        }

        public void Undo()
        {
        }

        public CommandId CommandId
        {
            get { throw new NotImplementedException(); }
        }
    }
}