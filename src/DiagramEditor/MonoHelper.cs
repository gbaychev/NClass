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

namespace NClass.DiagramEditor
{
	public static class MonoHelper
	{
		static bool isMono;
		static string version;

		static MonoHelper()
		{
			Type monoRuntime = Type.GetType("Mono.Runtime");

			if (monoRuntime != null)
			{
				isMono = true;
				MethodInfo method = monoRuntime.GetMethod("GetDisplayName",
					BindingFlags.NonPublic | BindingFlags.Static);
				
				if (method != null)
					version = method.Invoke(null, null) as string;
				else
					version = "Unknown version";
			}
			else
			{
				isMono = false;
				version = string.Empty;
			}
		}

		public static bool IsRunningOnMono
		{
			get { return isMono; }
		}

		public static string Version
		{
			get { return version; }
		}

		public static bool IsOlderVersionThan(string version)
		{
			version = "Mono " + version;
			return (Version.CompareTo(version) < 0);
		}
	}
}
