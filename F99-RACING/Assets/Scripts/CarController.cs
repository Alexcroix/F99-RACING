using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CarController : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI TxtSpeed;

    public WheelCollider FrontLeft;
    public WheelCollider FrontRight;
    public WheelCollider BackLeft;
    public WheelCollider BackRight;

    public float Torque;
    public float Speed;
    public float MaxSpeed = 200f;
    public int Brake;
    public float CoefAcceleration;
    public float wheelAngleMax = 10f;
    public bool Freinage = false; 

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
                BackRight.motorTorque = Input.GetAxis("Vertical") * Torque * CoefAcceleration * Time.deltaTime ;
            }
            

        }
        
        //Deceleration
        if (!Input.GetKey(KeyCode.UpArrow) || Speed > MaxSpeed && !Freinage)
        { 
            BackLeft.motorTorque = 0;
            BackRight.motorTorque = 0;
            //FrontLeft.motorTorque = 0;
            //FrontRight.motorTorque = 0;
            
            BackLeft.brakeTorque = Brake * CoefAcceleration * Time.deltaTime;
            BackRight.brakeTorque = Brake * CoefAcceleration * Time.deltaTime;
            //FrontLeft.brakeTorque = Brake * CoefAcceleration * Time.deltaTime;
            //FrontRight.brakeTorque = Brake * CoefAcceleration * Time.deltaTime;
        }
        
        //Direction du véhicule
        FrontLeft.steerAngle = Input.GetAxis("Horizontal") * wheelAngleMax;
        FrontRight.steerAngle = Input.GetAxis("Horizontal") * wheelAngleMax;
        
        //Freinage
        if (Input.GetKey(KeyCode.Space))
        {
            Freinage = true;
            BackLeft.brakeTorque = Mathf.Infinity;
            BackRight.brakeTorque = Mathf.Infinity;
            FrontLeft.brakeTorque = Mathf.Infinity;
            FrontRight.brakeTorque = Mathf.Infinity;

            BackLeft.motorTorque = 0;
            BackRight.motorTorque = 0; 
            
            
        }
        else
        {
            Freinage = false;
        }
    }
}
