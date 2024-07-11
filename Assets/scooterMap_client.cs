using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class scooterMap_client : Client_Object {

    public GameObject QuestionairPrefab;
    private QN_Display qnmanager;
    public Camera minimapCamera;

    private const string OffsetFileName = "scooterMap_client";
    private NetworkVariable<SpawnType> m_spawnType=new NetworkVariable<SpawnType>();
    private NetworkVariable<ParticipantOrder> m_participantOrder=new NetworkVariable<ParticipantOrder>();
    private NetworkVariable<ActionState> m_ActionState=new NetworkVariable<ActionState>();
    private Interactable_Object m_InteractableObject;
    private NetworkVehicleController m_VehicleController;

    private CharacterController m_characterController;

    public float walkingSpeed=1.5f;
    public Transform m_Camera;
    private const float k_LookMultiplier = 500;
    // Start is called before the first frame update
    void Start() {

        m_characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        if (IsServer) {
            m_ActionState.Value = ConnectionAndSpawning.Singleton.ServerState;
        }
        if (!IsLocalPlayer) return;
        
    
    }


    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        if (!IsLocalPlayer) {
            DisableNonLocalobjects();
        }
        else if (IsServer){
       
        }
       
        else {
            m_ActionState.OnValueChanged += ActionStateUpdate;
            GetMainCamera();
        }
        
    }

    private void ActionStateUpdate(ActionState previousvalue, ActionState newvalue) {
        if (newvalue is ActionState.READY or ActionState.DRIVE &&
            
            previousvalue is ActionState.LOADINGSCENARIO or ActionState.LOADINGVISUALS) {
            Debug.Log("Find event to change cameras! ");
            m_Camera = FindObjectOfType<exDispCamera>().transform;

            Debug.Log($"Find Object {m_Camera.name}. ");
            if (m_Camera != null) {
                var component = m_Camera.GetComponent<Camera>();
                Debug.Log("Finda camera trying to change targetDisplay. ");
                if (component != null) component.targetDisplay = 0;
            }

            else {
                m_Camera = null;
            }
        }
    }

    private void DisableNonLocalobjects() {
       // GetComponentInChildren<Camera>().enabled = false;
       // GetComponentInChildren<AudioListener>().enabled = false;

    }

    public override ParticipantOrder GetParticipantOrder() {
        return m_participantOrder.Value;
    }

    public override void SetParticipantOrder(ParticipantOrder _ParticipantOrder) {
        m_participantOrder.Value = _ParticipantOrder;
    }
    public override void SetSpawnType(SpawnType _spawnType) {
        m_spawnType.Value = _spawnType;
        if (IsServer) {
            switch (m_spawnType.Value) {
                case SpawnType.NONE:
                    break;
                case SpawnType.CAR:
                    GetComponent<CharacterController>().enabled = false;
        
                    break;
                case SpawnType.PEDESTRIAN:
                    GetComponent<CharacterController>().enabled = true;
                    break;
                case SpawnType.PASSENGER:
                    break;
                case SpawnType.ROBOT:
                    break;
                case SpawnType.SCOOTER:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public override void AssignFollowTransform(Interactable_Object MyInteractableObject, ulong targetClient) {
        
        
    }
    
    [ClientRpc]
    private void AssignInteractable_ClientRPC(NetworkObjectReference MyInteractable, ulong targetClient)
    {
       
    }

    public override Interactable_Object GetFollowTransform() {
       return null;
    }

    public override void De_AssignFollowTransform(ulong targetClient, NetworkObject netobj)
    {
       
    }

    [ClientRpc]
    private void De_AssignFollowTransformClientRPC(ulong targetClient)
    {
         
    }

    
    public override Transform GetMainCamera() {
       return m_Camera;
    }

    public override void CalibrateClient(Action<bool> finishedCalibration) {
        if (!IsLocalPlayer) return;
        finishedCalibration.Invoke(true);
        
       
    }

    
    public override void StartQuestionair(QNDataStorageServer m_QNDataStorageServer) {
      GoForPostQuestion();
    }

    public override void GoForPostQuestion() {
        if (!IsLocalPlayer) return;
        PostQuestionServerRPC(OwnerClientId);
    }

    public override void SetNewNavigationInstruction(Dictionary<ParticipantOrder, NavigationScreen.Direction> Directions) {
       
    }

    [ServerRpc]
    public void PostQuestionServerRPC(ulong clientID)
    {
        ConnectionAndSpawning.Singleton.FinishedQuestionair(clientID);
    }
}
