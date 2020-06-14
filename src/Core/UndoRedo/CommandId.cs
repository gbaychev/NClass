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

using System.Collections.Generic;

namespace NClass.Core.UndoRedo
{
    public enum CommandId
    {
        AddShape,
        AddConnection,
        DeleteElements,
        ChangeProperty,
        MoveElements,
        Paste,
        AddMember,
        DeleteMember
    }

    public class CommandIdToString
    {
        private static Dictionary<CommandId, string> _strings = new Dictionary<CommandId, string>();

        static CommandIdToString()
        {
            _strings[CommandId.AddShape] = "Add Shape";
            _strings[CommandId.AddConnection] = "Add Connection";
            _strings[CommandId.DeleteElements] = "Delete Elements";
            _strings[CommandId.ChangeProperty] = "Change Property";
            _strings[CommandId.MoveElements] = "Move Elements";
            _strings[CommandId.Paste] = "Paste";
            _strings[CommandId.AddMember] = "Add Member";
            _strings[CommandId.DeleteMember] = "Delete Member";
        }

        public static string GetString(CommandId commandId)
        {
            switch (commandId)
            {
                case CommandId.AddShape:
                    return _strings[CommandId.AddShape];
                case CommandId.AddConnection:
                    return _strings[CommandId.AddConnection];
                case CommandId.DeleteElements:
                    return _strings[CommandId.DeleteElements];
                case CommandId.ChangeProperty:
                    return _strings[CommandId.ChangeProperty];
                case CommandId.MoveElements:
                    return _strings[CommandId.MoveElements];
                case CommandId.Paste:
                    return _strings[CommandId.Paste];
                case CommandId.AddMember:
                    return _strings[CommandId.AddMember];
                case CommandId.DeleteMember:
                    return _strings[CommandId.DeleteMember];
                default:
                    return "<unknown>";
            }
        }
    }
}