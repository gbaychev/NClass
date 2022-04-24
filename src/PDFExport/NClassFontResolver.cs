// NClass - Free class diagram editor
// Copyright (C) 2020 Georgi Baychev
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

using NClass.DiagramEditor;
using PdfSharp.Fonts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace PDFExport
{
    /// <summary>
    /// Implements the 'glorious' mechanism of
    /// PdfSharp to resolve fonts
    /// </summary>
   public class NClassFontResolver : IFontResolver
    {
        /// <summary>
        /// General information about font, including filename
        /// and calculating a unique id - facename
        /// </summary>
        private class FontInfo
        {
            public string Filepath { get; set; }
            public string FamilyName { get; set; }
            public bool Bold { get; set; }
            public bool Italic { get; set; }

            public string FaceName
            {
                get
                {
                    var facename = FamilyName;
                    if (Bold)
                        facename += "_b";
                    if (Italic)
                        facename += "_i";
                    return facename;
                }
            }

            public static FontInfo FromFont(Font f)
            {
                return new FontInfo
                {
                    FamilyName = f.FontFamily.Name,
                    Bold = f.Bold,
                    Italic = f.Italic
                };
            }

            public override string ToString()
            {
                return $"{Filepath} {FamilyName} {Bold} {Italic}";
            }
        }

        /// <summary>
        /// All the font properties of nclass style,
        /// which could land on a pdf file
        /// </summary>
        private List<Func<Style, Font>> fontProperties = new List<Func<Style, Font>>
        {
            s => s.AbstractNameFont,
            s => s.NameFont,
            s => s.ActorFont,
            s => s.CommentFont,
            s => s.IdentifierFont,
            s => s.MemberFont,
            s => s.PackageFont,
            s => s.RelationshipTextFont,
            s => s.SystemBoundaryFont,
        };

        /// <summary>
        /// Contains a cache of font infos about the properties we are 
        /// interested in. See <see cref="fontProperties"/>
        /// </summary>
        private readonly IDictionary<string, FontInfo> fontInfoCache;
        /// <summary>
        /// Contains the actual binary data of the font files
        /// </summary>
        private readonly IDictionary<string, byte[]> fontDataCache;

        /// <summary>
        /// Ok, since fonts under linux/unix is such clusterfuck,
        /// the only hope is to call fc-list from fontconfig
        /// and to parse its output and it maybe will contain some useful information
        /// about the font files
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, FontInfo> EnumerateUnixFonts()
        {
            var p = new Process
            {
                StartInfo = {FileName = "fc-list", RedirectStandardOutput = true, UseShellExecute = false}
            };

            var cache = new Dictionary<string, FontInfo>(25);

            // each line of fc-list is:
            // <filepath>: <family name>:style="..."
            // style may or may not be comma or whitespace separated
            // seriously, there are some crappy fonts on my system that
            // even fontconfig cannot grok and spews some garbage about
            p.OutputDataReceived += (sender, e) =>
            {
                if (e.Data == null)
                    return;
                var parts = e.Data.Split(':');
                if (!parts[0].EndsWith(".ttf") || parts.Length != 3)
                    return;
                var styles = parts[2].Replace("style=", "").Split(' ', ',');
                var fi = new FontInfo
                {
                    Filepath = parts[0],
                    FamilyName = parts[1].Trim(),
                    Bold = styles.Any(s => s.ToLowerInvariant() == "bold"),
                    Italic = styles.Any(s => s.ToLowerInvariant() == "italic")
                };
                cache[fi.FaceName] = fi;
            };
            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();

            return cache;
        }

        /// <summary>
        /// So this weird shit works as follows:
        /// PdfSharp passes a font name, whether or not it is italic
        /// and/or bold and expects a unique name based on that. <see cref="ResolveTypeface"/>
        /// Then it passes this name to <see cref="GetFont"/> and gets the binary data of the font
        /// So this constructor basically looks up the truetype fonts on a unix system and
        /// loads the binary data of the ones we are interested in
        /// <see cref="fontInfoCache"/>
        /// <see cref="fontDataCache"/>
        /// </summary>
        public NClassFontResolver()
        {
            fontInfoCache = EnumerateUnixFonts();
            fontDataCache = new Dictionary<string, byte[]>();

            var style = Style.CurrentStyle;
            foreach (var fontProperty in fontProperties)
            {
                var font = fontProperty(style);

                var facename = FontInfo.FromFont(font).FaceName;

                if (!fontInfoCache.ContainsKey(facename))
                    continue;

                var fontInfo = fontInfoCache[facename];
                byte[] data;
                using (var fs = new FileStream(fontInfo.Filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var size = fs.Length;
                    if (size > int.MaxValue)
                        throw new Exception("Font file is too big.");
                    var length = (int)size;
                    data = new byte[length];
                    var read = fs.Read(data, 0, length);
                    if (length != read)
                        throw new Exception("Reading font file failed.");
                }

                fontDataCache[facename] = data;
            }
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            var fontInfo = new FontInfo
            {
                FamilyName = familyName,
                Bold = isBold,
                Italic = isItalic
            };

            var fontInfoDefault = new FontInfo
            {
                FamilyName = familyName,
                Bold = false,
                Italic = false
            };

            var facename = fontInfo.FaceName;
            var defaultFacename = fontInfoDefault.FaceName;

            if (fontInfoCache.ContainsKey(facename))
            {
                return new FontResolverInfo(fontInfo.FaceName, fontInfo.Bold, fontInfo.Italic);
            }

            if (fontInfoCache.ContainsKey(defaultFacename))
            {
                return new FontResolverInfo(fontInfoDefault.FaceName, fontInfoDefault.Bold, fontInfoDefault.Italic);
            }

            return null;
        }

        public byte[] GetFont(string faceName)
        {
            return fontDataCache.ContainsKey(faceName) ? fontDataCache[faceName] : null;
        }
    }
}