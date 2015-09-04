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
using System.Reflection;
using System.Xml.Serialization;
using System.Collections.Generic;
using NClass.Translations;

namespace NClass.Core
{
	public abstract class Language
	{
		public abstract string Name
		{
			get;
		}

		public abstract string AssemblyName
		{
			get;
		}

		[XmlIgnore]
		public abstract Dictionary<AccessModifier, string> ValidAccessModifiers
		{
			get;
		}

		[XmlIgnore]
		public abstract Dictionary<ClassModifier, string> ValidClassModifiers
		{
			get;
		}

		[XmlIgnore]
		public abstract Dictionary<FieldModifier, string> ValidFieldModifiers
		{
			get;
		}

		[XmlIgnore]
		public abstract Dictionary<OperationModifier, string> ValidOperationModifiers
		{
			get;
		}

		public abstract bool SupportsAssemblyImport
		{
			get;
		}

		public abstract bool SupportsInterfaces
		{
			get;
		}

		public abstract bool SupportsStructures
		{
			get;
		}

		public abstract bool SupportsEnums
		{
			get;
		}

		public abstract bool SupportsDelegates
		{
			get;
		}

		public abstract bool SupportsExplicitImplementation
		{
			get;
		}

		public abstract bool ExplicitVirtualMethods
		{
			get;
		}

		public abstract string DefaultFileExtension
		{
			get;
		}

		protected abstract string[] ReservedNames
		{
			get;
		}

		protected abstract string[] TypeKeywords
		{
			get;
		}

		public bool IsForbiddenName(string name)
		{
			return (
				Contains(ReservedNames, name) ||
				Contains(TypeKeywords, name)
			);
		}

		public bool IsForbiddenTypeName(string name)
		{
			return Contains(ReservedNames, name);
		}

		//TODO: a languageName ne az assembly neve legyen, hanem a Name property!
		public static Language GetLanguage(string languageName)
		{
			try
			{
				string languageString = languageName;
				Assembly assembly = Assembly.Load("NClass." + languageString);

				foreach (Type type in assembly.GetTypes())
				{
					if (type.IsSubclassOf(typeof(Language)))
					{
						object languageInstance = type.InvokeMember("Instance",
							BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty,
							null, null, null);
						return (languageInstance as Language);
					}
				}
				return null;
			}
			catch
			{
				return null;
			}
		}

		private static bool Contains(string[] values, string value)
		{
			if (values == null)
				return false;

			for (int i = 0; i < values.Length; i++)
				if (values[i] == value)
					return true;

			return false;
		}

		/// <exception cref="ArgumentException">
		/// The language does not support explicit interface implementation.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="operation"/> is null.-or-
		/// <paramref name="newParent"/> is null.
		/// </exception>
		protected internal abstract Operation Implement(Operation operation,
			CompositeType newParent, bool explicitly);

		/// <exception cref="ArgumentException">
		/// <paramref name="operation"/> cannot be overridden.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="operation"/> is null.
		/// </exception>
		protected internal abstract Operation Override(Operation operation, CompositeType newParent);

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="name"/> does not fit to the syntax.
		/// </exception>
		public virtual string GetValidName(string name, bool isGenericName)
		{
			if (IsForbiddenName(name))
				throw new BadSyntaxException(Strings.ErrorForbiddenName);

			return name;
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="name"/> does not fit to the syntax.
		/// </exception>
		public virtual string GetValidTypeName(string name)
		{
			if (IsForbiddenTypeName(name))
				throw new BadSyntaxException(Strings.ErrorForbiddenTypeName);

			return name;
		}

		public virtual ClassModifier TryParseClassModifier(string value)
		{
			try {
				return (ClassModifier) Enum.Parse(
					typeof(ClassModifier), value, true);
			}
			catch {
				return ClassModifier.None;
			}
		}

		public virtual AccessModifier TryParseAccessModifier(string value)
		{
			try {
				if (string.IsNullOrEmpty(value))
					return AccessModifier.Default;
				else
					return (AccessModifier) Enum.Parse(typeof(AccessModifier), value, true);
			}
			catch {
				return AccessModifier.Default;
			}
		}

		public virtual OperationModifier TryParseOperationModifier(string value)
		{
			try {
				if (string.IsNullOrEmpty(value)) {
					return OperationModifier.None;
				}
				else {
					return (OperationModifier) Enum.Parse(
						typeof(OperationModifier), value, true);
				}
			}
			catch {
				return OperationModifier.None;
			}
		}

		public abstract bool IsValidModifier(FieldModifier modifier);

		public abstract bool IsValidModifier(OperationModifier modifier);

		public abstract bool IsValidModifier(AccessModifier modifier);

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="operation"/> contains invalid modifier combinations.
		/// </exception>
		protected internal abstract void ValidateOperation(Operation operation);

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="field"/> contains invalid modifier combinations.
		/// </exception>
		protected internal abstract void ValidateField(Field field);

		protected internal abstract ClassType CreateClass();

		/// <exception cref="InvalidOperationException">
		/// The language does not support structures.
		/// </exception>
		protected internal abstract StructureType CreateStructure();

		/// <exception cref="InvalidOperationException">
		/// The language does not support interfaces.
		/// </exception>
		protected internal abstract InterfaceType CreateInterface();

		/// <exception cref="InvalidOperationException">
		/// The language does not support enums.
		/// </exception>
		protected internal abstract EnumType CreateEnum();

		/// <exception cref="InvalidOperationException">
		/// The language does not support delegates.
		/// </exception>
		protected internal abstract DelegateType CreateDelegate();

		protected internal abstract ArgumentList CreateParameterCollection();


		public abstract string GetAccessString(AccessModifier access, bool forCode);

		public abstract string GetFieldModifierString(FieldModifier modifier, bool forCode);

		public abstract string GetOperationModifierString(OperationModifier modifier, bool forCode);

		public abstract string GetClassModifierString(ClassModifier modifier, bool forCode);

		public string GetAccessString(AccessModifier access)
		{
			return GetAccessString(access, false);
		}

		public string GetFieldModifierString(FieldModifier modifier)
		{
			return GetFieldModifierString(modifier, false);
		}

		public string GetOperationModifierString(OperationModifier modifier)
		{
			return GetOperationModifierString(modifier, false);
		}

		public string GetClassModifierString(ClassModifier modifier)
		{
			return GetClassModifierString(modifier, false);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}