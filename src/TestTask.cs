using System;

namespace TasksInOrderOfCompletion
{
	internal class TestTaskData
	{
		public TestTaskData(int id, int delay)
		{
			_id = id;
			_delay = delay;
		}

		private readonly int _id;
		private readonly int _delay;

		public override string ToString()
		{
			return $"{_id} with delay {_delay}";
		}
	}
}