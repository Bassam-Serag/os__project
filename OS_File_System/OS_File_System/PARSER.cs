using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_File_System
{
    internal class PARSER
    {
        public static bool IsCommand(string arg)
        {
            return arg == "type" || arg == "import" || arg == "export" || arg == "cd" || arg == "cls" || arg == "dir" || arg == "quit"
                || arg == "copy" || arg == "del"    || arg == "help"   || arg == "md" || arg == "rd"  || arg == "rename";
        }
        public static void Call(string com = "", string arg= "")
        {
            switch (com)
            {
                case "cls":
                    Command.Clear(arg);
                    break;
                case "quit":
                    Command.Quit(arg);
                    break;
                case "copy":
                    break;
                case "help":
                    Command.Help(arg);
                    break;
                case "md":
                    Command.CreateDirectory(arg);
                    break;
                case "cd":
                    Command.CD(arg);
                    break;
                case "rd":
                    Command.RD(arg);
                    break;
            }
        }

    }
}
