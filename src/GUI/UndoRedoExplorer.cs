using System.Windows.Forms;
using NClass.Core.UndoRedo;

namespace NClass.GUI
{
    public partial class UndoRedoExplorer : Form
    {
        public UndoRedoExplorer()
        {
            InitializeComponent();
        }

        public void Track(UndoRedoEventArgs args)
        {
            if ((args.Action & UndoRedoAction.RedoClear) == UndoRedoAction.RedoClear)
            {
                redoStack.Items.Clear();
            }
            if ((args.Action & UndoRedoAction.RedoPop) == UndoRedoAction.RedoPop)
            {
                redoStack.Items.RemoveAt(redoStack.Items.Count - 1);
            }
            if ((args.Action & UndoRedoAction.UndoPop) == UndoRedoAction.UndoPop)
            {
                undoStack.Items.RemoveAt(undoStack.Items.Count - 1);
            }

            if ((args.Action & UndoRedoAction.RedoPush) == UndoRedoAction.RedoPush)
            {
                redoStack.Items.Add(args.DebugTag);
            }

            if ((args.Action & UndoRedoAction.UndoPush) == UndoRedoAction.UndoPush)
            {
                undoStack.Items.Add(args.DebugTag);
            }
        }
    }
}
