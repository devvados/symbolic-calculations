using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Symbolic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            AppearanceManager.Current.AccentColor = Colors.Purple;
            ContentSource = MenuLinkGroups.First().Links.First().Source;
        }
    }
}
