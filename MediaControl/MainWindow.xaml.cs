using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace MediaControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static HotKeyManager.HotKeyManager HotKeyManager;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        public const int VK_VOLUME_UP = 0xAF;
        public const int VK_VOLUME_DOWN = 0xAE;

        public const int VK_MEDIA_NEXT_TRACK = 0xB0;
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const int VK_MEDIA_PREV_TRACK = 0xB1;
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var hWnd = new WindowInteropHelper(this).Handle;

            HotKeyManager = new HotKeyManager.HotKeyManager(hWnd);
            
            HwndSource source = HwndSource.FromHwnd(hWnd);
            source.AddHook(new HwndSourceHook(WndProc));

            HotKeyManager.RegisterHotKey(Keys.NumPad1, (int)Keys.Control | (int)Keys.Alt, () =>
            {
                PressKey(VK_MEDIA_NEXT_TRACK);
            });
            
            HotKeyManager.RegisterHotKey(Keys.NumPad2, (int)Keys.Control | (int)Keys.Alt, () =>
            {
                PressKey(VK_MEDIA_PREV_TRACK);
            });

            HotKeyManager.RegisterHotKey(Keys.NumPad3, (int)Keys.Control | (int)Keys.Alt, () =>
            {
                PressKey(VK_MEDIA_PLAY_PAUSE);
            });

            this.Visibility = Visibility.Hidden;
        }

        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            HotKeyManager.HotKeyLoop(msg, wParam);

            return IntPtr.Zero;
        }

        private void PressKey(byte key)
        {
            keybd_event(key, 0, KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
            keybd_event(key, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
        }
    }
}
