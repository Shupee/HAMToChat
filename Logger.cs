using System.Runtime.InteropServices;

namespace HR
{
    public class Logger {

        protected class AnsiInjector {
            private const int STD_OUTPUT_HANDLE = -11;
            private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
            private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

            [DllImport("kernel32.dll")]
            private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

            [DllImport("kernel32.dll")]
            private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr GetStdHandle(int nStdHandle);

            [DllImport("kernel32.dll")]
            public static extern uint GetLastError();

            public static void Inject () {
                var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
                if (!GetConsoleMode(iStdOut, out uint outConsoleMode)) {
                    Console.WriteLine("Failed to get output console mode");
                    Console.ReadKey();
                    return;
                }

                outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
                if (!SetConsoleMode(iStdOut, outConsoleMode)) {
                    Console.WriteLine($"Failed to set output console mode. Error code: {GetLastError()}");
                    Console.ReadKey();
                    return;
                }
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocConsole();

        public static void EnableANSI () {
            AnsiInjector.Inject();
        }

        protected static bool _debug = false;
        public static void EnableDebug (bool enable) {
            _debug = enable;
        }

        protected static bool stylize = true;
        public static void AllowStylization(bool allow) {
            stylize = allow;
        }

        public static void DebugInfo (string message) {
            if (!_debug) return;
            Console.WriteLine($"[{DateTime.Now.ToString("hh:mm:ss")}][DInf]→ {message}");
        }
        public static void DebugWarn (string message) {
            if (!_debug) return;
            Console.WriteLine($"[{DateTime.Now.ToString("hh:mm:ss")}][DWrn]→ {message}");
        }

        public static void Info (string message, ConsoleColor color = ConsoleColor.Gray) {
            Console.ForegroundColor = color;
            Console.WriteLine ($"[{DateTime.Now.ToString("hh:mm:ss")}][Info]→ {message}");
            Console.ResetColor();
        }
        public static void Warn (string message) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{DateTime.Now.ToString("hh:mm:ss")}][Warn]→ {message}");
            Console.ResetColor();
        }
        public static void Error (string message) {
            if (!Program.IsConsole)
            {
                MessageBox.Show(
message,
"Error",
MessageBoxButtons.OK,
MessageBoxIcon.Error,
MessageBoxDefaultButton.Button1,
MessageBoxOptions.DefaultDesktopOnly);
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now.ToString("hh:mm:ss")}][Fail]→ {message}");
            Console.ResetColor();
        }
    }
}
