using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ScooterController : Interactable_Object
{
    private ulong m_CLID;

    public Transform Head;

    private Rigidbody rb;


    public override void AssignClient(ulong CLID_, ParticipantOrder _participantOrder_)
    {
        m_participantOrder.Value = _participantOrder_;
        m_CLID = CLID_;

    }

    public override Transform GetCameraPositionObject()
    {
        return Head;
    }

    public override bool HasActionStopped()
    {
        if (rb.velocity.magnitude < 0.01f)
        {
            return true;
        }

        return false;
    }

    public override void SetStartingPose(Pose _pose)
    {
        if (!IsServer) return;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = _pose.position;
        transform.rotation = _pose.rotation;
    }

    public override void Stop_Action()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }




    [System.Serializable]
    public class AxleInfo_es
    {
        public WheelCollider Wheel;
        public bool motor;
        public bool steering;
    }

    public List<AxleInfo_es> AxleInfo_ess;
    public float maxMotorTorque;
    public float maxSteeringAngle;

    // Movement along X and Y axes.
    public float accellation;
    public float brake;
    public float motor_log;
    public float steering_log;
    public float movementX;
    public float movementY;

    
    
    // Gyro data
    
    // Gyro data
    private float acc_x;
    private float acc_y;
    private float acc_z;
    private float rot_x;
    private float rot_y;
    private float rot_z;


    public float acc_x_log;
    public float acc_y_log;
    public float acc_z_log;
    public float rot_x_log;
    public float rot_y_log;
    public float rot_z_log;

    
    
    public string PortName;

    // Communication with Arduino
    SerialPort data_stream;
    public string receivedstring;
    public bool running = false;

    void Start()
    {
        running = true;
        rb = GetComponent<Rigidbody>();
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -1.0f, -0.2f);

        data_stream = new SerialPort("COM3", 115200);

        ListAvailablePorts();

        StartCoroutine(GetSerialData());
    }

    void OnDisable()
    {
        running = false;
        //data_stream.Close();

    }

    // 対応する視覚的なホイールを見つけます
    // Transform を正しく適用します
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void Update()
    {
        // float motor = maxMotorTorque * Input.GetAxis("Vertical");
        // float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        float motor = maxMotorTorque * movementX;
        float steering = maxSteeringAngle * movementY;
        // Debug.Log("debug:" + movementX + ", " + movementY);
        motor_log = motor;
        steering_log = steering;
        
        acc_x_log = acc_x;
        acc_y_log = acc_y;
        acc_z_log = acc_z;
        rot_x_log = rot_x;
        rot_y_log = rot_y;
        rot_z_log = rot_z;


        foreach (AxleInfo_es AxleInfo_es in AxleInfo_ess)
        {
            if (AxleInfo_es.steering)
            {
                AxleInfo_es.Wheel.steerAngle = steering;
            }
            if (AxleInfo_es.motor)
            {
                AxleInfo_es.Wheel.motorTorque = motor;
            }
            ApplyLocalPositionToVisuals(AxleInfo_es.Wheel);
        }
    }

    void ListAvailablePorts()
    {
        string[] ports = SerialPort.GetPortNames();
        Debug.Log("Available Ports:");
        foreach (string port in ports)
        {
            Debug.Log(port);
        }
    }

    IEnumerator GetSerialData()
    {
 
        data_stream.RtsEnable = true;
        data_stream.DtrEnable = true;
        
        data_stream.Open();
        Debug.Log("startCorutine");
        while (running)
        {
            Debug.Log("debug:" + acc_x + ", " + acc_y + ", " + acc_z + ", " + rot_x + ", " + rot_y + ", " + rot_z);
            while (data_stream.BytesToRead > 0)
            {
                receivedstring = data_stream.ReadLine();
                if (receivedstring.Length == 0) continue;
                string[] datas = receivedstring.Split(',');
                if (datas.Length != 9) continue;
                float.TryParse(datas[0], out accellation);
                float.TryParse(datas[1], out brake);
                float.TryParse(datas[2], out movementY);
                float.TryParse(datas[3], out acc_x);
                float.TryParse(datas[4], out acc_y);
                float.TryParse(datas[5], out acc_z);
                float.TryParse(datas[6], out rot_x);
                float.TryParse(datas[7], out rot_y);
                float.TryParse(datas[8], out rot_z);

                movementX = accellation - brake * 3  ;//> 0? accellation - brake : 0;
        // Debug.Log("debug:" + acc_x + ", " + acc_y + ", " + acc_z + ", " + rot_x + ", " + rot_y + ", " + rot_z);
            }

            yield return null;
        }
        data_stream.Close();
    }
}
