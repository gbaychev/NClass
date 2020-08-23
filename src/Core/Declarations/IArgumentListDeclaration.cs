using System.Collections.Generic;

namespace NClass.Core
{
    public interface IArgumentListDeclaration<out TParameter> : IEnumerable<TParameter> 
        where TParameter : IParameterDeclaration
    { }
}
