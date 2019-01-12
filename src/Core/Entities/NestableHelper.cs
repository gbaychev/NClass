using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NClass.Core
{
    /// <summary>
    /// This is helper class providing default implementation methods for INestable interface
    /// </summary>
    internal class NestableHelper
    {
        INestable nestable = null;

        internal event NestingEventHandler AddedNestedChild;
        internal event NestingEventHandler RemovedNestedChild;

        private List<INestableChild> nestedChilds = new List<INestableChild>();

        internal IEnumerable<INestableChild> NestedChilds { get { return nestedChilds; } }

        internal NestableHelper(INestable nestable)
        {
            if (nestable == null)
                throw new ArgumentException(nameof(nestable));

            this.nestable = nestable;
        }

        internal bool IsNestedAncestor(INestableChild type)
        {
            var thisNestableChild = nestable as INestableChild;

            if (thisNestableChild != null && thisNestableChild.NestingParent != null && thisNestableChild.NestingParent.IsNestedAncestor(type))
                return true;
            else
                return (type == thisNestableChild);
        }

        internal void AddNestedChild(INestableChild element)
        {
            if (element != null && !nestedChilds.Contains(element))
            {
                nestedChilds.Add(element);
                OnAddNestedChild(new NestingEventArgs(element));
            }
        }

        internal void RemoveNestedChild(INestableChild element)
        {
            if (element != null && nestedChilds.Remove(element))
                OnRemoveNestedChild(new NestingEventArgs(element));
        }

        private void OnAddNestedChild(NestingEventArgs e)
        {
            if (AddedNestedChild != null)
                AddedNestedChild(this, e);
        }

        private void OnRemoveNestedChild(NestingEventArgs e)
        {
            if (RemovedNestedChild != null)
                RemovedNestedChild(this, e);
        }
    }
}
