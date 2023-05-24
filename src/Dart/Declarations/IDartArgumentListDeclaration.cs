using NClass.Core;

namespace NClass.Dart
{
    public interface IDartArgumentListDeclaration<out TParameter> : IArgumentListDeclaration<TParameter> 
        where TParameter : IDartParameterDeclaration
    { }
}
