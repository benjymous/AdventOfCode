
using Fluid;

var year = "2024";
var yearFolder = Path.Combine("AoC", $"Advent{year}");
var testFolder = Path.Combine("test", $"Advent{year}");

var parser = new FluidParser();
var daySolutionTemplate = File.ReadAllText("SolutionTemplate.txt");
var dayTestTemplate = File.ReadAllText("TestTemplate.txt");

Console.WriteLine(Directory.GetCurrentDirectory());
Directory.SetCurrentDirectory(@"..\..\..\..");
Console.WriteLine(Directory.GetCurrentDirectory());

if (!Directory.Exists(yearFolder)) _ = Directory.CreateDirectory(yearFolder);
if (!Directory.Exists(testFolder)) _ = Directory.CreateDirectory(testFolder);

for (int day = 1; day <= 25; ++day)
{

    var model = new { Year = year, Day = day.ToString().PadLeft(2, '0') };

    for (int i = 0; i < 2; ++i)
    {
        if (parser.TryParse(i == 0 ? daySolutionTemplate : dayTestTemplate, out var template, out var error))
        {
            var context = new TemplateContext(model);

            var rendered = template.Render(context);

            File.WriteAllText(Path.Combine(i == 0 ? yearFolder : testFolder, $"Day{model.Day}{(i == 0 ? "_" : "Test")}.cs"), rendered, System.Text.Encoding.UTF8);
        }
        else
        {
            Console.WriteLine($"Error: {error}");
            break;
        }
    }
}
