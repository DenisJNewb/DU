namespace DU.ConsoleApp
{
    public static class Extensions
    {
        public static bool SizeInBytes { get; set; }

        public static void SizeToConsole(this long size, string name, bool isDir, bool hasInaccessibleEntries, EndLine endLine)
        {
            Console.Write($"{size.SizeToString(),7}\t");
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = isDir ? ConsoleColor.Green : ConsoleColor.Blue;
            if (hasInaccessibleEntries) Console.Write("[IE]");

            switch (endLine)
            {
                case EndLine.No: { Console.Write(name); break; }
                case EndLine.NewLine: { Console.WriteLine(name); break; }
                case EndLine.Space: { Console.Write($"{name} "); break; }
            }

            Console.ForegroundColor = defaultColor;
        }

        public static string SizeToString(this long size)
        {
            if (SizeInBytes || size < 1000) return $"{size:N1} b";

            var m = "kb";
            double result = size;

            while ((result /= 1024D) >= 1000)
            {
                switch (m)
                {
                    case "kb": { m = "mb"; break; }
                    case "mb": { m = "gb"; break; }
                    case "gb": { m = "pb"; break; }
                    default: throw new NotImplementedException();
                }
            }

            return $"{result:N1}{m}";
        }
    }
}
