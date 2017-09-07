using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

public class Location
{
    static void Main(string[] args)
    {
        try
        {
            // variables
            const string DEFAULT_HOST = "whois.net.dcs.hull.ac.uk";
            string host = "whois.net.dcs.hull.ac.uk";
            int port = 43;
            string streamRead;
            string protocolType = "whois";
            bool hostChange = false;
            string[] sections;
            string userName = "";
            string location = "";

            // add command line arguments to list for manipulation
            List<string> argumentsList = new List<string>(args);

            // set up the TcpClient          
            TcpClient client = new TcpClient();

            #region Server change checks

            // check for port number changes
            // using the -p parameter
            // if found it is removed before it can be sent to the server

            for (int i = 0; i < argumentsList.Count; i++)
            {
                try
                {
                    int j = i + 1;
                    if (argumentsList[i] == "-p")
                    {
                        port = int.Parse(argumentsList[j]);
                        argumentsList.RemoveRange(i, 2);
                    }
                }
                catch
                {

                }
            }

            // check for servername & protocol changes
            // using the -h parameter
            // if found it is removed before it can be sent to the server
            for (int i = 0; i < argumentsList.Count; i++)
            {
                // whois
                if (argumentsList[i] == "-h")
                {
                    int j = i + 1;
                    try
                    {
                        host = argumentsList[j];
                        client.Connect(host, port);
                        argumentsList.RemoveRange(i, 2);
                        i = -1; // reset for loop
                        hostChange = true;
                    }
                    catch
                    {
                        host = DEFAULT_HOST;
                        protocolType = "whois";
                        argumentsList.RemoveAt(i);
                    }
                    continue;
                }
                // HTTP 0.9
                if (argumentsList[i] == "-h9")
                {
                    protocolType = "HTTP/0.9";
                    argumentsList.RemoveAt(i);
                    continue;
                }
                // HTTP 1.0
                if (argumentsList[i] == "-h0")
                {
                    protocolType = "HTTP/1.0";
                    argumentsList.RemoveAt(i);
                    continue;
                }
                // HTTP1.1
                if (argumentsList[i] == "-h1")
                {
                    protocolType = "HTTP/1.1";
                    argumentsList.RemoveAt(i);
                    continue;
                }
            }

            #endregion

            // if no host change occured connect client
            if (hostChange == false)
            {
                client.Connect(host, port);
            }

            //Console.WriteLine(protocolType);
            //Console.WriteLine();

            //timneout after 1000 milliseconds of inactivity
            client.ReceiveTimeout = 1000;
            client.SendTimeout = 1000;

            // set up stream
            StreamWriter sw = new StreamWriter(client.GetStream());
            StreamReader sr = new StreamReader(client.GetStream());

            // set up username and location variables to allow easier recognition for response and request code below
            if (argumentsList.Count == 1)
            {
                userName = argumentsList[0];
            }
            else if (argumentsList.Count == 2)
            {
                userName = argumentsList[0];
                location = argumentsList[1];
            }

            #region Request & Response

            // set up protocol type
            switch (protocolType)
            {
                #region Whois
                case "whois":
                    // whois protocol request
                    switch (argumentsList.Count)
                    {
                        default:
                            // more than 3 arguments sent - not allowed
                            Console.WriteLine("Invalid command");
                            break;
                        case 1:
                            // request client location
                            sw.WriteLine(userName);
                            sw.Flush();
                            break;
                        case 2:
                            // change client location
                            sw.WriteLine(userName + " " + location);
                            sw.Flush();
                            break;
                    }

                    streamRead = sr.ReadToEnd().Trim();
                   // Console.WriteLine(streamRead);
                   // Console.WriteLine();

                    // whois protocol response
                    switch (streamRead)
                    {
                        default:
                            // location request
                            Console.WriteLine(userName + " is " + streamRead);
                            break;
                        case "OK":
                            // location change
                            Console.WriteLine(userName + " location changed to be " + location);
                            break;
                        case "ERROR: no entries found":
                            // no entries found
                            Console.WriteLine("ERROR: no entries found");
                            break;
                    }
                    break;
                #endregion

                #region HTTP/0.9
                case "HTTP/0.9":
                    // HTTP 0.9 protocol request
                    switch (argumentsList.Count)
                    {
                        default:
                            Console.WriteLine("Invalid command");
                            break;
                        case 1:
                            // location request
                            sw.WriteLine("GET /{0}", userName);
                            sw.Flush();
                            break;
                        case 2:
                            // location change
                            sw.WriteLine("PUT /{0}", userName);
                            sw.WriteLine("");
                            sw.WriteLine(location);
                            sw.Flush();
                            break;
                    }

                    streamRead = sr.ReadToEnd().Trim();
                    //Console.WriteLine(streamRead);
                    //Console.WriteLine();

                    sections = streamRead.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                    // HTTP 0.9 protocol response
                    // location request
                    if ( (sections.Length == 4) 
                        && (sections[0] == "HTTP/0.9 200 OK") 
                        && (sections[1] == "Content-Type: text/plain") 
                        && (sections[2] == "") )
                    {
                        Console.WriteLine(userName + " is " + sections[3]); // sections 3 = location
                    }
                    // location change
                    else if ((sections.Length == 2)
                        && (sections[0] == "HTTP/0.9 200 OK")
                        && (sections[1] == "Content-Type: text/plain") )
                    {
                        Console.WriteLine(userName + " location changed to be " + location);
                    }
                    // no entry found
                    else if ((sections.Length == 2)
                        && (sections[0] == "HTTP/0.9 404 Not Found")
                        && (sections[1] == "Content-Type: text/plain"))
                    {
                        Console.WriteLine("ERROR: no entries found");
                    }
                    else
                    {
                        Console.WriteLine(userName + " is " + streamRead);
                    }
                    break;
                #endregion

                #region HTTP/1.0
                case "HTTP/1.0":
                    // HTTP 1.0 protocol request
                    switch (argumentsList.Count)
                    {
                        default:
                            Console.WriteLine("Invalid command");
                            break;
                        case 1:
                            // location request
                            sw.WriteLine("GET /{0} HTTP/1.0{1}",userName, Environment.NewLine);
                            sw.Flush();
                            break;
                        case 2:
                            // location change
                            sw.WriteLine("POST /{0} HTTP/1.0{1}Content-Length: {2}{1}{1}{3}", userName, Environment.NewLine, location.Length, location);
                            sw.Flush();
                            break;
                    }

                    streamRead = sr.ReadToEnd().Trim();
                    //Console.WriteLine(streamRead);
                    //Console.WriteLine();

                    sections = streamRead.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                    // HTTP 1.0 protocol response
                    // location request
                    if ( (sections.Length == 4) 
                        && (sections[0] == "HTTP/1.0 200 OK") 
                        && (sections[1] == "Content-Type: text/plain") 
                        && (sections[2] == "") )
                    {
                        Console.WriteLine(userName + " is " + sections[3]); // sections 3 = location
                    }
                    // location change
                    else if ((sections.Length == 2)
                        && (sections[0] == "HTTP/1.0 200 OK")
                        && (sections[1] == "Content-Type: text/plain") )
                    {
                        Console.WriteLine(userName + " location changed to be " + location);
                    }
                    // no entry found
                    else if ((sections.Length == 2)
                        && (sections[0] == "HTTP/1.0 404 Not Found")
                        && (sections[1] == "Content-Type: text/plain"))
                    {
                        Console.WriteLine("ERROR: no entries found");
                    }
                    else
                    {
                        Console.WriteLine(userName + " is " + streamRead);
                    }
                    break;
                    #endregion

                #region HTTP/1.1
                case "HTTP/1.1":
                    // HTTP 1.1 protocol request
                    switch (argumentsList.Count)
                    {
                        default:
                            Console.WriteLine("Invalid command");
                            break;
                        case 1:
                            // location request
                            sw.WriteLine("GET /{0} HTTP/1.1{1}Host: {2}{1}", userName, Environment.NewLine, host);
                            sw.Flush();
                            break;
                        case 2:
                            // location change
                            sw.WriteLine("POST /{0} HTTP/1.1{1}Host: {2}{1}Content-Length: {3}{1}{1}{4}", userName, Environment.NewLine, host, location.Length, location);
                            sw.Flush();
                            break;
                    }

                    streamRead = sr.ReadToEnd().Trim();
                    //Console.WriteLine(streamRead);
                    //Console.WriteLine();

                    sections = streamRead.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                    // HTTP 1.1 protocol response
                    // location request
                    if ((sections.Length == 4)
                        && (sections[0] == "HTTP/1.1 200 OK")
                        && (sections[1] == "Content-Type: text/plain")
                        && (sections[2] == ""))
                    {
                        Console.WriteLine(userName + " is " + sections[3]); // sections 3 = location
                    }
                    // location change
                    else if ((sections.Length == 2)
                        && (sections[0] == "HTTP/1.1 200 OK")
                        && (sections[1] == "Content-Type: text/plain"))
                    {
                        Console.WriteLine(userName + " location changed to be " + location);
                    }
                    // no entry found
                    else if ((sections.Length == 2)
                        && (sections[0] == "HTTP/1.1 404 Not Found")
                        && (sections[1] == "Content-Type: text/plain"))
                    {
                        Console.WriteLine("ERROR: no entries found");
                    }
                    else
                    {
                        Console.WriteLine(userName + " is " + streamRead);
                    }
                    break;
                #endregion
            }

            #endregion
        }
        catch (Exception e)
        {
            Console.WriteLine("Error - please seek halp");
            Console.WriteLine("Exception: " + e.ToString());
        }
    }  
} 

