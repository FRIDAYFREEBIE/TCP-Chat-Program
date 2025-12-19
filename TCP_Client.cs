// TCP 채팅 프로그램 Client

using System;
using System.Net.Sockets;
using System.Threading;

namespace TCP_Client
{
    class Client
    {
        private TcpClient client;       // 서버와 TCP 연결
        private StreamReader reader;    // 수신
        private StreamWriter writer;    // 송신

        public void Start(string host, int port)
        {
            try
            {
                // 1. 서버에 접속
                client = new TcpClient();
                client.Connect(host, port);

                NetworkStream stream = client.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream)
                {
                    AutoFlush = true
                };

                Console.WriteLine("[CLIENT] Connected to the Server");

                // 2. 메시지 수신 (스레드)
                Thread receiveThread = new Thread(ReceiveLoop);
                receiveThread.IsBackground = true;
                receiveThread.Start();

                // 3. 메시지 송신 (루프)
                string line;
                while ((line = Console.ReadLine()) != null)
                {
                    writer.WriteLine(line);

                    // 3.1. 종료
                    if (line.Equals("/quit", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
            }
        }

        // 서버에서 메시지를 읽는 함수, 스레드로 실행
        private void ReceiveLoop()
        {
            try
            {
                while (true)
                {
                    string message = reader.ReadLine();
                    if (message == null)
                    {
                        throw new Exception("Disconnected from Server");
                    }

                    Console.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
            }
        }

        static void Main(string[] args)
        {
            // 127.0.0.1 (로컬)
            Console.Write("[CLIENT] Enter Server IP: ");
            string host = Console.ReadLine();

            int port = 9000;
            Client client = new Client();
            client.Start(host, port);
        }
    }
}
