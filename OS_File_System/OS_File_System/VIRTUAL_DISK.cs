using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_File_System
{
    internal class Virtual_Disk
    {
        public static FileStream Disk;
        public static void CREATEorOPEN_Disk(string path)
        {
            Disk = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }
        public static int GetFreeSpace()
        {
            return (1024 * 1024) - (int)Disk.Length;
        }
        public static void Initalize(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    CREATEorOPEN_Disk(path);
                    byte[] b = new byte[1024];
                    for (int i = 0; i < b.Length; i++)
                    {
                        b[i] = 0;
                    }

                    WriteBlock(b, 0);
                    MINI_FAT.CreateFat();
                    Directory root = new Directory("B:", 0x10, 5, null);
                    root.WriteDirectory();
                    MINI_FAT.SetNextCluster(5, -1);
                    Program.current = root;
                    MINI_FAT.WriteFat();
                }
                else
                {
                    CREATEorOPEN_Disk(path);
                    MINI_FAT.ReadFat();
                    Directory root = new Directory("B:", 0x10, 5, null);
                    root.ReadDirectory();
                    Program.current = root;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void WriteBlock(byte[] block, int Index, int offset = 0, int count = 1024)
        {
            Disk.Seek(Index * 1024, SeekOrigin.Begin);
            Disk.Write(block, offset, count);
            Disk.Flush();
        }
        public static byte[] ReadBlock(int Index)
        {
            Disk.Seek(Index * 1024, SeekOrigin.Begin);
            byte[] bytes = new byte[1024];
            Disk.Read(bytes, 0, 1024);
            return bytes;
        }
    }
}
