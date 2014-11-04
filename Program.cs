using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NDesk.Options;
namespace Mp3NameChanger
{
            
    class MagicStr
    { /* Magic string helps findingg already edited songs names. */
        static public string magicStr = "$$";
    }
    class Program
    {
       
        enum EOptions { EOptions_Update, EOptions_Trim, EOptions_Last};
        static void Main(string[] args)
        {
            EOptions options = EOptions.EOptions_Last;
            bool show_help = false;
            string path = Directory.GetCurrentDirectory();

            var p = new OptionSet() 
            {
                { "t|trim", "trims song name", v => options = EOptions.EOptions_Trim}, 
                { "u|update", "updates song name with numbers 0-99", v => options = EOptions.EOptions_Update },
			    { "m|magic=", "magic {STRING}. String that will separate" +
                "number and original song name i.e 01 $$NobodyMusic.mp3", v => MagicStr.magicStr = v  },
                { "d|dir=",   "songs {path} i.e. C:\\Mp3 default = current directory", v => path = v  },
			    { "h|help",  "shows help", v => show_help = v != null },
            };

            List<string> extra;
            try
            {
                extra = p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("Some problem: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `--help' for more information.");
                return;
            }

            if (show_help)
            {
                ShowHelp(p);
                return;
            }
  
            Console.WriteLine(" ");
            Console.WriteLine("================= Mp3 name changer v.1 by Adi =================");
            Console.WriteLine(" ");

            ushort SongsCnt = 1; //number of the song
            string fileName;  //name of the file
            string filePath; // path to the file
            string[] filePaths = null;
            
            /* Find all the files in path  with mp3 extension */
            try
            {
                filePaths = Directory.GetFiles(@path, "*.mp3");
            }
            catch (Exception e)
            {
                Console.WriteLine("Get filesfailed {0}", e.ToString());
                return;
            }
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
                    Console.WriteLine("File saveing failed {0}", e.ToString());
                }
                SongsCnt++;

            }
        }
        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: greet [OPTIONS]+ message");
            Console.WriteLine("Greet a list of individuals with an optional message.");
            Console.WriteLine("If no message is specified, a generic greeting is used.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
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
