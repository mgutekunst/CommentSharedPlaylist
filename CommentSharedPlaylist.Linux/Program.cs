using System;
using Gtk;
using CommentSharedPlaylist.Linux.Init;

namespace CommentSharedPlaylist.Linux
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			// init bootstrapper
			var strap = new BootStrapper();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}
