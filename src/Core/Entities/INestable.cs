using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NClass.Core
{
    /// <summary>
    /// This interface determines if entity is allowed to have nested childs
    /// </summary>
    public interface INestable : IEntity
    {
        IEnumerable<INestableChild> NestedChilds { get; }

        void AddNestedChild(INestableChild type);
        void RemoveNestedChild(INestableChild type);
        bool IsNestedAncestor(INestableChild type);
    }
}
