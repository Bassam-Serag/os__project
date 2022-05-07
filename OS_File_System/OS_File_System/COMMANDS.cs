using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_File_System
{
    public static class Command
    {
        private static Directory moveTodir(string name, bool is_)
        {
            Directory d = null;
            string path;

            if (name != "..")
            {
                int i = Program.current.Search(name);
                if (i == -1)
                    return null;
                else
                {
                    string n = new string(Program.current.directory[i].FileName);
                    byte at = Program.current.directory[i].FileAttr;
                    int fc = Program.current.directory[i].FirstCluster;
                    d = new Directory(n, at, fc, Program.current);
                    d.ReadDirectory();
                    path = Program.currentPath;
                    path += "\\" + n.Trim(new char[] { '\0', ' ' });
                    if (is_)
                        Program.currentPath = path;
                }
            }
            else
            {
                if (Program.current.parent != null)
                {
                    d = Program.current.parent;
                    d.ReadDirectory();
                    path = Program.currentPath;
                    path = path.Substring(0, path.LastIndexOf('\\'));
                    if (is_)
                        Program.currentPath = path;
                }
                else
                {
                    d = Program.current;
                    d.ReadDirectory();
                }
            }
            return d;

        }

        public static void CD(string name="")
        {
            if (name == "")
            {
                Console.WriteLine("Error: RD command syntax is \n RD + name folder");
            }
            else
            {
                Directory dir = moveTodir(name, true);
                if (dir != null)
                {
                    dir.ReadDirectory();
                    Program.current = dir;
                }
                else
                {
                    Console.WriteLine($"Error : this path \" {name} \" is not exists!");
                }
            }
        }
        public static void RD(string name="")
        {

            if (name == "")
            {
                Console.WriteLine("Error: RD command syntax is \n RD + name folder");
            }

            else
            {
                Directory dir = moveTodir(name, false);
                if (dir != null)
                {
                    dir.DleteDirectory();
                }
                else
                    Console.WriteLine($"Error : this directory \" {name} \" is not exists!");
            }
        }
        public static void Clear(string arg="")
        {
            if(arg =="")
            {
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Error: cls command syntax is \n cls \n function: Clear the screen.");
            }
        }
        public static void Quit(string arg="")
        {
            if (arg == "")
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Error: quit command syntax is \n quit \n function: Quit the shell.");
            }
            
        }
        public static void Help(string argument = "")
        {
            bool is_argument = PARSER.IsCommand(argument);
            if ( is_argument==true)
            {
                if (argument == "cd")
                {
                    Console.WriteLine("Change the current default directory to.");
                    Console.WriteLine(argument + " command syntax is \n cd \n or \n cd [directory]");
                }
                else if (argument == "cls")
                {
                    Console.WriteLine("Clear the screen.");
                    Console.WriteLine(argument + " command syntax is \n cls");
                }
                else if (argument == "dir")
                {
                    Console.WriteLine("List the contents of directory given in the argument.");
                    Console.WriteLine(argument + " command syntax is \n dir \n or \n dir [directory]");
                }
                else if (argument == "quit")
                {
                    Console.WriteLine("Quit the shell.");
                    Console.WriteLine(argument + " command syntax is \n quit");
                }
                else if (argument == "copy")
                {
                    Console.WriteLine("Copies one or more files to another location.");
                    Console.WriteLine(argument + " command syntax is \n copy [source]+ [destination]");
                }
                else if (argument == "del")
                {
                    Console.WriteLine("Deletes one or more files.");
                    Console.WriteLine(argument + " command syntax is \n del [file]+");
                }
                else if (argument == "help")
                {
                    Console.WriteLine("Provides Help information for commands.");
                    Console.WriteLine(argument + " command syntax is \n help \n or \n For more information on a specific command, type help [command]");
                    Console.WriteLine("command - displays help information on that command.");
                }
                else if (argument == "md")
                {
                    Console.WriteLine("Creates a directory.");
                    Console.WriteLine(argument + " command syntax is \n md [directory]");
                }
                else if (argument == "rd")
                {
                    Console.WriteLine("Removes a directory.");
                    Console.WriteLine(argument + " command syntax is \n rd [directory]");
                }
                else if (argument == "rename")
                {
                    Console.WriteLine("Renames a file.");
                    Console.WriteLine(argument + " command syntax is \n rd [fileName] [new fileName]");
                }
                else if (argument == "type")
                {
                    Console.WriteLine("Displays the contents of a text file.");
                    Console.WriteLine(argument + " command syntax is \n type [file]");
                }
                else if (argument == "import")
                {
                    Console.WriteLine("– import text file(s) from your computer ");
                    Console.WriteLine(argument + " command syntax is \n import [destination] [file]+");
                }
                else if (argument == "export")
                {
                    Console.WriteLine("– export text file(s) to your argumentputer ");
                    Console.WriteLine(argument + " command syntax is \n export [destination] [file]+");
                }
            }
            else if (argument == "")
            {
                Console.WriteLine("cd       - Change the current default directory to .");
                Console.WriteLine("           If the argument is not present, report the current directory.");
                Console.WriteLine("           If the directory does not exist an appropriate error should be reported.");
                Console.WriteLine("cls      - Clear the screen.");
                Console.WriteLine("dir      - List the contents of directory .");
                Console.WriteLine("quit     - Quit the shell.");
                Console.WriteLine("copy     - Copies one or more files to another location");
                Console.WriteLine("del      - Deletes one or more files.");
                Console.WriteLine("help     - Provides Help information for commands.");
                Console.WriteLine("md       - Creates a directory.");
                Console.WriteLine("rd       - Removes a directory.");
                Console.WriteLine("rename   - Renames a file.");
                Console.WriteLine("type     - Displays the contents of a text file.");
                Console.WriteLine("import   – import text file(s) from your computer");
                Console.WriteLine("export   – export text file(s) to your computer");
            }
            else if ( is_argument == false)
            {
                Console.WriteLine("Error: =>" + argument + " This command is not supported by the help  utility.");
            }
        }
     

        public static void CreateDirectory(string name="")
        {
            if (name == "")
            {
                Console.WriteLine("Error: md command syntax is \n md [directory]\n[directory] can be a new directory name or fullpath of a new directory\nCreates a directory.");

            }
            else
            {
                if (Program.current.Search(name) == -1)
                {
                    if (MINI_FAT.GetAvilableCluster() != -1)
                    {
                        Directory_Entry d = new Directory_Entry(name, 0x10, 0);
                        Program.current.directory.Add(d);
                        Program.current.WriteDirectory();
                        if (Program.current.parent != null)
                        {
                            Program.current.parent.Update(Program.current.GetDirectoryEntry());
                            Program.current.parent.WriteDirectory();
                        }
                        MINI_FAT.WriteFat();
                    }
                    else
                    {
                        Console.WriteLine("Error : sorry the disk is full!");
                    }
                }
                else
                {
                    Console.WriteLine($"Error : this directory \" {name} \" is already exists!");
                }
            }
        }

    }
}
