namespace DU.ConsoleApp
{
    internal static class Extensions
    {
        public static bool SizeInBytes { get; set; }

        public static void SizeToConsole(this long size, string name, bool isDir, bool hasInaccessibleEntries)
        {
            Console.Write($"{size.SizeToString(),7}\t");
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = isDir ? ConsoleColor.Green : ConsoleColor.Blue;
            if (hasInaccessibleEntries) Console.Write("[IE]");
            Console.WriteLine(name);
            Console.ForegroundColor = defaultColor;
        }

        public static string SizeToString(this long size)
        {
            if (SizeInBytes) return $"{size}b";
            var curSize = size / Math.Pow(2, 30);
            if (curSize > 1) return $"{curSize:N1}gb";
            curSize = size / Math.Pow(2, 20);
            if (curSize > 1) return $"{curSize:N1}mb";
            curSize = size / Math.Pow(2, 10);
            if (curSize > 1) return $"{curSize:N1}kb";
            return $"{size}b";
        }
    }
}
