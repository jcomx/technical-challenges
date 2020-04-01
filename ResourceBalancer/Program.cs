using ResourceBalancer.Abstraction;
using ResourceBalancer.BL;
using ResourceBalancer.Models;
using ResourceBalancer.Repositories;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ResourceBalancer
{
    class Program
    {
        static int resourceCapacity = 0;
        static ITasksRepository<ITask> foregroundTasks;
        static ITasksRepository<ITask> backgroundTasks;

        static readonly string pattern = @"([(]\w,\s*\d[)]\s*,?\s*)*";

        static void Main(string[] args)
        {
            //// Read from console
            //ReadResourceCapacity();
            //foregroundTasks = Parse(ReadTasks());
            //backgroundTasks = Parse(ReadTasks());
            //Console.WriteLine(LoadBalancer.GetOptimalConfiguration(resourceCapacity, foregroundTasks, backgroundTasks));

            // Read from text file
            ReadInputFile();
        }

        static void ReadInputFile()
        {
            StringBuilder sbResults = new StringBuilder();
            var fileLines = System.IO.File.ReadAllLines(@"challenge.in");
            int lineIdx = 0;
            foreach (var line in fileLines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                switch (lineIdx)
                {
                    case 0:
                        GetResourceCapacity(line);
                        lineIdx++;
                        break;
                    case 1:
                        AreValidTasks(line);
                        foregroundTasks = Parse(line);
                        lineIdx++;
                        break;
                    case 2:
                    default:
                        AreValidTasks(line);
                        backgroundTasks = Parse(line);

                        sbResults.AppendLine(LoadBalancer.GetOptimalConfiguration(resourceCapacity, foregroundTasks, backgroundTasks));
                        lineIdx = 0;
                        InitValues();
                        break;
                }
            }

            CreateOutputFile(sbResults);
        }

        static void CreateOutputFile(StringBuilder sbResults)
        {
            using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(@"challenge.out"))
            {
                outputFile.WriteLine(sbResults.ToString());
            }
        }

        static void InitValues()
        {
            resourceCapacity = 0;
            foregroundTasks = null;
            backgroundTasks = null;
        }

        /// <summary>
        /// Validate and parse a string to integer. This value will be stored in the resourceCapacity variable.
        /// </summary>
        /// <param name="input">A string with a valid integer</param>
        static void GetResourceCapacity(string input)
        {
            if (!int.TryParse(input, out resourceCapacity))
                throw new Exception("GetResourceCapacity: Invalid input values.");
        }

        /// <summary>
        /// Validate if a string has a correct tasks input format.
        /// </summary>
        /// <param name="input">A string with a valid format.</param>
        /// <example>
        /// (1, 2), (3, 4)
        /// </example>
        /// <returns></returns>
        static bool AreValidTasks(string input)
        {
            var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
            if (!match.Success) throw new Exception("AreValidTasks: Invalid input values.");
            return true;
        }

        /// <summary>
        /// Read an input value from console that should be integer. If the input value is not valid it will ask for a new value.
        /// </summary>
        static void ReadResourceCapacity()
        {
            if (!int.TryParse(Console.ReadLine(), out resourceCapacity))
                ReadResourceCapacity();
        }

        /// <summary>
        /// Read an input value from console that should be a list of tasks. If the input value is not valid it will ask for a new value.
        /// </summary>
        static string ReadTasks()
        {
            string line = Console.ReadLine();
            var match = Regex.Match(line, pattern, RegexOptions.IgnoreCase);
            if (!match.Success) return ReadTasks();
            return line.Trim();
        }

        static ITasksRepository<ITask> Parse(string line)
        {
            ITasksRepository<ITask> result = new TaskRepository<ITask>();

            var entries = line.Split(')');
            foreach (string entry in entries)
            {
                if (!String.IsNullOrWhiteSpace(entry))
                {
                    var values = entry.Substring(entry.IndexOf('(') + 1).Split(',');
                    if (values.Length == 2)
                    {
                        result.InsertTask(
                            new SimpleTask(
                                values[0].Trim(),
                                int.Parse(values[1].Trim())
                                )
                            );
                    }
                }
            }

            return result;
        }
    }
}
