using System;
using System.Linq;
using crypt.helpers;

namespace crypt
{
    public class CaesarCrypt
    {
        private const string Alphabet =
            "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZабвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ\n\r,.:;!-() ";

        public string Crypt(int key, string value, CryptType type)
        {
            var result = "";
            var counter = new Counter(Alphabet.Length, key);
            foreach (var ch in value)
            {
                var newCh = GetNewCh(counter.GetNewValue(), ch, type);
                result += newCh;
            }

            return result;
        }

        private char GetNewCh(int key, char ch, CryptType type)
        {
            var chIndex = Alphabet.IndexOf(ch);
            if (type == CryptType.Decrypt)
            {
                key = Alphabet.Length - key;
            }

            var copyAlphabetWithShift = Alphabet.Substring(key) + Alphabet.Substring(0, key);
            return copyAlphabetWithShift.ElementAt(chIndex);
        }
    }
}