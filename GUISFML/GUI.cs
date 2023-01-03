namespace GUISFML
{
    class GUI
    {
        private List<GUIObject> objects = new();
        private GUIObject SelectedObject = null;

        public GUI(RenderWindow window)
        {
            AttachControl(window);
        }

        void MouseBtnPress(object o, MouseButtonEventArgs e)
        {
            foreach (var obj in objects)
            {
                if (obj.GlobalRect.Contains(e.X, e.Y))
                {
                    if (SelectedObject != null)
                        SelectedObject.InFocus = false;
                    obj.InFocus = true;

                    SelectedObject = obj;
                    obj.MouseAction(e);
                    return;
                }
            }
            if (SelectedObject != null)
            {
                SelectedObject.InFocus = false;
                SelectedObject = null;
            }
        }

        void KeyBoardBtnPress(object o, KeyEventArgs e)
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

        public void Delete(GUIObject obj) { objects.Remove(obj); }
        public void Add(GUIObject obj) { objects.Add(obj); }

        public void AttachControl(RenderWindow window)
        {
            window.MouseButtonPressed += MouseBtnPress;
            window.KeyPressed += KeyBoardBtnPress;
        }
        public void DetachControl(RenderWindow window)
        {
            window.MouseButtonPressed -= MouseBtnPress;
            window.KeyPressed += KeyBoardBtnPress;
        }
    }
}
