using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;


public class CarController : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI TxtSpeed;

    public WheelCollider FrontLeft;
    public WheelCollider FrontRight;
    public WheelCollider BackLeft;
    public WheelCollider BackRight;
    
    public GameObject BackLight;

    public Transform FL;
    public Transform FR;
    public Transform BL;
    public Transform BR;
    
    public float Torque;
    public float Speed;
    public float MaxSpeed = 800f;
    public int Brake;
    public float CoefAcceleration;
    public float wheelAngleMax = 10f;
    public bool Freinage = false;
    public float DAMax = 40f;

    private void Start()
    {
        //Régler le centre de masse
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, -0.2f, -0.8f);
        
    }

    void Update()
    {   
        //Son du moteur
        GetComponent<AudioSource>().pitch = Speed / MaxSpeed + 0.7f;
        
        //Affichage de la vitesse
        Speed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
        TxtSpeed.text = "Speed: " + (int)Speed;
        
        //Acceleration
        if (Input.GetKey(KeyCode.UpArrow) && Speed < MaxSpeed)
        {
            if (!Freinage)
            {
                BackLeft.brakeTorque = 0;
                BackRight.brakeTorque = 0;
                FrontLeft.brakeTorque = 0;
                FrontRight.brakeTorque = 0;
                
                BackLeft.motorTorque = Input.GetAxis("Vertical") * Torque * CoefAcceleration * Time.deltaTime;
                BackRight.motorTorque = Input.GetAxis("Vertical") * Torque * CoefAcceleration * Time.deltaTime;
            }

        }
        
        //Deceleration
        else if (!Input.GetKey(KeyCode.UpArrow) && !Freinage || Speed > MaxSpeed )
        { 
            BackLeft.motorTorque = 0;
            BackRight.motorTorque = 0;
            FrontLeft.motorTorque = 0;
            FrontRight.motorTorque = 0;

            BackLeft.brakeTorque = Brake * CoefAcceleration * Time.deltaTime / 10;
            BackRight.brakeTorque = Brake * CoefAcceleration * Time.deltaTime / 10;
        }
        
        //Direction du véhicule

        float Da = (((wheelAngleMax - DAMax) / MaxSpeed) * Speed) + DAMax; 
        FrontLeft.steerAngle = Input.GetAxis("Horizontal") * Da;
        FrontRight.steerAngle = Input.GetAxis("Horizontal") * Da;

        //Rotation des roues
        FL.Rotate(FrontLeft.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        FR.Rotate(FrontRight.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        BL.Rotate(BackLeft.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        BR.Rotate(BackRight.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        
        //SteerAngle Mesh
        FL.localEulerAngles = new Vector3(FL.localEulerAngles.x, FrontLeft.steerAngle - FL.localEulerAngles.z,
            FL.localEulerAngles.z);
        FR.localEulerAngles = new Vector3(FR.localEulerAngles.x, FrontRight.steerAngle - FR.localEulerAngles.z,
            FR.localEulerAngles.z);
        BR.localEulerAngles = new Vector3(BR.localEulerAngles.x, BackRight.steerAngle - BR.localEulerAngles.z,
            BR.localEulerAngles.z);
        BL.localEulerAngles = new Vector3(BL.localEulerAngles.x, BackLeft.steerAngle - BL.localEulerAngles.z,
            BL.localEulerAngles.z);

        //Marche arrière
        if (Input.GetKey(KeyCode.DownArrow))
        {
            BackLeft.brakeTorque = 0;
            BackRight.brakeTorque = 0;
            FrontLeft.brakeTorque = 0;
            FrontRight.brakeTorque = 0;
                
            BackLeft.motorTorque = Input.GetAxis("Vertical") * Torque * CoefAcceleration * Time.deltaTime;
            BackRight.motorTorque = Input.GetAxis("Vertical") * Torque * CoefAcceleration * Time.deltaTime ;
        }
        
        //Freinage

        if (Input.GetKey(KeyCode.Space))
        {
            Freinage = true;

            BackLeft.brakeTorque = 9000;
            BackRight.brakeTorque = 9000;
            FrontLeft.brakeTorque = 9000;
            FrontRight.brakeTorque = 9000;

            BackLeft.motorTorque = 0;
            BackRight.motorTorque = 0;
            FrontLeft.motorTorque = 0;
            FrontRight.motorTorque = 0;
            
            BackLight.SetActive(true);
        }
        
        else
        {
            Freinage = false;
            BackLight.SetActive(false);
        }
    }
}
