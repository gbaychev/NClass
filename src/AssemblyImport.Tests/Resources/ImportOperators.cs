namespace AssemblyImport.Tests
{
	public class ImportOperators
	{
		public static ImportOperators operator+ (ImportOperators first, ImportOperators second) 
		{
			return new ImportOperators();
		}

		public static implicit operator ImportOperators(int value)
		{
			return new ImportOperators();
		}
	}
}