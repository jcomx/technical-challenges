using ResourceBalancer.Abstraction;
using ResourceBalancer.BL;
using ResourceBalancer.Models;
using ResourceBalancer.Repositories;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ResourceBalancer
{
    /*
     You are currently working with highly specialized devices that may perform a single background task and a single foreground task. 
     Each task consumes a fixed number of resources (resource consumption) from the device’s available resource capacity. 
        E.g., a device with a capacity of 3 may run the following configurations:
        -	A background task that consumes 1 resource and a foreground task that consumes 2 resources or vice versa.
        -	A background task that consumes 1 resource and a foreground task that consumes 1 resource.

    From the previously enumerated configurations, only the former can be considered an optimal configuration. 
    A device may not be configured with tasks whose sum of resource consumption surpasses the device’s resource capacity.

    Challenge
    In general, given a set of background tasks and a set of foreground tasks, a device is optimally configured when the device is loaded 
    with a background task and a foreground task whose resource consumption is equal to or as close as possible to the device’s resource capacity 
    without surpassing it.

    Your Task
    Given, 
    -	A device with capacity N, 
    -	A set of foreground tasks identified by an ID and its resource consumption
    -	A set of background tasks identified by an ID and its resource consumption
    
    Write a program that produces the IDs of the combination of tasks that yields an optimally configured device. 
    There might be more than one optimal configuration; that being the case, produce all of them.

    Sample Input and Outputs
    Your program should read a plain text file named challenge.in that may contain 1 or more scenarios with the following format:
        7
        (1, 6), (2, 2), (3, 4)
        (1, 2)

    The first line represents the resource capacity of the scenario’s device. 
    The second line represents a list of the pair (Task ID, resource consumption) of the scenario’s foreground tasks. 
    The third line represents a list of the pair (Task ID, resource consumption) of the scenario’s background tasks.

    The expected output of that scenario should be stored in a plain text file named challenge.out as follows:
        (3, 1)
    That is, the device is optimally configured when running the foreground task ID 3 and background task 1.
    
    The following is a more comprehensive example of input file along its expected result.
*/
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
