using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NClass.Core
{
    public delegate void NestingEventHandler(object sender, NestingEventArgs e);

    public class NestingEventArgs : EventArgs
    {
        INestableChild element;

        public NestingEventArgs(INestableChild element)
        {
            this.element = element;
        }

        public INestableChild Element
        {
            get { return element; }
        }
    }
}
