using System;
using System.Collections.Generic;

namespace GAF.Console
{
	public class ComandParser
	{
		//private List<string> _elements;

		public ComandParser (string commandLine)
		{
			string[] elements;
			if (commandLine != null) {
				elements = commandLine.Split (" ".ToCharArray ());
			}

			var elementCount = elements.Length;

			if (elementCount > 0) {

				for(int index = 0; index < elementCount; index++) {
					elements[index] = elements[index].Trim ();
				}

				//add everything
				Arguments.AddRange (elements);
				Command = Arguments [0];
				Arguments.RemoveAt (0);

			}
		}

		public string Command { get; private set; }
		public List<string> Arguments { get; private set; }

		public string GetArgument(int index)
		{
			if (index > 0 && index < Arguments.Count) {
				return Arguments [index];
			}
			return null;
		}

		public int ArgumentCount {
			get { 
				return _elements.Length - 1;
				} 
		}
	}
}

