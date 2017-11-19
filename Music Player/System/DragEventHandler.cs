using System.Windows.Forms;

namespace System
{
    internal class DragEventHandler
    {
        private Action<object, EventArgs> listBox1_DragDrop;
        private Action<object, DragEventArgs> listBox1_DragEnter;

        public DragEventHandler(Action<object, DragEventArgs> listBox1_DragEnter)
        {
            this.listBox1_DragEnter = listBox1_DragEnter;
        }

        public DragEventHandler(Action<object, EventArgs> listBox1_DragDrop)
        {
            this.listBox1_DragDrop = listBox1_DragDrop;
        }
    }
}