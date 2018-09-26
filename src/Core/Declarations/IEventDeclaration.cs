namespace NClass.Core
{
	public interface IEventDeclaration : IMemberDeclaration
	{
		bool IsStatic { get; }
		bool IsVirtual { get; }
		bool IsAbstract { get; }
		bool IsOverride { get; }
		bool IsSealed { get; }
		bool IsHider { get; }
	}
}