using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PPB
{
    public class PPB
    {
        public void Run()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8000);
            listener.Start(5);

            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);

            while (true)
            {

                Console.WriteLine("Waiting for a request...");
                var socket = listener.AcceptTcpClient();
                new Thread(() => RunRequest(socket)).Start();
            }
        }

        public void RunRequest(TcpClient socket)
        {
            try
            {
                Console.WriteLine("Request recieved!");
                using var writer = new StreamWriter(socket.GetStream()) { AutoFlush = true };
                using var reader = new StreamReader(socket.GetStream());

                // Streamreader
                string firstline = reader.ReadLine(); // firstline of header
                string httpverb = firstline.Split(" ")[0]; // split firstline for the httpverb
                string resource = firstline.Split(" ")[1]; // split firstline for the resource
                string httpversion = firstline.Split(" ")[2]; // split firstline for the httpversion
                string header;
                int contentLength = 0;
                string body;
                Dictionary<string, string> headerDict = new Dictionary<string, string>();  // store request headers

                while (true)
                {
                    header = reader.ReadLine(); // store header

                    if (header == "")
                        break;

                    headerDict[header.Split(": ")[0]] = header.Split(": ")[1]; // split request headers [key] [value] 
                    if (header.StartsWith("Content-Length:"))
                    {
                        contentLength = Convert.ToInt32(header.Substring(16)); // convert to int and store in contentLength
                    }
                }

                // store body
                char[] bodyBuffer = new char[contentLength];
                reader.ReadBlock(bodyBuffer);
                body = new string(bodyBuffer);

                // instantiate object context
                RequestContext context = new RequestContext(httpverb, resource, httpversion, headerDict, body);

                // output message
                Console.WriteLine(httpverb + " " + resource + " " + httpversion);

                // calling ProcessRequest() with object parameter 
                // store return values into two variables
                var (returnStatusCode, returncontent) = ProcessRequest(context);


                // Streamwriter
                // httpversion                //statusint                  //statusmessage
                writer.WriteLine("HTTP/1.1 " + Convert.ToInt32(returnStatusCode) + " " + returnStatusCode);
                writer.WriteLine("Content-Length: " + returncontent.Length);
                writer.WriteLine();
                if (returncontent.Length > 0)
                    writer.Write(returncontent);
                writer.Flush(); // send immediately

            }
            catch (Exception exc)
            {
                Console.WriteLine("error occurred: " + exc.Message);
            }
        }

        public (HttpStatusCode returnstatusCode, string returnBody) ProcessRequest(RequestContext context)
        {
            string returncontent = ""; // empty by default
            System.Net.HttpStatusCode returnStatusCode = HttpStatusCode.NotFound; // not found by default


            return (returnStatusCode, returncontent);
        }

    }
}
