using GUISFML;

RenderWindow window = new RenderWindow(new VideoMode(900, 900), "TestGui");
window.Closed += (object? o, EventArgs e) => ((RenderWindow)o).Close();

//This must be the first
GUI Gui = new(new Font("CorporateAPro-Regular.ttf"));

InputTextBox textBox = new (new (500, 50)) 
{ Text = "Hello world!!!", Position = new Vector2f(10, 10)};
Gui.Add(textBox);

Button btn = new(new(150, 70))
{ Position = new (10, textBox.Size.Y + 20), Color = Color.Black, Content = "0" };
btn.ButtonIsPress += (object? o, MouseButtonEventArgs e) => ((Button)o).Content = (int.Parse(((Button)o).Content) + 1).ToString();
btn.CharacterSize = 50;
Gui.Add(btn);

Gui.AttachControl(window);

while (window.IsOpen)
{
    window.DispatchEvents();

    Gui.Draw(window);

    window.Display();
    window.Clear(new Color(122,25,230));
}