using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace ThWindowResizer
{
    /// <summary>
    /// DirectResizerWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DirectResizerWindow : Window
    {
        private Process? _gameProcess;

        public Process? GameProcess
        {
            get
            {
                return _gameProcess;
            }

            set
            {
                _gameProcess = value;
                if (value != null)
                {
                    DispatcherTimer timer = new()
                    {
                        Interval = TimeSpan.FromMilliseconds(50)
                    };

                    timer.Tick += (e, s) =>
                    {
                        WindowPosition gameWindowPosition = GameWindowHandler.GetWindowPosition(value);

                        this.Left = gameWindowPosition.X;
                        this.Top = gameWindowPosition.Y - this.Height;
                    };

                    timer.Start();
                }
            }
        }

        public DirectResizerWindow()
        {
            InitializeComponent();
        }

        private void Resize(int width, int height)
        {
            try
            {
                if (this.GameProcess != null)
                {
                    GameWindowHandler.ResizeWindow(this.GameProcess, width, height);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "エラー",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItem1Click(object sender, RoutedEventArgs e)
        {
            Resize(640, 480);
        }

        private void MenuItem2Click(object sender, RoutedEventArgs e)
        {
            Resize(960, 720);
        }

        private void MenuItem3Click(object sender, RoutedEventArgs e)
        {
            Resize(1280, 960);
        }

        private void CloseMenuItemClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
