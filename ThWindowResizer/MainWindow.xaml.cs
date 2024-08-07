using System.Diagnostics;
using System.Windows;

namespace ThWindowResizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Process? GameProcess { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            this.Title = $"{VersionInfo.AppName} ver.{VersionInfo.AppVersion}";
        }

        private void SearchGameProcessButtonClick(object sender, RoutedEventArgs e)
        {
            string[] gameFilesName =
            [
                "東方紅魔郷",
                "th07",
                "th08",
                "th08tr",
                "th09",
                "th09tr",
                "th095",
                "th10",
                "th10tr",
                "th11",
                "th12",
                "th125",
                "th128",
                "th13",
                "th14",
                "th143",
                "th15",
                "th16",
                "th165",
                "th17",
                "th18",
                "th185",
                "th19"
            ];

            foreach (string gameProcessName in gameFilesName)
            {
                if (Process.GetProcessesByName(gameProcessName).Length > 0)
                {
                    this.GameProcess = Process.GetProcessesByName(gameProcessName)[0];

                    MessageBox.Show(this,
                        $"プロセスが見つかりました。\n{this.GameProcess.ProcessName}", "ゲームプロセスの検索",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                }
            }
        }

        private void StartWindowResizerButtonClick(object sender, RoutedEventArgs e)
        {
            if (ResizerTypeComboBox.SelectedIndex > -1 && this.GameProcess != null)
            {
                if (ResizerTypeComboBox.SelectedIndex == 0)
                {
                    ResizerFrameWindow resizerFrameWindow = new()
                    {
                        GameProcess = this.GameProcess
                    };

                    resizerFrameWindow.Show();
                }
                else if (ResizerTypeComboBox.SelectedIndex == 1)
                {
                    DirectResizerWindow directResizerWindow = new()
                    {
                        GameProcess = this.GameProcess
                    };

                    directResizerWindow.Show();
                }
            }
            else if (ResizerTypeComboBox.SelectedIndex == -1)
            {
                MessageBox.Show(this, "リサイザーの種類が選択されていません。", "ウィンドウリサイザーの起動",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (this.GameProcess == null)
            {
                MessageBox.Show(this, "有効なゲームプロセスが見つかっていません。", "ウィンドウリサイザーの起動",
                   MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}