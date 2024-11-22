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
                case "01|1010|01": return 'A';
                case "01|01010|1010|01010|01": return 'A';
                case "1|10101|01010": return 'B';
                case "010|101|01010": return 'C';
                case "1|10101|101": return 'E';
                case "1|1010|10": return 'F';
                case "010|10101|0101": return 'G';
                case "010|101|10101|0101": return 'G';
                case "010|101|10101|101010|0101": return 'G';
                case "1|010|1": return 'H';
                case "010|101|10": return 'J';
                case "010|01|101|10": return 'J';
                case "1|01010|101": return 'K';
                case "1|010|01010|101": return 'K';
                case "1|01": return 'L';
                case "1|1010|010": return 'P';
                case "1|1010|0101": return 'R';
                case "0101|10101|1010": return 'S';
                case "10|01|10": return 'U';
                case "101|10101|101": return 'Z';

                default:
                    {
                        Console.WriteLine(encoded);
                        return '?';
                    }
            }
        }
    }
}
