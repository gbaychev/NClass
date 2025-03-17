﻿// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// Copyright (C) 2025 Georgi Baychev

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

namespace NClass.CodeGenerator
{
    public class ToolStripSimplifiedRenderer : ToolStripProfessionalRenderer
    {
        static ToolStripSimplifiedRenderer renderer = new ToolStripSimplifiedRenderer();

        private ToolStripSimplifiedRenderer()
        {
        }

        public static ToolStripSimplifiedRenderer Default
        {
            get { return renderer; }
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is ToolStripDropDown)
                base.OnRenderToolStripBackground(e);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is ToolStripDropDown)
                base.OnRenderToolStripBorder(e);
        }
    }
}
