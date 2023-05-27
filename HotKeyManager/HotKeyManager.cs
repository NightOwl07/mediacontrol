using System.Runtime.InteropServices;

namespace HotKeyManager
{
    public class HotKeyManager
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        private static extern bool GetMessage(out Message lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        private const int MOD_ALT = 0x0001;
        private const int MOD_CONTROL = 0x0002;
        private const int MOD_SHIFT = 0x0004;
        private const int MOD_WIN = 0x0008;

        public event Action<int> HotKeyPressed;

        private int currentId;

        private Dictionary<int, HotKeyInfo> hotKeys;

        private IntPtr hWnd;

        public HotKeyManager(IntPtr handle)
        {
            currentId = 0;
            hotKeys = new Dictionary<int, HotKeyInfo>();
            hWnd = handle;
        }

        private class HotKeyInfo
        {
            public Keys Key { get; set; }
            public int Modifiers { get; set; }
            public bool IsPressed { get; set; }

            public event Action HotKeyPressed;

            public HotKeyInfo(Action HotKeyPressedAction)
            {
                HotKeyPressed = HotKeyPressedAction;
            }

            public void InvokeHotKeyPressed()
            {
                HotKeyPressed?.Invoke();
            }
        }

        public void RegisterHotKey(Keys key, int modifiers, Action hotKeyPressed)
        {
            int modifierCode = GetModifierCode(modifiers);
            int virtualKeyCode = (int)key;

            int id = hotKeys.Count + 1;

            if (!RegisterHotKey(hWnd, id, modifierCode, virtualKeyCode))
            {
                throw new InvalidOperationException($"The hotkey could not be registered. Error: {Marshal.GetLastSystemError()}");
            }

            hotKeys[id] = new HotKeyInfo(hotKeyPressed) { Key = key, Modifiers = modifiers };
        }

        private int GetModifierCode(int modifiers)
        {
            int modifierCode = 0;

            if ((modifiers & (int)Keys.Alt) != 0)
            {
                modifierCode |= MOD_ALT;
            }

            if ((modifiers & (int)Keys.Control) != 0)
            {
                modifierCode |= MOD_CONTROL;
            }

            if ((modifiers & (int)Keys.Shift) != 0 || (modifiers & (int)Keys.LShiftKey) != 0 || (modifiers & (int)Keys.RShiftKey) != 0)
            {
                modifierCode |= MOD_SHIFT;
            }

            if ((modifiers & (int)Keys.LWin) != 0 || (modifiers & (int)Keys.RWin) != 0)
            {
                modifierCode |= MOD_WIN;
            }

            return modifierCode;
        }

        public void UnregisterHotKey()
        {
            if (!UnregisterHotKey(hWnd, currentId))
            {
                throw new InvalidOperationException("The hotkey could not be removed.");
            }

            hotKeys.Remove(currentId);
            currentId--;
        }

        public void HotKeyLoop(int message, IntPtr wParam)
        {
            const int WM_HOTKEY = 0x0312;

            if (message == WM_HOTKEY)
            {
                int id = wParam.ToInt32();

                if (!hotKeys.TryGetValue(id, out HotKeyInfo hotKeyInfo) || hotKeyInfo == null)
                {
                    return;
                }

                hotKeyInfo.InvokeHotKeyPressed();
            }
        }
    }
}
