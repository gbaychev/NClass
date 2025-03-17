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

namespace NClass.Core.UndoRedo
{
    public class RenameDelegateParameterCommand : ICommand
    {
        private Parameter delegateParameter;
        private readonly DelegateType delegateType;
        private Parameter changedDelegateParameter;
        private readonly string newValue;
        private readonly string oldValue;

        public Parameter Parameter => changedDelegateParameter;

        public RenameDelegateParameterCommand(Parameter delegateParameter, DelegateType delegateType, string newValue)
        {
            this.delegateParameter = delegateParameter;
            this.delegateType = delegateType;
            this.newValue = newValue;
            this.oldValue = delegateParameter.ToString();
        }

        public void Execute()
        {
            changedDelegateParameter = delegateType.ModifyParameter(delegateParameter, newValue);
        }

        public void Undo()
        {
            delegateParameter = delegateType.ModifyParameter(changedDelegateParameter, oldValue);
        }

        public CommandId CommandId => CommandId.RenameDelegateParameter;
        public string DisplayText => CommandIdToString.GetString(CommandId);
    }
}