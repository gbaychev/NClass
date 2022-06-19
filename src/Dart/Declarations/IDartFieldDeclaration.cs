using NClass.Core;

namespace NClass.Dart
{
    public interface IDartFieldDeclaration : IFieldDeclaration
    {
        bool IsHider { get; }
    }
}
