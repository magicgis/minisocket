using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class MiniSocket
    {
        public MiniSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
        {
            connection = new Socket(addressFamily, socketType, protocolType);
            isConnected = false;
        }

        ~MiniSocket()
        {
            connection.Close();
        }


        // Member Functions

        public void connectToHost(string ip, string port)
        {
            // Generates server connection using given ip address and port
            connection.Connect(IPAddress.Parse(ip), Int32.Parse(port));
            stream = new NetworkStream(connection);
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
            isConnected = true;
        }

        public bool getConnectionProperty()
        {
            return isConnected;
        }

        public void write(string message)
        {
            writer.Write(message); // Send information to server
        }

        public string read()
        {
            return reader.ReadString(); // Recieve message from server
        }

        public void StreamFlush()
        {
            stream.Flush();
        }

        // Private Data Members

        private Socket connection;
        private Boolean isConnected;

        // Protected Data Members & Properties

        protected NetworkStream stream { get; set; }
        protected BinaryReader reader { get; set; }
        protected BinaryWriter writer { get; set; }
    }
}