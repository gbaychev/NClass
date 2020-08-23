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

using NClass.Core;
using NClass.Core.UndoRedo;
using NClass.DiagramEditor.ClassDiagram.Shapes;

namespace NClass.DiagramEditor.Commands
{
    public class AddEnumMemberCommand : ICommand
    {
        private EnumType enumType;
        private string enumMember;
        private EnumValue enumValue;

        public EnumValue EnumValue => enumValue;

        public AddEnumMemberCommand(EnumType enumType, string enumMember)
        {
            this.enumType = enumType;
            this.enumMember = enumMember;
        }

        public void Execute()
        {
            if (enumValue == null)
            {
                enumValue = enumType.AddValue(enumMember);
            }
            else
            {
                enumType.ReinsertValue(enumValue);
            }
        }

        public void Undo()
        {
            enumType.RemoveValue(enumValue);
        }

        public CommandId CommandId => CommandId.AddEnumMember;
        public string DisplayText => CommandIdToString.GetString(CommandId);
    }
}