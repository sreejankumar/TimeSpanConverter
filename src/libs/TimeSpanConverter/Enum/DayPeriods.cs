namespace TimeSpanConverter.Enum
{
    public class DayPeriods
    {
        public static DayPeriods Am = new DayPeriods("AM");
        public static DayPeriods Pm = new DayPeriods("PM");

        private DayPeriods(string type)
        {
            _type = type;
        }

        private readonly string _type;

        public override string ToString()
        {
            return _type;
        }
    }
}
