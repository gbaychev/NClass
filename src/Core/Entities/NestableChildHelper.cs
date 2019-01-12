using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NClass.Core
{
    /// <summary>
    /// This is helper class providing default implementation methods for INestableChild interface
    /// </summary>
    internal class NestableChildHelper
    {
        INestableChild nestableChild = null;
        INestable nestingParent = null;

        internal event NestingChildEventHandler  NestingParentChanged;

        internal NestableChildHelper(INestableChild nestableChild)
        {
            if (nestableChild == null)
                throw new ArgumentException(nameof(nestableChild));

            this.nestableChild = nestableChild;
        }

        internal INestable NestingParent
        {
            get
            {
                return nestingParent;
            }
            set
            {
                if (nestingParent != value)
                {
                    if (nestingParent != null)
                        nestingParent.RemoveNestedChild(nestableChild);
                    nestingParent = value;
                    if (nestingParent != null)
                        nestingParent.AddNestedChild(nestableChild);
                    OnNestingParentChanged(new NestingChildEventArgs(nestingParent));
                }
            }
        }

        private void OnNestingParentChanged(NestingChildEventArgs e)
        {
            if (NestingParentChanged != null)
                NestingParentChanged(this, e);
        }
    }
}
