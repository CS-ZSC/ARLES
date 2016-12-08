using System;
using System.IO.Ports;
using System.Threading;

namespace ARLES
{
	public static class Serial
	{
		public static string[] GetPorts ()
		{
			return SerialPort.GetPortNames ();
		}

		public static void write(string port, byte[] program)
		{
			int baud = 9600;
			SerialPort serial = new SerialPort (port, baud);

			try {
				serial.Open();

				/* Give Arduino time to reset */
				Thread.Sleep(2000);

				byte[] len = new byte[2];
				len[0] = (byte) 0x1C;	/* Start of transmition marker */
				len[1] = (byte) program.Length;

				serial.Write(len, 0, 2);
				serial.Write(program, 0, program.Length);

				Log.Debug("# Response from Arduino");
				Log.Debug(serial.ReadLine());

				bool valid = true;

				string response = "";

				for (int i = 0; i < program.Length; ++i) {
					byte b = (byte) serial.ReadByte();
					response += string.Format("0x{0:X2} ", b);
					if (b != program[i])
						valid = false;
				}

				Log.Debug (response);
				Log.Debug (valid? "# Upload was successful" : "# Upload was unsuccessful");
			} catch {
				/* TODO */
			}
		}

		private static bool DetectArduino(string port)
		{
			try {
				SerialPort serial = new SerialPort(port, 9600);
				byte[] buffer = new byte[5];

				/* Hand-shake sequence */
				buffer[0] = Convert.ToByte(16);
				buffer[1] = Convert.ToByte(128);
				buffer[2] = Convert.ToByte(0);
				buffer[3] = Convert.ToByte(0);
				buffer[4] = Convert.ToByte(4);

				serial.Open();
				serial.Write(buffer, 0, 5);
				Thread.Sleep(2000);

				int intReturnASCII = 0;
				int count = serial.BytesToRead;
				string returnMessage = "";
				while (count > 0) {
					intReturnASCII = serial.ReadByte();
					returnMessage = returnMessage + Convert.ToChar(intReturnASCII);
					count--;
				}
					
				serial.Close();
				if (returnMessage.Contains("HELLO FROM ARDUINO")) {
					return true;
				} else {
					return false;
				}
			} catch {
				return false;
			}
		}
	
	}
}

