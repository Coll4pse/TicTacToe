using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Domain.Domain;
using Point = System.Drawing.Point;

namespace Application
{
    public partial class GameWindow : Window, IUi
    {
        private readonly Ai ai;

        private readonly Button[,] buttons;

        private readonly bool isPlayerNoughts;

        private readonly Player player;

        private readonly int size;
        
        private bool isAborted;

        public GameWindow(int size, string nick, Ai ai, bool isPlayerNoughts)
        {
            this.size = size;
            player = new Player(nick, this);
            this.ai = ai;
            this.isPlayerNoughts = isPlayerNoughts;
            buttons = new Button[size, size];
            InitializeComponent();
        }

        public void DrawInstance(GameGrid gameGrid)
        {
            if (isAborted) return;

            var grid = gameGrid.Grid;
            for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
                switch (grid[i, j])
                {
                    case CellInstance.Empty:
                        break;
                    case CellInstance.Cross:
                        Dispatcher.Invoke(() =>
                        {
                            buttons[i, j].IsEnabled = false;
                            DrawCross(buttons[i, j]);
                        });
                        break;
                    case CellInstance.Nought:
                        Dispatcher.Invoke(() =>
                        {
                            buttons[i, j].IsEnabled = false;
                            DrawNought(buttons[i, j]);
                        });
                        break;
                }
        }

        public Point GetMove()
        {
            Button button = null;

            MouseButtonEventHandler func = (sender, args) =>
            {
                if (args.Source is Button b && args.ChangedButton == MouseButton.Left)
                    button = b;
            };

            grid.PreviewMouseUp += func;

            while (button == null) Thread.Sleep(100);

            grid.PreviewMouseUp -= func;

            for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
                if (ReferenceEquals(buttons[i, j], button))
                    return new Point(i, j);
            throw new ApplicationException();
        }

        private void OnUGridLoad(object sender, RoutedEventArgs args)
        {
            grid.Rows = grid.Columns = size;
            FillButtons();
            playerInstance.Text = isPlayerNoughts ? "Нолики" : "Крестики";
            aiInstance.Text = isPlayerNoughts ? "Крестики" : "Нолики";
            StartGameAsync();
        }

        private void OnExit(object sender, RoutedEventArgs args)
        {
            var menu = new MenuWindow();
            menu.Show();
            Close();
        }

        private async void StartGameAsync()
        {
            var game = isPlayerNoughts
                ? new Game(0, size, ai, player)
                : new Game(0, size, player, ai);
            game.Move += (sender, gameGrid) => DrawInstance(gameGrid);
            abort.Click += (sender, args) =>
            {
                game.Abort();
                exit.IsEnabled = true;
                isAborted = true;
            };
            await Task.Run(() => game.Start());
            exit.IsEnabled = true;
            if (!isAborted)
                MessageBox.Show(game.Winner == GameWinner.Draw ? "Ничья" : $"Победили {game.Winner}",
                    "Реузльтаты", MessageBoxButton.OK);
        }

        private void FillButtons()
        {
            for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
            {
                buttons[i, j] = new Button();
                grid.Children.Add(buttons[i, j]);
            }
        }

        private static void DrawCross(Button button)
        {
            var canvas = new Canvas
            {
                Width = button.ActualWidth,
                Height = button.ActualHeight
            };
            canvas.Children.Add(new Line
            {
                X1 = 0,
                Y1 = 0,
                X2 = canvas.Width,
                Y2 = canvas.Height,
                Stroke = Brushes.Black,
                StrokeThickness = 2.5
            });
            canvas.Children.Add(new Line
            {
                X1 = 0,
                Y1 = canvas.Height,
                X2 = canvas.Width,
                Y2 = 0,
                Stroke = Brushes.Black,
                StrokeThickness = 2.5
            });
            button.Content = canvas;
        }

        private static void DrawNought(Button button)
        {
            var ellipse = new Ellipse
            {
                Width = button.ActualHeight * 0.75,
                Height = button.ActualHeight * 0.75,
                Stroke = Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                StrokeThickness = 2.5
            };
            button.Content = ellipse;
        }
    }
}