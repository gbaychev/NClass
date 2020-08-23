using NClass.Core;
using NClass.Translations;
using System.Text.RegularExpressions;

namespace NClass.CSharp
{
    public class CSharpPropertyNameDeclaration
    {
        const string PropertyNamePattern =
            @"^\s*(?<name>" + CSharpLanguage.GenericOperationNamePattern + @")\s*$";

        static readonly Regex nameRegex = new Regex(PropertyNamePattern, RegexOptions.ExplicitCapture);

        readonly Match match;

        public CSharpPropertyNameDeclaration(Match match)
        {
            if (!match.Success)
            {
                throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
            }

            this.match = match;
        }

        public static CSharpPropertyNameDeclaration Create(string declaration)
        {
            var match = nameRegex.Match(declaration);
            return new CSharpPropertyNameDeclaration(match);
        }

        public string Name
        {
            get { return match.Groups["name"].Value; }
        }

        public bool IsExplicitImplementation
        {
            get { return match.Groups["namedot"].Success; }
        }
    }
}
