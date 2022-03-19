using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Network
{
    public class Server : MonoBehaviour
    {
        public Thread inbound;
        public UdpClient client;
        public List<IPEndPoint> connections;
        public Queue<Action> queue = new Queue<Action>();

        public Dictionary<Packet.OPCodes, Action<Packet>> Callbacks = new Dictionary<Packet.OPCodes, Action<Packet>>();

        private void Start()
        {
            connections = new List<IPEndPoint>();
            client = new UdpClient(Network.Instance.clientPort);
            inbound = new Thread(new ThreadStart(Inbound));
            inbound.Start();

            Debug.Log($"listening to: {client} {Network.Instance.clientPort}");
            foreach (var opCode in Utilities.GetEnums(typeof(Packet.OPCodes)).Cast<Packet.OPCodes>())
            {
                Callbacks[opCode] = (x) => { };
            }
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

        public void Defer(Action action)
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
                IPEndPoint address;

                try
                {
                    address = new IPEndPoint(IPAddress.Any, Network.Instance.clientPort);
                    Packet data = new Packet(client.Receive(ref address));

                    switch (data.GetOpCode())
                    {
                        case Packet.OPCodes.Connecting:
                            bool exists = connections.Any(c => c.Address.Equals(Network.Instance.clientAddresss) && c.Port == Network.Instance.clientPort);
                            if (!exists)
                            {
                                connections.Add(address);
                            }

                            Debug.Log($"player connected from {address}");
                            SendPacket(new Packet(Packet.OPCodes.Connected), address);
                            break;
                        case Packet.OPCodes.Ping:
                            Debug.Log($"pinged by {address}");
                            SendPacket(new Packet(Packet.OPCodes.Ping), address);
                            break;
                    }
                }
                catch (SocketException)
                {
                    
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        void SendPacket(Packet packet, IPEndPoint address)
        {
            client.Send(packet.GetBytes(), packet.Size(), address);
        }
    }
}