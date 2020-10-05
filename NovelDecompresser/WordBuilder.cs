using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NovelDecompresser
{
    class WordBuilder
    {
        public enum Format
        {
            None,
            Lowercase,
            Captialised,
            Uppercase
        }

        public Format Formatting;
        public string WordIndex;

        public void Print(StringBuilder text, string[] words)
        {
            if (WordIndex == "")
            {
                return;
            }
            int index = int.Parse(WordIndex);
            if (index < words.Length)
            {
                string word = words[index];
                if (Formatting == Format.Captialised)
                {
                    string firstLetter = word.Substring(0, 1);
                    word = firstLetter.ToUpper() + word.Remove(0, 1);
                } else if (Formatting == Format.Uppercase)
                {
                    word = word.ToUpper();
                } else if (Formatting == Format.Lowercase)
                {
                    word = word.ToLower();
                }
                text.Append(word + " ");
            }
            else
            {
                throw new IndexOutOfRangeException("Invalid word index");
            }
        }

        public WordBuilder()
        {
            this.WordIndex = "";
            this.Formatting = Format.None;
        }
    }
}
