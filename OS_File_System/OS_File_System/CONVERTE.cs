using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_File_System
{
    internal class ConvertTypes
    {
        public static byte[] ToBytes(int[] array)
        {
            byte[] to_bytes = new byte[array.Length * 4];
            int k = 0;
            for (int i = 0; i < array.Length; i++)
            {
                byte[] b = BitConverter.GetBytes(array[i]);
                for (int j = 0; j < 4; j++)
                {
                    to_bytes[k] = b[j];
                    k++;
                }
            }
            return to_bytes;
        }
        public static List<byte[]> GetNumberOfblock(byte[] bytes)
        {
            List<byte[]> lest_of_block = new List<byte[]>();
            int num_of_block = bytes.Length / 1024;
            int remender = bytes.Length % 1024;
            for (int i = 0; i < num_of_block; i++)
            {
                int k = 0;
                byte[] arr = new byte[1024];
                for (int j = i * 1024; ; j++)
                {
                    if (k < 1024)
                    {
                        arr[k] = bytes[j];
                        k++;
                    }
                    else
                        break;
                }
                lest_of_block.Add(arr);
            }
            if (remender > 0)
            {
                byte[] arr = new byte[1024];
                int k = 0;
                for (int i = num_of_block * 1024; ; i++)
                {

                    if (k < remender)
                    {
                        arr[k] = bytes[i];
                        k++;
                    }
                    else
                        break;
                }
                lest_of_block.Add(arr);
            }
            return lest_of_block;
        }
        public static int[] ToInt(byte[] bytes)
        {
            int[] to_int = null;
            to_int = new int[bytes.Length / 4];
            int k = 0;
            for (int i = 0; i < to_int.Length; i++)
            {
                byte[] arr = new byte[4];
                for (int j = 0; j < arr.Length; j++)
                {
                    arr[j] = bytes[k];
                    k++;
                }
                to_int[i] = BitConverter.ToInt32(arr, 0);
            }
            return to_int;
        }

        public static byte[] StringToBytes(string s)
        {
            byte[] bytes = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                bytes[i] = (byte)s[i];
            }
            return bytes;
        }
        public static string BytesToString(byte[] bytes)
        {
            string streng = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
            {
                if ((char)bytes[i] == '\0')
                    break;
                else
                streng += (char)bytes[i];
            }
            return streng;
        }
        public static byte[] DirectoryEntryToBytes(Directory_Entry d)
        {
            byte[] bytes = new byte[32];
            for (int i = 0; i < d.FileName.Length; i++)
                bytes[i] = (byte)d.FileName[i];
            bytes[11] = d.FileAttr;
            int j = 12;
            for (int i = 0; i < d.FileEmpty.Length; i++)
            {
                bytes[j] = d.FileEmpty[i];
                j++;
            }
            byte[] first_cluster = BitConverter.GetBytes(d.FirstCluster);
            for (int i = 0; i < first_cluster.Length; i++)
            {
                bytes[j] = first_cluster[i];
                j++;
            }
            byte[] file_size = BitConverter.GetBytes(d.FileSize);
            for (int i = 0; i < file_size.Length; i++)
            {
                bytes[j] = file_size[i];
                j++;
            }
            return bytes;
        }
        public static Directory_Entry BytesToDirectoryEntry(byte[] bytes)
        {
            char[] file_name = new char[11];
            byte[] file_empty = new byte[12];
            byte[] first_cluster = new byte[4];
            byte[] file_size = new byte[4];
            for (int i = 0; i < file_name.Length; i++)
                file_name[i] = (char)bytes[i];
            byte attr = bytes[11];
            
            int j = 12;
            for (int i = 0; i < file_empty.Length; i++)
            {
                file_empty[i] = bytes[j];
                j++;
            }
           
            for (int i = 0; i < first_cluster.Length; i++)
            {
                first_cluster[i] = bytes[j];
                j++;
            }
            int firstcluster = BitConverter.ToInt32(first_cluster, 0);
            
            for (int i = 0; i < file_size.Length; i++)
            {
                file_size[i] = bytes[j];
                j++;
            }
            int filesize = BitConverter.ToInt32(file_size, 0);
            Directory_Entry de = new Directory_Entry(new string(file_name), attr, firstcluster);
            de.FileEmpty = file_empty;
            de.FileSize = filesize;
            return de;
        }
        
    }
}
