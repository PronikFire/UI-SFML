using System.Runtime.InteropServices;

namespace GUISFML
{
    public interface GUIObject
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
    }

    public interface KeyboardUsage : GUIObject
    {
        public void KeyboardAction(KeyEventArgs e);
    }

    class Helper
    {
        //This is to determine which keyboard layout.
        //The dynamic library user32.dll is used
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

        #region Alphabets
        public static char[] AlphabetEN { get; } = {
            'A',
            'B',
            'C',
            'D',
            'E',
            'F',
            'G',
            'H',
            'I',
            'J',
            'K',
            'L',
            'M',
            'N',
            'O',
            'P',
            'Q',
            'R',
            'S',
            'T',
            'U',
            'V',
            'W',
            'X',
            'Y',
            'Z',
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            ')',
            '!',
            '@',
            '#',
            '$',
            '%',
            '^',
            '&',
            '*',
            '(',
            '[',
            ']',
            ';',
            ',',
            '.',
            "'"[0],
            '/',
            @"\"[0],
            '`',
            '=',
            '-',
            '{',
            '}',
            ':',
            '<',
            '>',
            '"',
            '?',
            '|',
            '~',
            '+',
            '_'
        };
        public static char[] AlphabetRU { get; } = {
            'Ф',
            'И',
            'С',
            'В',
            'У',
            'А',
            'П',
            'Р',
            'Ш',
            'О',
            'Л',
            'Д',
            'Ь',
            'Т',
            'Щ',
            'З',
            'Й',
            'К',
            'Ы',
            'Е',
            'Г',
            'М',
            'Ц',
            'Ч',
            'Н',
            'Я',
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            ')',
            '!',
            '"',
            '№',
            ';',
            '%',
            ':',
            '?',
            '*',
            '(',
            'Х',
            'Ъ',
            'Ж',
            'Б',
            'Ю',
            'Э',
            '.',
            @"\"[0],
            'Ё',
            '=',
            '-',
            ' ',
            ' ',
            ' ',
            ' ',
            ' ',
            ' ',
            ',',
            '/',
            ' ',
            '+',
            '_'
        };
        #endregion
    }
}
