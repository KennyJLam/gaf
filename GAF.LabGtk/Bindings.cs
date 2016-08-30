using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Diagnostics;

namespace GAF.LabGtk
{
	/// <summary>
	/// This is a simple one way (Read) bindings class that allows basic properties (typically UI Components) to
	/// be bound to the Api properties. Only one way (Read) bindings are supported.
	/// </summary>
	public class Bindings
	{
		private List<Binding> _bindings = new List<Binding> ();
		private List<Type> _subscriptions = new List<Type> ();

		public Bindings ()
		{

		}

		public Binding RegisterBinding (object targetObject, string targetProperty, object sourceObject, string sourceProperty, Func<object,object> dataConvert = null)
		{	
			if (string.IsNullOrWhiteSpace (sourceProperty)) {
				throw new ArgumentNullException (nameof (sourceProperty));
			}
			if (sourceObject == null) {
				throw new ArgumentNullException (nameof (sourceObject));
			}
			if (string.IsNullOrWhiteSpace (targetProperty)) {
				throw new ArgumentNullException (nameof (targetProperty));
			}
			if (targetObject == null) {
				throw new ArgumentNullException (nameof (targetObject));
			}

			INotifyPropertyChanged source = sourceObject as INotifyPropertyChanged;

			if (source == null) {
				throw new ArgumentException ("Source object must implement INotifyPropertyChanged.");
			}

			//check that we are already hooked up to the PropertyChanged event
			if (!_subscriptions.Contains (source.GetType ())) {
				source.PropertyChanged += (object sender, PropertyChangedEventArgs e) => {

					var bindings = _bindings.Where (b => 
						b.SourceProperty == e.PropertyName &&
						sender.GetType() == b.SourceObject.GetType());

					if (bindings != null) {
						foreach (var binding in bindings) {
							CopyValue (binding);
						}
					}

				};
				_subscriptions.Add (source.GetType ());
			}

			var newBinding = new Binding (sourceProperty, sourceObject, targetProperty, targetObject, dataConvert);

			//initial copy
			CopyValue (newBinding);

			_bindings.Add (newBinding);

			return newBinding;
		}

		private void CopyValue (Binding binding)
		{
			Gtk.Application.Invoke (delegate {

				//Debug.WriteLine ("Copying {0}.{1} to {2}.{3}.", new [] {binding.SourceObject, binding.SourceProperty, binding.TargetObject, binding.TargetProperty});

				var targetType = binding.TargetObject.GetType ();
				var sourceType = binding.SourceObject.GetType ();

				PropertyInfo propertyInfoSource = sourceType.GetProperty (binding.SourceProperty);
				PropertyInfo propertyInfoTarget = targetType.GetProperty (binding.TargetProperty);

				if (propertyInfoSource == null) {
					throw new BinderException ("Could not find property {0}.{1}.", binding.SourceObject.ToString (), binding.SourceProperty);
				}
				if (propertyInfoTarget == null) {
					throw new BinderException ("Could not find property {0}.{1}.", binding.TargetObject.ToString (), binding.TargetProperty);
				}

				//read the source property value
				var propertyValue = propertyInfoSource.GetValue (binding.SourceObject, null);

				//check to see if an explicit data conversion has been specified
				if (binding.DataConvert != null) {

					try {
						var convertedPropertyValue = binding.DataConvert (propertyValue);


						//if they are the same type then update the target
						if (convertedPropertyValue.GetType () == propertyInfoTarget.PropertyType) {
							propertyInfoTarget.SetValue (binding.TargetObject, convertedPropertyValue);
						} else {
							throw new BinderException ("The specified data convert function for target property {0}, returned a {1} and should have returned a {2}.", propertyInfoTarget.Name, convertedPropertyValue.GetType (), propertyInfoTarget.PropertyType);
						}
					} catch (Exception ex) {
						throw new BinderException ("The specfied data convert function for target property {0}, threw an exception. Please see inner exception for details,", propertyInfoTarget.Name, ex);
					}

				} else {

					//no specific data conversion so copy accross

					//check that the source and destination is the same type
					if (propertyInfoSource.PropertyType == propertyInfoTarget.PropertyType) {
						//properties are the same type so just copy accross
						propertyInfoTarget.SetValue (binding.TargetObject, propertyValue);
					} else {
						//implicit data convert required
						propertyInfoTarget.SetValue (binding.TargetObject, ConvertValue (propertyInfoTarget, propertyValue));

					}
				}
			});
		}

		private object ConvertValue (PropertyInfo propertyInfoTarget, object propertyValue)
		{
			
			//can only deal with strings for now
			if (propertyInfoTarget.PropertyType == typeof(System.String)) {
				return  propertyValue.ToString ();
			} else if (propertyInfoTarget.PropertyType == typeof(double) && propertyValue is int) {
				return Convert.ToDouble (propertyValue);
			} else {
				throw new BinderException ("The implicit data convert function for target property {0}, could not convert the source Type ({1}) to the target Type {2}.", propertyInfoTarget.Name, propertyValue.GetType (), propertyInfoTarget.PropertyType);
			}


		}
	}
}

