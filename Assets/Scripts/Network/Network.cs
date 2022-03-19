using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace Network
{
    public class Network : MonoBehaviour
    {
        private static Network _instance;
        public static Network Instance
        {
            get => _instance;
            private set
            {
                if (_instance != null)
                {
                    Debug.LogError("attempting to overwrite a network instance.");
                }
                _instance = value;
            }
        }

        public bool active;
        public bool isServer = false;
        public bool emulateServer = false;
        public string clientAddressValue = "127.0.0.1";
        public string serverAddressValue = "127.0.0.1";
        public int clientPort = 6969;

        public IPAddress clientAddresss;
        [NonSerialized] public Server server;
        [NonSerialized] public Client client;

        void Start()
        {
            Instance = this;
            DontDestroyOnLoad(this);

            // running in server mode
            if (Application.isBatchMode || emulateServer)
            {
                StartServer();
            }
            // running in client mode
            else
            {
                StartClient();
            }
            active = true;

        }

        private void StartServer()
        {
            Application.targetFrameRate = 60;
            Debug.Log("running server mode:");

#if !UNITY_EDITOR

                try
                {
                    // parse out port and level and whatever else
                    ParseArguments();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
#endif

            clientAddresss = IPAddress.Parse(serverAddressValue);
            server = this.gameObject.AddComponent<Server>();
            isServer = true;
        }

        private void StartClient()
        {
            if (SteamManager.Initialized)
            {
                Debug.Log($"running client mode:\nUser: {Steamworks.SteamFriends.GetPersonaName()} \nID: {Steamworks.SteamUser.GetSteamID()}");

                clientAddresss = IPAddress.Parse(clientAddressValue);
                client = this.gameObject.AddComponent<Client>();
                isServer = false;
                return;
            }

            Debug.LogError("You are not logged into steam.");
        }

        public void RegisterListener(Packet.OPCodes oPCode, Action<Packet> action)
        {
            if (!active)
            {
                Debug.LogError("attempting to register callback before active.");
            }
            
            if (isServer)
            {
                server.Callbacks[oPCode] += action;
            }
            else
            {
                client.Callbacks[oPCode] += action;
            }
        }

        public void UnregisterListener(Packet.OPCodes oPCode, Action<Packet> action)
        {
            if (!active)
            {
                Debug.LogError("attempting to register callback before active.");
            }

            if (isServer)
            {
                server.Callbacks[oPCode] -= action;
            }
            else
            {
                client.Callbacks[oPCode] -= action;
            }
        }

        private void ParseArguments()
        {
            string[] args = System.Environment.GetCommandLineArgs();

            for (int i = 0; i < args.Length; ++i)
            {
                switch (args[i])
                {
                    case "--port":
                        clientPort = int.Parse(args[i + 1]);
                        Debug.Log($"set port to {clientPort}");
                        break;
                    case "--ip":
                        serverAddressValue = args[i + 1];
                        clientAddressValue = args[i + 1];
                        Debug.Log($"set ip to {clientAddressValue}");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}