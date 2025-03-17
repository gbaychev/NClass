// NClass - Free class diagram editor
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
using System.Collections.Generic;
using NClass.DiagramEditor.Diagrams;

namespace NClass.DiagramEditor
{
    public static class Clipboard
    {
        static IClipboardItem item = null;

        public static IClipboardItem Item
        {
            get => Clipboard.item;
            set => Clipboard.item = value;
        }

        public static bool IsEmpty => (item == null);

        public static void Clear()
        {
            item = null;
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="document"/> is null.
        /// </exception>
        public static PasteResult Paste(IDocument document)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            return item.Paste(document);
        }
    }
}
