using System;
using System.Collections.Generic;
using GAF.Api;
using Gtk;

namespace GAF.LabGtk
{
	/// <summary>
	/// The list only contains one type of object.
	/// To get multiple columns, pass that number of parameters to the constructor
	/// and implement RenderCell accordingly.
	/// </summary>
	public abstract class ListView<T> : TreeView
	{
		/// <summary>
		/// List of all items selected
		/// </summary>
		public T[] SelectedItems { get; private set; }
		public event Action<T[]> ItemSelected;
		public event Action<T> ItemActivated;
		private string [] _columnNames;
		private int _maxColumnCount;

		protected abstract void RenderCell (CellRendererText render, int index, T item);

		ListStore store = new ListStore (typeof(T));
		Dictionary<T, TreeIter> storeList = new Dictionary<T, TreeIter> ();
		List<TreeViewColumn> columns = new List<TreeViewColumn> ();

		/// <summary>
		/// Pass one string parameter for every column
		/// </summary>
		/// <param name="columnNames">
		/// Column Titles, one parameter for each column.
		/// </param>
		public ListView (params string[] columnNames)
		{
			//keep the names so that we can restore the column names when we clear the list
			_columnNames = columnNames;
			_maxColumnCount = columnNames.Length;


			this.Model = store;
			foreach (string s in columnNames) {
				TreeViewColumn c = this.AppendColumn (s, new CellRendererText (), this.ColumnCellData);
				columns.Add (c);
			}
			
			this.SelectedItems = new T[0];
			this.Selection.Changed += HandleSelectionChanged;
			this.Selection.Mode = SelectionMode.Single;
		}

		private void ColumnCellData (TreeViewColumn column, CellRenderer renderer, TreeModel model, TreeIter iter)
		{
			T item = (T)model.GetValue (iter, 0);
			CellRendererText textRender = (CellRendererText)renderer;

			int index = columns.IndexOf (column);
			RenderCell (textRender, index, item);
		}

		#region Add, Remove and Clear Items

		public void AddColumn (string title)
		{
			// cant use this as is due to the fact that we need to restrict to fixed number of columns
			// with each title changing
			Predicate<TreeViewColumn> columnFinder = (TreeViewColumn c) => { return c.Title.Equals(title); };

			//check to see if column exists already
			if (!columns.Exists (columnFinder))
			{
				TreeViewColumn c = this.AppendColumn (title, new CellRendererText (), this.ColumnCellData);
				columns.Add (c);
				if (columns.Count > _maxColumnCount) {
					columns.RemoveAt (0);
				}			
			}
		}

		public void AddItem (T item)
		{
			var iter = store.AppendValues (item);
			storeList.Add (item, iter);
		}

		public void ClearItems ()
		{
			store.Clear ();
			storeList.Clear ();
			for (int index = 0; index < _maxColumnCount; index++) {
				columns [index].Title = _columnNames [index];
			}
		}

		public void RemoveItem (T item)
		{
			if (! storeList.ContainsKey (item))
				return;
			
			TreeIter iter = storeList[item];
			store.Remove (ref iter);
			storeList.Remove (item);
		}

		#endregion

		#region Selection and Activation triggers

		void HandleSelectionChanged (object sender, EventArgs e)
		{
			TreeSelection selection = (TreeSelection)sender;

			//TreeIter iterCell = new TreeIter ();
			//selection.GetSelected (out iterCell);

			TreePath[] paths = selection.GetSelectedRows ();
			T[] items = new T[paths.Length];
			for (int n = 0; n < paths.Length; n++) {
				TreeIter iter;
				Model.GetIter (out iter, paths[n]);
				items[n] = (T)Model.GetValue (iter, 0);
			}
			
			SelectedItems = items;
			
			var itemEvent = ItemSelected;
			if (itemEvent != null)
				itemEvent (items);
		}

		protected override void OnRowActivated (TreePath path, TreeViewColumn column)
		{
			TreeIter iter;
			Model.GetIter (out iter, path);
			T item = (T)Model.GetValue (iter, 0);
			
			var e = ItemActivated;
			if (e != null)
				e (item);
			
			base.OnRowActivated (path, column);
		}
		
		#endregion
	}
}

