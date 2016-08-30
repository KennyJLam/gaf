using System;
using System.ComponentModel;

namespace GAF.Api.Operators
{
	public interface IOperator : INotifyPropertyChanged
	{
		event Api.ExceptionHandler OnException;
		event Api.LoggingEventHandler OnLogging;
		bool Enabled { set; get; }
		string Description { get; }
	}
}

