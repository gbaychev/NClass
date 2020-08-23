namespace NClass.Core
{
    public interface IFieldDeclaration : IMemberDeclaration
    {
        bool HasInitialValue { get; }
        string InitialValue { get; }
        bool IsStatic { get; }
        bool IsReadonly { get; }
        bool IsConstant { get; }
        bool IsVolatile { get; }
    }
}
