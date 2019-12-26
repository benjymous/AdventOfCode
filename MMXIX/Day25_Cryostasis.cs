using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    public class Day25 : IPuzzle
    {
        public string Name { get { return "2019-25"; } }

        static List<string> goodItems = new List<string>()
        {
            "fuel cell",
            "festive hat",
            "space heater",
            "hologram",
            "space law space brochure",
            "tambourine",
            "spool of cat6",
            "food ration",
        };

        class SearchDroid : NPSA.ASCIITerminal
        {
            public SearchDroid(string program) : base(program)
            {
                inputs.Enqueue("west");
                inputs.Enqueue("take hologram");

                inputs.Enqueue("north");
                inputs.Enqueue("take space heater");

                inputs.Enqueue("east");
                inputs.Enqueue("take space law space brochure");

                inputs.Enqueue("east");
                inputs.Enqueue("take tambourine");

                inputs.Enqueue("west");
                inputs.Enqueue("west");
                inputs.Enqueue("south");
                inputs.Enqueue("east");

                inputs.Enqueue("east");
                inputs.Enqueue("take festive hat");

                inputs.Enqueue("east");
                inputs.Enqueue("take food ration");

                inputs.Enqueue("east");
                inputs.Enqueue("take spool of cat6");

                inputs.Enqueue("west");
                inputs.Enqueue("west");

                inputs.Enqueue("south");
                inputs.Enqueue("east");
                inputs.Enqueue("east");
                inputs.Enqueue("take fuel cell");

                inputs.Enqueue("east");

                var perms = goodItems.Combinations();
                foreach (var perm in perms)
                {
                    itemCombos.Enqueue(perm);
                }
            }

            void TryCombo(IEnumerable<string> items)
            {
                foreach (var item in goodItems)
                {
                    inputs.Enqueue($"drop {item}");
                }

                foreach (var item in items)
                {
                    inputs.Enqueue($"take {item}");
                }

                inputs.Enqueue("south");
            }

            Queue<string> inputs = new Queue<string>();
            Queue<IEnumerable<string>> itemCombos = new Queue<IEnumerable<string>>();

            public string Run()
            {
                cpu.Run();
                return buffer.Lines.Last();
            }

            public override IEnumerable<string> AutomaticInput()
            {
                var l = buffer.Lines.ToArray();
                buffer.Clear();

                var roomName = l.Where(line => line.StartsWith("==")).LastOrDefault();
               
                if (roomName !=null && roomName == "== Security Checkpoint ==")
                {
                    if (inputs.Count == 0 && itemCombos.Count > 0)
                    {
                        var combo = itemCombos.Dequeue();
                        TryCombo(combo);
                    }
                }

                if (inputs.Any())
                {
                    yield return inputs.Dequeue();
                }
            }
        }

        public static int Part1(string input)
        {
            var droid = new SearchDroid(input);

            bool interactive = false;

            droid.SetDisplay(interactive);
            droid.Interactive = interactive;
            
            var result = droid.Run();

            if (!interactive)
            {
                Console.WriteLine(result);
            }

            if (result.Contains("Oh, hello")) return int.Parse(result.Split()[11]);

            return 0;
        }

        public void Run(string input, System.IO.TextWriter console)
        {
            console.WriteLine("- Pt1 - " + Part1(input));
        }
    }
}