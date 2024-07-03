using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.Exception;

public class exDispCamera : MonoBehaviour
{
    public Transform NetworkedScooter; //Head = _interactableObject.GetCameraPositionObject().rotation;
    private Camera cam; //Head = _interactableObject.GetCameraPositionObject().rotation;

    private Vector3 movement;
    private float rotation;

    Websocket_escooter Websocket_escooter;

    // Start is called before the first frame update
    void Start ()
    {
        Debug.Log ("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON, so start at index 1.
        // Check if additional displays are available and activate each.
        for(int i = 1; i < Display.displays.Length; i++)
            {
                Display.displays[i].Activate();
            }
        cam = this.camera;
    }


    void LateUpdate ()
    {
        Vector3 newPosition = NetworkedScooter.position;

        // if (Websocket_escooter.zoomout_flg){
        //     newPosition.y = transform.position.y + 100;
        // }
        // else{
        //     newPosition.y = transform.position.y;
        // }
        newPosition.y = transform.position.y;
        if (Input.GetKey(KeyCode.KeypadPlus)){
            cam.orthographicSWize -= .1f;
        }
        else if (Input.GetKey(KeyCode.KeypadMinus)){
            cam.orthographicSWize += .1f;
        }
        transform.position = newPosition;

        // transform.rotation = Quaternion.Euler(90f, NetworkedScooter.eulerAngles.y, 0f);

    }
}