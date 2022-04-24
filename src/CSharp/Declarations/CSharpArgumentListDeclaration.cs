using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NClass.CSharp
{
    public class CSharpArgumentListDeclaration : ICSharpArgumentListDeclaration<CSharpParameterDeclaration>
    {
        // [this] [<modifiers>] <type> <name> [,]
        const string ParameterPattern =
            @"(?<this>this)?\s*" +
            @"(?<modifier>out|ref|params)?(?(modifier)\s+|)" +
            @"(?<type>" + CSharpLanguage.GenericTypePattern2 + @")\s+" +
            @"(?<name>" + CSharpLanguage.NamePattern + @")" +
            @"(\s*=\s*(?<defval>([^,""]+|""(\\""|[^""])*"")))?";

        static readonly Regex parameterRegex =
            new Regex(ParameterPattern, RegexOptions.ExplicitCapture);

        readonly IEnumerable<CSharpParameterDeclaration> parameters;

        public CSharpArgumentListDeclaration(IEnumerable<CSharpParameterDeclaration> parameters)
        {
            this.parameters = parameters;
        }

        public static CSharpArgumentListDeclaration Create(string declaration)
        {
            var matches = parameterRegex.Matches(declaration);
            var parameters = matches.Cast<Match>().Select(match => new CSharpParameterDeclaration(match));

            return new CSharpArgumentListDeclaration(parameters);
        }

        public IEnumerator<CSharpParameterDeclaration> GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
