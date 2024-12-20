using AdventOfCode.lib;

namespace AdventOfCode;

internal static class Day15
{
    [AnswerMethod(2024, 15, 1)]
    public static string Part1(string[] input)
    {
        //input = """
        //    ########
        //    #..O.O.#
        //    ##@.O..#
        //    #...O..#
        //    #.#.O..#
        //    #...O..#
        //    #......#
        //    ########

        //    <^^>>>vv<v>>v<<
        //    """.Split('\n').Select(s => s.Trim()).ToArray();
        var map = input.TakeWhile(s => !string.IsNullOrEmpty(s)).ToArray().CreateGrid();
        var moves = string.Join("", input.SkipWhile(s => !string.IsNullOrEmpty(s)).Skip(1).Select(s => s.Trim()));

        var start = (0, 0);
        foreach (var pos in map)
        {
            if (pos.Value == '@')
            {
                start = pos.Key;
                break;
            }
        }

        foreach (var move in moves)
        {
            // map.PrintGrid();

            var dir = move switch
            {
                '^' => Direction.North,
                '>' => Direction.Right,
                '<' => Direction.Left,
                'v' => Direction.South,
                _ => throw new Exception()
            };

            if (TryMove(start, dir))
            {
                start = start.Plus(dir);
            }

            bool TryMove((int, int) pos, (int, int) dir)
            {
                if (map[pos] == '#')
                {
                    return false;
                }

                if (map[pos] == '.') return true;

                var next = pos.Plus(dir);
                if (TryMove(next, dir))
                {
                    map[next] = map[pos];
                    map[pos] = '.';
                    return true;
                }

                return false;
            }
        }

        var ans = 0;

        foreach (var item in map)
        {
            if (item.Value == 'O')
            {
                ans += item.Key.Item1 * 100 + item.Key.Item2;
            }
        }

        return ans.ToString();
    }

    [AnswerMethod(2024, 15, 2)]
    public static string Part2(string[] input)
    {
        //input = """
        //    ##########
        //    #..O..O.O#
        //    #......O.#
        //    #.OO..O.O#
        //    #..O@..O.#
        //    #O#..O...#
        //    #O..O..O.#
        //    #.OO.O.OO#
        //    #....O...#
        //    ##########

        //    <vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
        //    vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
        //    ><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
        //    <<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
        //    ^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
        //    ^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
        //    >^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
        //    <><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
        //    ^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
        //    v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
        //    """.Split('\n').Select(s => s.Trim()).ToArray();
        var map = input.TakeWhile(s => !string.IsNullOrEmpty(s)).ToArray().CreateGrid();
        var moves = string.Join("", input.SkipWhile(s => !string.IsNullOrEmpty(s)).Skip(1).Select(s => s.Trim()));

        var map2 = new Dictionary<(int, int), char>();
        foreach (var item in map)
        {
            if (item.Value == 'O')
            {
                map2[(item.Key.Item1, item.Key.Item2 * 2)] = '[';
                map2[(item.Key.Item1, item.Key.Item2 * 2 + 1)] = ']';
            }
            else if (item.Value == '#')
            {
                map2[(item.Key.Item1, item.Key.Item2 * 2)] = '#';
                map2[(item.Key.Item1, item.Key.Item2 * 2 + 1)] = '#';
            }
            else if (item.Value == '.')
            {
                map2[(item.Key.Item1, item.Key.Item2 * 2)] = '.';
                map2[(item.Key.Item1, item.Key.Item2 * 2 + 1)] = '.';
            }
            else if (item.Value == '@')
            {
                map2[(item.Key.Item1, item.Key.Item2 * 2)] = '@';
                map2[(item.Key.Item1, item.Key.Item2 * 2 + 1)] = '.';
            }
            else
                throw new Exception();
        }

        map = map2;

        var curr = (0, 0);
        foreach (var pos in map)
        {
            if (pos.Value == '@')
            {
                curr = pos.Key;
                break;
            }
        }

        foreach (var move in moves)
        {
            //PrintMap(map);
            var dir = move switch
            {
                '^' => Direction.North,
                '>' => Direction.Right,
                '<' => Direction.Left,
                'v' => Direction.South,
                _ => throw new Exception()
            };

            var visited = new List<(int, int)>();
            if (CanMove(curr, dir))
            {
                DoMove(curr, dir);
                curr = curr.Plus(dir);
            }

            bool CanMove((int, int) pos, (int, int) dir)
            {
                if (map[pos] == '#')
                {
                    return false;
                }

                if (map[pos] == '.') return true;

                if (map[pos] == '[')
                {
                    if (dir == Direction.North || dir == Direction.South)
                    {
                        return CanMove(pos.Plus(dir), dir) && CanMove(pos.Plus(Direction.Right).Plus(dir), dir);
                    }
                    else
                    {
                        return CanMove(pos.Plus(dir), dir);
                    }
                }

                if (map[pos] == ']')
                {
                    if (dir == Direction.North || dir == Direction.South)
                    {
                        return CanMove(pos.Plus(dir), dir) && CanMove(pos.Plus(Direction.Left).Plus(dir), dir);
                    }
                    else
                    {
                        return CanMove(pos.Plus(dir), dir);
                    }
                }

                if (map[pos] == '@')
                {
                    return CanMove(pos.Plus(dir), dir);
                }

                throw new Exception();
            }

            void DoMove((int, int) pos, (int, int) dir)
            {
                if (map[pos] == '#')
                {
                    return;
                }

                if (map[pos] == '.') return;

                if (map[pos] == '[')
                {
                    if (dir == Direction.North || dir == Direction.South)
                    {
                        DoMove(pos.Plus(dir), dir);
                        DoMove(pos.Plus(Direction.Right).Plus(dir), dir);
                        map[pos.Plus(dir)] = '[';
                        map[pos.Plus(Direction.Right).Plus(dir)] = ']';
                        map[pos] = '.';
                        map[pos.Plus(Direction.Right)] = '.';
                    }
                    else
                    {
                        DoMove(pos.Plus(dir), dir);
                        map[pos] = '.';
                        map[pos.Plus(dir)] = '[';
                    }
                }
                else if (map[pos] == ']')
                {
                    if (dir == Direction.North || dir == Direction.South)
                    {
                        DoMove(pos.Plus(dir), dir);
                        DoMove(pos.Plus(Direction.Left).Plus(dir), dir);
                        map[pos.Plus(dir)] = ']';
                        map[pos.Plus(Direction.Left).Plus(dir)] = '[';
                        map[pos] = '.';
                        map[pos.Plus(Direction.Left)] = '.';
                    }
                    else
                    {
                        DoMove(pos.Plus(dir), dir);
                        map[pos] = '.';
                        map[pos.Plus(dir)] = ']';
                    }
                }
                else if (map[pos] == '@')
                {
                    DoMove(pos.Plus(dir), dir);
                    map[pos.Plus(dir)] = '@';
                    map[pos] = '.';
                }
                else 
                    throw new Exception();
            }
        }

        var ans = 0;

        foreach (var item in map)
        {
            if (item.Value == '[')
            {
                ans += item.Key.Item1 * 100 + item.Key.Item2;
            }
        }

        return ans.ToString();
    }
}