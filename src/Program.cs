using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TasksInOrderOfCompletion
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Retrieving task results one by one");
			Run(ResultCollectionOrder.Serial).Wait();
			Console.WriteLine();
			Console.WriteLine("Retrieving task results in completion order");
			Run(ResultCollectionOrder.Completion).Wait();
			Console.WriteLine();
			Console.WriteLine("Press any key to close...");
			Console.ReadKey();
		}

		private enum ResultCollectionOrder
		{
			Serial,
			Completion
		}

		static async Task Run(ResultCollectionOrder resultCollectionOrder)
		{
			var tasks = new List<Task<TestTaskData>>
			{
				CreateTask(1, 800),
				CreateTask(2, 200),
				CreateTask(3, 150),
				CreateTask(4, 400)
			};

			var tasksToExecute = resultCollectionOrder == ResultCollectionOrder.Completion 
				? tasks.InCompletionOrder() 
				: tasks;

			foreach (var task in tasksToExecute)
			{
				var testTaskData = await task;
				Console.WriteLine(testTaskData);
			}
		}

		static Task<TestTaskData> CreateTask(int id, int delay)
		{
			return Task.Delay(delay).ContinueWith(task => new TestTaskData(id, delay));
		}
	}
}
