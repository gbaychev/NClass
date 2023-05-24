using System.Text.RegularExpressions;
using NClass.Core;
using NClass.Translations;

namespace NClass.Dart
{
    public class DartMethodNameDeclaration
    {
        const string OverloadableOperators =
            @"(\+|-|\*|/|%|&|\||\^|!|~|\+\+|--|<<|>>|==|!=|>|<|>=|<=)";

        // [explicit | implicit] operator <operator> | <name>
        const string MethodNamePattern =
            @"^\s*(?<name>((?<convop>implicit\s+|explicit\s+)?operator" + 
            @"(?<operator>(?(convop)\s+" + DartLanguage.GenericTypePattern2 + @"|\s*" + 
            OverloadableOperators + @"))|" + DartLanguage.GenericOperationNamePattern + @"))\s*$";

        private static readonly Regex nameRegex = new Regex(MethodNamePattern, RegexOptions.ExplicitCapture);

        readonly Match match;

        public DartMethodNameDeclaration(Match match)
        {
            if (!match.Success)
            {
                throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
            }

            this.match = match;
        }

        public static DartMethodNameDeclaration Create(string declaration)
        {
            var match = nameRegex.Match(declaration);
            return new DartMethodNameDeclaration(match);
        }

        public string Name
        {
            get { return match.Groups["name"].Value; }
        }

        public bool IsOperator
        {
            get { return match.Groups["operator"].Success; }
        }

        public bool IsConversionOperator
        {
            get { return match.Groups["convop"].Success; }
        }

        public bool IsExplicitImplementation
        {
            get { return match.Groups["namedot"].Success; }
        }
    }
}
