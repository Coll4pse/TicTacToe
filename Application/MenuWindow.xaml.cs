using System.Windows;
using Domain.Domain;

namespace Application
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }

        private void PlayCrosses(object sender, RoutedEventArgs args)
        {
            var size = int.Parse(gridSize.Text);
            var ui = new GameWindow(size, nick.Text, new Ai(), false);
            ui.Show();
            Close();
        }

        private void PlayNoughts(object sender, RoutedEventArgs e)
        {
            var size = int.Parse(gridSize.Text);
            var ui = new GameWindow(size, nick.Text, new Ai(), true);
            ui.Show();
            Close();
        }

        private void UpSize(object sender, RoutedEventArgs e)
        {
            var size = int.Parse(gridSize.Text);
            if (size >= 5) return;
            gridSize.Text = (size + 1).ToString();
        }

        private void DownSize(object sender, RoutedEventArgs e)
        {
            var size = int.Parse(gridSize.Text);
            if (size <= 3) return;
            gridSize.Text = (size - 1).ToString();
        }
    }
}