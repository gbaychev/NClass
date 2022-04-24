using System.Collections.Generic;
using System.Text.RegularExpressions;
using NClass.Core;
using NClass.Translations;

namespace NClass.CSharp
{
    public class CSharpEventDeclaration : ICSharpEventDeclaration
    {
        // [<access>] [<modifiers>] event <type> <name>
        const string EventPattern =
            @"^\s*" + CSharpLanguage.AccessPattern + CSharpLanguage.OperationModifiersPattern +
            @"event\s+" +
            @"(?<type>" + CSharpLanguage.GenericTypePattern2 + @")\s+" +
            @"(?<name>" + CSharpLanguage.GenericOperationNamePattern + ")" +
            CSharpLanguage.DeclarationEnding;

        static readonly Regex eventRegex = new Regex(EventPattern, RegexOptions.ExplicitCapture);

        readonly Match match;
        readonly HashSet<string> modifiers;

        public CSharpEventDeclaration(Match match)
        {
            if (!match.Success)
            {
                throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
            }

            this.match = match;
            this.modifiers = GetModifiers(match);
        }

        public static CSharpEventDeclaration Create(string declaration)
        {
            var match = eventRegex.Match(declaration);
            return new CSharpEventDeclaration(match);
        }

        public string Name
        {
            get { return match.Groups["name"].Value; }
        }

        public string Type
        {
            get { return match.Groups["type"].Value; }
        }

        public AccessModifier AccessModifier
        {
            get { return ParseAccessModifier(match.Groups["access"].Value); }
        }

        public bool IsStatic
        {
            get { return modifiers.Contains("static"); }
        }

        public bool IsVirtual
        {
            get { return modifiers.Contains("virtual"); }
        }

        public bool IsAbstract
        {
            get { return modifiers.Contains("abstract"); }
        }

        public bool IsOverride
        {
            get { return modifiers.Contains("override"); }
        }

        public bool IsSealed
        {
            get { return modifiers.Contains("sealed"); }
        }

        public bool IsHider
        {
            get { return modifiers.Contains("new"); }
        }

        public bool IsExplicitImplementation
        {
            get { return match.Groups["namedot"].Success; }
        }

        private static AccessModifier ParseAccessModifier(string value)
        {
            return CSharpLanguage.Instance.TryParseAccessModifier(value);
        }

        private static HashSet<string> GetModifiers(Match match)
        {
            var captures = match.Groups["modifier"].Captures;
            var modifiers = new HashSet<string>();

            foreach (Capture capture in captures)
            {
                modifiers.Add(capture.Value);
            }

            return modifiers;
        }
    }
}
