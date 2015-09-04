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
using System.Windows.Forms;
using NClass.Translations;

namespace NClass.GUI
{
	public abstract class SimplePlugin : Plugin
	{
		ToolStripMenuItem menuItem;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="environment"/> is null.
		/// </exception>
		protected SimplePlugin(NClassEnvironment environment) : base(environment)
		{
			menuItem = new ToolStripMenuItem();
			menuItem.Text = MenuText;
			menuItem.ToolTipText = string.Format(Strings.PluginTooltip, Name, Author);
			menuItem.Click += new EventHandler(menuItem_Click);
		}

		public override ToolStripItem MenuItem
		{
			get { return menuItem; }
		}

		private void menuItem_Click(object sender, EventArgs e)
		{
			try
			{
				Launch();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Strings.UnknownError,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#region Abstract members

		public abstract string Name
		{
			get;
		}

		public abstract string Author
		{
			get;
		}

		public abstract string MenuText
		{
			get;
		}

		protected abstract void Launch();

		#endregion
	}
}