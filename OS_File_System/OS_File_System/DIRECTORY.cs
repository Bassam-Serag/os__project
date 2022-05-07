using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_File_System
{
    public class Directory : Directory_Entry
    {
        public List<Directory_Entry> directory;
        public Directory parent;
        public Directory(string filename, byte fileattr, int firstCluster, Directory parant) : base(filename, fileattr, firstCluster)
        {
            if (parant != null)
                this.parent = parant;
        }
        
        public Directory_Entry GetDirectoryEntry()
        {
            Directory_Entry d = new Directory_Entry(new string(this.FileName), this.FileAttr, this.FirstCluster);
            return d;
        }
        public void WriteDirectory()
        {
            byte[] directory_bytes = new byte[directory.Count * 32];
            int j = 0;
            for (int i = 0; i < directory.Count; i++)
            {
                byte[] b = ConvertTypes.DirectoryEntryToBytes(this.directory[i]);
                
                for ( int k = 0; k < b.Length; k++)
                {
                    directory_bytes[j] = b[k];
                    j++;
                }
            }
            List<byte[]> bytes = ConvertTypes.GetNumberOfblock(directory_bytes);
            int first_cluster;
            if (this.FirstCluster != 0)
            {
                first_cluster = this.FirstCluster;
            }
            else
            {
                first_cluster = MINI_FAT.GetAvilableCluster();
                this.FirstCluster = first_cluster;
            }
            int last_cluster = -1;
            for (int i = 0; i < bytes.Count; i++)
            {
                if (first_cluster != -1)
                {
                    Virtual_Disk.WriteBlock(bytes[i], first_cluster, 0, bytes[i].Length);
                    MINI_FAT.SetNextCluster(first_cluster, -1);
                    if (last_cluster != -1)
                    {
                        MINI_FAT.SetNextCluster(last_cluster, first_cluster);
                    }

                    last_cluster = first_cluster;
                    first_cluster = MINI_FAT.GetAvilableCluster();
                }
            }
            if (this.parent != null)
            {
                this.parent.Update(this.GetDirectoryEntry());
                this.parent.WriteDirectory();
            }
            MINI_FAT.WriteFat();
        }

        public void ReadDirectory()
        {
            if (this.FirstCluster != 0)
            {
                directory = new List<Directory_Entry>();
                int first_cluster = this.FirstCluster;
                int next = MINI_FAT.GetNextCluster(first_cluster);
                List<byte> lest = new List<byte>();
                do
                {
                    lest.AddRange(Virtual_Disk.ReadBlock(first_cluster));
                    first_cluster = next;
                    if (first_cluster != -1)
                    {
                        next = MINI_FAT.GetNextCluster(first_cluster);
                    }
                }
                while (next != -1);
                for (int i = 0; i < lest.Count; i++)
                {
                    byte[] arr = new byte[32];
                    for (int k = i * 32, m = 0; m < arr.Length && k < lest.Count; m++, k++)
                    {
                        arr[m] = lest[k];
                    }
                    if (arr[0] == 0)
                    {
                        break;
                    }
                    directory.Add(ConvertTypes.BytesToDirectoryEntry(arr));
                }
            }
        }
        public void DleteDirectory()
        {
            if (this.FirstCluster != 0)
            {
                int cluster = this.FirstCluster;
                int next = MINI_FAT.GetNextCluster(cluster);
                do
                {
                    MINI_FAT.SetNextCluster(cluster, 0);
                    cluster = next;
                    if (cluster != -1)
                    {
                        next = MINI_FAT.GetNextCluster(cluster);
                    }
                }
                while (cluster != -1);
            }
            if (this.parent != null)
            {
                int index = this.parent.Search(new string(this.FileName));
                if (index != -1)
                {
                    this.parent.directory.RemoveAt(index);
                    this.parent.WriteDirectory();
                }
            }
            if (Program.current == this)
            {
                if (this.parent != null)
                {
                    Program.current = this.parent;
                    Program.currentPath = Program.currentPath.Substring(0, Program.currentPath.LastIndexOf('\\'));
                    Program.current.ReadDirectory();
                }
            }
            MINI_FAT.WriteFat();
        }
        public int Search(string name)
        {
            if (name.Length < 11)
            {
                name += "\0";
                for (int i = name.Length + 1; i < 12; i++)
                {
                    name += " ";
                }
            }
            else
            {
                name = name.Substring(0, 11);
            }
            for (int i = 0; i < directory.Count; i++)
            {
                string Name = new(directory[i].FileName);
                if (Name == name)
                {
                    return i;
                }
            }
            return -1;
        }
        public void Update(Directory_Entry d)
        {
            int index = Search(new string(d.FileName));
            if (index != -1)
            {
                directory.RemoveAt(index);
                directory.Insert(index, d);
            }
        }
    }
}
