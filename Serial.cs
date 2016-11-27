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

				bool valid = true;

				for (int i = 0; i < program.Length; ++i) {
					byte b = (byte) serial.ReadByte();

					if (b != program[i])
						valid = false;
				}

				if (!valid) {
					/* TODO */
				}
			} catch {
				/* TODO */
			}
		}
	}
}

