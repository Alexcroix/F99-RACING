using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;

[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}
     
public class SimpleCarController : MonoBehaviour {
    public List<AxleInfo> axleInfos; 
    public float maxMotorTorque;
    public float maxSteeringAngle;
    
    public Camera C1;
    public Camera C2;
    public GameObject Camera;
    
    public Transform FL;
    public Transform FR;
    public Transform BL;
    public Transform BR;
    
    public WheelCollider FrontLeft;
    public WheelCollider FrontRight;
    public WheelCollider BackLeft;
    public WheelCollider BackRight;
    
    public PhotonView View;

    public Light backlight;
    
    public void Start()
    {
        if (View.IsMine)
        {
            Camera.SetActive(true);
        }
        
        //backlight.Reset();
        
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) {
            return;
        }
     
        Transform visualWheel = collider.transform.GetChild(0);
     
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
     
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
     
    public void FixedUpdate()
    {
        
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
     
        foreach (AxleInfo axleInfo in axleInfos) {
            axleInfo.rightWheel.brakeTorque = 0;
            axleInfo.leftWheel.brakeTorque = 0;
            
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            
            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("coucou");
                axleInfo.motor = false;
                axleInfo.rightWheel.brakeTorque = motor + 600;
                axleInfo.leftWheel.brakeTorque = motor + 600;
            }
            else if (axleInfo.motor) {
                
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

        
        
        //Changement POV
        if (Input.GetKey(KeyCode.A))
        {
            C1.enabled = false;
            C2.enabled = true;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            C2.enabled = false;
            C1.enabled = true;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            backlight.intensity = 10000;
        }
        
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
    }
}