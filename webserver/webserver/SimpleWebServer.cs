using System;
using System.Net;
using System.Text;
using System.Threading;

namespace webserver
{
    public class SimpleWebServer
    {
        private readonly HttpListener _listener;
        private readonly string _prefix;

        public SimpleWebServer(string prefix)
        {
            _listener = new HttpListener();
            _prefix = prefix;
        }

        public void Start()
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("HTTP Listener is not supported on this platform");
                return;
            }

            _listener.Prefixes.Add(_prefix);
            _listener.Start();
            Console.WriteLine($"Server started and listening on {_prefix}");

            ThreadPool.QueueUserWorkItem((o) =>
            {
                try
                {
                    while (_listener.IsListening)
                    {
                        var context = _listener.GetContext();
                        ProcessRequest(context);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
            Console.WriteLine("Server stopped");
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                string responseText = "Hello, Web";
                byte[] buffer = Encoding.UTF8.GetBytes(responseText);

                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing request: {ex.Message}");
            }
            finally
            {
                context.Response.Close();
            }
        }
    }

}
