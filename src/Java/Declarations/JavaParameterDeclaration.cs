using System.Text.RegularExpressions;
using NClass.Core;
using NClass.Translations;

namespace NClass.Java
{
    public class JavaParameterDeclaration : IJavaParameterDeclaration
    {
        // <type> <name> [,]
        const string JavaParameterPattern =
            @"\s*(?<type>" + JavaLanguage.GenericTypePattern2 + @")\s+" +
            @"(?<name>" + JavaLanguage.NamePattern + @")\s*(,|$)";

        static readonly Regex parameterRegex =
            new Regex("^" + JavaParameterPattern + "$", RegexOptions.ExplicitCapture);

        private readonly Match match;

        public JavaParameterDeclaration(Match match)
        {
            if (!match.Success)
            {
                throw new BadSyntaxException(Strings.ErrorInvalidParameterDeclaration);
            }

            this.match = match;
        }

        public static JavaParameterDeclaration Create(string declaration)
        {
            var match = parameterRegex.Match(declaration);
            return new JavaParameterDeclaration(match);
        }

        public string Name
        {
            get { return match.Groups["name"].Value; }
        }

        public string Type
        {
            get { return match.Groups["type"].Value; }
        }

        public ParameterModifier Modifier
        {
            get { return ParseParameterModifier(match.Groups["modifier"].Value); }
        }

        public bool HasDefaultValue
        {
            get { return match.Groups["defval"].Success; }
        }

        public string DefaultValue
        {
            get { return match.Groups["defval"].Value; }
        }

        private static ParameterModifier ParseParameterModifier(string modifierString)
        {
            switch (modifierString)
            {
                case "ref":
                    return ParameterModifier.Inout;

                case "out":
                    return ParameterModifier.Out;

                case "params":
                    return ParameterModifier.Params;

                case "in":
                default:
                    return ParameterModifier.In;
            }
        }
    }
}
