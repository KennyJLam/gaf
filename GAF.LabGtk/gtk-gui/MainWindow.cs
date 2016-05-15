
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.UIManager UIManager;
	
	private global::Gtk.Action FileAction;
	
	private global::Gtk.Action OpenFitnessFunctionAction;
	
	private global::Gtk.Action removeAction;
	
	private global::Gtk.Action quitAction;
	
	private global::Gtk.Action HelpAction;
	
	private global::Gtk.Action AboutAction;
	
	private global::Gtk.Action FileOpenAction;
	
	private global::Gtk.Action OpenFitnessFunctionAction1;
	
	private global::Gtk.Action openAction;
	
	private global::Gtk.VBox vbox1;
	
	private global::Gtk.MenuBar menubar1;
	
	private global::Gtk.VPaned vpaned1;
	
	private global::Gtk.HBox hbox1;
	
	private global::Gtk.VBox vbox2;
	
	private global::Gtk.Frame frame2;
	
	private global::Gtk.Alignment GtkAlignment1;
	
	private global::Gtk.ToggleButton togglebutton1;
	
	private global::Gtk.Label GtkLabel7;
	
	private global::Gtk.Frame frame3;
	
	private global::Gtk.Frame frame1;
	
	private global::Gtk.Alignment GtkAlignment;
	
	private global::Gtk.VBox vbox4;
	
	private global::Gtk.RadioButton radiobutton1;
	
	private global::Gtk.RadioButton radiobutton2;
	
	private global::Gtk.RadioButton radiobutton3;
	
	private global::Gtk.Label GtkLabel8;
	
	private global::Gtk.Frame frame5;
	
	private global::Gtk.Alignment GtkAlignment3;
	
	private global::Gtk.Label GtkLabel6;
	
	private global::Gtk.Frame frame4;
	
	private global::Gtk.Alignment GtkAlignment2;
	
	private global::Gtk.Label GtkLabel9;
	
	private global::Gtk.HPaned hpaned2;
	
	private global::Gtk.Notebook notebook4;
	
	private global::Gtk.VBox vbox5;
	
	private global::Gtk.Frame frame6;
	
	private global::Gtk.Alignment GtkAlignment4;
	
	private global::Gtk.VBox vbox6;
	
	private global::Gtk.ProgressBar progressbar1;
	
	private global::Gtk.ProgressBar progressbar2;
	
	private global::Gtk.Label GtkLabel10;
	
	private global::Gtk.Label label6;
	
	private global::Gtk.Label label11;
	
	private global::Gtk.Notebook notebook5;
	
	private global::Gtk.VBox vbox3;
	
	private global::Gtk.CheckButton checkbutton2;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow;
	
	private global::Gtk.TextView textview1;
	
	private global::Gtk.Label label7;
	
	private global::Gtk.Label label9;
	
	private global::Gtk.Label label10;
	
	private global::Gtk.Notebook notebook1;
	
	private global::Gtk.Label label3;
	
	private global::Gtk.Statusbar statusbar1;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.UIManager = new global::Gtk.UIManager ();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
		this.FileAction = new global::Gtk.Action ("FileAction", global::Mono.Unix.Catalog.GetString ("File"), null, null);
		this.FileAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("File");
		w1.Add (this.FileAction, "function");
		this.OpenFitnessFunctionAction = new global::Gtk.Action ("OpenFitnessFunctionAction", global::Mono.Unix.Catalog.GetString ("Open Fitness Function"), null, null);
		this.OpenFitnessFunctionAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Open");
		w1.Add (this.OpenFitnessFunctionAction, null);
		this.removeAction = new global::Gtk.Action ("removeAction", global::Mono.Unix.Catalog.GetString ("Close Fitness Function"), null, "gtk-remove");
		this.removeAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("E&xit");
		w1.Add (this.removeAction, null);
		this.quitAction = new global::Gtk.Action ("quitAction", global::Mono.Unix.Catalog.GetString ("_Quit"), null, "gtk-quit");
		this.quitAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Quit");
		w1.Add (this.quitAction, "<Control>x");
		this.HelpAction = new global::Gtk.Action ("HelpAction", global::Mono.Unix.Catalog.GetString ("Help"), null, null);
		this.HelpAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Help");
		w1.Add (this.HelpAction, null);
		this.AboutAction = new global::Gtk.Action ("AboutAction", global::Mono.Unix.Catalog.GetString ("About"), null, null);
		this.AboutAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("About");
		w1.Add (this.AboutAction, null);
		this.FileOpenAction = new global::Gtk.Action ("FileOpenAction", global::Mono.Unix.Catalog.GetString ("FileOpen"), null, null);
		this.FileOpenAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("FileOpen");
		w1.Add (this.FileOpenAction, null);
		this.OpenFitnessFunctionAction1 = new global::Gtk.Action ("OpenFitnessFunctionAction1", global::Mono.Unix.Catalog.GetString ("Open Fitness Function"), null, null);
		this.OpenFitnessFunctionAction1.ShortLabel = global::Mono.Unix.Catalog.GetString ("Open Fitness Function");
		w1.Add (this.OpenFitnessFunctionAction1, null);
		this.openAction = new global::Gtk.Action ("openAction", global::Mono.Unix.Catalog.GetString ("Open Fitness Function"), null, "gtk-open");
		this.openAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Open Fitness Function");
		w1.Add (this.openAction, null);
		this.UIManager.InsertActionGroup (w1, 0);
		this.AddAccelGroup (this.UIManager.AccelGroup);
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("GAF Lab");
		this.Icon = new global::Gdk.Pixbuf (global::System.IO.Path.Combine (global::System.AppDomain.CurrentDomain.BaseDirectory, "./GAF128.ico"));
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		this.BorderWidth = ((uint)(10));
		this.DefaultWidth = 1000;
		this.DefaultHeight = 700;
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox ();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.UIManager.AddUiFromString ("<ui><menubar name='menubar1'><menu name='FileAction' action='FileAction'><menuitem name='openAction' action='openAction'/><menuitem name='removeAction' action='removeAction'/><menuitem name='quitAction' action='quitAction'/></menu><menu name='HelpAction' action='HelpAction'><menuitem name='AboutAction' action='AboutAction'/></menu></menubar></ui>");
		this.menubar1 = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menubar1")));
		this.menubar1.Name = "menubar1";
		this.vbox1.Add (this.menubar1);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.menubar1]));
		w2.Position = 0;
		w2.Expand = false;
		w2.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.vpaned1 = new global::Gtk.VPaned ();
		this.vpaned1.CanFocus = true;
		this.vpaned1.Name = "vpaned1";
		this.vpaned1.Position = 300;
		this.vpaned1.BorderWidth = ((uint)(1));
		// Container child vpaned1.Gtk.Paned+PanedChild
		this.hbox1 = new global::Gtk.HBox ();
		this.hbox1.Name = "hbox1";
		this.hbox1.Spacing = 6;
		// Container child hbox1.Gtk.Box+BoxChild
		this.vbox2 = new global::Gtk.VBox ();
		this.vbox2.Name = "vbox2";
		this.vbox2.Spacing = 6;
		// Container child vbox2.Gtk.Box+BoxChild
		this.frame2 = new global::Gtk.Frame ();
		this.frame2.Name = "frame2";
		this.frame2.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame2.Gtk.Container+ContainerChild
		this.GtkAlignment1 = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
		this.GtkAlignment1.Name = "GtkAlignment1";
		this.GtkAlignment1.LeftPadding = ((uint)(12));
		// Container child GtkAlignment1.Gtk.Container+ContainerChild
		this.togglebutton1 = new global::Gtk.ToggleButton ();
		this.togglebutton1.CanFocus = true;
		this.togglebutton1.Name = "togglebutton1";
		this.togglebutton1.UseUnderline = true;
		this.togglebutton1.Label = global::Mono.Unix.Catalog.GetString ("Start/Stop");
		this.GtkAlignment1.Add (this.togglebutton1);
		this.frame2.Add (this.GtkAlignment1);
		this.GtkLabel7 = new global::Gtk.Label ();
		this.GtkLabel7.Name = "GtkLabel7";
		this.GtkLabel7.LabelProp = global::Mono.Unix.Catalog.GetString ("Start/Stop");
		this.GtkLabel7.UseMarkup = true;
		this.frame2.LabelWidget = this.GtkLabel7;
		this.vbox2.Add (this.frame2);
		global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.frame2]));
		w5.Position = 0;
		w5.Expand = false;
		w5.Fill = false;
		// Container child vbox2.Gtk.Box+BoxChild
		this.frame3 = new global::Gtk.Frame ();
		this.frame3.Name = "frame3";
		this.frame3.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame3.Gtk.Container+ContainerChild
		this.frame1 = new global::Gtk.Frame ();
		this.frame1.Name = "frame1";
		this.frame1.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame1.Gtk.Container+ContainerChild
		this.GtkAlignment = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
		this.GtkAlignment.Name = "GtkAlignment";
		this.GtkAlignment.LeftPadding = ((uint)(12));
		// Container child GtkAlignment.Gtk.Container+ContainerChild
		this.vbox4 = new global::Gtk.VBox ();
		this.vbox4.Name = "vbox4";
		this.vbox4.Spacing = 6;
		// Container child vbox4.Gtk.Box+BoxChild
		this.radiobutton1 = new global::Gtk.RadioButton (global::Mono.Unix.Catalog.GetString ("radiobutton1"));
		this.radiobutton1.CanFocus = true;
		this.radiobutton1.Name = "radiobutton1";
		this.radiobutton1.DrawIndicator = true;
		this.radiobutton1.UseUnderline = true;
		this.radiobutton1.Group = new global::GLib.SList (global::System.IntPtr.Zero);
		this.vbox4.Add (this.radiobutton1);
		global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.radiobutton1]));
		w6.Position = 0;
		w6.Expand = false;
		w6.Fill = false;
		// Container child vbox4.Gtk.Box+BoxChild
		this.radiobutton2 = new global::Gtk.RadioButton (global::Mono.Unix.Catalog.GetString ("radiobutton2"));
		this.radiobutton2.CanFocus = true;
		this.radiobutton2.Name = "radiobutton2";
		this.radiobutton2.DrawIndicator = true;
		this.radiobutton2.UseUnderline = true;
		this.radiobutton2.Group = this.radiobutton1.Group;
		this.vbox4.Add (this.radiobutton2);
		global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.radiobutton2]));
		w7.Position = 1;
		w7.Expand = false;
		w7.Fill = false;
		// Container child vbox4.Gtk.Box+BoxChild
		this.radiobutton3 = new global::Gtk.RadioButton (global::Mono.Unix.Catalog.GetString ("radiobutton3"));
		this.radiobutton3.CanFocus = true;
		this.radiobutton3.Name = "radiobutton3";
		this.radiobutton3.DrawIndicator = true;
		this.radiobutton3.UseUnderline = true;
		this.radiobutton3.Group = this.radiobutton1.Group;
		this.vbox4.Add (this.radiobutton3);
		global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.radiobutton3]));
		w8.Position = 2;
		w8.Expand = false;
		w8.Fill = false;
		this.GtkAlignment.Add (this.vbox4);
		this.frame1.Add (this.GtkAlignment);
		this.frame3.Add (this.frame1);
		this.GtkLabel8 = new global::Gtk.Label ();
		this.GtkLabel8.Name = "GtkLabel8";
		this.GtkLabel8.LabelProp = global::Mono.Unix.Catalog.GetString ("Population");
		this.GtkLabel8.UseMarkup = true;
		this.frame3.LabelWidget = this.GtkLabel8;
		this.vbox2.Add (this.frame3);
		global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.frame3]));
		w12.Position = 1;
		w12.Expand = false;
		w12.Fill = false;
		// Container child vbox2.Gtk.Box+BoxChild
		this.frame5 = new global::Gtk.Frame ();
		this.frame5.Name = "frame5";
		this.frame5.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame5.Gtk.Container+ContainerChild
		this.GtkAlignment3 = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
		this.GtkAlignment3.Name = "GtkAlignment3";
		this.GtkAlignment3.LeftPadding = ((uint)(12));
		this.frame5.Add (this.GtkAlignment3);
		this.GtkLabel6 = new global::Gtk.Label ();
		this.GtkLabel6.Name = "GtkLabel6";
		this.GtkLabel6.LabelProp = global::Mono.Unix.Catalog.GetString ("Chromosome");
		this.GtkLabel6.UseMarkup = true;
		this.frame5.LabelWidget = this.GtkLabel6;
		this.vbox2.Add (this.frame5);
		global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.frame5]));
		w14.Position = 2;
		// Container child vbox2.Gtk.Box+BoxChild
		this.frame4 = new global::Gtk.Frame ();
		this.frame4.Name = "frame4";
		this.frame4.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame4.Gtk.Container+ContainerChild
		this.GtkAlignment2 = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
		this.GtkAlignment2.Name = "GtkAlignment2";
		this.GtkAlignment2.LeftPadding = ((uint)(12));
		this.frame4.Add (this.GtkAlignment2);
		this.GtkLabel9 = new global::Gtk.Label ();
		this.GtkLabel9.Name = "GtkLabel9";
		this.GtkLabel9.LabelProp = global::Mono.Unix.Catalog.GetString ("Parent Selection Method");
		this.GtkLabel9.UseMarkup = true;
		this.frame4.LabelWidget = this.GtkLabel9;
		this.vbox2.Add (this.frame4);
		global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.frame4]));
		w16.Position = 3;
		this.hbox1.Add (this.vbox2);
		global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.vbox2]));
		w17.Position = 0;
		w17.Expand = false;
		w17.Fill = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this.hpaned2 = new global::Gtk.HPaned ();
		this.hpaned2.CanFocus = true;
		this.hpaned2.Name = "hpaned2";
		this.hpaned2.Position = 550;
		// Container child hpaned2.Gtk.Paned+PanedChild
		this.notebook4 = new global::Gtk.Notebook ();
		this.notebook4.CanFocus = true;
		this.notebook4.Name = "notebook4";
		this.notebook4.CurrentPage = 0;
		// Container child notebook4.Gtk.Notebook+NotebookChild
		this.vbox5 = new global::Gtk.VBox ();
		this.vbox5.Name = "vbox5";
		this.vbox5.Spacing = 6;
		// Container child vbox5.Gtk.Box+BoxChild
		this.frame6 = new global::Gtk.Frame ();
		this.frame6.Name = "frame6";
		this.frame6.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame6.Gtk.Container+ContainerChild
		this.GtkAlignment4 = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
		this.GtkAlignment4.Name = "GtkAlignment4";
		this.GtkAlignment4.LeftPadding = ((uint)(12));
		// Container child GtkAlignment4.Gtk.Container+ContainerChild
		this.vbox6 = new global::Gtk.VBox ();
		this.vbox6.Name = "vbox6";
		this.vbox6.Spacing = 6;
		// Container child vbox6.Gtk.Box+BoxChild
		this.progressbar1 = new global::Gtk.ProgressBar ();
		this.progressbar1.Name = "progressbar1";
		this.progressbar1.Text = global::Mono.Unix.Catalog.GetString ("Diversity");
		this.vbox6.Add (this.progressbar1);
		global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.vbox6 [this.progressbar1]));
		w18.Position = 1;
		w18.Expand = false;
		w18.Fill = false;
		w18.Padding = ((uint)(2));
		// Container child vbox6.Gtk.Box+BoxChild
		this.progressbar2 = new global::Gtk.ProgressBar ();
		this.progressbar2.Name = "progressbar2";
		this.progressbar2.Text = global::Mono.Unix.Catalog.GetString ("Solutions in Upper Quartile");
		this.vbox6.Add (this.progressbar2);
		global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.vbox6 [this.progressbar2]));
		w19.Position = 2;
		w19.Expand = false;
		w19.Fill = false;
		w19.Padding = ((uint)(2));
		this.GtkAlignment4.Add (this.vbox6);
		this.frame6.Add (this.GtkAlignment4);
		this.GtkLabel10 = new global::Gtk.Label ();
		this.GtkLabel10.Name = "GtkLabel10";
		this.GtkLabel10.LabelProp = global::Mono.Unix.Catalog.GetString ("Diagnostics");
		this.GtkLabel10.UseMarkup = true;
		this.frame6.LabelWidget = this.GtkLabel10;
		this.vbox5.Add (this.frame6);
		global::Gtk.Box.BoxChild w22 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.frame6]));
		w22.Position = 1;
		w22.Padding = ((uint)(5));
		this.notebook4.Add (this.vbox5);
		// Notebook tab
		this.label6 = new global::Gtk.Label ();
		this.label6.Name = "label6";
		this.label6.LabelProp = global::Mono.Unix.Catalog.GetString ("Status");
		this.notebook4.SetTabLabel (this.vbox5, this.label6);
		this.label6.ShowAll ();
		// Notebook tab
		global::Gtk.Label w24 = new global::Gtk.Label ();
		w24.Visible = true;
		this.notebook4.Add (w24);
		this.label11 = new global::Gtk.Label ();
		this.label11.Name = "label11";
		this.label11.LabelProp = global::Mono.Unix.Catalog.GetString ("Source Code");
		this.notebook4.SetTabLabel (w24, this.label11);
		this.label11.ShowAll ();
		this.hpaned2.Add (this.notebook4);
		global::Gtk.Paned.PanedChild w25 = ((global::Gtk.Paned.PanedChild)(this.hpaned2 [this.notebook4]));
		w25.Resize = false;
		// Container child hpaned2.Gtk.Paned+PanedChild
		this.notebook5 = new global::Gtk.Notebook ();
		this.notebook5.CanFocus = true;
		this.notebook5.Name = "notebook5";
		this.notebook5.CurrentPage = 0;
		// Container child notebook5.Gtk.Notebook+NotebookChild
		this.vbox3 = new global::Gtk.VBox ();
		this.vbox3.Name = "vbox3";
		this.vbox3.Spacing = 6;
		// Container child vbox3.Gtk.Box+BoxChild
		this.checkbutton2 = new global::Gtk.CheckButton ();
		this.checkbutton2.CanFocus = true;
		this.checkbutton2.Name = "checkbutton2";
		this.checkbutton2.Label = global::Mono.Unix.Catalog.GetString ("checkbutton2");
		this.checkbutton2.DrawIndicator = true;
		this.checkbutton2.UseUnderline = true;
		this.vbox3.Add (this.checkbutton2);
		global::Gtk.Box.BoxChild w26 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.checkbutton2]));
		w26.Position = 0;
		w26.Expand = false;
		w26.Fill = false;
		// Container child vbox3.Gtk.Box+BoxChild
		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
		this.textview1 = new global::Gtk.TextView ();
		this.textview1.CanFocus = true;
		this.textview1.Name = "textview1";
		this.GtkScrolledWindow.Add (this.textview1);
		this.vbox3.Add (this.GtkScrolledWindow);
		global::Gtk.Box.BoxChild w28 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.GtkScrolledWindow]));
		w28.Position = 3;
		this.notebook5.Add (this.vbox3);
		// Notebook tab
		this.label7 = new global::Gtk.Label ();
		this.label7.Name = "label7";
		this.label7.LabelProp = global::Mono.Unix.Catalog.GetString ("Crossover");
		this.notebook5.SetTabLabel (this.vbox3, this.label7);
		this.label7.ShowAll ();
		// Notebook tab
		global::Gtk.Label w30 = new global::Gtk.Label ();
		w30.Visible = true;
		this.notebook5.Add (w30);
		this.label9 = new global::Gtk.Label ();
		this.label9.Name = "label9";
		this.label9.LabelProp = global::Mono.Unix.Catalog.GetString ("Binary Mutate");
		this.notebook5.SetTabLabel (w30, this.label9);
		this.label9.ShowAll ();
		// Notebook tab
		global::Gtk.Label w31 = new global::Gtk.Label ();
		w31.Visible = true;
		this.notebook5.Add (w31);
		this.label10 = new global::Gtk.Label ();
		this.label10.Name = "label10";
		this.label10.LabelProp = global::Mono.Unix.Catalog.GetString ("Elite");
		this.notebook5.SetTabLabel (w31, this.label10);
		this.label10.ShowAll ();
		this.hpaned2.Add (this.notebook5);
		this.hbox1.Add (this.hpaned2);
		global::Gtk.Box.BoxChild w33 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.hpaned2]));
		w33.Position = 1;
		this.vpaned1.Add (this.hbox1);
		global::Gtk.Paned.PanedChild w34 = ((global::Gtk.Paned.PanedChild)(this.vpaned1 [this.hbox1]));
		w34.Resize = false;
		// Container child vpaned1.Gtk.Paned+PanedChild
		this.notebook1 = new global::Gtk.Notebook ();
		this.notebook1.CanFocus = true;
		this.notebook1.Name = "notebook1";
		this.notebook1.CurrentPage = 0;
		// Notebook tab
		global::Gtk.Label w35 = new global::Gtk.Label ();
		w35.Visible = true;
		this.notebook1.Add (w35);
		this.label3 = new global::Gtk.Label ();
		this.label3.Name = "label3";
		this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("Output");
		this.notebook1.SetTabLabel (w35, this.label3);
		this.label3.ShowAll ();
		this.vpaned1.Add (this.notebook1);
		this.vbox1.Add (this.vpaned1);
		global::Gtk.Box.BoxChild w37 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.vpaned1]));
		w37.Position = 1;
		// Container child vbox1.Gtk.Box+BoxChild
		this.statusbar1 = new global::Gtk.Statusbar ();
		this.statusbar1.Name = "statusbar1";
		this.statusbar1.Spacing = 6;
		this.vbox1.Add (this.statusbar1);
		global::Gtk.Box.BoxChild w38 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.statusbar1]));
		w38.Position = 2;
		w38.Expand = false;
		w38.Fill = false;
		this.Add (this.vbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.removeAction.Activated += new global::System.EventHandler (this.MenuFileClose);
		this.quitAction.Activated += new global::System.EventHandler (this.MenuFileQuit);
		this.AboutAction.Activated += new global::System.EventHandler (this.MenuHelpAbout);
		this.openAction.Activated += new global::System.EventHandler (this.MenuFileOpen);
		this.togglebutton1.Clicked += new global::System.EventHandler (this.StartStop);
	}
}
