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
                    case "h":
                        DisplayHelp();
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

                    if (IsFileNameUpdateNeeded(fileName))
                    {

                        UpdateFileName(ref fileName, SongsCnt);
                    }
                    else
                    {
                        SongsCnt++;
                        continue;
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

        /* Function checks whether file needs to be updated*/
        static private bool IsFileNameUpdateNeeded(string fileName)
        {
            int index = 0;
            /* Checks if file needs to be updated (does it hace special character?) */
            while (fileName.IndexOf("$$", index) != -1)
            {
                index++;
            }
            /* Function has special character so update is not needed */
            if (index != 0)
                return false;
            else /* Function doesnt have special haracter so it needs to be updated */
                return true;
                
        }

        /* Function addes prefix to file given as argument */
        static private void UpdateFileName(ref string fileName, int counter)
        {
            string fileWithNumber;
            /* Addes number to file and updates file name */
            fileWithNumber = string.Format("{0:00}", counter);
            /* Addes special character to file */
            fileName = fileWithNumber + " $$" + fileName;
        }

        static private void DisplayHelp()
        {
            Console.WriteLine("This is help");
            Console.WriteLine("Possible parameters:");
            Console.WriteLine("u - updates songs names. Ignores songs already renamed.");
            Console.WriteLine("r - trims songs names. Removes prefixs.");
            Console.WriteLine("No param - updtes all songs names.");
        }
    }
}
