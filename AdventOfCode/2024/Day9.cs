namespace AdventOfCode
{
    internal static class Day9
    {
        [AnswerMethod(2024, 9, 1)]
        public static string Part1(string input)
        {
            //input = "2333133121414131402";
            var fs = new List<long>();
            var mode = true;
            var id = 0;
            var cnt = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (mode)
                {
                    for (var j = 0; j < (input[i] - '0'); j++)
                    {
                        fs.Add(id);
                        cnt++;
                    }
                    id++;
                }
                else
                {
                    for (var j = 0; j < (input[i] - '0'); j++)
                    {
                        fs.Add(-1);
                    }
                }

                mode = !mode;
            }

            var fst = 0;
            var lst = fs.Count - 1;

            while (true)
            {
                while (fst < lst && fs[fst] != -1) fst++;
                if (fst >= lst) break;
                while (fst < lst && fs[lst] == -1) lst--;
                if (fst >= lst) break;

                (fs[fst], fs[lst]) = (fs[lst], fs[fst]);
            }

            return fs.Index().Where(tup => tup.Item != -1).Select(tup => tup.Index * tup.Item).Sum().ToString();
        }

        [AnswerMethod(2024, 9, 2)]
        public static string Part2(string input)
        {
            //input = "2333133121414131402";
            var fs = new LinkedList<(long size, int id)>();
            var id = 0;
            var mode = true;
            var alloc = new List<LinkedListNode<(long size, int id)>>();

            for (int i = 0; i < input.Length; i++)
            {
                if (mode)
                {
                    fs.AddLast((input[i] - '0', id));
                    alloc.Add(fs.Last!);
                    id++;
                }
                else
                {
                    fs.AddLast((input[i] - '0', -1));
                }

                mode = !mode;
            }

            alloc.Reverse();
            foreach (var itemToMove in alloc)
            {
                var node = fs.First;
                while (node != null && node != itemToMove)
                {
                    if (node.Value.id == -1 && node.Value.size >= itemToMove.Value.size)
                    {
                        var newNode = new LinkedListNode<(long, int)>(itemToMove.Value);
                        fs.AddBefore(node, newNode);
                        node.Value = (node.Value.size - itemToMove.Value.size, -1);
                        itemToMove.Value = (itemToMove.Value.size, -1);
                        break;
                    }

                    node = node.Next;
                }
            }

            var idx = 0L;
            long sum = 0;
            foreach (var item in fs)
            {
                if (item.id != -1)
                {
                    sum += item.id * (idx * item.size + item.size * (item.size - 1) / 2);
                }

                idx += item.size;
            }

            return sum.ToString();
        }
    }
}