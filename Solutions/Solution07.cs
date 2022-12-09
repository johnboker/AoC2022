using System.Text.RegularExpressions;

namespace AoC2022.Solutions
{
    public class Solution07 : ISolution
    {
        public Solution07()
        {
            Input = new List<string>();
        }

        public List<string> Input { get; set; }

        public async Task ReadInput(string file)
        {
            Input = (await System.IO.File.ReadAllLinesAsync(file)).ToList();
        }

        public void Solve1()
        {
            var terminal = new Terminal();
            terminal.ProcessCommands(Input);
            while (terminal.CurrentDirectory?.Parent != null) terminal.CurrentDirectory = terminal.CurrentDirectory.Parent;
            Console.WriteLine(terminal.GetSize(terminal.CurrentDirectory, 100000));
        }

        public void Solve2()
        {
            var terminal = new Terminal();
            terminal.ProcessCommands(Input);
            while (terminal.CurrentDirectory?.Parent != null) terminal.CurrentDirectory = terminal.CurrentDirectory.Parent;

            var flatList = Flatten(terminal.CurrentDirectory);

            var totalUsed = terminal.CurrentDirectory?.Size() ?? 0;
            var totalFree = 70000000 - totalUsed;
            var needed = 30000000 - totalFree;

            var toDelete = flatList
                .Select(a => new { Directory = a, ifDeletedSize = a.Size() - needed })
                .Where(a => a.ifDeletedSize >= 0)
                .OrderBy(a => a.ifDeletedSize)
                .FirstOrDefault();

            if (toDelete != null)
                Console.WriteLine($"{toDelete.Directory.Name} {toDelete.Directory.Size()}");
        }

        private List<Directory> Flatten(Directory? directory)
        {
            if (directory == null) return new List<Directory>();

            var list = new List<Directory>();
            list.Add(directory);
            foreach (var d in directory.SubDirectories)
            {
                list.AddRange(Flatten(d));
            }
            return list;
        }

        public class Terminal
        {
            private Regex CommandRegex = new Regex(@"\$\s(?<cmd>[a-zA-Z]+)\s?(?<param1>[a-zA-Z./]+)?");
            public Terminal() { }
            public Directory? CurrentDirectory { get; set; }

            public void ProcessCommands(List<string> input)
            {
                for (var i = 0; i < input.Count(); i++)
                {
                    var cmd = input[i];
                    if (cmd.StartsWith("$ "))
                    {
                        var match = CommandRegex.Match(cmd);
                        var command = match.Groups["cmd"].Value;
                        var parameter1 = match.Groups["param1"].Value;
                        List<string> output = GetCommandOutput(i, input);

                        switch (command)
                        {
                            case "cd":
                                if (CurrentDirectory == null)
                                {
                                    CurrentDirectory = new Directory(parameter1);
                                }
                                else if (parameter1 == ".." && CurrentDirectory.Parent != null)
                                {
                                    CurrentDirectory = CurrentDirectory.Parent;
                                }
                                else
                                {
                                    CurrentDirectory = CurrentDirectory.SubDirectories.First(a => a.Name == parameter1);
                                }
                                break;
                            case "ls":
                                foreach (var o in output)
                                {
                                    var parts = o.Split(" ");
                                    if (parts[0] == "dir")
                                    {
                                        CurrentDirectory?.SubDirectories.Add(new Directory(parts[1]) { Parent = CurrentDirectory });
                                    }
                                    else
                                    {
                                        CurrentDirectory?.Files.Add(new File(parts[1], Convert.ToInt32(parts[0])));
                                    }
                                }
                                break;
                        }
                    }
                }
            }

            public int GetSize(Directory? directory, int maxSizeToInlcude)
            {
                if (directory == null) return 0;

                var total = 0;

                var size = directory.Size();
                if (size <= maxSizeToInlcude) total += size;

                foreach (var d in directory.SubDirectories)
                {
                    total += GetSize(d, maxSizeToInlcude);
                }

                return total;
            }

            private void PrintTree(Directory? currentDirectory, int level)
            {
                if (currentDirectory == null) return;

                Console.WriteLine($"{new String(' ', level)}{currentDirectory.Name} {currentDirectory.Size()} {currentDirectory.FilesSize()}");
                foreach (var s in currentDirectory.SubDirectories)
                {
                    PrintTree(s, level + 1);
                }
                foreach (var f in currentDirectory.Files)
                {
                    Console.WriteLine($"{new String(' ', level)}{f.Size} {f.Name}");
                }
            }

            private List<string> GetCommandOutput(int commandLocation, List<string> input)
            {
                var output = new List<string>();
                for (var i = commandLocation + 1; i < input.Count(); i++)
                {
                    if (input[i].StartsWith("$")) break;
                    output.Add(input[i]);
                }
                return output;
            }
        }

        public class Directory
        {
            public Directory(string name)
            {
                Name = name;
                SubDirectories = new List<Directory>();
                Files = new List<File>();
            }

            public int Size()
            {
                var size = FilesSize();
                size += SubDirectories.Sum(a => a.Size());
                return size;
            }
            public int FilesSize()
            {
                return Files.Sum(a => a.Size);
            }

            public Directory? Parent { get; set; }
            public string Name { get; set; }
            public List<Directory> SubDirectories { get; set; }
            public List<File> Files { get; set; }
        }

        public class File
        {
            public File(string name, int size)
            {
                Name = name;
                Size = size;
            }

            public string Name { get; set; }
            public int Size { get; set; }
        }
    }
}