// TCP 채팅 프로그램 Server

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Server
    {
        private TcpListener listener;                                               // 리스너
        private readonly List<ClientHandler> clients = new List<ClientHandler>();   // 현재 접속된 클라이언트 목록
        private readonly object clientLock = new object();                          // 클라이언트 목록 락

        public Server(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            // 포트를 열고 접속 대기
            listener.Start();
            Console.WriteLine("[SERVER] Server Starting, Waiting for Connection");

            while (true)
            {
                // 클라이언트 접속 대기 (블로킹)
                TcpClient tcpClient = listener.AcceptTcpClient();
                Console.WriteLine("[SERVER] Client Connected");

                // 새롭게 접속한 클라이언트
                ClientHandler handler = new ClientHandler(tcpClient, this);

                // 공유자원 보호
                lock (clientLock)
                {
                    clients.Add(handler);
                }

                // 클라이언트 1개당 스레드 하나 생성
                Thread clientThread = new Thread(handler.Run);
                clientThread.IsBackground = true;
                clientThread.Start();
            }
        }

        // 모든 클라이언트에게 메시지 전송
        public void Broadcast(string message, ClientHandler excludeClient = null)
        {
            lock (clientLock)
            {
                foreach (var client in clients.ToArray())
                {
                    if (client != excludeClient)
                    {
                        client.Send(message);
                    }
                }
            }
        }

        // 클라이언트 종료 시 목록에서 제거
        public void RemoveClient(ClientHandler client)
        {
            lock (clientLock)
            {
                clients.Remove(client);
            }
        }

        static void Main(string[] args)
        {
            int port = 9000;
            Server server = new Server(port);
            server.Start();
        }
    }

    // 클라이언트
    class ClientHandler
    {
        private TcpClient client;       // 클라이언트와 TCP 연결
        private Server server;          // 서버 참조 (메시지)
        private StreamReader reader;    // 문자열 수신
        private StreamWriter writer;    // 문자열 송신

        // 클라이언트 이름
        public string Name { get; private set; } = "name";

        public ClientHandler(TcpClient client, Server server)
        {
            this.client = client;
            this.server = server;

            NetworkStream stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream)
            {
                AutoFlush = true
            };
        }

        // 스레드로 실행되는 함수, 해당 클라이언트와 통신
        public void Run()
        {
            try
            {
                // 1. 이름 입력받기
                writer.WriteLine("[SERVER] Enter Name: ");
                Name = reader.ReadLine();

                Console.WriteLine($"[SERVER] {Name} is Connection");
                server.Broadcast($"[SERVER] {Name} is Connection");

                // 2. 메시지 입력받기 (루프)
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // 2.1. 종료
                    if (line.Equals("/quit", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    // 2.2. 메시지 전송
                    Console.WriteLine($"[{Name}] {line}");
                    server.Broadcast($"[{Name}] {line}", this);
                }
            }
            catch
            {
                // 강제 종료
                Console.WriteLine($"[{Name}] Connection Terminated");
                server.Broadcast($"[SERVER] {Name} Connection Terminated");
                server.RemoveClient(this);
                client.Close();
            }
        }

        // 서버가 클라이언트에게 메시지 전송
        public void Send(string message)
        {
            writer.WriteLine(message);
        }
    }
}
