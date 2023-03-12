using GUISFML;

RenderWindow window = new RenderWindow(new VideoMode(900, 900), "TestGui");
window.Closed += (object? o, EventArgs e) => ((RenderWindow)o).Close();

GUI Gui = new(new Font("CorporateAPro-Regular.ttf"));

InputTextBox textBox = new (new (500, 50)) 
{ Text = "Hello world!!!", Position = new Vector2f(10, 10) };
Gui.Add(textBox);

Button btn = new(new(150, 200))
{ Position = new (10, textBox.Size.Y + 20), Color = Color.Black, Text = "0" };
btn.ButtonIsPress += (object? o, MouseButtonEventArgs e) => ((Button)o).Text = (int.Parse(((Button)o).Text) + 1).ToString();
Gui.Add(btn);

Gui.AttachControl(window);

while (window.IsOpen)
{
    window.DispatchEvents();

    Gui.Draw(window);

    window.Display();
    window.Clear(new Color(122,25,230));
}