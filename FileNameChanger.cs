using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mp3NameChanger
{
    class FileNameChanger
    {
        /* Function checks whether file needs to be updated*/
        static public bool IsFileNameUpdateNeeded(string fileName)
        {
            int index = 0;
            /* Checks if file needs to be updated (does it hace special character?) */
            while (fileName.IndexOf(MagicStr.magicStr, index) != -1)
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
        static public void UpdateFileName(ref string fileName, int counter)
        {
            string fileWithNumber;
            /* Addes number to file and updates file name */
            fileWithNumber = string.Format("{0:00}", counter);
            /* Addes special character to file */
            fileName = fileWithNumber + " " + MagicStr.magicStr + fileName;
        }

        static public void TrimFileName(ref string fileName)
        {
            /* Number of characters to trim */
            int magicNbr = 0;
            /* Looping till found last magic number $$ */
            while (fileName.IndexOf(MagicStr.magicStr, magicNbr) != -1)
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
    }
}
