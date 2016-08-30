using System;
using Gtk;

namespace GAF.LabGtk
{
	public class PopulationHistoryView : ListView<PopulationViewItem>
	{


		public PopulationHistoryView (params string [] columnNames) : base (columnNames)
		{
		}

		protected override void RenderCell (CellRendererText render, int index, PopulationViewItem item)
		{
			var count = item.RowData.Count;
			if (index >= count || index < 0) {
				throw new IndexOutOfRangeException (string.Format ("Index specified was {0}. The item.RowData property returns {1} elements.", index, count ));
			}
			render.Text = item.RowData [index];
		}

	}
}

