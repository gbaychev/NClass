using System.Text.RegularExpressions;
using NClass.Core;
using NClass.Translations;

namespace NClass.CSharp
{
    public class CSharpParameterDeclaration : ICSharpParameterDeclaration
    {
        // [this] [<modifiers>] <type> <name> [,]
        const string ParameterPattern =
            @"(?<this>this)?\s*" +
            @"(?<modifier>out|ref|params)?(?(modifier)\s+|)" +
            @"(?<type>" + CSharpLanguage.GenericTypePattern2 + @")\s+" +
            @"(?<name>" + CSharpLanguage.NamePattern + @")" +
            @"(\s*=\s*(?<defval>([^,""]+|""(\\""|[^""])*"")))?";

        static readonly Regex parameterRegex =
            new Regex("^" + ParameterPattern + "$", RegexOptions.ExplicitCapture);

        private readonly Match match;

        public CSharpParameterDeclaration(Match match)
        {
            if (!match.Success)
            {
                throw new BadSyntaxException(Strings.ErrorInvalidParameterDeclaration);
            }

            this.match = match;
        }

        public static CSharpParameterDeclaration Create(string declaration)
        {
            var match = parameterRegex.Match(declaration);
            return new CSharpParameterDeclaration(match);
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
