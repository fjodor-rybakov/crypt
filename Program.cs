using System;
using System.IO;
using System.Linq;
using crypt.helpers;

namespace crypt
{
    class Program
    {
        static void Main()
        {
            var caesarCrypt = new CaesarCrypt();
            const string inputLine = "Съешь же ещё этих мягких французских булок";
            Console.WriteLine($"Input line: {inputLine}");
            var encryptResult = caesarCrypt.Crypt(5, inputLine, CryptType.Encrypt);
            Console.WriteLine($"Encrypt result: {encryptResult}");
            var decryptResult = caesarCrypt.Crypt(5, encryptResult, CryptType.Decrypt);
            Console.WriteLine($"Decrypt result: {decryptResult}");
            
            Console.WriteLine("Run hack...");
            Hack(Hacker.Encoding.GetBytes(encryptResult));
        }

        private static void Hack(byte[] str)
        {
            var input = str;
            var keys = Hacker.GetKeys(input);
            if (!keys.Any())
            {
                Console.WriteLine("No keys were found");
                return;
            }

            Console.WriteLine("Keys found");
            using var fileStream = new FileStream("output.txt", FileMode.Create);
            using var streamWriter = new StreamWriter(fileStream, Hacker.Encoding);
            var list = keys;
            foreach (var key in list.Select(t => Hacker.Encoding.GetString(t)))
            {
                streamWriter.WriteLine(key);
            }
        }
    }
}