using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode
{
    //public static partial class AnswerMethods
    //{
    //    public static partial Func<string, string>[] GetAnswerMethods();
    //}

    internal class Program
    {
        static Dictionary<(int, int), string> inMemoryInputCache = new();

        static void Main(string[] args)
        {
            void PrintUsage()
            {
                Console.WriteLine("Usage: AOC2024.exe [<year> [<day> [<part>]]]");
            }

            if (args.Length > 3)
            {
                PrintUsage();
                return;
            }

            int year;
            int? dayArg = null;
            int? partArg = null;

            if (args.Length >= 1)
            {
                if (!int.TryParse(args[0], out year))
                {
                    PrintUsage();
                    return;
                }

                if (args.Length >= 2)
                {
                    if (!int.TryParse(args[1], out var inputDay))
                    {
                        PrintUsage();
                        return;
                    }

                    dayArg = inputDay;

                    if (args.Length == 3)
                    {
                        if (!int.TryParse(args[1], out var pt))
                        {
                            PrintUsage();
                            return;
                        }

                        partArg = pt;
                    }
                }
            }
            else
            {
                var currentTimeEst = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
                if (currentTimeEst.Month != 12 || currentTimeEst.Day > 25)
                {
                    var nextAdvent = currentTimeEst.Month == 12
                        ? new DateTime(currentTimeEst.Year + 1, 12, 1)
                        : new DateTime(currentTimeEst.Year, 12, 1);
                    var timeToNextAdvent = nextAdvent - currentTimeEst;
                    Console.WriteLine($"It's not yet {nextAdvent} EST! Time until start: {timeToNextAdvent.Days} days, {timeToNextAdvent.Hours} hours, {timeToNextAdvent.Minutes} minutes, and {timeToNextAdvent.Seconds} seconds ");
                    return;
                }

                dayArg = currentTimeEst.Day;
                year = currentTimeEst.Year;
            }

            Span<int> parts = partArg.HasValue ? [partArg.Value] : [2, 1];
            int[] days = dayArg.HasValue ? [dayArg.Value] : Enumerable.Range(1, 25).Reverse().ToArray();

            foreach (var day in days)
            {
                foreach (var part in parts)
                {
                    var runs = AnswerMethods.GetAnswerMethods(year, day, part);
                    if (runs.Length == 0)
                        continue;

                    var runArg = GetInputAsync(year, day).GetAwaiter().GetResult();
                    foreach (var m in runs)
                    {
                        var ans = m(runArg);
                        Console.WriteLine($"""The answer for {new DateTime(year, 12, day).ToShortDateString()} part {part} is: {ans}""");
                    }
                }
            }

            Console.WriteLine();

            static async Task<string> GetInputAsync(int year, int day)
            {
                if (inMemoryInputCache.TryGetValue((year, day), out var cached))
                {
                    return cached;
                }

                var tempDir = Path.GetTempPath();
                var tempFile = $"aoc_{year}_{day}.in";
                var filePath = Path.Combine(tempDir, tempFile);
                if (File.Exists(filePath))
                {
                    var ret = await File.ReadAllTextAsync(filePath);
                    inMemoryInputCache[(year, day)] = ret;
                    return ret;
                }

                var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
                var cookie = config["SessionCookie"];
                if (cookie == null)
                {
                    throw new Exception("Could not find session cookie in user secrets.");
                }

                var client = new HttpClient();
                // Get new cookie by going going to site on browser and inspecting the network request for any day's puzzle input
                client.DefaultRequestHeaders.Add("Cookie", $"session={cookie}");
                var input = await client.GetStringAsync($"https://adventofcode.com/{year}/day/{day}/input");
                input = input.TrimEnd('\n');
                await File.WriteAllTextAsync(filePath, input);
                inMemoryInputCache[(year, day)] = input;
                return input;
            }
        }
    }
}
