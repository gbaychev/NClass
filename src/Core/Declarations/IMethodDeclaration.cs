namespace NClass.Core
{
    public interface IMethodDeclaration : IMemberDeclaration
    {
        IArgumentListDeclaration<IParameterDeclaration> ArgumentList { get; }
    }
}