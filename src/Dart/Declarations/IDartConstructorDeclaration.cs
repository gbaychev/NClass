using NClass.Core;

namespace NClass.Dart
{
    public interface IDartConstructorDeclaration : IMethodDeclaration
    {
        bool IsStatic { get; }

        bool IsFactory { get; }
    }
}
