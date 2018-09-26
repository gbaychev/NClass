namespace NClass.Core
{
	public interface IPropertyDeclaration : IMemberDeclaration
	{
		bool IsReadonly { get; }
		bool IsWriteonly { get; }
		AccessModifier ReadAccess { get; }
		AccessModifier WriteAccess { get; }
		bool IsStatic { get; }
		bool IsVirtual { get; }
		bool IsAbstract { get; }
		bool IsOverride { get; }
		bool IsSealed { get; }
		bool IsHider { get; }
		IArgumentListDeclaration<IParameterDeclaration> ArgumentList { get; }
	}
}