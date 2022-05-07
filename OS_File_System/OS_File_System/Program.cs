using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_File_System
{
    public static class Program
    {
        public static Directory current;
        public static string currentPath;
        static void Main(string[] args)
        {
           
            Virtual_Disk.Initalize("FILE");
            currentPath = new string(current.FileName);
            currentPath = currentPath.Trim(new char[] { '\0', ' ' });
            while (true)
            {
                Console.Write(currentPath + "\\" + ">");
                string command_input = Console.ReadLine();
                string[] command_input_list = command_input.Split(' ');
                if (command_input.Length == 0)
                {
                    continue;
                }
                List<string> input_lest = new List<string>();
                for (int i = 0; i < command_input_list.Length; i++)
                {
                    if (command_input_list[i] == " ")
                    {
                        continue ;
                    }
                    else
                    {
                        input_lest.Add(command_input_list[i]);
                    }
                }

                string[] argument = input_lest.ToArray();
                string z=argument[0];
                argument[0] = argument[0].ToLower();
                int conter= argument.Length;
                bool is_command = PARSER.IsCommand(argument[0]);
                if (is_command == true)
                {
                    if (conter > 1)
                    {
                        PARSER.Call(argument[0], argument[1]);
                    }
                    else if (conter ==1)
                    {
                        PARSER.Call(argument[0]);
                    }
                }
                else
                {
                    Console.WriteLine("\'" + z + " \' is not recognized as an internal or external command,operable program or batch file.");
                }
            }
        }
    }
}
