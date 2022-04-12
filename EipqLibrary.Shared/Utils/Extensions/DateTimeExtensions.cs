using System;

namespace EipqLibrary.Shared.Utils.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime DropTimePart(this DateTime dateTime)
        {
            return Convert.ToDateTime(dateTime.ToShortDateString());
        }
    }
}
