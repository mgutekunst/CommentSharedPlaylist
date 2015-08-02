using System;
using Gtk;
using SharedPlaylist.Core.ViewModels;
using Microsoft.Practices.ServiceLocation;

public partial class MainWindow: Gtk.Window
{
    private MainViewModel _vm;
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
	    _vm = ServiceLocator.Current.GetInstance<MainViewModel>();
		Build ();
		this.Title = _vm.Title;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
