using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diagnostics.Logging
{
    public static class ConsoleUtils
    {
        public static void ConsoleErrorColor()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }

        public static void ConsoleInformationColor()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void ConsoleVerboseColor()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ConsoleWarningColor()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        public static void ConsoleResetColor()
        {
            Console.ResetColor();
        }
    }
}
