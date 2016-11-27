﻿using System;
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
	}
}

