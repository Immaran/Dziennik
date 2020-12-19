using System.Windows.Input;

namespace SBD.Commands
{
    class CustomCommands
    {
        public static readonly RoutedUICommand Export = new RoutedUICommand
        (
            "Export",   // ewentualny text do buttona
            "Export",   // nazwa polecenia
            typeof(CustomCommands), // wlasciciel polecenia

            // skrot klawiszowy ctrl + e
            new InputGestureCollection() { new KeyGesture(Key.E, ModifierKeys.Control) }
        );
    }
}
