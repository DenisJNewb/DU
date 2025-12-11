namespace DU.ConsoleApp
{
    internal static class DUProgram
    {
        private static void Main(string[] args)
        {
            DirectoryInfo parentDirectory = null!;
            var shortAnswer = false;

            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                switch (arg)
                {
                    case "-b": Extensions.SizeInBytes = true; break;
                    case "-s": shortAnswer = true; break;
                    case "-d": parentDirectory = new DirectoryInfo(args[i + 1]); i++; break;
                }
            }

            parentDirectory ??= new DirectoryInfo(Environment.CurrentDirectory);

            if (!parentDirectory.Exists) throw new DirectoryNotFoundException(parentDirectory.FullName);

            Console.Title = parentDirectory.FullName;

            if (!shortAnswer)
            {
                Console.WriteLine("[IE] - Directory or Sub dirs has inaccessible files or dirs");
            }

            var parentDirFiles = parentDirectory.EnumerateFiles()
                .Select(f => new { f.Name, Size = f.Length })
                .ToArray();

            var parentDirFilesSize = parentDirFiles.Sum(f => f.Size);
            parentDirFilesSize.SizeToConsole(".files", false, false, EndLine.Space);

            var parentDirChildDirsSize = 0L;
            var parentAnyInaccessible = false;

            var enumerationOptions = new EnumerationOptions
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = true
            };

            var childDirectories = parentDirectory.EnumerateDirectories()
                .Select(childDir =>
                {
                    var fsz = 0L;
                    var hasInaccessible = false;

                    try
                    {
                        fsz = childDir.EnumerateFiles("", SearchOption.AllDirectories).Sum(f => f.Length);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        hasInaccessible = parentAnyInaccessible = true;
                        fsz = childDir.EnumerateFiles("*", enumerationOptions).Sum(f => f.Length);
                    }

                    parentDirChildDirsSize += fsz;

                    return new { childDir.Name, Size = fsz, HasInaccessible = hasInaccessible };
                })
                .ToArray();

            parentDirChildDirsSize.SizeToConsole(".dirs", true, parentAnyInaccessible, EndLine.Space);
            (parentDirFilesSize + parentDirChildDirsSize).SizeToConsole(".total", true, parentAnyInaccessible, EndLine.NewLine);

            if (!shortAnswer)
            {
                foreach (var dir in childDirectories.OrderByDescending(d => d.Size))
                {
                    dir.Size.SizeToConsole(dir.Name, true, dir.HasInaccessible, EndLine.NewLine);
                }

                foreach (var file in parentDirFiles.OrderByDescending(d => d.Size))
                {
                    file.Size.SizeToConsole(file.Name, false, false, EndLine.NewLine);
                }
            }

            if (args.Contains("-w"))
            {
                Console.WriteLine("Done");
                Console.ReadLine();
            }
        }
    }
}
