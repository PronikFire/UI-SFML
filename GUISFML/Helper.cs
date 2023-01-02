using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GUISFML
{
    interface GUIObject
    {
        public void Draw(RenderWindow window);
        public FloatRect GlobalRect
        {
            get;
        }
        public bool InFocus
        {
            get; set;
        }
        public void MouseAction(MouseButtonEventArgs e);
        public void KeyboardAction(KeyEventArgs e);
    }
    class Helper
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(int idThread);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowThreadProcessId([In] IntPtr hWnd, [Out, Optional] IntPtr lpdwProcessId);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();
        public static ushort GetKeyboardLayout()
        {
            return (ushort)GetKeyboardLayout(GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero));
        }
    }
}
