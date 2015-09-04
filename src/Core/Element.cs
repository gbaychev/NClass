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
using System.Xml;

namespace NClass.Core
{
	public abstract class Element : IModifiable
	{
		bool isDirty = false;
		bool initializing = false;
		int dontRaiseRequestCount = 0;

		public event EventHandler Modified;

		public bool IsDirty
		{
			get { return isDirty; }
		}

		public virtual void Clean()
		{
			isDirty = false;
			//TODO: tagok tisztítása
		}

		protected bool Initializing
		{
			get { return initializing; }
			set { initializing = value; }
		}

		protected bool RaiseChangedEvent
		{
			get
			{
				return (dontRaiseRequestCount == 0);
			}
			set
			{
				if (!value)
					dontRaiseRequestCount++;
				else if (dontRaiseRequestCount > 0)
					dontRaiseRequestCount--;

				if (RaiseChangedEvent && isDirty)
					OnModified(EventArgs.Empty);
			}
		}

		protected void Changed()
		{
			if (!Initializing)
			{
				if (RaiseChangedEvent)
					OnModified(EventArgs.Empty);
				else
					isDirty = true;
			}
		}

		private void OnModified(EventArgs e)
		{
			isDirty = true;
			if (Modified != null)
				Modified(this, e);
		}
	}
}