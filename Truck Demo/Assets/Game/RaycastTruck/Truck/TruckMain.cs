using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckMain : MonoBehaviour
{
    public AudioSource engineSound;
    //Implement center of mass simulation
    public TruckType truck;
    public Transform cg;
    public Transform FrontWeight;
    public Transform RearWeight;
    public Rigidbody rb;
    float currentTime;
    float oldTime;
    Vector3 CurrentVelocity;
    Vector3 LastVelocity;
    public void Init()
    {  
     
        rb = GetComponent<Rigidbody>();
        rb.mass = truck.Mass;
        rb.centerOfMass = Vector3.down *2;
        
        foreach (WheelSuspension wheel in truck.wheels)
        {
            wheel.truck = this;
              
        }
    }

    public float GetAccel()
    {
        
        currentTime = Time.time;
        CurrentVelocity = rb.velocity;
        float  acceleration = (CurrentVelocity.magnitude - LastVelocity.magnitude)/ 1 + (currentTime - oldTime);
        oldTime = currentTime;
        LastVelocity = CurrentVelocity;
       
        return acceleration;

        
    }
    void AntiRoll()
    {
        //If the vehicle begins to rotate on the z or x axis reset the rotation its not realistic but thats the point
        Vector3 Roll = transform.localEulerAngles;

          if(Mathf.Abs( Roll.x) >= 0)
        {
            //transform.localEulerAngles = new Vector3(0, Roll.y, Roll.z);
        }
        if (Mathf.Abs(Roll.z) >= 0)
        {
          // transform.localEulerAngles = new Vector3(Roll.x, Roll.y, 0);
        }

      

    }
    void ShiftWeight()
    {
        CurrentVelocity = transform.TransformDirection(rb.velocity);
       
        if(GetAccel() > 0)
        {
           cg.localPosition  = Vector3.MoveTowards(cg.localPosition, RearWeight.localPosition, GetAccel() * Time.deltaTime);
        }
        else if(GetAccel() < 1)
        {

           cg.localPosition = Vector3.MoveTowards(cg.localPosition, FrontWeight.localPosition,  GetAccel() * Time.deltaTime);
        }
        else
        {
            cg.localPosition = Vector3.down;
        }

        rb.centerOfMass = cg.localPosition;
    }
    void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {

       ShiftWeight();
       AntiRoll();
    


    }




}
