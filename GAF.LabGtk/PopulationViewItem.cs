using System;
using System.Collections.Generic;

namespace GAF.LabGtk
{
public class PopulationViewItem
	{
		public PopulationViewItem (List<string> rowData)
		{
			RowData = rowData;
			//SourceGeneration = sourceGeneration;
			//IsDuplicate = isDuplicate;
		}

		public List<string> RowData { get; private set; }
//		public int SourceGeneration { get; private set; }
//		public bool IsDuplicate { get; private set; }
	}
}

