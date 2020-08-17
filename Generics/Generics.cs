using AnagramSolver.Contracts.Enums;
using System;

namespace AnagramSolver.Generics
{
    public class Generics
    {
        public static Gender MapIntToGender(int value)
        {
            Gender result;
            if(!Gender.TryParse(value.ToString(), out result))
            {
                throw new Exception($"Value '{value}' is not part of Gender enum");
            }
            return result;
        }

        public static Gender MapStringToGender(string value)
        {
            Gender result;
            if (!Gender.TryParse(value, out result))
            {
                throw new Exception($"Value '{value}' is not part of Gender enum");
            }
            return result;
        }

        public static Weekday MapStringToWeekday(string value)
        {
            Weekday result;
            if (!Weekday.TryParse(value, out result))
            {
                throw new Exception($"Value '{value}' is not part of Weekday enum");
            }
            return result;
        }

        public static General MapValueToEnum<General, T>(T value) where General: struct
        {
            if(!Enum.TryParse(value.ToString(), out General result))
            {
                throw new Exception($"Value '{value}' is not part of General enum");
            }
            return result;
        }
    }
}
