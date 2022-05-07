using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_File_System
{
    public class FileEntry : Directory_Entry
    {
        public string content;
        public Directory parent;
        public FileEntry(string name, byte dir_attr, int FirstCluster, Directory parent) :
        base(name, dir_attr, FirstCluster)
        {
            content = string.Empty;
            if (parent!= null)
              this.parent = parent;
        }
        public Directory_Entry GetDirectoryEntry()
        {
            Directory_Entry de = new Directory_Entry(new string(this.FileName), this.FileAttr, this.FirstCluster);
            return de;
        }
        public void WriteFile()
        {
            byte[] content_to_byte = ConvertTypes.StringToBytes(content);
            List<byte[]> bytesls = ConvertTypes.GetNumberOfblock(content_to_byte);
            int first_cluster;
            if (this.FirstCluster != 0)
                first_cluster = this.FirstCluster;
            else
            {
                first_cluster = MINI_FAT.GetAvilableCluster();
                this.FirstCluster= first_cluster;
            }
            int lastCluster = -1;
            for (int i = 0; i < bytesls.Count; i++)
            {
                if (first_cluster != -1)
                {
                    Virtual_Disk.WriteBlock(bytesls[i], first_cluster, 0, bytesls[i].Length);
                    MINI_FAT.SetNextCluster(first_cluster, -1);
                    if (lastCluster != -1)
                        MINI_FAT.SetNextCluster(lastCluster, first_cluster);
                    lastCluster = first_cluster;
                    first_cluster = MINI_FAT.GetAvilableCluster();
                }
            }
        }
        public void ReadFile()
        {
            if (this.FirstCluster != 0)
            {
                content = string.Empty;
                int first_cluster = this.FirstCluster;
                int next = MINI_FAT.GetNextCluster(first_cluster);
                List<byte> ls = new List<byte>();
                do
                {
                    ls.AddRange(Virtual_Disk.ReadBlock(first_cluster));
                    first_cluster = next;
                    if (first_cluster != -1)
                        next = MINI_FAT.GetNextCluster(first_cluster);
                }
                while (next != -1);
                content = ConvertTypes.BytesToString(ls.ToArray());
            }
        }
        public void DeleteFile()
        {
            if (this.FirstCluster != 0)
            {
                int first_cluster = this.FirstCluster;
                int next = MINI_FAT.GetNextCluster(first_cluster);
                do
                {
                    MINI_FAT.SetNextCluster(first_cluster, 0);
                    first_cluster = next;
                    if (first_cluster != -1)
                        next = MINI_FAT.GetNextCluster(first_cluster);
                }
                while (first_cluster != -1);
            }
            
        }
    }
}
