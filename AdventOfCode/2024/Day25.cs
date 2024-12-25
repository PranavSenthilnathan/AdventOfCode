using AdventOfCode.lib;

namespace AdventOfCode;

internal static partial class Day25
{
    [AnswerMethod(2024, 25, 1)]
    public static string Part1(string[] input)
    {
        //input = example;
        var locks = new List<int[]>();
        var keys = new List<int[]>();

        for (var i = 0; i < input.Length; i += 8)
        {
            var buff = new int[5];
            Array.Fill(buff, -1);
            var isLock = input[i].All(x => x == '#');
            for (var j = 0; j < 7; j++)
            {
                foreach (var (idx, ch) in input[i + j].Index())
                {
                    buff[idx] += ch == '#' ? 1 : 0;
                }
            }

            //var nondecreasing = true;
            //var nonincreasing = true;
            //for (global::System.Int32 j = 1; j < 5; j++)
            //{
            //    if (buff[j - 1] < buff[j])
            //    {
            //        nonincreasing = false;
            //    }
            //    if (buff[j - 1] > buff[j])
            //    {
            //        nondecreasing = false;
            //    }
            //}

            //if (isLock && nondecreasing)
            //    locks.Add(buff);
            //else if (!isLock && nonincreasing)
            //    keys.Add(buff);

            if (isLock)
                locks.Add(buff);
            else
                keys.Add(buff);
        }

        //var ans = 0;
        //foreach (var key in keys)
        //{
        //    foreach (var @lock in locks)
        //    {
        //        for (var i = 0; i < 5; i++)
        //        {
        //            for (var j = 4; j >= 0; j--)
        //            {
        //                if (key[i] + @lock[j] >= 6)
        //                {
        //                    goto next;
        //                }
        //            }
        //        }

        //        ans++;

        //    next:;
        //    }
        //}

        var ans = 0;
        foreach (var key in keys)
        {
            foreach (var @lock in locks)
            {
                for (var i = 0; i < 5; i++)
                {
                    
                    if (key[i] + @lock[i] >= 6)
                    {
                        goto next;
                    }
                }

                ans++;

            next:;
            }
        }

        return ans.ToString();
    }

    private static string[] example = """
        #####
        .####
        .####
        .####
        .#.#.
        .#...
        .....

        #####
        ##.##
        .#.##
        ...##
        ...#.
        ...#.
        .....

        .....
        #....
        #....
        #...#
        #.#.#
        #.###
        #####

        .....
        .....
        #.#..
        ###..
        ###.#
        ###.#
        #####

        .....
        .....
        .....
        #....
        #.#..
        #.#.#
        #####
        """.Split('\n', StringSplitOptions.TrimEntries);
}