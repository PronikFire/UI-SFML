using GUISFML;

RenderWindow window = new RenderWindow(new VideoMode(900, 900), "TestGui");
InputTextBox textBox = new(new Font("CorporateAPro-Regular.ttf"), new Vector2f(500, 50)) { Text = "Test12345678901011121314", Position = new Vector2f(100, 100) };
GUI Gui = new(window);
Gui.Add(textBox);
window.Closed += (object o, EventArgs e) => ((RenderWindow)o).Close();

while (window.IsOpen)
{
    window.DispatchEvents();

    Gui.Draw(window);

    window.Display();
    window.Clear(Color.Red);
}