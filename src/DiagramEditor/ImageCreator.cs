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
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using NClass.Translations;
using System.IO;
using NClass.DiagramEditor.ClassDiagram;

namespace NClass.DiagramEditor
{
	public static class ImageCreator
	{
		const string DialogFilter = "BMP (*.bmp)|*.bmp|GIF (*.gif)|*.gif|" +
			"JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG (*.png)|*.png|" +
			"Transparent PNG (*.png)|*.png|Enhanced Metafile (*.emf)|*.emf";
		const string DialogFilterWithoutTransparentPNG = 
			"BMP (*.bmp)|*.bmp|GIF (*.gif)|*.gif|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
			"PNG (*.png)|*.png|Enhanced Metafile (*.emf)|*.emf";

		static Control control = new Control();
		static string initDir = null;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="document"/> is null.
		/// </exception>
		public static void CopyAsImage(IPrintable document)
		{
			CopyAsImage(document, true);
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="document"/> is null.
		/// </exception>
		public static void CopyAsImage(IPrintable document, bool selectedOnly)
		{
			if (document == null)
				throw new ArgumentNullException("document");

			RectangleF areaF = document.GetPrintingArea(true);
			areaF.Offset(0.5F, 0.5F);
			Rectangle area = Rectangle.FromLTRB((int) areaF.Left, (int) areaF.Top,
				(int) Math.Ceiling(areaF.Right), (int) Math.Ceiling(areaF.Bottom));

			using (Bitmap image = new Bitmap(area.Width, area.Height, PixelFormat.Format24bppRgb))
			using (Graphics g = Graphics.FromImage(image))
			{
				// Set drawing parameters
				g.SmoothingMode = SmoothingMode.HighQuality;
				if (DiagramEditor.Settings.Default.UseClearTypeForImages)
					g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
				else
					g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
				g.TranslateTransform(-area.Left, -area.Top);

				// Draw image
				g.Clear(Style.CurrentStyle.BackgroundColor);
				IGraphics graphics = new GdiGraphics(g);
				document.Print(graphics, selectedOnly, Style.CurrentStyle);

				try
				{
					System.Windows.Forms.Clipboard.SetImage(image);
				}
				catch
				{
					//UNDONE: exception handling of CopyAsImage()
				}
			}
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="document"/> is null.
		/// </exception>
		public static void SaveAsImage(IDocument document)
		{
			SaveAsImage(document, false);
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="document"/> is null.
		/// </exception>
		public static void SaveAsImage(IDocument document, bool selectedOnly)
		{
			if (document == null)
				throw new ArgumentNullException("document");

			using (SaveFileDialog saveAsImageDialog = new SaveFileDialog())
			{
				saveAsImageDialog.DefaultExt = "png";
				if (Settings.Default.UseClearTypeForImages)
					saveAsImageDialog.Filter = DialogFilterWithoutTransparentPNG;
				else
					saveAsImageDialog.Filter = DialogFilter;
				saveAsImageDialog.FilterIndex = 4;
				saveAsImageDialog.FileName = document.GetSelectedElementName() ?? document.Name;
				if (initDir == null && document.Project != null)
					saveAsImageDialog.InitialDirectory = document.Project.GetProjectDirectory();
				else
					saveAsImageDialog.InitialDirectory = initDir;

				if (saveAsImageDialog.ShowDialog() == DialogResult.OK)
				{
					initDir = Path.GetDirectoryName(saveAsImageDialog.FileName);

					string extension = System.IO.Path.GetExtension(saveAsImageDialog.FileName);
					ImageFormat format;

					switch (extension.ToLower())
					{
						case ".bmp":
							format = ImageFormat.Bmp;
							break;

						case ".gif":
							format = ImageFormat.Gif;
							break;

						case ".jpg":
						case ".jpeg":
							format = ImageFormat.Jpeg;
							break;

						case ".emf":
							format = ImageFormat.Emf;
							break;

						case ".png":
						default:
							format = ImageFormat.Png;
							break;
					}
					bool transparent = (saveAsImageDialog.FilterIndex == 5 &&
						!Settings.Default.UseClearTypeForImages);
					
					SaveAsImage(document, saveAsImageDialog.FileName, format,
						selectedOnly, transparent);
				}
			}
		}

		private static void SaveAsImage(IPrintable document, string path,
			ImageFormat format, bool selectedOnly, bool transparent)
		{
			const int Margin = 20;

			RectangleF areaF = document.GetPrintingArea(selectedOnly);
			areaF.Offset(0.5F, 0.5F);
			Rectangle area = Rectangle.FromLTRB((int) areaF.Left, (int) areaF.Top,
				(int) Math.Ceiling(areaF.Right), (int) Math.Ceiling(areaF.Bottom));

			if (format == ImageFormat.Emf) // Save to metafile
			{
				Graphics metaG = control.CreateGraphics();
				IntPtr hc = metaG.GetHdc();
				Graphics g = null;

				try
				{
					// Set drawing parameters
					Metafile meta = new Metafile(path, hc);
					g = Graphics.FromImage(meta);
					g.SmoothingMode = SmoothingMode.HighQuality;
					if (DiagramEditor.Settings.Default.UseClearTypeForImages)
						g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
					else
						g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
					g.TranslateTransform(-area.Left, -area.Top);

					// Draw image
					IGraphics graphics = new GdiGraphics(g);
					document.Print(graphics, selectedOnly, Style.CurrentStyle);

					meta.Dispose();
				}
				catch (Exception ex)
				{
					MessageBox.Show(
						string.Format("{0}\n{1}: {2}", Strings.ErrorInSavingImage,
							Strings.ErrorsReason, ex.Message),
						Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				finally
				{
					metaG.ReleaseHdc();
					metaG.Dispose();
					if (g != null)
						g.Dispose();
				}
			}
			else // Save to rastered image
			{
				int width = area.Width + Margin * 2;
				int height = area.Height + Margin * 2;
				PixelFormat pixelFormat;

				if (transparent)
					pixelFormat = PixelFormat.Format32bppArgb;
				else
					pixelFormat = PixelFormat.Format24bppRgb;

				using (Bitmap image = new Bitmap(width, height, pixelFormat))
				using (Graphics g = Graphics.FromImage(image))
				{
					// Set drawing parameters
					g.SmoothingMode = SmoothingMode.HighQuality;
					if (DiagramEditor.Settings.Default.UseClearTypeForImages && !transparent)
						g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
					else
						g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
					g.TranslateTransform(Margin - area.Left, Margin - area.Top);

					// Draw image
					if (!transparent)
						g.Clear(Style.CurrentStyle.BackgroundColor);

					IGraphics graphics = new GdiGraphics(g);
					document.Print(graphics, selectedOnly, Style.CurrentStyle);

					try
					{
						image.Save(path, format);
					}
					catch (Exception ex)
					{
						MessageBox.Show(
							string.Format("{0}\n{1}: {2}", Strings.ErrorInSavingImage,
								Strings.ErrorsReason, ex.Message),
							Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}
	}
}
