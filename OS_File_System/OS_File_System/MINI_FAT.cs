using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_File_System
{
    internal class MINI_FAT
    {
        public static int[] fat_table = new int[1024];

        public static void CreateFat()
        {
            for (int i = 0; i < fat_table.Length; i++)
            {
                if (i == 0 || i == 4)
                    fat_table[i] = -1;
                else if (i >= 1 && i <= 3)
                    fat_table[i] = i + 1;
                else
                    fat_table[i] = 0;
            }
        }
        
        public static void WriteFat()
        {
            byte[] fat_to_pytes = ConvertTypes.ToBytes(fat_table);
            List<byte[]> lest = ConvertTypes.GetNumberOfblock(fat_to_pytes);

            for (int i = 0; i < lest.Count; i++)
                Virtual_Disk.WriteBlock(lest[i], i + 1, 0, lest[i].Length);
        }
        
        public static void ReadFat()
        {
            List<byte> lest_fat = new();
            for (int i = 1; i < 5; i++)
                lest_fat.AddRange(Virtual_Disk.ReadBlock(i));
            fat_table = ConvertTypes.ToInt(lest_fat.ToArray());
        }
        public static void PrintFat()
        {
            for (int i = 0; i < fat_table.Length; i++)
                Console.WriteLine("Mini Fat[" + i + "] =  " + fat_table[i]);
        }
        public static int GetAvilableCluster()
        {
            for (int i = 0; i < 1024; i++)
            {
                if (fat_table[i] == 0)
                    return i;
            }
            return -1;
        }
        public static void SetNextCluster(int index, int next)
        {
            fat_table[index] = next;
        }
        public static int GetNextCluster(int index)
        {
            return  fat_table[index];
        }
    }
}
