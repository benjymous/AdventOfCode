using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2019.NPSA
{
    public class Image
    {
        public Image(string data, int width, int height)
        {
            var chars = data.Trim().ToCharArray();

            Width = width;
            Height = height;

            groups = Util.Slice(chars, width * height);

            foreach (var layer in groups.Reverse())
            {
                if (pixelData.Count == 0)
                {
                    pixelData.AddRange(layer);
                }
                else
                {
                    var l = layer.ToArray();
                    for (var i = 0; i < l.Length; ++i)
                    {
                        if (l[i] != '2')
                        {
                            pixelData[i] = l[i];
                        }
                    }
                }
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        readonly IEnumerable<IEnumerable<char>> groups;
        readonly List<char> pixelData = new();

        public int GetChecksum()
        {
            int leastZeros = int.MaxValue;
            int onesByTwos = 0;

            foreach (var g in groups)
            {
                var zeroes = g.Where(i => i == '0').Count();

                if (zeroes < leastZeros)
                {
                    leastZeros = zeroes;
                    onesByTwos = g.Count(i => i == '1') * g.Count(i => i == '2');
                }
            }

            return onesByTwos;
        }

        public override string ToString()
        {
            var final = Util.Slice(pixelData.Select(c => c == '1' ? '#' : ' '), Width);
            var outStr = "";
            foreach (var l in final)
            {
                outStr += string.Join("", l) + '\n';
            }

            return outStr;
        }
    }
}