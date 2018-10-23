using System;
using System.Net.WebSockets;
using DNet.ClientMessages;
using DNet.Http;
using DNet.Http.Gateway;
using Newtonsoft.Json;
using PureWebSockets;

namespace DNet.Socket
{
    // Discord Event Handler Delegates
    public delegate void MessageCreateHandler(DNet.Structures.Message message);

    public class SocketHandle
    {
        // Discord Events
        public event EventHandler OnRateLimit;
        public event EventHandler OnReady;
        public event EventHandler OnResumed;
        public event EventHandler OnGuildCreate;
        public event EventHandler OnGuildDelete;
        public event EventHandler OnGuildUpdate;
        public event EventHandler OnGuildUnavailable;
        public event EventHandler OnGuildAvailable;
        public event EventHandler OnGuildMemberAdd;
        public event EventHandler OnGuildMemberRemove;
        public event EventHandler OnGuildMemberUpdate;
        public event EventHandler OnGuildMemberAvailable;
        public event EventHandler OnGuildMemberSpeaking;
        public event EventHandler OnGuildMembersChunk;
        public event EventHandler OnGuildIntegrationsUpdate;
        // ...
        public event MessageCreateHandler OnMessageCreate;
        public event EventHandler OnMessageDelete;
        public event EventHandler OnMessageUpdate;

        private readonly Client client;

        private Nullable<int> heartbeatLastSequence = null;
        private PureWebSocket socket;

        public SocketHandle(Client client)
        {
            this.client = client;
        }

        public async void Connect()
        {
            Console.WriteLine($"Authenticating using token '{this.client.GetToken()}'");

            var response = await Fetch.GetJsonAsyncAuthorized<GetGatewayBotResponse>(ApiEndpoints.BotGateway(), this.client.GetToken());
            var convertedResponse = JsonConvert.SerializeObject(response);
            var connectionUrl = "wss://gateway.discord.gg/?v=6&encoding=json";

            this.socket = new PureWebSocket(connectionUrl, new PureWebSocketOptions() { });

            // Events
            this.socket.OnMessage += this.WS_OnMessage;
            this.socket.OnClosed += this.WS_OnClosed;

            // Connect
            this.socket.Connect();

            // TODO: Debugging
            Console.WriteLine($"GOT url => {convertedResponse}");
        }

        private void WS_OnMessage(string messageString)
        {
            // TODO: Debugging
            // Console.WriteLine($"WS Received => {messageString}");

            GatewayMessage message = JsonConvert.DeserializeObject<GatewayMessage>(messageString);

            Console.WriteLine($"WS Handling message with OPCODE '{message.opCode}'");

            switch (message.opCode)
            {
                case OpCode.Hello:
                    {
                        GatewayHelloMessage helloMessage = (GatewayHelloMessage)JsonConvert.DeserializeObject<GatewayHelloMessage>(JsonConvert.SerializeObject(message.data));

                        Console.WriteLine($"WS Acknowledged heartbeat at {helloMessage.heartbeatInterval}ms interval");

                        Utils.SetInterval(() =>
                        {
                            this.Send(OpCode.Heartbeat, new ClientHeartbeatMessage(1, this.heartbeatLastSequence));
                        }, TimeSpan.FromMilliseconds(helloMessage.heartbeatInterval));

                        var dat = new ClientIdentifyMessage(
                            this.client.GetToken(),

                            new ClientIdentifyMessageProperties(
                                "Linux",
                                "disco",
                                "disco"
                            ),

                            false,

                            250,

                            // TODO: Hard-coded
                            new int[] { 0, 1 },

                            new ClientPresence(
                                new ClientPresenceGame("Testing bot", 0),

                                "dnd",

                                91879201,
                                false
                            )
                        );

                        Console.WriteLine("Sending ...");

                        this.Send(OpCode.Identify, dat);

                        break;
                    }

                case OpCode.Dispatch:
                    {
                        switch (message.type) {
                            case "MESSAGE_CREATE": {
                                Console.WriteLine("Handling message create ...");

                                var ser = JsonConvert.SerializeObject((string)message.data);

                                Console.WriteLine("SER", ser);
                                // TODO: Left here

                                var msg = JsonConvert.DeserializeObject<DNet.Structures.Message>(ser);

                                Console.WriteLine("pass");

                                this.OnMessageCreate.Invoke(msg);

                                break;
                            }

                            default: {
                                Console.WriteLine($"Unknown dispatch message type: {message.type}");

                                break;
                            }
                        }

                        break;
                    }

                default:
                    {
                        Console.WriteLine($"WS Unable to handle OPCODE '{message.opCode}' (Not implemented)");

                        break;
                    }
            }
        }

        private void WS_OnClosed(WebSocketCloseStatus reason)
        {
            Console.WriteLine($"WS Socket closed => {reason.ToString()}");
        }

        private void Send(OpCode opCode, dynamic data)
        {
            var message = new ClientMessage()
            {
                data = data,
                opCode = opCode
            };

            var serializedMessage = JsonConvert.SerializeObject(message);

            this.socket.Send(serializedMessage);
            Console.WriteLine($"WS Sent => {serializedMessage}");
        }
    }
}