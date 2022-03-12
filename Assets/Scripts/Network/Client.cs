using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using System.Linq;
using UnityEngine;
using Network;

namespace Network
{
    public class Client : MonoBehaviour
    {
        public static Thread inbound;
        public static UdpClient client;
        public static IPEndPoint endpoint;

        static public Queue<Action> queue = new Queue<Action>();

        public static bool connected = false;
        System.Timers.Timer connecting;
        System.Timers.Timer ping;

        void Start()
        {
            endpoint = new IPEndPoint(Network.Instance.clientAddresss, Network.Instance.clientPort);

            client = new UdpClient();
            client.Connect(endpoint);
            connected = false;

            inbound = new Thread(new ThreadStart(Inbound));
            inbound.Start();

            connecting = new System.Timers.Timer();
            connecting.Elapsed += new ElapsedEventHandler((object source, ElapsedEventArgs e) => {
                SendPacket(new Packet(Packet.OPCodes.Connecting));
            });
            connecting.Interval = 500;
            connecting.Enabled = true;
        }

        private void OnDestroy()
        {
            inbound.Abort();
        }

        public void Update()
        {
            while (queue.Count > 0)
            {
                Action action = null;
                lock (queue)
                {
                    if (queue.Count > 0)
                        action = queue.Dequeue();
                }
                action?.Invoke();
            }
        }


        // use for updating from network. Otherwise just send data.
        void Defer(Action action)
        {
            lock (queue)
            {
                queue.Enqueue(action);
            }
        }

        void Inbound()
        {
            while (true)
            {
                try
                {
                    if (!connected)
                    {
                        client = new UdpClient();
                        client.Connect(endpoint);
                        connected = true;
                    }

                    Packet data = new Packet(client.Receive(ref endpoint));
                    switch (data.GetOpCode())
                    {
                        case Packet.OPCodes.Connected:
                            Debug.Log("connected");
                            connecting.Stop();
                            ping = new System.Timers.Timer();
                            ping.Elapsed += new ElapsedEventHandler((object source, ElapsedEventArgs e) => {
                                SendPacket(new Packet(Packet.OPCodes.Ping));
                            });
                            ping.Interval = 500;
                            ping.Enabled = true;
                            break;
                        case Packet.OPCodes.Ping:
                            Debug.Log($"pong at {System.DateTime.Now}");
                            break;
                    }
                }
                catch (SocketException)
                {
                    connected = false;
                    Debug.Log($"socket problem, waiting 500ms and reconnecting");
                    Thread.Sleep(500);
                }
            }
        }

        void SendPacket(Packet packet)
        {
            client.Send(packet.GetBytes(), packet.Size());
        }
    }

}