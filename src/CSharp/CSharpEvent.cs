// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// 
// This program is free software; you can redistribute it and/or modify it under 
// the terms of the GNU General Public License as published by the Free Software 
// Foundation; either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with 
// this program; if not, write to the Free Software Foundation, Inc., 
// 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Text;
using System.Text.RegularExpressions;
using NClass.Core;
using NClass.Translations;

namespace NClass.CSharp
{
	internal sealed class CSharpEvent : Event
	{
		// [<access>] [<modifiers>] event <type> <name>
		const string EventPattern =
			@"^\s*" + CSharpLanguage.AccessPattern + CSharpLanguage.OperationModifiersPattern +
			@"event\s+" +
			@"(?<type>" + CSharpLanguage.GenericTypePattern2 + @")\s+" +
			@"(?<name>" + CSharpLanguage.GenericOperationNamePattern + ")" +
			CSharpLanguage.DeclarationEnding;

		const string EventNamePattern =
			@"^\s*(?<name>" + CSharpLanguage.GenericOperationNamePattern + @")\s*$";

		static Regex eventRegex = new Regex(EventPattern, RegexOptions.ExplicitCapture);
		static Regex nameRegex = new Regex(EventNamePattern, RegexOptions.ExplicitCapture);

		bool isExplicitImplementation = false;
		
		/// <exception cref="ArgumentNullException">
		/// <paramref name="parent"/> is null.
		/// </exception>
		internal CSharpEvent(CompositeType parent) : this("NewEvent", parent)
		{
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="name"/> does not fit to the syntax.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The language of <paramref name="parent"/> does not equal.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="parent"/> is null.
		/// </exception>
		internal CSharpEvent(string name, CompositeType parent) : base(name, parent)
		{
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="value"/> does not fit to the syntax.
		/// </exception>
		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				Match match = nameRegex.Match(value);

				if (match.Success) {
					try {
						RaiseChangedEvent = false;
						ValidName = match.Groups["name"].Value;
						IsExplicitImplementation = match.Groups["namedot"].Success;
					}
					finally {
						RaiseChangedEvent = true;
					}
				}
				else {
					throw new BadSyntaxException(Strings.ErrorInvalidName);
				}
			}
		}

		protected override string DefaultType
		{
			get { return "EventHandler"; }
		}

		/// <exception cref="BadSyntaxException">
		/// Cannot set access visibility.
		/// </exception>
		public override AccessModifier AccessModifier
		{
			get
			{
				return base.AccessModifier;
			}
			set
			{
				if (value != AccessModifier.Default && IsExplicitImplementation) {
					throw new BadSyntaxException(
						Strings.ErrorExplicitImplementationAccess);
				}
				if (value != AccessModifier.Default && Parent is InterfaceType) {
					throw new BadSyntaxException(
						Strings.ErrorInterfaceMemberAccess);
				}

				base.AccessModifier = value;
			}
		}

		public override bool IsAccessModifiable
		{
			get
			{
				return (base.IsAccessModifiable && !IsExplicitImplementation);
			}
		}

		public bool IsExplicitImplementation
		{
			get
			{
				return isExplicitImplementation;
			}
			private set
			{
				if (isExplicitImplementation != value) {
					try {
						RaiseChangedEvent = false;

						if (value)
							AccessModifier = AccessModifier.Default;
						isExplicitImplementation = value;
						Changed();
					}
					finally {
						RaiseChangedEvent = true;
					}
				}
			}
		}

		public override bool HasBody
		{
			get
			{
				return IsExplicitImplementation;
			}
		}

		public override Language Language
		{
			get { return CSharpLanguage.Instance; }
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="declaration"/> does not fit to the syntax.
		/// </exception>
		public override void InitFromString(string declaration)
		{
			Match match = eventRegex.Match(declaration);
			RaiseChangedEvent = false;

			try {
				if (match.Success) {
					ClearModifiers();
					Group nameGroup = match.Groups["name"];
					Group typeGroup = match.Groups["type"];
					Group accessGroup = match.Groups["access"];
					Group modifierGroup = match.Groups["modifier"];
					Group nameDotGroup = match.Groups["namedot"];

					if (CSharpLanguage.Instance.IsForbiddenName(nameGroup.Value))
						throw new BadSyntaxException(Strings.ErrorInvalidName);
					if (CSharpLanguage.Instance.IsForbiddenTypeName(typeGroup.Value))
						throw new BadSyntaxException(Strings.ErrorInvalidTypeName);

					ValidName = nameGroup.Value;
					ValidType = typeGroup.Value;
					IsExplicitImplementation = nameDotGroup.Success;
					AccessModifier = Language.TryParseAccessModifier(accessGroup.Value);

					foreach (Capture modifierCapture in modifierGroup.Captures) {
						if (modifierCapture.Value == "static")
							IsStatic = true;
						if (modifierCapture.Value == "virtual")
							IsVirtual = true;
						if (modifierCapture.Value == "abstract")
							IsAbstract = true;
						if (modifierCapture.Value == "override")
							IsOverride = true;
						if (modifierCapture.Value == "sealed")
							IsSealed = true;
						if (modifierCapture.Value == "new")
							IsHider = true;
					}
				}
				else {
					throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
				}
			}
			finally {
				RaiseChangedEvent = true;
			}
		}

		public override string GetDeclaration()
		{
			bool needsSemicolon = !IsExplicitImplementation;
			return GetDeclarationLine(needsSemicolon);
		}

		public string GetDeclarationLine(bool withSemicolon)
		{
			StringBuilder builder = new StringBuilder(50);

			if (AccessModifier != AccessModifier.Default) {
				builder.Append(Language.GetAccessString(AccessModifier, true));
				builder.Append(" ");
			}
			
			if (IsHider)
				builder.Append("new ");
			if (IsStatic)
				builder.Append("static ");
			if (IsVirtual)
				builder.Append("virtual ");
			if (IsAbstract)
				builder.Append("abstract ");
			if (IsSealed)
				builder.Append("sealed ");
			if (IsOverride)
				builder.Append("override ");

			builder.AppendFormat("event {0} {1}", Type, Name);

			if (withSemicolon && !HasBody)
				builder.Append(";");

			return builder.ToString();
		}

		public override Operation Clone(CompositeType newParent)
		{
			CSharpEvent newEvent = new CSharpEvent(newParent);
			newEvent.CopyFrom(this);
			return newEvent;
		}

		public override string ToString()
		{
			return GetDeclarationLine(false);
		}
	}
}
