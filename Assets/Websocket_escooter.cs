using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using NativeWebSocket;

public class Websocket_escooter : MonoBehaviour
{
    public string Websocket_message;
    public string raw_message;
    public string previous_message = "";

    public string server_addr = "ws://192.168.1.10:8888";

  WebSocket websocket;

  // Start is called before the first frame update
  async void Start()
  {
    websocket = new WebSocket(server_addr);

    websocket.OnOpen += () =>
    {
      Debug.Log("Connection open!");
    };

    websocket.OnError += (e) =>
    {
      Debug.Log("Error! " + e);
    };

    websocket.OnClose += (e) =>
    {
      Debug.Log("Connection closed!");
    };

    websocket.OnMessage += (bytes) =>
    {
      raw_message = System.Text.Encoding.UTF8.GetString(bytes);
      var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

      // getting the message as a string
    //   if (message != previous_message){
        Websocket_message = timestamp + ": " + raw_message;
        // previous_message = message;
    //   }
    //   else{
    //     Websocket_message = timestamp + ": ";
    //   }
        Debug.Log("OnMessage! " + Websocket_message);

    };

    // Keep sending messages at every 0.3s
    // InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

    // waiting for messages
    await websocket.Connect();
  }

  void Update()
  {
    #if !UNITY_WEBGL || UNITY_EDITOR
      websocket.DispatchMessageQueue();
    #endif
    // if (raw_message != previous_message){
    //     previous_message = raw_message;
    // }
    // else{
    //     Websocket_message = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + ": ";
    // }

  }

//   void Late 

  async void SendWebSocketMessage()
  {
    if (websocket.State == WebSocketState.Open)
    {
      // Sending bytes
    //   await websocket.Send(new byte[] { 10, 20, 30 });

      // Sending plain text
    //   await websocket.SendText("plain text message");
    }
  }

  private async void OnApplicationQuit()
  {
    await websocket.Close();
  }

}