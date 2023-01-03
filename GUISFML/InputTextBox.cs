using System.Globalization;

namespace GUISFML
{
    class InputTextBox : GUIObject
    {
        private Text _Text;
        private RectangleShape rectangle;
        private VertexArray line = new(PrimitiveType.Lines, 2);
        private uint CursorPos = 0;
        private Color color = Color.White;
        private string FullText;

        public bool InFocus { get; set; }

        public Vector2f Position
        {
            get
            {
                return rectangle.Position;
            }
            set
            {
                rectangle.Position = value;
                _Text.Position = value + new Vector2f(2, -10);
                CursorUpdate();
            }
        }
        public Vector2f Size
        {
            get { return rectangle.Size; }
            set
            {
                rectangle.Size = value;
                _Text.CharacterSize = (uint)value.Y;
            }
        }
        public Color Color
        {
            get
            {
                return rectangle.FillColor;
            }
            set
            {
                rectangle.FillColor = value;
                color = value;
            }
        }
        public string Text
        {
            get
            {
                return FullText;
            }
            set
            {
                FullText = value;
                _Text.DisplayedString = value;
                if (_Text.Position.X + _Text.FindCharacterPos((uint)value.Length).X <= rectangle.Position.X + rectangle.Size.X)
                    return;

                uint i = 0;
                do
                {
                    if (_Text.Position.X + _Text.FindCharacterPos((uint)value.Length - i).X <= rectangle.Position.X + rectangle.Size.X)
                    {
                        _Text.DisplayedString = value.Substring(0, value.Length - (int)i);
                        break;
                    }
                    i++;
                } while (i < value.Length);
            }
        }
        public FloatRect GlobalRect
        {
            get
            {
                return rectangle.GetGlobalBounds();
            }
        }

        public InputTextBox(Font font, Vector2f size)
        {
            rectangle = new RectangleShape(size) { FillColor = Color.White };
            _Text = new Text(null, font) { FillColor = Color.Black, Position = new Vector2f(2, -10), CharacterSize = (uint)size.Y };
        }

        public void Draw(RenderWindow window)
        {
            rectangle.Size += new Vector2f(6, 6);
            rectangle.Position -= new Vector2f(3, 3);
            rectangle.FillColor = Color.Black;
            window.Draw(rectangle);

            rectangle.Size -= new Vector2f(6, 6);
            rectangle.Position += new Vector2f(3, 3);
            rectangle.FillColor = color;
            window.Draw(rectangle);

            window.Draw(_Text);

            if (InFocus)
                window.Draw(line);
        }

        private void CursorUpdate()
        {
            line[0] = new Vertex(new Vector2f(_Text.Position.X + _Text.FindCharacterPos(CursorPos - (uint)FullText.IndexOf(_Text.DisplayedString)).X, rectangle.Position.Y + 5), Color.Black);
            line[1] = new Vertex(new Vector2f(_Text.Position.X + _Text.FindCharacterPos(CursorPos - (uint)FullText.IndexOf(_Text.DisplayedString)).X, rectangle.Position.Y + rectangle.Size.Y - 5), Color.Black);
        }

        public void KeyboardAction(KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.Backspace:
                    if (CursorPos == 0)
                        break;
                    CursorPos--;
                    Text = Text.Remove((int)CursorPos, 1);
                    break;
                case Keyboard.Key.Space:
                    _Text.DisplayedString = (_Text.DisplayedString.Insert((int)CursorPos - FullText.IndexOf(_Text.DisplayedString), " ")).Substring(0, _Text.DisplayedString.Length);
                    FullText = FullText.Insert((int)CursorPos, " ");
                    CursorPos++;
                    break;
                case Keyboard.Key.Left:
                    if (CursorPos == 0)
                        break;
                    CursorPos--;
                    if (CursorPos < FullText.IndexOf(_Text.DisplayedString))
                        _Text.DisplayedString = FullText.Substring(FullText.IndexOf(_Text.DisplayedString) - 1, _Text.DisplayedString.Length);
                    break;
                case Keyboard.Key.Right:
                    if (CursorPos == Text.Length)
                        break;
                    CursorPos++;
                    if (CursorPos > FullText.IndexOf(_Text.DisplayedString) + _Text.DisplayedString.Length)
                        _Text.DisplayedString = FullText.Substring(FullText.IndexOf(_Text.DisplayedString) + 1, _Text.DisplayedString.Length);
                    break;
                default:
                    Console.WriteLine(CultureInfo.GetCultureInfo(Helper.GetKeyboardLayout()).Name);
                    var Alphabet = CultureInfo.GetCultureInfo(Helper.GetKeyboardLayout()).Name == "ru-RU" ? Helper.AlphabetRU : Helper.AlphabetEN;
                    if (((int)e.Code < 0 || 57 <= (int)e.Code) || ((int)e.Code >= 36 && (int)e.Code <= 45))
                        break;

                    var code = (int)e.Code;
                    if (!char.IsLetter(Alphabet[(int)e.Code]))
                        Text = Text.Insert((int)CursorPos, e.Shift ? Alphabet[(int)e.Code + ((int)e.Code > 45 ? 11 : 10)].ToString() : Alphabet[(int)e.Code].ToString());
                    else
                        Text = Text.Insert((int)CursorPos, e.Shift ? Alphabet[(int)e.Code].ToString() : Alphabet[(int)e.Code].ToString().ToLower());
                    CursorPos++;
                    break;
            }
            CursorUpdate();
        }

        public void MouseAction(MouseButtonEventArgs e)
        {
            if (_Text.DisplayedString.Length == 0)
                return;

            if (e.X >= _Text.Position.X + (_Text.FindCharacterPos((uint)_Text.DisplayedString.Length).X + _Text.FindCharacterPos((uint)_Text.DisplayedString.Length - 1).X) / 2)
            {
                CursorPos = (uint)_Text.DisplayedString.Length;
            }
            else
            {
                for (uint i = 1; i <= _Text.DisplayedString.Length; i++)
                {
                    if ((i == 1 ? _Text.Position.X : _Text.Position.X + (_Text.FindCharacterPos(i - 1).X + _Text.FindCharacterPos(i - 2).X) / 2) <= e.X
                        && e.X < _Text.Position.X + (_Text.FindCharacterPos(i).X + _Text.FindCharacterPos(i - 1).X) / 2)
                        CursorPos = i - 1;
                }
            }
            CursorUpdate();
        }
    }
}