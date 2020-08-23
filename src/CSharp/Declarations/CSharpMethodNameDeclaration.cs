using System.Text.RegularExpressions;
using NClass.Core;
using NClass.Translations;

namespace NClass.CSharp
{
    public class CSharpMethodNameDeclaration
    {
        const string OverloadableOperators =
            @"(\+|-|\*|/|%|&|\||\^|!|~|\+\+|--|<<|>>|==|!=|>|<|>=|<=| true| false)";

        // [explicit | implicit] operator <operator> | <name>
        const string MethodNamePattern =
            @"^\s*(?<name>((?<convop>implicit\s+|explicit\s+)?operator" + 
            @"(?<operator>(?(convop)\s+" + CSharpLanguage.GenericTypePattern2 + @"|\s*" + 
            OverloadableOperators + @"))|" + CSharpLanguage.GenericOperationNamePattern + @"))\s*$";

        static readonly Regex nameRegex = new Regex(MethodNamePattern, RegexOptions.ExplicitCapture);

        readonly Match match;

        public CSharpMethodNameDeclaration(Match match)
        {
            if (!match.Success)
            {
                throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
            }

            this.match = match;
        }

        public static CSharpMethodNameDeclaration Create(string declaration)
        {
            var match = nameRegex.Match(declaration);
            return new CSharpMethodNameDeclaration(match);
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
