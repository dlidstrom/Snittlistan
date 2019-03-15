namespace Snittlistan.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public abstract class CalendarEvent
    {
        protected const string TextLineFeed = @"\" + "n";

        public abstract void Write(TextWriter writer);

        protected static void WriteWithLineFeeds(TextWriter writer, string desc)
        {
            writer.Write(FoldLines(Escape(desc)));
        }

        private static string Escape(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            value = value.Replace(SerializationConstants.LineBreak, @"\n");
            value = value.Replace("\r", @"\n");
            value = value.Replace("\n", @"\n");
            value = value.Replace(";", @"\;");
            value = value.Replace(",", @"\,");
            return value;
        }

        private static string FoldLines(string incoming)
        {
            // The spec says nothing about trimming, but it seems reasonable...
            var trimmed = incoming.Trim();
            if (trimmed.Length <= 75)
            {
                return trimmed + SerializationConstants.LineBreak;
            }

            const int TakeLimit = 74;

            var firstLine = trimmed.Substring(0, TakeLimit);
            var remainder = trimmed.Substring(TakeLimit, trimmed.Length - TakeLimit);

            var chunkedRemainder = string.Join(SerializationConstants.LineBreak + " ", Chunk(remainder));
            return firstLine + SerializationConstants.LineBreak + " " + chunkedRemainder + SerializationConstants.LineBreak;
        }

        private static IEnumerable<string> Chunk(string str, int chunkSize = 73)
        {
            for (var index = 0; index < str.Length; index += chunkSize)
            {
                yield return str.Substring(index, Math.Min(chunkSize, str.Length - index));
            }
        }

        private static class SerializationConstants
        {
            public const string LineBreak = "\r\n";
        }
    }
}