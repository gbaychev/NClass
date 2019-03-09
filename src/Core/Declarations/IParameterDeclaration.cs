namespace NClass.Core
{
	public interface IParameterDeclaration
	{
		string Name { get; }
		string Type { get; }
		ParameterModifier Modifier { get; }
		bool HasDefaultValue { get; }
		string DefaultValue { get; }
	}
}