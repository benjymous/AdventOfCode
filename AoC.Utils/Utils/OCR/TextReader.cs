namespace AoC.Utils.OCR
{
    public static class TextReader
    {
        public static string Read(string textBlock) => Read(Util.ParseMatrix<char>(textBlock.Replace("\r", "")));

        public static string Read(char[,] inputData)
        {
            List<char> result = [];
            var cols = inputData.Columns();

            List<List<bool>> currentChar = [];

            foreach (var col in cols)
            {
                var current = col.First() != ' ';
                List<bool> currentCol = [current];

                foreach (var c in col.Skip(1))
                {
                    var next = c != ' ';

                    if (next != current)
                    {
                        currentCol.Add(next);
                        current = next;
                    }
                }

                if (currentCol.All(v => !v))
                {
                    if (currentChar.Count > 0)
                    {
                        result.Add(DecodeLetter(currentChar));
                        currentChar.Clear();
                    }
                }
                else if (currentChar.Count == 0 || currentCol.GetCombinedHashCode() != currentChar.Last().GetCombinedHashCode())
                {
                    currentChar.Add(currentCol);
                }
            }

            if (currentChar.Count > 0)
            {
                result.Add(DecodeLetter(currentChar));
                currentChar.Clear();
            }

            return result.AsString();
        }

        public static char DecodeLetter(List<List<bool>> result)
        {
            //if (result.Count > 3)
            //{
            //    List<List<bool>> nr = [
            //        result.First(),
            //        result.ElementAt(result.Count / 2),
            //        result.Last()
            //    ];

            //    result = nr;
            //}

            var encoded = string.Join("|", result.Select(v => v.Select(c => c ? '1' : '0').AsString()));

            switch (encoded)
            {
                case "01|1010|01": return 'A'; //<< 2019-11
                case "01|01010|1010|01010|01": return 'A'; // 2018-10
                case "1|10101|01010": return 'B';  // << 2022-10
                case "010|101|01010": return 'C';  // << // 2019-08, 2022-10
                case "1|10101|101": return 'E'; // << 2021-13
                case "1|1010|10": return 'F';   // << 2022-10
                //case "010|10101|0101": return 'G';
                case "010|101|10101|0101": return 'G';  // << // 2019-08, 2022-10
                case "010|101|10101|101010|0101": return 'G'; // 2018-10
                case "1|010|1": return 'H';  // << // 2019-08, 2022-10
                case "101|1|101": return 'I'; // << from others examples
                //case "010|101|10": return 'J';
                case "010|01|101|10": return 'J';  // << 2022-10
                //case "1|01010|101": return 'K';
                case "1|010|01010|101": return 'K';  // << 2019-11
                case "1|01": return 'L'; // << 2021-13
                case "010|101|010": return 'O';  // << from others examples
                case "1|1010|010": return 'P'; // << 2021-13
                case "1|1010|0101": return 'R'; // << 2022-10
                case "0101|10101|1010": return 'S'; // 2016-08
                case "10|01|10": return 'U'; // 2019-08
                case "101|10101|101": return 'Z';  // << 2022-10

                default:
                    {
                        Console.WriteLine(encoded);
                        return '?';
                    }
            }
        }
    }
}
