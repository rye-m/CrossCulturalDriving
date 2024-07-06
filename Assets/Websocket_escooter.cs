using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using NativeWebSocket;

public class Websocket_escooter : MonoBehaviour
{
  public string Websocket_message;
  public string raw_message;
  public string previous_message = "";

  public string server_addr = "ws://192.168.1.2:8888";
  private bool zoom_flg = false;
  private bool eta_flg = true;

  public Camera secondCam;
  public GameObject ETA_object;
  public TMP_Text textMeshPro;
  public Transform NetworkedScooter; 

  private float totalDistanceTraveled;
  private float totalDistance = 620;
  private Vector3 lastPosition;


  WebSocket websocket;

  void Awake(){
      lastPosition = NetworkedScooter.transform.position;
  }

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

    };

    // waiting for messages
    await websocket.Connect();
  }

  void Update()
  {   
    #if !UNITY_WEBGL || UNITY_EDITOR
      websocket.DispatchMessageQueue();
    #endif

    if (raw_message == "ETA"){
        Debug.Log("raw_message: " + raw_message);
        eta_flg = !eta_flg;
        ETA_object.SetActive(eta_flg);
        previous_message = raw_message;
    }
    
    else if (raw_message == "zoom"){
        zoom_flg = !zoom_flg;
        zoom();
        previous_message = raw_message;
    }
    
    Websocket_message = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + ": " + raw_message;

    SetText();
    raw_message = "";
  }

    void LateUpdate ()
    {
      totalDistanceTraveled += Vector3.Distance(NetworkedScooter.transform.position, lastPosition);
      lastPosition = NetworkedScooter.transform.position;
    }

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

  private void zoom(){
        if (zoom_flg){
            secondCam.orthographicSize = 100;
        }
        else if(!zoom_flg){
            secondCam.orthographicSize = 10;
        }

  }

  void SetText(){
      float ETA_ratio = totalDistanceTraveled/totalDistance;
      textMeshPro.text = "Progress(Howfaryou'vecome):" + ETA_ratio.ToString("F2") + "%";
      // unitycccDefaultText.text = "test";
  }

}