using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Domain.Domain;
using Domain.Infrastructure;
using Point = System.Drawing.Point;

namespace Application
{
    public partial class GameWindow : Window, IUi
    {
        private bool isAborted;
        
        private Button[,] buttons;
        public int Size { get; }
        
        public GameWindow(int size)
        {
            Size = size;
            InitializeComponent();
        }

        private void OnUGridLoad(object sender, RoutedEventArgs args)
        {
            grid.Rows = grid.Columns = Size;
            FillButtons();
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
            var player = new Player("Collapse", this);
            var ai = new Ai("1");
            var game = new Game(0, Size, player, ai);
            abort.Click += (sender, args) =>
            {
                game.Abort();
                exit.IsEnabled = true;
                isAborted = true;
            };
            await Task.Run(() => game.Start());
            exit.IsEnabled = true;
        }

        private void FillButtons()
        {
            buttons = new Button[Size, Size];
            for (var i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    buttons[i, j] = new Button();
                    grid.Children.Add(buttons[i, j]);
                }
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
                StrokeThickness = 2.5,
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
        
        public void DrawInstance(GameGrid gameGrid)
        {
            if (isAborted) return;
            
            var grid = gameGrid.Grid;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
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

            while (button == null)
            {
                Thread.Sleep(100);
            }

            grid.PreviewMouseUp -= func;
            
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (ReferenceEquals(buttons[i, j], button))
                        return new Point(i, j);
                }
            }
            throw new ApplicationException();
        }
    }
}