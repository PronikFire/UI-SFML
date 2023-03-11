using System.Net.Http.Headers;

namespace GUISFML
{
    public class GUI
    {
        internal static Font? Font = null;
        public GUI(Font font)
        {
            Font = font;
        }

        public List<GUIObject> objects { get; private set; } = new List<GUIObject>();
        private KeyboardUsage? SelectedObject = null;

        private void MouseBtnPress(object? o, MouseButtonEventArgs e)
        {
            foreach (var obj in objects)
            {
                if (obj.GlobalRect.Contains(e.X, e.Y))
                {
                    if (SelectedObject != null)
                        SelectedObject.InFocus = false;
                    obj.InFocus = true;

                    obj.MouseAction(e);
                    if (obj is KeyboardUsage keyboardUsage)
                        SelectedObject = keyboardUsage;
                    return;
                }
            }
            if (SelectedObject != null)
            {
                SelectedObject.InFocus = false;
                SelectedObject = null;
            }
        }

        private void KeyBoardBtnPress(object? o, KeyEventArgs e)
        {
            if (SelectedObject is null) return;

            SelectedObject.KeyboardAction(e);
            if (e.Code == Keyboard.Key.Enter)
            {
                SelectedObject.InFocus = false;
                SelectedObject = null;
            }
        }

        public void Draw(RenderWindow window)
        {
            foreach (var obj in objects)
            {
                obj.Draw(window);
            }
        }

        public void Delete(GUIObject obj) 
        {
            if (!objects.Contains(obj))
                throw new Exception("Object not found");
            if (SelectedObject == obj)
                SelectedObject = null;
            objects.Remove(obj);
        }
        public void Delete(int index) { Delete(objects[index]); }
        public void Add(GUIObject obj) { objects.Add(obj); }

        /// <summary>
        /// This method is needed so that objects can respond to interactions.
        /// </summary>
        public void AttachControl(Window window)
        {
            window.MouseButtonPressed += MouseBtnPress;
            window.KeyPressed += KeyBoardBtnPress;
        }
        public void DetachControl(Window window)
        {
            window.MouseButtonPressed -= MouseBtnPress;
            window.KeyPressed += KeyBoardBtnPress;
        }
    }
}
