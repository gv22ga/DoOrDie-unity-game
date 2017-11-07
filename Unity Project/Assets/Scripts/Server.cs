
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using SimpleJSON;

public class Server : MonoBehaviour
{

    // Use this for initialization
    private static Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private static Socket server1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private static List<Socket> clientList = new List<Socket>();
    private static int index = 0;
    private static int index1 = 0;
    private static List<Byte[]> bufferList = new List<byte[]>();
    private static List<Byte[]> bufferList1 = new List<byte[]>();
    
    


    void Start()
    {
        print("in g1 script");
        setupServer();
        setupServer1();

    }

    private void setupServer()
    {
        print("server1started");
        server.Bind(new IPEndPoint(IPAddress.Any, 22112));
        server.Listen(5);
        server.BeginAccept(new AsyncCallback(AcceptCallback), null);
    }
    private void setupServer1()
    {
        print("server2started");
        server1.Bind(new IPEndPoint(IPAddress.Any, 22113));
        server1.Listen(5);
        server1.BeginAccept(new AsyncCallback(AcceptCallback1), null);
    }

    private static void AcceptCallback(IAsyncResult AR)
    {
        Socket client = server.EndAccept(AR);

        List<System.Object> objectList = new List<System.Object>();
        objectList.Add(index);

        objectList.Add(client);
        print("connection received1");
        Byte[] buffer = new Byte[1000];
        bufferList.Add(buffer);
        client.BeginReceive(bufferList[index], 0, bufferList[index].Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), objectList);
        index++;
        server.BeginAccept(new AsyncCallback(AcceptCallback), null);

    }
    private static void AcceptCallback1(IAsyncResult AR)
    {
        Socket client = server1.EndAccept(AR);

        List<System.Object> objectList = new List<System.Object>();
        objectList.Add(index1);

        objectList.Add(client);
        print("connection received2");
        Byte[] buffer = new Byte[1000];
        bufferList1.Add(buffer);
        client.BeginReceive(bufferList1[index1], 0, bufferList1[index1].Length, SocketFlags.None, new AsyncCallback(ReceiveCallback1), objectList);
        index1++;
        server1.BeginAccept(new AsyncCallback(AcceptCallback1), null);

    }

    private static void ReceiveCallback(IAsyncResult AR)
    {


        List<System.Object> objectList = (List<System.Object>)AR.AsyncState;
        int index = (int)objectList[0];

        Socket socket = (Socket)objectList[1];
        int received = socket.EndReceive(AR);
        byte[] data = new byte[received];

        Array.Copy(bufferList[index], data, received);

        string text = Encoding.ASCII.GetString(data);

        //print("receivedLength1:" + text.Length);
        int length = text.IndexOf('}');
        string text2 = text.Substring(0, length + 1);
        //print("text2"+text2);
        try
        {
            JSONNode json = JSON.Parse(text2);
            //print("json"+json);
            if (json != null)
            {
                float x = json["x"].AsFloat;
                float y = json["y"].AsFloat;
                print("x:" + x);
                print("y:" + y);
                switch (index)
                {
                    case 0:
                        PlayerControl.h = y;
                        //Player1Script.y = y;
                        break;
                    case 1:
                        PlayerControl2.h = y;
                        //Player2Script.y = y;
                        break;
                }
            }
        }
        catch (Exception e)
        {
            print("exceptiion:" + e);

        }
        socket.BeginReceive(bufferList[index], 0, bufferList[index].Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), objectList);

    }

    private static void ReceiveCallback1(IAsyncResult AR)
    {


        List<System.Object> objectList = (List<System.Object>)AR.AsyncState;
        int index = (int)objectList[0];

        Socket socket = (Socket)objectList[1];
        int received = socket.EndReceive(AR);
        byte[] data = new byte[received];

        Array.Copy(bufferList1[index], data, received);

        string text = Encoding.ASCII.GetString(data);

        //print("receivedLength2:" + text.Length);

        try
        {
            JSONNode json = JSON.Parse(text);
            if (json != null)
            {
                string x= json["name"];
                switch (index)
                {
                    case 0:
                        switch (x)
                        {
                            case "fire":
                                Gun.h = true;
                                break;
                            case "jump":
                                PlayerControl.j = true;
                                break;
                            case "bomb":
                                LayBombs.h = true;
                                break;
                            default:
                                PlayerControl.name = x;
                                break;
                        }
                        break;
                    case 1:
                        switch (x)
                        {
                            case "fire":
                                Gun2.h = true;
                                break;
                            case "jump":
                                PlayerControl2.j = true;
                                break;
                            case "bomb":
                                LayBombs2.h = true;
                                break;
                            default:
                                PlayerControl2.name = x;
                                break;


                        }
                        break;
                }
            }
        }
        catch (Exception e)
        {
            print("exceptiion:" + e);

        }
        socket.BeginReceive(bufferList1[index], 0, bufferList1[index].Length, SocketFlags.None, new AsyncCallback(ReceiveCallback1), objectList);

    }


    // Update is called once per frame
    void Update()
    {

    }
}
