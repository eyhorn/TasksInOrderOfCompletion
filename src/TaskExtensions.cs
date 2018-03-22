using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TasksInOrderOfCompletion
{
	public static class TaskExtensions
	{
		public static IEnumerable<Task<T>> InCompletionOrder<T>(this IEnumerable<Task<T>> tasks)
		{
			var tasksArray = tasks as Task<T>[] ?? tasks.ToArray();
			var completionSources = tasksArray.Select(task => new TaskCompletionSource<T>())
				.ToArray();
			int taskIndex = -1;

			foreach (var task in tasksArray)
			{
				task.ContinueWith(completedTask =>
						completionSources[Interlocked.Increment(ref taskIndex)]
							.PopulateFromCompletedTask(completedTask),
					TaskContinuationOptions.ExecuteSynchronously);
			}

			return completionSources.Select(source => source.Task);
		}

		public static void PopulateFromCompletedTask<T>(this TaskCompletionSource<T> completionSource,
			Task<T> task)
		{
			switch (task.Status)
			{
				case TaskStatus.RanToCompletion:
					completionSource.TrySetResult(task.Result);
					break;
				case TaskStatus.Faulted:
					completionSource.TrySetException(task.Exception.InnerExceptions);
					break;
				case TaskStatus.Canceled:
					completionSource.TrySetCanceled();
					break;
				default:
					throw new ArgumentException("Task is not in a completed state");
			}
		}
	}
}
