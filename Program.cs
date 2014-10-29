using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mp3NameChanger
{
    class Program
    {
        static void Main(string[] args)
        {
            bool trimName = false;
            bool updateName = false;
            if (args.Length == 1)
            {
                switch (args[0])
                {
                    case "r":
                        trimName = true;
                        break;
                    case "u":
                        updateName = true;
                        break;
                    default:
                        Console.WriteLine("Invalid param please use help 'h'");
                        break;
                }
            }

            ushort SongsCnt = 1; //number of the song
            string songsCntStr; //number of the song in string
            string fileName;  //name of the file
            string filePath; // path to the file
            /* GEt current folder path */
            string path = Directory.GetCurrentDirectory();
            Console.WriteLine("====================Mp3NameChanger by AdI v.1======================");
            Console.WriteLine(" ");
            /* Find all the files in path  with mp3 extension */
            string[] filePaths = Directory.GetFiles(@path, "*.mp3");

            /* Modify all found files names */
            foreach (string s in filePaths)
            {
               
                fileName = Path.GetFileName(s);
                filePath = Path.GetDirectoryName(s);
                /*Used when updating nemes with numbers */
                if (trimName == false && updateName == false)
                {
                    /* Addes number to file and updates file name */
                    songsCntStr = string.Format("{0:00}", SongsCnt);
                    fileName = songsCntStr + " $$" + fileName;
                }
                else if (updateName)
                {
                    int magicNbr = 0; //number of characters to trim
                    while (fileName.IndexOf("$$", magicNbr) != -1)
                    {
                        magicNbr++;
                    }

                    if (magicNbr != 0)
                    {
                        SongsCnt++;
                        continue;
                    }
                    else
                    {
                        /* Addes number to file and updates file name */
                        songsCntStr = string.Format("{0:00}", SongsCnt);
                        fileName = songsCntStr + " $$" + fileName;
                    }
                }
                /* When removing existing indexes */
                else
                {
                    int magicNbr = 0; //number of characters to trim
                    /* Looping till found last magic number $$ */
                    while (fileName.IndexOf("$$", magicNbr) != -1)
                    {
                        magicNbr++;
                    }
                    /* Only when find sth */
                    if (magicNbr > 0)
                    {
                        fileName = fileName.Remove(0, magicNbr + 1);
                        Console.WriteLine("Trimed ::: {0}", fileName);
                    }
                    else
                    {
                        Console.WriteLine("Nothing to trim ::: {0}", fileName);
                    }

                }

                System.IO.File.Move(s, filePath + "\\" + fileName);
                SongsCnt++;

            }
        }
    }
}
