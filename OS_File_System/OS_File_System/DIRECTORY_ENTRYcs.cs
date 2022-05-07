using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_File_System
{
    public class Directory_Entry
    {
        public char[] FileName = new char[11];
        public byte FileAttr;
        public byte[] FileEmpty = new byte[12];
        public int FirstCluster;
        public int FileSize;
        public Directory_Entry(){}
        public Directory_Entry(string FileName, byte FileAttr, int FirstCluster)
        {
            this.FileAttr = FileAttr;
            if (FileAttr == 0x0)
            {
                string[] fileName = FileName.Split('.');
                NameExtension(fileName[0].ToCharArray(), fileName[1].ToCharArray());
            }
            else if (FileAttr == 0x10)
            {

                NameNoExtention(FileName.ToCharArray());
            }
            this.FirstCluster = FirstCluster;
        }
        public void NameExtension(char[] name, char[] extension)
        {
            if (name.Length <= 7 && extension.Length == 3)
            {
                int j = 0;
                for (int i = 0; i < name.Length; i++)
                {
                    j++;
                    this.FileName[i] = name[i];
                }
                j++;
                this.FileName[j] = '.';
                for (int i = 0; i < extension.Length; i++)
                {
                    j++;
                    this.FileName[j] = extension[i];
                }
                for (int i = ++j; i < FileName.Length; i++)
                {
                    this.FileName[i] = ' ';
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    this.FileName[i] = name[i];
                }
                this.FileName[7] = '.';
                for (int i = 0, j = 8; i < 4; i++, j++)
                {
                    this.FileName[j] = extension[i];
                }
            }
        }
        public void NameNoExtention(char[] name)
        {
            if (name.Length <= 11)
            {
                int j = 0;
                for (int i = 0; i < name.Length; i++)
                {
                    j++;
                    this.FileName[i] = name[i];
                }
                for (int i = ++j; i < FileName.Length; i++)
                {
                    this.FileName[i] = ' ';
                }
            }
            else
            {
                for (int i = 0; i < 11; i++)
                {
                    this.FileName[i] = name[i];
                }
            }
        }
    }
}
