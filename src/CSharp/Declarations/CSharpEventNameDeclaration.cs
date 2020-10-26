using NClass.Core;
using NClass.Translations;
using System.Text.RegularExpressions;

namespace NClass.CSharp
{
    public class CSharpEventNameDeclaration
    {
        const string EventNamePattern =
            @"^\s*(?<name>" + CSharpLanguage.GenericOperationNamePattern + @")\s*$";

        static readonly Regex nameRegex = new Regex(EventNamePattern, RegexOptions.ExplicitCapture);

        readonly Match match;

        public CSharpEventNameDeclaration(Match match)
        {
            if (!match.Success)
            {
                throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
            }

            this.match = match;
        }

        public static CSharpEventNameDeclaration Create(string declaration)
        {
            var match = nameRegex.Match(declaration);
            return new CSharpEventNameDeclaration(match);
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
