using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;



namespace socket
{

    class Program
    {
        static void Main(string[] args)
        {
            byte[] bytes = new Byte[1024];
            string data = null;
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 12347);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPHostEntry ipHostInfo = Dns.GetHostEntry("127.1.0.0");
				IPAddress ipAddress = ipHostInfo.AddressList[0];
				IPEndPoint remoteEP = new IPEndPoint(ipAddress, 12345);
				Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp );

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
                            $"To ja proces 3a w c# - server otrzymalem wiaodmosc i wysylam ja dalej! Oto wiadomosc: ");
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                        Console.WriteLine(data);
                        

                        sender.Connect(remoteEP);  
					Console.WriteLine($"Socket connected to {sender.RemoteEndPoint.ToString()}");
                     Console.WriteLine(
                            $"To ja proces 3a w c# ");
                    byte[] mess = Encoding.ASCII.GetBytes(data);
					sender.Send(mess);					
					sender.Shutdown(SocketShutdown.Both);  
					sender.Close();

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
            Console.Read();

		// 	try {
        //         IPHostEntry ipHostInfo = Dns.GetHostEntry("127.0.0.1");
		// 		IPAddress ipAddress = ipHostInfo.AddressList[0];
		// 		IPEndPoint remoteEP = new IPEndPoint(ipAddress, 12345);
		// 		Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp );
		// 		try {
		// 			sender.Connect(remoteEP);  
		// 			Console.WriteLine($"Socket connected to {sender.RemoteEndPoint.ToString()}");
        //              Console.WriteLine(
        //                     $"To ja proces 3a w c# ");
        //             byte[] mess = Encoding.ASCII.GetBytes(data);
		// 			sender.Send(mess);					
		// 			sender.Shutdown(SocketShutdown.Both);  
		// 			sender.Close();
		// 		} catch (Exception e) {

		// 			Console.WriteLine(e.ToString());
		// 		}
		// 	} catch (Exception e) {
		// 		Console.WriteLine( e.ToString());
		// 	}

         }
        
    }
} 
