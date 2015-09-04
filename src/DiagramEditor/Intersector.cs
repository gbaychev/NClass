// NClass - Free class diagram editor
// Copyright (C) 2006-2007 Balazs Tihanyi
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
using System.Collections.Generic;

namespace NClass.DiagramEditor
{
	internal sealed class Intersector<T>
	{
		private class DomainElement
		{
			int count = 1;
			T value;

			public DomainElement(T value)
			{
				this.value = value;
			}

			public T Value
			{
				get { return value; }
			}

			public int Count
			{
				get { return count; }
				set { count = value; }
			}
		}

		int setCount = 0;
		int domainIndex = 0;
		List<DomainElement> domain = new List<DomainElement>();

		public void ClearSets()
		{
			setCount = 0;
			domainIndex = 0;
			domain.Clear();
		}

		public void AddSet(IEnumerable<T> values)
		{
			foreach (T value in values)
				AddToDomain(value);
			setCount++;
		}

		private void AddToDomain(T value)
		{
			bool found = false;

			for (int i = domainIndex; i < domain.Count && !found; i++) {
				if (EqualityComparer<T>.Default.Equals(domain[i].Value, value)) {
					domain[i].Count++;
					domainIndex = (i + 1) % domain.Count;
					found = true;
				}
			}
			for (int i = 0; i < domainIndex && !found; i++) {
				if (EqualityComparer<T>.Default.Equals(domain[i].Value, value)) {
					domain[i].Count++;
					domainIndex = (i + 1) % domain.Count;
					found = true;
				}
			}

			if (!found) {
				DomainElement newElement = new DomainElement(value);
				domain.Add(newElement);
				domainIndex = 0;
			}
		}

		public IEnumerable<T> GetIntersection()
		{
			for (int i = 0; i < domain.Count; i++) {
				if (domain[i].Count == setCount)
					yield return domain[i].Value;
			}
		}
	}
}
