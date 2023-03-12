namespace GUISFML
{
    public class InputTextBox : GUIObject, KeyboardUsage
    {
        private Text _Text;
        private RectangleShape rectangle;
        private VertexArray line = new(PrimitiveType.Lines, 2);
        private uint CursorPos = 0;
        private string FullText;
        private int xCam = 0;
        private Color _Color = Color.White;
        private uint _Padding = 3;

        public bool InFocus { get; set; }
        public Vector2f Position
        {
            get => rectangle.Position;
            set
            {
                rectangle.Position = value;
                _Text.Position = value + new Vector2f(_Padding, -_Padding);
                CursorUpdate();
            }
        }
        public Vector2f Size
        {
            get => rectangle.Size;
            set
            {
                rectangle.Size = value;
                _Text.CharacterSize = (uint)value.Y - _Padding;
            }
        }
        public string Text
        {
            get => FullText;
            set
            {
                FullText = value;
                UpdateText();
            }
        }
        public FloatRect GlobalRect
        {
            get => rectangle.GetGlobalBounds();
        }
        public Color Color
        {
            get => _Color;
            set
            {
                _Color = value;
                rectangle.FillColor = value;
            }
        }
        public string Content { get => _Text.DisplayedString; set => _Text.DisplayedString = value; }
        public uint CharacterSize { get => _Text.CharacterSize;}
        public Color CharacterColor { get => _Text.FillColor; set => _Text.FillColor = value; }
        public uint Padding 
        { 
            get => _Padding;
            set
            {
                _Text.CharacterSize += _Padding;
                _Text.CharacterSize -= value;
                _Text.Position += new Vector2f(-_Padding, _Padding);
                _Text.Position += new Vector2f(-value, value);
                _Padding = value;
            }
        }

        public Outline Outline = new ();

        public InputTextBox(Vector2f size)
        {
            rectangle = new RectangleShape(size) { FillColor = Color.White };
            if (GUI.Font == null)
                throw new Exception("Объект был реализован раньше GUI");
            _Text = new Text(null, GUI.Font) { FillColor = Color.Black, Position = new Vector2f(_Padding, -_Padding), CharacterSize = (uint)size.Y - _Padding };
        }

        public void Draw(RenderWindow window)
        {
            if (Outline.Enable)
            {
                rectangle.Size += new Vector2f(Outline.Sise, Outline.Sise);
                rectangle.Position -= new Vector2f(Outline.Sise, Outline.Sise) / 2; 
                rectangle.FillColor = Outline.Color;
                window.Draw(rectangle);

                rectangle.Size -= new Vector2f(Outline.Sise, Outline.Sise);
                rectangle.Position += new Vector2f(Outline.Sise, Outline.Sise) / 2;
                rectangle.FillColor = _Color;
            }

            window.Draw(rectangle);

            window.Draw(_Text);

            if (InFocus)
                window.Draw(line);
        }

        private void CursorUpdate()
        {
            line[0] = new Vertex(new Vector2f(_Text.Position.X + _Text.FindCharacterPos(CursorPos - (uint)xCam).X, rectangle.Position.Y + 5), Color.Black);
            line[1] = new Vertex(new Vector2f(_Text.Position.X + _Text.FindCharacterPos(CursorPos - (uint)xCam).X, rectangle.Position.Y + rectangle.Size.Y - 5), Color.Black);
        }

        //This piece of code was taken from another library.
        //I modified it a little.
        private void UpdateText(int start = 0)
        {
            _Text.DisplayedString = FullText.Substring(start);

            int lastLength;
            do
            {
                lastLength = _Text.DisplayedString.Length;
                if (_Text.FindCharacterPos(CursorPos - (uint)xCam).X >= rectangle.Size.X)
                {
                    _Text.DisplayedString = _Text.DisplayedString.Remove(0, 1);
                    xCam++;
                }
            }
            while (_Text.DisplayedString.Length != lastLength);

            while (_Text.FindCharacterPos((uint)_Text.DisplayedString.Length).X >= rectangle.Size.X)
            {
                _Text.DisplayedString = _Text.DisplayedString.Remove(_Text.DisplayedString.Length - 1, 1);
            }

            CursorUpdate();
        }

        public void KeyboardAction(KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.Backspace:
                    if (CursorPos == 0)
                        break;
                    CursorPos--;
                    FullText = FullText.Remove((int)CursorPos, 1);
                    if (xCam > 0)
                        xCam--;
                    break;
                case Keyboard.Key.Space:
                    FullText = FullText.Insert((int)CursorPos, " ");
                    CursorPos++;
                    break;
                case Keyboard.Key.Left:
                    if (CursorPos == 0)
                        break;
                    CursorPos--;
                    break;
                case Keyboard.Key.Right:
                    if (CursorPos == Text.Length)
                        break;
                    CursorPos++;
                    break;
                default:
                    var Alphabet = CultureInfo.GetCultureInfo(Helper.GetKeyboardLayout()).Name == "ru-RU" ? Helper.AlphabetRU : Helper.AlphabetEN;
                    if (((int)e.Code < 0 || 57 <= (int)e.Code) || ((int)e.Code >= 36 && (int)e.Code <= 45))
                        break;

                    if (!char.IsLetter(Alphabet[(int)e.Code]))
                        FullText = FullText.Insert((int)CursorPos, e.Shift ? Alphabet[(int)e.Code + ((int)e.Code > 45 ? 11 : 10)].ToString() : Alphabet[(int)e.Code].ToString());
                    else
                        FullText = FullText.Insert((int)CursorPos, e.Shift ? Alphabet[(int)e.Code].ToString() : Alphabet[(int)e.Code].ToString().ToLower());
                    
                    CursorPos++;
                    break;
            }

            if (CursorPos <= xCam && xCam > 0)
                xCam--;

            UpdateText(xCam);

            CursorUpdate();
        }

        public void MouseAction(MouseButtonEventArgs e)
        {
            if (_Text.DisplayedString.Length == 0)
                return;

            if (e.X >= _Text.Position.X + (_Text.FindCharacterPos((uint)_Text.DisplayedString.Length).X + _Text.FindCharacterPos((uint)_Text.DisplayedString.Length - 1).X) / 2)
            {
                CursorPos = (uint)(_Text.DisplayedString.Length + xCam);
            }
            else
            {
                for (uint i = 1; i <= _Text.DisplayedString.Length; i++)
                {
                    if ((i == 1 ? _Text.Position.X : _Text.Position.X + (_Text.FindCharacterPos(i - 1).X + _Text.FindCharacterPos(i - 2).X) / 2) <= e.X
                        && e.X < _Text.Position.X + (_Text.FindCharacterPos(i).X + _Text.FindCharacterPos(i - 1).X) / 2)
                        CursorPos = i - 1 + (uint)xCam;
                }
            }
            CursorUpdate();
        }
    }
}