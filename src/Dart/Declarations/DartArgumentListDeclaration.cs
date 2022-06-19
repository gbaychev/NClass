using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NClass.Dart
{
    public class DartArgumentListDeclaration : IDartArgumentListDeclaration<DartParameterDeclaration>
    {
        // [this] [<modifiers>] <type> <name> [,]
        const string ParameterPattern =
            @"(?<this>this)?\s*" +
            @"(?<type>" + DartLanguage.GenericTypePattern2 + @")\s+" +
            @"(?<name>" + DartLanguage.NamePattern + @")" +
            @"(\s*=\s*(?<defval>([^,""]+|""(\\""|[^""])*"")))?";

        private static readonly Regex parameterRegex =
            new Regex(ParameterPattern, RegexOptions.ExplicitCapture);

        readonly IEnumerable<DartParameterDeclaration> parameters;

        public DartArgumentListDeclaration(IEnumerable<DartParameterDeclaration> parameters)
        {
            this.parameters = parameters;
        }

        public static DartArgumentListDeclaration Create(string declaration)
        {
            var matches = parameterRegex.Matches(declaration);
            var parameters = matches.Cast<Match>().Select(match => new DartParameterDeclaration(match));

            return new DartArgumentListDeclaration(parameters);
        }

        public IEnumerator<DartParameterDeclaration> GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
