using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;

namespace GAF.Api
{
	public abstract class GafApiBase : INotifyPropertyChanged
	{

		/// <summary>
		/// ExceptionHandler
		/// </summary>
		public event Api.ExceptionHandler OnException;

		/// <summary>
		/// Event definition for the LoggingEventHandler event handler.
		/// </summary>
		public event Api.LoggingEventHandler OnLogging;


		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		protected bool UpdateField<T>(ref T field, T value, string propertyName)
		{
			return UpdateField<T> (ref field, value, propertyName, false);
		}

		protected bool UpdateField<T>(ref T field, T value, string propertyName, bool forceUpdate)
		{
			bool update = false;

//			if (T == double.GetType()) {
//				if (!Math.AboutEqual (field, value)) {
//					update = true;
//				}
//					
//			}
//			else 
				if (!EqualityComparer<T>.Default.Equals(field, value) || forceUpdate)
			{
				update = true;
			}

			if(update)
			{
				field = value;
				RaisePropertyChangedEvent(propertyName);
			}

			return update;
		}

		protected void RaisePropertyChangedEvent (string propertyName)
		{
			//we have had a property change event raised in one of the GAF objects so
			//re-raise this here for consumers of the Api
			if (this.PropertyChanged != null) {
				var args = new System.ComponentModel.PropertyChangedEventArgs (propertyName);
				this.PropertyChanged (this, args);
			}
		}

		protected void RaiseLoggingEvent (string message, bool isWarning)
		{
			if (this.PropertyChanged != null) {

				var level = isWarning ? "WARN" : "INFO";
				var lArgs = new Api.LoggingEventArgs (string.Format ("{0}: {1}", level, message), isWarning);
				this.OnLogging (this, lArgs);
			}
		}

		protected void RaiseExceptionEvent (string methodName, string exceptionMessage)
		{
			var message = string.Format ("Exception in {0}.{1}: {2}", this.GetType (), methodName, exceptionMessage);
			Debug.Print(message);

			//we have had a property change event raised in one of the GAF objects so
			//re-raise this here for consumers of the Api
			if (OnException != null) {
				var args = new ExceptionEventArgs (methodName, message);
				this.OnException (this, args);
			}
		}

		protected void RaiseExceptionEvent (string methodName, Exception exception)
		{
			RaiseExceptionEvent (methodName, exception.Message);
		}
	}
}

