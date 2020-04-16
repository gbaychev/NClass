using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NClass.Core;
using NClass.Core.UndoRedo;

namespace NClass.DiagramEditor.Commands
{
    public class AddMemberCommand : ICommand
    {
        readonly TypeBase shape;

        public AddMemberCommand(TypeBase shape, Member member)
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