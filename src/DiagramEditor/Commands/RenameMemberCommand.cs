// NClass - Free class diagram editor
// Copyright (C) 2020 Georgi Baychev
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
    public class RenameMemberCommand : ICommand
    {
        private readonly Member member;
        private readonly string oldValue;
        private readonly string newValue;
        private readonly Action<Member, string> setter;

        public RenameMemberCommand(Member member, string oldValue, string newValue, Action<Member, string> setter)
        {
            this.member = member;
            this.oldValue = oldValue;
            this.newValue = newValue; 
            this.setter = setter;
        }

        public void Execute()
        {
            setter(member, newValue);
        }

        public void Undo()
        {
            setter(member, oldValue);
        }

        public CommandId CommandId => CommandId.RenameMember;
        public string DisplayText => CommandIdToString.GetString(CommandId);
    }
}