using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class UDPReceiver : MonoBehaviour
{
    // receiving thread
    Thread receiveThread;

    // udpclient object
    UdpClient client;

    // IP and port
    public string IP = "127.0.0.1";
    public int port = 6454;

    private static void Main()
    {
        UDPReceiver receiveObj = new UDPReceiver();
        receiveObj.init();
    }

    // Use this for initialization
    void Start () {
        init();
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    private void OnApplicationQuit()
    {
        stopThread();
    }

    private void init()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void OnDisable()
    {
        if (receiveThread != null)
        {
            receiveThread.Abort();
        }
        client.Close();
    }

    private void stopThread()
    {
        if(receiveThread.IsAlive)
        {
            receiveThread.Abort();
        }
        client.Close();
    }

    private void ReceiveData()
    {
        client = new UdpClient(port);
        while(true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse(IP), 0);
                byte[] data = client.Receive(ref anyIP);
                string text = Encoding.UTF8.GetString(data);
                // offset: 0
                // first 7 bytes are "Art-Net"
                if (text.Substring(0, 7) == "Art-Net")
                {
                    // offset: 8
                    // next 2 bytes are opcode
                    UInt16 opCodeLo = data[8];
                    UInt16 opCodeHi = data[9];
                    ushort opCode = (ushort)(opCodeLo | (opCodeHi << 8));
                    if (opCode == 0x5000)
                    {
                        // offset: 10
                        // next two bytes are protocol version

                        // offset: 16
                        // next two bytes are length
                        String len = text.Substring(16, 1);

                        // values start at offset 18
                        for (int i = 0; i < 512; i++)
                        {
                            int set = data[i + 18];
                            GlobalScript.dmxUniverse[i] = set;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }
}
