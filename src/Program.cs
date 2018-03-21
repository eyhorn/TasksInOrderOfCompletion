using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TasksInOrderOfCompletion
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Running tasks one by one");
			Run(false).Wait();
			Console.WriteLine();
			Console.WriteLine("Running tasks in completion order");
			Run(true).Wait();
			Console.ReadLine();
		}

		static async Task Run(bool isExecuteInCompletionOrder)
		{
			var sw = new Stopwatch();

			sw.Start();

			var tasks = new List<Task<TestTaskData>>
			{
				CreateTask(1, 600),
				CreateTask(2, 200),
				CreateTask(3, 150),
				CreateTask(4, 400)
			};

			var tasksToExecute = isExecuteInCompletionOrder ? tasks.InCompletionOrder() : tasks;

			foreach (var task in tasksToExecute)
			{
				var testTaskData = await task;
				Console.WriteLine($"{testTaskData.Id} finished at {testTaskData.Date:o}");
			}

			sw.Stop();

			Console.WriteLine($"Execution time is {sw.Elapsed} milliseconds");
		}

		static Task<TestTaskData> CreateTask(int id, int delay)
		{
			return Task.Delay(delay).ContinueWith(task => new TestTaskData(id));
		}
	}
}
