// NClass - Free class diagram editor
// Copyright (C) 2025 Georgi Baychev
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
using NClass.Core.UndoRedo;

namespace NClass.DiagramEditor.Commands
{
    public class AddNewMemberCommand : ICommand
    {
        private CompositeType parent;
        private Member newMember;
        private Func<CompositeType, Member> addOperation;

        public Member Member => newMember;

        public AddNewMemberCommand(CompositeType parent, Func<CompositeType, Member> addOperation)
        {
            this.parent = parent;
            this.addOperation = addOperation;
        }

        public void Execute()
        {
            if (newMember == null)
            {
                newMember = addOperation(parent);
            }
            else
            {
                parent.ReinsertMember(newMember, parent.MemberCount);
            }
        }

        public void Undo()
        {
            parent.RemoveMember(newMember);
        }

        public CommandId CommandId => CommandId.AddNewMember;
        public string DisplayText => CommandIdToString.GetString(CommandId);
    }
}