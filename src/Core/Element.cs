// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
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
using System.Xml;
using NClass.Core.UndoRedo;

namespace NClass.Core
{
    public abstract class Element : IModifiable
    {
        bool isDirty = false;
        int dontRaiseRequestCount = 0;

        public event ModifiedEventHandler Modified;

        public bool IsDirty
        {
            get { return isDirty; }
        }

        public virtual void Clean()
        {
            isDirty = false;
            //TODO: tagok tisztítása
        }

        protected bool Initializing { get; set; }

        public bool RaiseChangedEvent
        {
            get => (dontRaiseRequestCount == 0);
            set
            {
                if (!value)
                    dontRaiseRequestCount++;
                else if (dontRaiseRequestCount > 0)
                    dontRaiseRequestCount--;

                if (RaiseChangedEvent && isDirty)
                    OnModified(ModificationEventArgs.Empty);
            }
        }

        protected void Changed()
        {
            Changed(ModificationEventArgs.Empty);
        }

        protected void Changed(ModificationEventArgs args)
        {
            if (Initializing) return;

            if (RaiseChangedEvent)
                OnModified(args);
            else
                isDirty = true;
        }

        private void OnModified(ModificationEventArgs e)
        {
            isDirty = true;
            Modified?.Invoke(this, e);
        }
    }
}