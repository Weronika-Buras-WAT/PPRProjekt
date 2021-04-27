using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Horizon.XmlRpc.Core;
using Horizon.XmlRpc.Client;
using System.Diagnostics;
using System.ComponentModel;
using Horizon.XmlRpc.Server;


namespace socket
{
    public interface IAddServiceProxy : IXmlRpcProxy
    {
        [XmlRpcMethod("sample.read")]
        int AddNumbers(string s);
    }

    class Program
    {
        static void Main(string[] args)
        {
            byte[] bytes = new Byte[1024];
            string data = null;
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 12345);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    try
                    {
                        Console.WriteLine("Waiting for a connection...");
                        Socket handler = listener.Accept();
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            bytes[i] = 0;
                        }
                        handler.Receive(bytes);
                        data = System.Text.Encoding.UTF8.GetString(bytes);
                        int index = data.IndexOf('\0');
                        if (index >= 0)
                            data = data.Remove(index);
                        Console.WriteLine(
                            $"To ja proces 3 w c# - server otrzymalem wiaodmosc i wysylam ja dalej! Oto wiadomosc: ");
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                        Console.WriteLine(data);
                        var proxy = XmlRpcProxyGen.Create<IAddServiceProxy>();
                        proxy.Url = "http://127.0.0.1:12346/RPC2";
                        var result = proxy.AddNumbers(data);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }
    }
}