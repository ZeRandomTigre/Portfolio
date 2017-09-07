using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.IO;
class LocationServer
{
    static void Main(string[] args)
    {
        runServer();        
    }

    static void runServer()
    {        
        TcpListener listener;
        Socket connection;
        NetworkStream socketStream;

        // Stores clients & their locations
        Dictionary<string, string> clientLocations = new Dictionary<string, string>()
        {
            {"464814", "being tested"},
        };

        try
        {
            // listen for client connections
            listener = new TcpListener(IPAddress.Any, 43);
            listener.Start();
            Console.WriteLine("Server started listening");

            // loop round forever to allow clients to connect while server is running
            while (true)
            {
                connection = listener.AcceptSocket();  
                socketStream = new NetworkStream(connection);
                doRequest(socketStream, clientLocations); // handle client requests
                socketStream.Close();
                connection.Close(); // close connection with client
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.ToString());
        }
    }

    static void doRequest(NetworkStream socketStream, Dictionary<string, string> clientLocations)
    {
        try
        {
            Console.WriteLine("Client Connected");

            // server timeout after 1000 milliseconds of inactivitt
            //socketStream.ReadTimeout = 1000;
            //socketStream.WriteTimeout = 1000;

            // set up stream
            StreamWriter sw = new StreamWriter(socketStream);
            StreamReader sr = new StreamReader(socketStream);
            
            string streamRead;
            streamRead = sr.ReadLine().Trim();
            Console.WriteLine(streamRead);

            // split string into sections 
            string[] sections = streamRead.Split(new char[] { ' ' }, 2);
            
            string userLogin = sections[0];
            
            
            switch (sections.Length)
            {
                case 1:
                    if (clientLocations.ContainsKey(userLogin))
                    {
                        sw.Write(clientLocations[userLogin]);
                    }
                    else
                    {
                        sw.Write("ERROR: no entries found");                        
                    }
                    sw.Flush();
                    break;
                case 2:
                    if (clientLocations.ContainsKey(userLogin))
                    {                        
                        clientLocations[userLogin] = sections[1];
                    }
                    else
                    {
                        clientLocations.Add(sections[0], sections[1]);                        
                    }
                    sw.Write("OK");
                    sw.Flush();
                    break;
            }         
            
            
        }
        catch (Exception e)
        {
            Console.WriteLine("Error - please seek halp");
            Console.WriteLine("Exception: " + e.ToString());
        }
    }

}