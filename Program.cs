using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mp3NameChanger
{
            
    class MagicStr
    {
        static public string magicStr = "$$";
    }
    class Program
    {
        /* Magic string helps findingg already edited songs names. */

        enum EOptions { EOptions_Update, EOptions_Trim, EOptions_Last};
        static void Main(string[] args)
        {
            Console.WriteLine(" ");
            Console.WriteLine("================= Mp3 name changer v.1 by Adi =================");
            Console.WriteLine(" ");
            EOptions options = EOptions.EOptions_Last;
            
            if (args.Length == 1)
            {
                switch (args[0])
                {
                    case "r":
                        options = EOptions.EOptions_Trim;
                        break;
                    case "u":
                        options = EOptions.EOptions_Update;
                        break;
                    case "h":
                        DisplayHelp();
                        break;
                    default:
                        Console.WriteLine("Invalid param please use help 'h'");
                        options = EOptions.EOptions_Last;
                        return;
                }
            }
            else
            {
                Console.WriteLine("Invalid param please use help 'h'");
                return;
            }

            ushort SongsCnt = 1; //number of the song
            string fileName;  //name of the file
            string filePath; // path to the file
            /* GEt current folder path */
            string path = Directory.GetCurrentDirectory();

            
            /* Find all the files in path  with mp3 extension */
            string[] filePaths = Directory.GetFiles(@path, "*.mp3");

            /* Modify all found files names */
            foreach (string s in filePaths)
            {
               
                fileName = Path.GetFileName(s);
                filePath = Path.GetDirectoryName(s);
                /* Used when updating nemes with numbers */
                if (options == EOptions.EOptions_Update)
                {
                    /* Check if file update needed */
                    if (FileNameChanger.IsFileNameUpdateNeeded(fileName))
                    {
                        /* Updating file */
                        FileNameChanger.UpdateFileName(ref fileName, SongsCnt);
                        Console.WriteLine("Updated ::: {0}", fileName);
                    }
                    else
                    {
                        /* File doesnt have to be updated check next song.
                         * In ordet to maintain right numbering a congs counter needs to be updated. */
                        Console.WriteLine("Nothing to do ::: {0}", fileName);
                        SongsCnt++;
                        continue;
                    }
                }
                /* When removing existing indexes */
                else
                {
                    FileNameChanger.TrimFileName(ref fileName);

                }

                try
                {

                    System.IO.File.Move(s, filePath + "\\" + fileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine("File save failed {0}", e.ToString());
                }
                SongsCnt++;

            }
        }

        static private void DisplayHelp()
        {
            Console.WriteLine("This is help");
            Console.WriteLine("Possible parameters:");
            Console.WriteLine("u - updates songs names. Ignores songs already renamed.");
            Console.WriteLine("r - trims songs names. Removes prefixs.");
        }
    }
}
