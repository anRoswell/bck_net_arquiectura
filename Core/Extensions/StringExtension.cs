namespace Core.Extensions
{
    using System;

    public static class StringExtensions
    {
        public static string ToShortDateFormat(this string dateString)
        {
            string[] formats = { "dd/MM/yyyy HH:mm:ss", "d/MM/yyyy HH:mm:ss", "dd/M/yyyy HH:mm:ss", "d/M/yyyy HH:mm:ss",
                "dd/MM/yyyy H:mm:ss", "d/MM/yyyy H:mm:ss", "dd/M/yyyy H:mm:ss", "d/M/yyyy H:mm:ss",
                "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy", "d/M/yyyy" };

            foreach (string format in formats)
            {
                if (DateTime.TryParseExact(dateString, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    return date.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            return dateString;
        }

        public static bool IsValidDateFormat(this string dateString)
        {
            string shortDateFormat = dateString.ToShortDateFormat();

            string[] formats = { "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy" };

            foreach (string format in formats)
            {
                if (DateTime.TryParseExact(shortDateFormat, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _))
                {
                    return true;
                }
            }
            return false;
        }
    }
}