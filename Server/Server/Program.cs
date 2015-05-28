using System;
using System.IO;
using System.Net.Sockets;
using System.IO.Ports;

namespace Server
{
    class Program
    {
        private static Boolean ledState = false;
        static void Main(string[] args)
        {
            while (true)
            {
                listen();
            }
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static void listen()
        {
            SerialPort mySerialPort;
            mySerialPort = new SerialPort();
            mySerialPort.PortName = "COM3";
            mySerialPort.BaudRate = Convert.ToInt32("9600");
            mySerialPort.DataBits = 8;
            mySerialPort.Parity = Parity.None;

            try
            {
                mySerialPort.Open();
            }
            catch (Exception)
            {
                
                Console.WriteLine("COM Port Does Not Exist");
                Console.ReadKey();
            }
            

            // 
            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 10048);
            Console.WriteLine("Waiting for a client...");
            int counter = 0;
            listener.Start();
            Socket client = listener.AcceptSocket();
            Console.Clear();

            NetworkStream stream = new NetworkStream(client);
            BinaryReader reader = new BinaryReader(stream);
            BinaryWriter writer = new BinaryWriter(stream);

            ledState = false;
            mySerialPort.Write(GetBytes("0"), 0, 1);

            while (true)
            {
                counter++;

                string receivedMsg;
                try
                {
                    receivedMsg = reader.ReadString();
                }
                catch (EndOfStreamException e)
                {
                    Console.WriteLine("Connection Dropped");
                    listener.Stop();
                    mySerialPort.Close();
                    return;
                }
                catch (IOException e)
                {
                    Console.WriteLine("Connection Dropped");
                    listener.Stop();
                    mySerialPort.Close();
                    return;
                }

                Console.WriteLine("Client Message " + counter + ": " + receivedMsg);

                if (receivedMsg == "toggle")
                {
                    try
                    {
                        if (!ledState)
                        {
                            writer.Write("ledon");
                            mySerialPort.Write(GetBytes("1"), 0, 1);
                            ledState = true;
                        }
                        else
                        {
                            writer.Write("ledoff");
                            mySerialPort.Write(GetBytes("0"), 0, 1);
                            ledState = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception" + ex);
                    }
                }
            }
        }
    }
}