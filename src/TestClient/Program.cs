using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        private static string baseUrl = "http://localhost:5000/api";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Chat Client");

            // Register a new user
            Console.Write("Enter your nickname: ");
            var nickname = Console.ReadLine();
            await RegisterUser(nickname);

            // Log in the user
            var sessionId = await LoginUser(nickname);

            // Connect to WebSocket
            using (var webSocket = new ClientWebSocket())
            {
                //var roomId = Guid.NewGuid().ToString();
                var roomId = "8ad249ce-af22-4ced-a09b-8aca15f4ea9e";
                await webSocket.ConnectAsync(new Uri($"ws://localhost:5000/ws/{roomId}/{sessionId}"), CancellationToken.None);
                Console.WriteLine("Connected to chat room.");

                // Start listening for messages in a separate task
                var receivingTask = Task.Run(() => ReceiveMessages(webSocket));

                // Send messages
                while (true)
                {
                    var message = Console.ReadLine();
                    if (!string.IsNullOrEmpty(message))
                    {
                        await SendMessage(webSocket, message);
                    }
                }
            }
        }

        private static async Task RegisterUser(string nickname)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync($"{baseUrl}/user/register/{nickname}", null);
                response.EnsureSuccessStatusCode();
                Console.WriteLine("User registered successfully.");
            }
        }

        private static async Task<string> LoginUser(string nickname)
        {
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent($"{{\"nickname\":\"{nickname}\"}}", Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync($"{baseUrl}/user/login", content);
                response.EnsureSuccessStatusCode();

                var sessionId = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"User logged in. Session ID: {sessionId}");
                return sessionId;
            }
        }

        private static async Task SendMessage(ClientWebSocket webSocket, string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine($"Sent: {message}");
        }

        private static async Task ReceiveMessages(ClientWebSocket webSocket)
        {
            var buffer = new byte[1024];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Received: {message}");
                }
            }
        }
    }
}
