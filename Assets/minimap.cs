using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimap : MonoBehaviour {
    // private Interactable_Object _interactableObject = ScooterController;

    public Transform NetworkedScooter; //Head = _interactableObject.GetCameraPositionObject().rotation;

    private Vector3 movement;
    private float rotation;

    void LateUpdate ()
    {
        Vector3 newPosition = NetworkedScooter.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, NetworkedScooter.eulerAngles.y, 0f);

    }
}