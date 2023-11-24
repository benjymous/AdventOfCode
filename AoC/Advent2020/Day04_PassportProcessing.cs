namespace AoC.Advent2020;
public class Day04 : IPuzzle
{
    private static IEnumerable<Dictionary<string, string>> ParseData(string input, bool validate)
    {
        return input.Split("\n\n")
            .Select(line => line.Replace("\n", " ").Trim().Split(" "))
            .Select(entries => entries.Select(v => v.Split(":"))
            .Where(pair => !validate || ValidateEntry(pair[0], pair[1]))
            .ToDictionary(pair => pair[0], pair => pair[1]))
            .Where(r => r.Keys.Intersect(expectedFields).Count() == expectedFields.Count);
    }

    static readonly HashSet<string> eyeCols = ["amb", "blu", "brn", "gry", "grn", "hzl", "oth"];
    static readonly HashSet<string> expectedFields = ["byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"];
    private static bool ValidateEntry(string key, string val)
    {
        try
        {
            switch (key)
            {
                case "byr":
                    // Birth Year - four digits; at least 1920 and at most 2002.
                    return int.Parse(val) is >= 1920 and <= 2002;

                case "iyr":
                    // Issue Year - four digits; at least 2010 and at most 2020.
                    return int.Parse(val) is >= 2010 and <= 2020;

                case "eyr":
                    // Expiration Year - four digits; at least 2020 and at most 2030.
                    return int.Parse(val) is >= 2020 and <= 2030;

                case "hgt":
                    // Height - a number followed by either cm or in:
                    {
                        var height = int.Parse(val[..^2]);
                        var unit = val[^2..];
                        return unit switch
                        {
                            "cm" => height is >= 150 and <= 193,// If cm, the number must be at least 150 and at most 193.
                            "in" => height is >= 59 and <= 76,// If in, the number must be at least 59 and at most 76.
                            _ => false,
                        };
                    }

                case "hcl":
                    // Hair Color - a # followed by exactly six characters 0-9 or a-f.
                    return val.Length == 7 && val.StartsWith('#') && val.Skip(1).All(v => v.IsHex());

                case "ecl":
                    // Eye Color - exactly one of: amb blu brn gry grn hzl oth.
                    return eyeCols.Contains(val);

                case "pid":
                    // Passport ID - a nine-digit number, including leading zeroes
                    return val.Length == 9 && val.All(v => v.IsDigit());
            }
        }
        catch
        {
            return false;
        }

        return false;
    }

    public static int Part1(string input) => ParseData(input, false).Count();

    public static int Part2(string input) => ParseData(input, true).Count();

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}