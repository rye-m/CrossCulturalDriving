using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// using System.Exception;

public class exDispCamera : MonoBehaviour
{
    public Transform NetworkedScooter; //Head = _interactableObject.GetCameraPositionObject().rotation;
    public GameObject ETA_diplay;


    private Vector3 movement;
    private float rotation;


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

        ETA_diplay.SetActive(false);
    }

    void LateUpdate ()
    {
        Vector3 newPosition = NetworkedScooter.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(90f, NetworkedScooter.eulerAngles.y, 0f);
    }
}