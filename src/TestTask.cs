using System;

namespace TasksInOrderOfCompletion
{
	internal class TestTaskData
	{
		public TestTaskData(int id)
		{
			Id = id;
			Date = DateTime.Now;			
		}

		public int Id { get; }
		public DateTime Date { get; }
	}
}