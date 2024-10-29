using Protocol_HW;

MessagingClient client = new MessagingClient();


await client.SendTcpMessageAsync("Hello TCP!", "127.0.0.1", 5000);
string tcpResponse = await client.ReceiveTcpMessageAsync(5000);


await client.SendUdpMessageAsync("Hello UDP!", "127.0.0.1", 6000);
string udpResponse = await client.ReceiveUdpMessageAsync(6000);


string httpResponse = await client.SendHttpMessageAsync("http://localhost:5001/api/messages", "{\"message\":\"Hello HTTP!\"}");

client.Close();