using System;
using Gtk;

namespace ARLES
{
	public partial class MainWindow: Gtk.Window
	{
		public MainWindow () : base (Gtk.WindowType.Toplevel)
		{
			Build ();

			Log.buf = logs.Buffer;

			string[] ports = Serial.GetPorts ();
			foreach (string port in ports) {
				PortsComboBox.AppendText (port);
			}
		}

		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}

		protected void upload (object sender, EventArgs e)
		{
			Log.Debug ("Uploading");
			Parser parser = new Parser (ExpressionEntry.Text);
			byte[] code = parser.Compile ();

			for (int i = 0; i < code.Length; ++i)
				Log.Debug (code [i].ToString());
		}
	}

	public static class Log
	{
		public static TextBuffer buf = null;

		public static void Debug(string s)
		{
			buf.Text += s + "\n";
		}
	}
}