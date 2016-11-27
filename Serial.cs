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
			
		}
	}
}

