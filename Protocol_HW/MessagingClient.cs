using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol_HW
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public class MessagingClient
    {
        private readonly HttpClient _httpClient;
        private readonly UdpClient _udpClient;

        public MessagingClient()
        {
            _httpClient = new HttpClient();
            _udpClient = new UdpClient();
        }

      
        public async Task SendTcpMessageAsync(string message, string ipAddress, int port)
        {
            using (TcpClient client = new TcpClient(ipAddress, port))
            {
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);
                Console.WriteLine("TCP Message sent.");
            }
        }


        public async Task<string> ReceiveTcpMessageAsync(int port)
        {
            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Start();
            using (TcpClient client = await server.AcceptTcpClientAsync())
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("TCP Message received: " + message);
                return message;
            }
        }

        public async Task SendUdpMessageAsync(string message, string ipAddress, int port)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            await _udpClient.SendAsync(data, data.Length, ipAddress, port);
            Console.WriteLine("UDP Message sent.");
        }


        public async Task<string> ReceiveUdpMessageAsync(int port)
        {
            _udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));
            UdpReceiveResult result = await _udpClient.ReceiveAsync();
            string message = Encoding.UTF8.GetString(result.Buffer);
            Console.WriteLine("UDP Message received: " + message);
            return message;
        }

   
        public async Task<string> SendHttpMessageAsync(string url, string message)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(url, new StringContent(message, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("HTTP Message sent. Response: " + responseBody);
            return responseBody;
        }


        public void Close()
        {
            _udpClient.Close();
            _httpClient.Dispose();
        }
    }

}
