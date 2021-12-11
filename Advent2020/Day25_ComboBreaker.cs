using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2020
{
    public class Day25 : IPuzzle
    {
        public string Name { get { return "2020-25"; } }

        const int MagicNumber = 20201227;

        public static IEnumerable<(int loop, Int64 val)> LoopVals(int subject)
        {
            yield return (0, 0); // Keep things zero indexed

            int loop = 1;
            Int64 val = 1;

            while (true) yield return (loop++, val = (val * subject) % MagicNumber);
        }

        private static int CalculatePrivateKey(int publicKey)
            => (int)LoopVals(7)
               .Where(v => v.val == publicKey)
               .First()
               .loop;

        private static int CalculateEncryptionKey(int publicKey, int privateKey)
            => (int)LoopVals(publicKey)
               .ElementAt(privateKey)
               .val;

        public static int Part1(string input)
        {
            var inputs = Util.Parse32(input);
            var doorPublicKey = inputs[0];
            var cardPublicKey = inputs[1];

            var privateKey = CalculatePrivateKey(doorPublicKey);

            return CalculateEncryptionKey(cardPublicKey, privateKey);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
        }
    }
}