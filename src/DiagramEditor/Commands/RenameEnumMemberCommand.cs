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

using NClass.Core;
using NClass.Core.UndoRedo;

namespace NClass.DiagramEditor.Commands
{
    public class RenameEnumMemberCommand : ICommand
    {
        private EnumValue enumValue;
        private readonly EnumType enumType;
        private EnumValue changedEnumValue;
        private readonly string newValue;
        private readonly string oldValue;

        public EnumValue EnumValue => changedEnumValue;

        public RenameEnumMemberCommand(EnumValue enumValue, EnumType enumType, string newValue)
        {
            this.enumValue = enumValue;
            this.enumType = enumType;
            this.newValue = newValue;
            this.oldValue = enumValue.Name;
        }

        public void Execute()
        {
            changedEnumValue = enumType.ModifyValue(enumValue, newValue);
        }

        public void Undo()
        {
            enumValue = enumType.ModifyValue(changedEnumValue, oldValue);
        }

        public CommandId CommandId => CommandId.RenameEnumMember;
        public string DisplayText => CommandIdToString.GetString(CommandId);
    }
}