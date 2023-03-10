using GUISFML;

RenderWindow window = new RenderWindow(new VideoMode(900, 900), "TestGui");
GUI Gui = new(new Font("CorporateAPro-Regular.ttf"));
InputTextBox textBox = new(new Vector2f(500, 50)) { Text = "", Position = new Vector2f(100, 100) };
Gui.Add(textBox);
Gui.AttachControl(window);
window.Closed += (object o, EventArgs e) => ((RenderWindow)o).Close();

while (window.IsOpen)
{
    window.DispatchEvents();

    Gui.Draw(window);

    window.Display();
    window.Clear(new Color(122,25,230));
}