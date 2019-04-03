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
                s += message+'\n';
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
        if (raw == "Spawn DD"||raw=="Cancel")//Test
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
        List<string> result=null;
        while(msgs.Count>0&&msgs.Peek()[0]==dedicated)
        {
            result=msgs.Dequeue();
        }
        return result;
    }
}
