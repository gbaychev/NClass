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
using System.Linq.Expressions;
using System.Reflection;

namespace NClass.Core.UndoRedo
{
    public class Modification
    {
        public Modification(Action undoAction, Action redoAction, string debugTag = "<unknown>")
        {
            UndoAction = undoAction;
            RedoAction = redoAction;
            DebugTag = debugTag;
        }

        public Action RedoAction { get; }
        public Action UndoAction { get;  }
        public string DebugTag { get; }

        public static Modification TrackPropertyModification<T, U>(T sender, Expression<Func<T, U>> propertySelector, U oldValue, U newValue) where T : IModifiable
        {
            var body = (MemberExpression)propertySelector.Body;
            var property = (PropertyInfo)body.Member;
            Action undoAction = () =>
            {
                sender.RaiseChangedEvent = false;
                property.SetValue(sender, oldValue);
                sender.RaiseChangedEvent = true;
            };
            Action redoAction = () =>
            {
                sender.RaiseChangedEvent = false;
                property.SetValue(sender, newValue);
                sender.RaiseChangedEvent = true;
            };
            return new Modification(undoAction, redoAction, $"PC: {body.Member.Name}, O: {oldValue}, N: {newValue}");
        }
    }
}