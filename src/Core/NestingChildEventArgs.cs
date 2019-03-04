using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NClass.Core
{
    public delegate void NestingChildEventHandler(object sender, NestingChildEventArgs e);

    public class NestingChildEventArgs : EventArgs
    {
        INestable element;

        public NestingChildEventArgs(INestable element)
        {
            this.element = element;
        }

        public INestable Element
        {
            get { return element; }
        }
    }
}
