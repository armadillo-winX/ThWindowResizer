using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace ThWindowResizer
{
    /// <summary>
    /// ResizerFrameDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class ResizerFrameWindow : Window
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
                    SetFramePosition(value);

                    DispatcherTimer timer = new()
                    {
                        Interval = TimeSpan.FromMilliseconds(50)
                    };

                    timer.Tick += (e, s) =>
                    {
                        WindowPosition gameWindowPosition = GameWindowHandler.GetWindowPosition(value);

                        this.Left = gameWindowPosition.X - 18;
                        this.Top = gameWindowPosition.Y - 18;
                    };

                    timer.Start();
                }
            }
        }

        public ResizerFrameWindow()
        {
            InitializeComponent();
        }

        private void SetFramePosition(Process gameProcess)
        {
            try
            {
                WindowPosition gameWindowPosition = GameWindowHandler.GetWindowPosition(gameProcess);
                int[] sizes = GameWindowHandler.GetWindowSizes(gameProcess);

                this.Left = gameWindowPosition.X - 18;
                this.Top = gameWindowPosition.Y - 18;

                this.Width = sizes[0] + 36;
                this.Height = sizes[1] + 36;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ResizeButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.GameProcess != null)
                {
                    int width = (int)(this.Width - 36);
                    int height = (int)(this.Height - 36);

                    GameWindowHandler.ResizeWindow(this.GameProcess, width, height);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //this.Close();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FixAspectRateCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (FixAspectRateCheckBox.IsChecked == true)
            {
                this.Height = this.Width * 0.75;
            }
        }

        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (FixAspectRateCheckBox.IsChecked == true)
            {
                this.Height = this.Width * 0.75;
            }
        }
    }
}
