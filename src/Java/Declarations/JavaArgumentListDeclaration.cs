using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NClass.Java
{
	public class JavaArgumentListDeclaration : IJavaArgumentListDeclaration<JavaParameterDeclaration>
	{
		// <type> <name> [,]
		const string JavaParameterPattern =
			@"\s*(?<type>" + JavaLanguage.GenericTypePattern2 + @")\s+" +
			@"(?<name>" + JavaLanguage.NamePattern + @")\s*(,|$)";

		static readonly Regex parameterRegex =
			new Regex(JavaParameterPattern, RegexOptions.ExplicitCapture);

		readonly IEnumerable<JavaParameterDeclaration> parameters;

		public JavaArgumentListDeclaration(IEnumerable<JavaParameterDeclaration> parameters)
		{
			this.parameters = parameters;
		}

		public static JavaArgumentListDeclaration Create(string declaration)
		{
			var matches = parameterRegex.Matches(declaration);
			var parameters = matches.Cast<Match>().Select(match => new JavaParameterDeclaration(match));

			return new JavaArgumentListDeclaration(parameters);
		}

		public IEnumerator<JavaParameterDeclaration> GetEnumerator()
		{
			return parameters.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}