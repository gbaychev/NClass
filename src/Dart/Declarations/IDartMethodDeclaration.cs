using NClass.Core;

namespace NClass.Dart
{
    public interface IDartMethodDeclaration : IMethodDeclaration
    {
        bool IsOperator { get; }
        bool IsStatic { get; }
        bool IsVirtual { get; }
        bool IsAbstract { get; }
        bool IsOverride { get; }
        bool IsSealed { get; }
        bool IsHider { get; }
        bool IsConversionOperator { get; }
        bool IsExplicitImplementation { get; }
    }
}
