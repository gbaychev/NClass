namespace NClass.Core
{
    public interface IMemberDeclaration
    {
        string Name { get; }
        string Type { get; }
        AccessModifier AccessModifier { get; }
    }
}
