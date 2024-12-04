namespace AdventOfCode
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AnswerMethodAttribute : Attribute
    {
        public AnswerMethodAttribute(int year, int day, int part)
        {
            if (day < 1 || day > 25)
            {
                throw new ArgumentOutOfRangeException(nameof(day));
            }

            if (part != 1 && part != 2)
            {
                throw new ArgumentOutOfRangeException(nameof(part));
            }

            Year = year;
            Day = day;
            Part = part;
        }

        public int Year { get; }
        public int Day { get; }
        public int Part { get; }
    }
}
