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
            }
            // running in client mode
            else
            {
                Debug.Log("running client mode:");
                clientAddresss = IPAddress.Parse(clientAddressValue);
                client = this.gameObject.AddComponent<Client>();
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