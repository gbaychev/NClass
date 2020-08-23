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

namespace NClass.Core.UndoRedo
{
    public class TrackableValue<T> : ITrackable
    {
        private readonly string _propertyName;
        private T _value, _originalValue;

        public TrackableValue(string propertyName, T defaultValue)
        {
            _propertyName = propertyName;
            _originalValue = _value = defaultValue;
        }

        public bool HasChanges => _originalValue.Equals(_value);
    }
}