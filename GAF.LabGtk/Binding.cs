using System;

namespace GAF.LabGtk
{
	public class Binding
	{

		public Binding (string sourceProperty, object sourceObject, string targetProperty, object targetObject, Func<object,object> dataConvert)
		{
			if (string.IsNullOrWhiteSpace(sourceProperty)) {
				throw new ArgumentNullException (nameof (sourceProperty));
			}
			if (sourceObject == null) {
				throw new ArgumentNullException (nameof (sourceObject));
			}
			if (string.IsNullOrWhiteSpace(targetProperty)) {
				throw new ArgumentNullException (nameof (targetProperty));
			}
			if (targetObject == null) {
				throw new ArgumentNullException (nameof (targetObject));
			}

			SourceProperty = sourceProperty;
			SourceObject = sourceObject;
			TargetProperty = targetProperty;
			TargetObject = targetObject;
			DataConvert = dataConvert;
		}

		public string TargetProperty { set; get; }

		public object SourceObject { set; get; }

		public string SourceProperty { set; get; }

		public object TargetObject { set; get; }

		public Func<object,object> DataConvert { get; set; }
	}
}

