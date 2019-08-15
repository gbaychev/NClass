using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NClass.Core
{
    /// <summary>
    /// This interface determines if entity is allowed to be nested in parent entity
    /// </summary>
    public interface INestableChild : IEntity
    {
        INestable NestingParent { get; set; }
        INestableChild CloneChild();
    }
}
