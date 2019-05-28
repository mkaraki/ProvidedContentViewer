using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCV_Request
{
    class TCPSender
    {
        public static void SendTCP(string host,int port,string msg)
        {
            System.Net.Sockets.TcpClient tcp;
            try
            {
                tcp = new System.Net.Sockets.TcpClient(host, port);
            }
            catch
            {
                return;
            }

            try
            {
                System.Net.Sockets.NetworkStream ns = tcp.GetStream();

                try
                {
                    ns.ReadTimeout = 10000;
                    ns.WriteTimeout = 10000;

                    Encoding enc = Encoding.UTF8;
                    byte[] sendBytes = enc.GetBytes(msg + '\n');
                    ns.Write(sendBytes, 0, sendBytes.Length);
                }
                finally
                {
                    ns.Close();
                }
            }
            finally
            {
                tcp.Close();
            }
        }
    }
}
