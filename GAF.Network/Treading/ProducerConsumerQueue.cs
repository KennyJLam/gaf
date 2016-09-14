using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GAF.Network.Threading
{
	/// <summary>
	/// Producer/Consumer queue implementation that provides a concurrent
	/// queue of delegates that is serviced by a number of worker tasks.
	/// </summary>
	/// <remarks>
	/// This implementation is based on the example from "C# 6 in a nutshell:
	/// The Definitive Reference. Albahari".
	/// 
	/// This implementation gives all the benefit of tasks but with exception
	/// propagation, return values and cancellation.
	/// 
	/// The class can be used as follows:
	/// 
	///   var queue = new ProducerConsumerQueue(4); //Max concurrency of 4.
	///   var result = await queue.Enqueue(() => "Hello World");
	/// 
	/// </remarks>
	public class ProducerConsumerQueue :IDisposable
	{
		public BlockingCollection<Task> _taskQ;
		public Task[] _allWorkers;

		public ProducerConsumerQueue (int workerCount)
		{
			_allWorkers = new Task[workerCount];
			_taskQ = new BlockingCollection<Task> ();

			//create and start a task for each consumer
			for (var i = 0; i < workerCount; i++) {
				Task.Factory.StartNew (Consume, TaskCreationOptions.LongRunning);
			}
		}

		/// <summary>
		/// Gets all action tasks.
		/// </summary>
		/// <value>All tasks.</value>
		public List<Task> AllTasks {
			get {
				return _taskQ.ToList ();
			}
		}

		/// <summary>
		/// Enqueues the specified action and cancelToken and returns to the caller the non-started task.
		/// </summary>
		/// <param name="action">Action.</param>
		/// <param name="cancelToken">Cancel token.</param>
		public Task Enqueue (Action action, CancellationToken cancelToken = default (CancellationToken))
		{
			var task = new Task (action, cancelToken);
			_taskQ.Add (task);
			return task;
		}

		/// <summary>
		/// Enqueues the specified action and cancelToken and returns to the caller the non-started task.
		/// </summary>
		/// <param name="func">Func.</param>
		/// <param name="cancelToken">Cancel token.</param>
		/// <typeparam name="TResult">The 1st type parameter.</typeparam>
		public Task Enqueue<TResult> (Func<TResult> func, CancellationToken cancelToken = default (CancellationToken))
		{
			var task = new Task<TResult> (func, cancelToken);
			_taskQ.Add (task);
			return task;
		}

		/// <summary>
		/// Runs the task synchronously on the consumers thread. See constructor.
		/// </summary>
		private void Consume ()
		{
			foreach (var task in _taskQ.GetConsumingEnumerable ()) {
				try {
					if (!task.IsCanceled) {
						task.RunSynchronously ();
					}
				} catch (InvalidOperationException) {
					// this handles the unlikely event that the task is 
					//cancelled between checking it and running it.
				}
			}
		}

		/// <summary>
		/// Releases all resource used by the <see cref="T:GAF.Network.Threading.ProducerConsumerQueue"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="T:GAF.Network.Threading.ProducerConsumerQueue"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="T:GAF.Network.Threading.ProducerConsumerQueue"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="T:GAF.Network.Threading.ProducerConsumerQueue"/> so the garbage collector can reclaim the memory that
		/// the <see cref="T:GAF.Network.Threading.ProducerConsumerQueue"/> was occupying.</remarks>
		public void Dispose ()
		{
			_taskQ.CompleteAdding ();
		}
	}
}

