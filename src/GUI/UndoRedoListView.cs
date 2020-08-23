using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using NClass.Core.UndoRedo;
using NClass.DiagramEditor;
using NClass.GUI.Properties;
using NClass.Translations;

namespace NClass.GUI
{
    public partial class UndoRedoListView : ListBox, IUndoRedoVisualizer
    {
        private int activeIndex = 0;
        private IDocument document;

        public UndoRedoListView()
        {
            InitializeComponent();
        }

        public void Track(UndoRedoEventArgs args)
        {
            if ((args.Action & UndoRedoAction.RedoClear) == UndoRedoAction.RedoClear)
            {
                var cnt = this.Items.Count - 1;
                while (cnt >= 0)
                {
                    if (!(this.Items[cnt] is UndoRedoListBoxItem item))
                        continue;
                    if (item.Type == UndoRedoType.Redo)
                        this.Items.RemoveAt(cnt);
                    else // UndoRedoType == Undo
                        break;

                    cnt = this.Items.Count - 1;
                }
            }
            
            if ((args.Action & UndoRedoAction.UndoPop) == UndoRedoAction.UndoPop)
            {
                if (!(this.Items[activeIndex] is UndoRedoListBoxItem item))
                    return;

                item.Type = UndoRedoType.Redo;
                this.Items[activeIndex--] = item;
                activeIndex = Math.Max(-1, activeIndex);
            }

            if ((args.Action & UndoRedoAction.UndoPush) == UndoRedoAction.UndoPush)
            {
                if (activeIndex == Items.Count - 1)
                {
                    this.Items.Add(new UndoRedoListBoxItem(args.DisplayText, UndoRedoType.Undo));
                    activeIndex++;
                }
                else
                {
                    if (!(this.Items[++activeIndex] is UndoRedoListBoxItem item))
                        return;

                    item.Type = UndoRedoType.Undo;
                    this.Items[activeIndex] = item;
                }
            }

            this.SelectedIndex = -1;
            this.Invalidate();
            this.TopIndex = this.Items.Count - 1;
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            base.OnMeasureItem(e);
            e.ItemHeight += 5;
        }

        private void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || Items.Count <= 0 || !(Items[e.Index] is UndoRedoListBoxItem item))
                return;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e = new DrawItemEventArgs(e.Graphics,
                    e.Font,
                    e.Bounds,
                    e.Index,
                    e.State ^ DrawItemState.Selected,
                    e.ForeColor,
                    SystemColors.ControlDark); //Choose the color
            }

            e.DrawBackground();

            var bounds = new Rectangle(e.Bounds.Left + 12, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height);

            if (e.Index == activeIndex)
            {
                var point1 = new PointF(e.Bounds.X, e.Bounds.Y);
                var point2 = new PointF(point1.X + 10, point1.Y);
                var point3 = new PointF(point1.X, point1.Y + 10);
                var points = new[] {point1, point2, point3};
                e.Graphics.DrawImage(Resources.Play, points);
            }

            using (var brush = new SolidBrush(item.Type == UndoRedoType.Undo || item.Type == UndoRedoType.Source ? SystemColors.ControlText : SystemColors.GrayText))
            {
                e.Graphics.DrawString(item.Text, e.Font, brush, bounds, StringFormat.GenericDefault);
            }

            using (var pen = new Pen(SystemColors.ActiveBorder, 1))
            {
                e.Graphics.DrawLine(pen, e.Bounds.X, e.Bounds.Y + e.Bounds.Height - 1, e.Bounds.X + e.Bounds.Width, e.Bounds.Y + e.Bounds.Height - 1);
            }
        }

        public void SetItems(IEnumerable<UndoRedoListBoxItem> items, UndoRedoSource source)
        {
            BeginUpdate();
            Items.Clear();
            var sourceText = source == UndoRedoSource.FileNew ? Strings.UndoRedoNewFile : Strings.UndoRedoOpenFile;
            Items.Add(new UndoRedoListBoxItem(sourceText, UndoRedoType.Source));
            Items.AddRange(items.ToArray());
            activeIndex = Items.Count - Items.Cast<UndoRedoListBoxItem>().Count(u => u.Type == UndoRedoType.Redo) - 1;
            EndUpdate();
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            if (this.document == null)
                return;

            var idx = this.SelectedIndex;
            var currentActiveIndex = activeIndex;
            if (idx < 0)
                return;

            if (idx == currentActiveIndex)
                return;

            if (idx < currentActiveIndex)
            {
                while (idx < activeIndex)
                {
                    document.Undo();
                }
            }
            else
            {
                while (idx > activeIndex)
                {
                    document.Redo();
                }
            }
        }

        public void SetDocument(IDocument document)
        {
            this.document = document;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Z when document.CanUndo:
                        document.Undo();
                        break;
                    case Keys.Y when document.CanRedo:
                        document.Redo();
                        break;
                }
            }
            base.OnKeyDown(e);
        }
    }
}
