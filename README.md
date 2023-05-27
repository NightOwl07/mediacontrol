# MediaControl

MediaControl is a simple Windows application that allows you to control media playback using customizable hotkeys. It provides a convenient way to control various media actions such as play/pause, next track, and previous track without switching to the media player window.

## Features

- Control media playback using hotkeys
- Supports popular media actions: next track, previous track, and play/pause
- Customizable hotkeys (Soon when I refactored the code)
- Runs in the background and minimizes to the system tray

## System Requirements

- Windows operating system
- .NET 6 or higher

## Installation

1. Clone the repository or download the source code as a ZIP file.
2. Open the solution in Visual Studio.
3. Build the solution to compile the application.
4. Run the compiled executable (`MediaControl.exe`).

(will add prebuilt releases soon)

## Usage

1. After launching MediaControl, the application will run in the background and hide its main window.
2. To control media playback, use the following hotkeys:
   - Next track: `Ctrl + Alt + NumPad1`
   - Previous track: `Ctrl + Alt + NumPad2`
   - Play/Pause: `Ctrl + Alt + NumPad3`
3. Press the corresponding hotkey to perform the desired media action.

## Customization

You can customize the hotkeys by modifying the code in the `MainWindow.xaml.cs` file. Locate the `MainWindow_Loaded` method and find the lines that register the hotkeys. Update the `Keys.NumPadX` values and the modifier keys (`Keys.Control` and `Keys.Alt`) according to your preference.

```csharp
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
```

## Contributions

Contributions to the MediaControl project are welcome! If you find any issues or have suggestions for improvements, feel free to open an issue or submit a pull request.

## License

MediaControl is released under GNU General Public License v3.0. See [LICENSE](LICENSE) for more information.
