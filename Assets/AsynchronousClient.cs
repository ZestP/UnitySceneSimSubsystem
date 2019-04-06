using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class AsynchronousClient : MonoBehaviour
{

    public Text status;
    public static string s;
    // Start is called before the first frame update
    const int portNo = 500;
    private TcpClient _client;
    byte[] data;

    string Error_Message;

    void Start()
    {
        msgs = new Queue<List<string>>();
        try
        {
            this._client = new TcpClient();
            this._client.Connect("127.0.0.1", portNo);
            data = new byte[this._client.ReceiveBufferSize];
            //SendMessage(txtNick.Text);
            SendMessage("Unity Demo Client is Ready!");
            this._client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(this._client.ReceiveBufferSize), ReceiveMessage, null);

        }
        catch (Exception ex)
        {


        }



    }

    // Update is called once per frame
    void Update()
    {
        status.text = s;
    }
    public new void SendMessage(string message)
    {
        try
        {
            NetworkStream ns = this._client.GetStream();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            ns.Write(data, 0, data.Length);
            ns.Flush();
        }
        catch (Exception ex)
        {
            Error_Message = ex.Message;
            //MessageBox.Show(ex.ToString());
        }
    }
    public void ReceiveMessage(IAsyncResult ar)
    {
        try
        {
            //清空errormessage
            Error_Message = "";
            int bytesRead;
            bytesRead = this._client.GetStream().EndRead(ar);
            if (bytesRead < 1)
            {
                return;
            }
            else
            {
                Debug.Log(System.Text.Encoding.UTF8.GetString(data, 0, bytesRead));
                string message = System.Text.Encoding.UTF8.GetString(data, 0, bytesRead);
                s += message + '\n';
                Parse(message);


            }
            this._client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(this._client.ReceiveBufferSize), ReceiveMessage, null);
        }
        catch (Exception ex)
        {
            Error_Message = ex.Message;
        }
    }


    void OnDestroy()
    {

        this._client.Close();
    }
    Queue<List<string>> msgs;
    void Parse(string raw)
    {
        if (raw == "Spawn DD" || raw == "Cancel"||raw=="Select")//Test
            msgs.Enqueue(new List<string>(raw.Split(' ')));

    }
    public List<string> GetMsg(string dedicated)
    {
        //s = "";
        //foreach(List<string> ss in msgs)
        //{
        //    foreach(string s2 in ss)
        //    {
        //        s += s2 + ' ';
        //    }
        //    s += '\n';
        //}
        List<string> result = null;
        while (msgs.Count > 0 && msgs.Peek()[0] == dedicated)
        {
            result = msgs.Dequeue();
        }
        return result;
    }
}

//public class StateObject
//{
//    public Socket workSocket = null;
//    public const int BufferSize = 1024;
//    public byte[] buffer = new byte[BufferSize];
//    public StringBuilder sb = new StringBuilder();
//    public StateObject()
//    {

//    }
//}
//public class AsynchronousClient : MonoBehaviour
//{
//    private IPAddress myIP = IPAddress.Parse("127.0.0.1");
//    private IPEndPoint MyServer;
//    private Socket mySocket;
//    private ManualResetEvent connectReset = new ManualResetEvent(false);
//    private ManualResetEvent sendReset = new ManualResetEvent(false);
//    public Text status;
//    public static string s;
//    // Start is called before the first frame update
//    const int portNo = 500;
//    private TcpClient _client;
//    byte[] data;

//    string Error_Message;

//    public void StartClient()
//    {
//        try
//        {
//            MyServer = new IPEndPoint(myIP, 12345);
//            mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//            mySocket.BeginConnect(MyServer, new AsyncCallback(ConnectCallback), mySocket);
//            connectReset.WaitOne();

//        }
//        catch(Exception ee)
//        {
//            status.text += ee.Message + "\n";
//        }

//    }
//    private void ConnectCallback(IAsyncResult ar)
//    {

//        try
//        {
//            Socket client = (Socket)ar.AsyncState;
//            client.EndConnect(ar);
//            try
//            {
//                byte[] byteData = System.Text.Encoding.BigEndianUnicode.GetBytes("Client ready!\n\r");
//                mySocket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), mySocket);
//            }
//            catch (Exception ee) { status.text += ee.Message + "\n"; }
//            status.text += "Connected to server\n";
//            Thread thread = new Thread(new ThreadStart(target));
//            thread.Start();
//            connectReset.Set();

//        }
//        catch
//        {

//        }

//    }

//    void Start()
//    {
//        msgs = new Queue<List<string>>();
//        StartClient();



//    }



//    public void StopClient()
//    {
//        try
//        {
//            mySocket.Close();
//            status.text += "Client stopped.\n";
//        }
//        catch
//        {
//            status.text += "Client failed stopping.\n";
//        }
//    }

//    void OnDestroy()
//    {

//        StopClient();
//    }
//    private void target()
//    {
//        try
//        {
//            StateObject state = new StateObject();
//            state.workSocket = mySocket;
//            mySocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
//        }
//        catch(Exception ee)
//        {
//            status.text += ee.Message + "\n";
//        }
//    }

//    private void ReceiveCallback(IAsyncResult ar)
//    {
//        try
//        {
//            StateObject state = (StateObject)ar.AsyncState;
//            Socket client = state.workSocket;
//            int bytesRead = client.EndReceive(ar);
//            state.sb.Append(System.Text.Encoding.BigEndianUnicode.GetString(state.buffer, 0, bytesRead));
//            string aa = state.sb.ToString();
//            state.sb.Remove(0, aa.Length);
//            status.text+=aa+'\n';
//            Parse(aa);
//            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
//        }
//        catch { }
//    }
//    public void SendMsg(string msg)
//    {
//        byte[] byteData = System.Text.Encoding.BigEndianUnicode.GetBytes(msg);
//        mySocket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), mySocket);
//    }

//    private void SendCallback(IAsyncResult ar)
//    {
//        try
//        {
//            Socket client = (Socket)ar.AsyncState;
//            sendReset.Set();
//        }
//        catch (Exception eee)
//        {
//            Console.WriteLine(eee.Message);
//        }
//    }

//    Queue<List<string>> msgs;
//    void Parse(string raw)
//    {
//        if (raw == "Spawn DD" || raw == "Cancel")//Test
//            msgs.Enqueue(new List<string>(raw.Split(' ')));

//    }
//    public List<string> GetMsg(string dedicated)
//    {
//        //s = "";
//        //foreach(List<string> ss in msgs)
//        //{
//        //    foreach(string s2 in ss)
//        //    {
//        //        s += s2 + ' ';
//        //    }
//        //    s += '\n';
//        //}
//        List<string> result = null;
//        while (msgs.Count > 0 && msgs.Peek()[0] == dedicated)
//        {
//            result = msgs.Dequeue();
//        }
//        return result;
//    }
//}
