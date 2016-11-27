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
			Log.Debug ("# Compiling");
			Parser parser = new Parser (ExpressionEntry.Text);
			byte[] code = parser.Compile ();

			if (!string.IsNullOrEmpty(PortsComboBox.ActiveText)) {
				Serial.write (PortsComboBox.ActiveText, code);
			} else {
				MessageDialog md = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Error: You have to specify Arduino port");
				md.Run ();
				md.Destroy ();
			}
		}

		protected void rescan (object sender, EventArgs e)
		{
			/* Really? */
			PortsComboBox.Clear ();
			CellRendererText cell = new CellRendererText();
			PortsComboBox.PackStart(cell, false);
			PortsComboBox.AddAttribute(cell, "text", 0);
			PortsComboBox.Model = new ListStore (typeof(string));

			string[] ports = Serial.GetPorts ();

			foreach (string port in ports) {
				PortsComboBox.AppendText (port);
			}
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