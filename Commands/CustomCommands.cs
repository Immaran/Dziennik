using System.Windows.Input;

namespace SBD.Commands
{
    class CustomCommands
    {
        //private static RoutedUICommand piecz;
        //static CustomCommands()
        //{
        //    piecz = new RoutedUICommand(
        //    "Upiecz Pizzę", "Piecz",
        //    typeof(CustomCommands));
        //    // dobrym pomysłem jest dodanie tutaj
        //    // skrótów klawiatury:
        //    piecz.InputGestures.Add(new KeyGesture(Key.P,
        //    ModifierKeys.Control));
        //}
        //public static RoutedUICommand Piecz
        //{
        //    get { return piecz; }
        //}
        public static readonly RoutedUICommand Export = new RoutedUICommand
        (
            "Export",
            "Export",
            typeof(CustomCommands),
            new InputGestureCollection() { new KeyGesture(Key.E, ModifierKeys.Control) }
        );
    }
}
