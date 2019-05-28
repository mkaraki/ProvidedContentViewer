using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ProvidedContentViewer
{
    internal class TCPService
    {
        internal class GotCmd
        {
            public IPAddress ClientIP { get; set; }
            public string[] Cmds { get; set; }
        }

        public static GotCmd GetSession()
        {
            string str = string.Empty;

            TcpListener listener = new TcpListener(IPAddress.IPv6Any,8888);
            listener.Server.SetSocketOption(SocketOptionLevel.IPv6,SocketOptionName.IPv6Only,0);
            listener.Start();
            IPAddress cip;

            try
            {
                Console.WriteLine("Start Listen");

                TcpClient client = listener.AcceptTcpClient();
                cip = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
                Console.WriteLine($"Client Connected");
                try
                {
                    if (!client.Connected) return null;
                    NetworkStream netStream = client.GetStream();
                    StreamReader sReader = new StreamReader(netStream, Encoding.UTF8);
                    try
                    {
                        str = sReader.ReadLine();
                    }
                    finally
                    {
                        sReader.Close();
                    }
                    Console.WriteLine("Session Closed");
                }
                finally
                {
                    client.Close();
                }
            }
            finally
            {
                listener.Stop();
            }

            GotCmd cmd = new GotCmd
            {
                Cmds = System.Text.RegularExpressions.Regex.Replace(str, @"^(?<![a-z]+)(?<cmd>[a-z]+\s.+)$", "${cmd}").Split(' '),
                ClientIP = cip,
            };

            return cmd;
        }
    }
}