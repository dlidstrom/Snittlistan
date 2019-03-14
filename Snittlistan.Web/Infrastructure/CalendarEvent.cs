namespace Snittlistan.Web.Infrastructure
{
    using System;
    using System.IO;

    public abstract class CalendarEvent
    {
        protected const string TextLineFeed = @"\" + "n";

        public abstract void Write(TextWriter writer);

        protected static void WriteWithLineFeeds(TextWriter writer, string desc)
        {
            var escaped = desc.Replace("\n", TextLineFeed);
            WriteNonEmptyLine(writer, escaped.Substring(0, Math.Min(escaped.Length, 74)));
            while (escaped.Length > 74)
            {
                escaped = escaped.Substring(74);
                WriteNonEmptyLine(writer, " " + escaped.Substring(0, Math.Min(escaped.Length, 74)));
            }
        }

        private static void WriteNonEmptyLine(TextWriter writer, string line)
        {
            if (string.IsNullOrWhiteSpace(line) == false)
            {
                writer.WriteLine(line);
            }
        }
    }
}