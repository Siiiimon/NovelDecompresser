using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NovelDecompresser
{
    class Program
    {
        static int Main(string[] args)
        {
            // arg parsing
            if (args.Length != 2)
            {
                Console.WriteLine("Please enter a dictionary- and chunks file");
                Console.ReadLine();
                return 1;
            }

            var dictPath = args[0];
            var chunksPath = args[1];
            if (!File.Exists(dictPath))
            {
                Console.WriteLine("The file {0} does not exist", dictPath);
                Console.ReadLine();
                return 1;
            }
            if (!File.Exists(chunksPath))
            {
                Console.WriteLine("The file {0} does not exist", chunksPath);
                Console.ReadLine();
                return 1;
            }


            // read index amount
            StreamReader dictFile = new StreamReader(dictPath);
            string sAmount = dictFile.ReadLine();
            if (!int.TryParse(sAmount, out var amount))
            {
                Console.WriteLine("Failed to parse index amount: {0}", sAmount);
                Console.ReadLine();
                dictFile.Close();
                return 2;
            }

            // read remaining dict words
            string[] words = new string[amount];
            for (int i = 0; i < amount; i++)
            {
                var line = dictFile.ReadLine();
                if (line == null)
                {
                    Console.WriteLine("Invalid index amount");
                    Console.ReadLine();
                    dictFile.Close();
                    return 3;
                }
                else
                {
                    words[i] = line;
                }
            }

            // parse chunks
            StringBuilder text = new StringBuilder();
            WordBuilder word = new WordBuilder();
            StreamReader chunksFile = new StreamReader(chunksPath);
            while (chunksFile.Peek() >= 0)
            {
                char chunk = (char) chunksFile.Read();
                double num = char.GetNumericValue(chunk);
                if (num >= 0 && num <= 9)
                {
                    word.WordIndex += chunk;
                }
                else if (chunk == ' ')
                {
                    try
                    {
                        word.Print(text, words);
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Console.WriteLine(e);
                        Console.Read();
                        dictFile.Close();
                        chunksFile.Close();
                        return 4;
                    }
                    word = new WordBuilder();
                }
                else if (chunk == '!' && word.WordIndex != "")
                {
                    word.Formatting = WordBuilder.Format.Uppercase;
                }
                else if (chunk == '^' && word.WordIndex != "")
                {
                    word.Formatting = WordBuilder.Format.Captialised;
                }
                else if (chunk == '-')
                {
                    text.Remove(text.Length - 1, 1);
                    text.Append(chunk);
                }
                else if (chunk == '.' || chunk == ',' || chunk == '?' || chunk == '!' || chunk == ';' || chunk == ':')
                {
                    text.Remove(text.Length - 1, 1);
                    text.Append(chunk + " ");
                }
                else if (chunk == 'R')
                {
                    text.Append("\n");
                }
                else if (chunk == 'E')
                {
                    Console.WriteLine(text);
                    break;
                }
            }


            dictFile.Close();
            chunksFile.Close();

            Console.ReadLine();
            return 0;
        }
    }
}
