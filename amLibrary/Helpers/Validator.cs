using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace amLibrary.Helpers
{
    public static class Validator
    {
        public static bool emailIsValid(string email)
        {
            //expresion;
            string expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
        public static bool IsNotEmpty(string str)
        {
            if (str.Length == 0)
                return false;
            else
                return true;


        }
        public static int Aged(this DateTime birthDate, DateTime offsetDate)
        {
            int result = 0;
            result = offsetDate.Year - birthDate.Year;

            if (offsetDate.DayOfYear < birthDate.DayOfYear)
            {
                result--;
            }

            return result;
        }
        public static bool IsBetween(this long value, long Min, long Max)
        {
            // return (value >= Min && value <= Max);
            if (value >= Min && value <= Max) return true;
            else return false;
        }

    }
}
