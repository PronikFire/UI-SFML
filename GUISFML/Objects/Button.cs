namespace GUISFML
{
    public class Button : GUIObject
    {
        private RectangleShape rectangle;
        private Text _Text = new Text("", GUI.Font);

        public Button(Vector2f Size) 
        { 
            rectangle = new RectangleShape(Size);
        }

        public bool InFocus { get; set; }

        public string Text
        { 
            get => _Text.DisplayedString;
            set 
            { 
                _Text.DisplayedString = value;
                _Text.Position = new(rectangle.Position.X + rectangle.Size.X / 2  - _Text.GetGlobalBounds().Width / 2, rectangle.Position.Y + rectangle.Size.Y / 2  - _Text.GetGlobalBounds().Height);
            }
        }
        public FloatRect GlobalRect { get => rectangle.GetGlobalBounds(); }
        public Vector2f Position 
        { 
            get => rectangle.Position;
            set
            {
                rectangle.Position = value;
                _Text.Position = new(rectangle.Position.X + rectangle.Size.X / 2 - _Text.GetGlobalBounds().Width / 2, rectangle.Position.Y + rectangle.Size.Y / 2 - _Text.GetGlobalBounds().Height);
            }
        }
        public Color Color { get => rectangle.FillColor; set => rectangle.FillColor = value; }
        public uint CharacterSize { get => _Text.CharacterSize; set => _Text.CharacterSize = value; }

        public void Draw(RenderWindow window)
        {
            window.Draw(rectangle);
            window.Draw(_Text);
        }

        public void MouseAction(MouseButtonEventArgs eventArgs)
        {
            if (eventArgs.Button == Mouse.Button.Left && ButtonIsPress is not null)
                ButtonIsPress(this, eventArgs);
        }

        public event EventHandler<MouseButtonEventArgs> ButtonIsPress; 
    }
}
