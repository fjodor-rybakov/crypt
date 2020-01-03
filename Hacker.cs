using System;
using System.Collections.Generic;
using System.Text;
using crypt.helpers;

namespace crypt
{
    public class Hacker
    {
        private const int MaxKeyLength = 5;
        public static readonly Encoding Encoding = Encoding.UTF8;

        private const string Alphabet =
            "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZабвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ\n\r,.:;!-() ";

        public static List<byte[]> GetKeys(byte[] encryptedBytes)
        {
            var result = new List<byte[]>();
            for (var keyLength = 1; keyLength <= MaxKeyLength; ++keyLength)
            {
                var keySet = new List<HashSet<byte>>();
                var isKeysFound = true;

                for (var keyIndex = 0; keyIndex < keyLength; ++keyIndex)
                {
                    var shiftSet = new HashSet<byte>();
                    for (var i = keyIndex; i < encryptedBytes.Length; i += keyLength)
                    {
                        if (i == keyIndex)
                        {
                            shiftSet.UnionWith(GetPossibleShifts(encryptedBytes[i]));
                        }
                        else
                        {
                            shiftSet.IntersectWith(GetPossibleShifts(encryptedBytes[i]));
                        }
                    }

                    if (shiftSet.Count == 0)
                    {
                        isKeysFound = false;
                        break;
                    }

                    keySet.Add(shiftSet);
                }

                if (!isKeysFound)
                    continue;

                var keys = new List<byte[]>();
                var stack = new Stack<ByteData>();
                stack.Push(new ByteData {Bytes = new byte [keyLength], Length = 0});

                while (stack.Count != 0)
                {
                    var data = stack.Pop();
                    if (data.Length == keyLength)
                    {
                        keys.Add(data.Bytes);
                    }
                    else
                    {
                        foreach (var possibleShift in keySet[data.Length])
                        {
                            var key = new byte [keyLength];
                            Array.Copy(data.Bytes, key, keyLength);
                            key[data.Length] = possibleShift;
                            stack.Push(new ByteData {Bytes = key, Length = data.Length + 1});
                        }
                    }
                }

                result.AddRange(keys);
            }

            return result;
        }

        private static HashSet<byte> GetPossibleShifts(byte encrypted)
        {
            var set = new HashSet<byte>();
            var alphabetBytes = Encoding.GetBytes(Alphabet);
            foreach (var b in alphabetBytes)
            {
                var letter = (char) b;
                var key = encrypted - letter;
                if (key < 0)
                {
                    key = 256 + key;
                }

                set.Add((byte) key);
            }

            return set;
        }
    }
}