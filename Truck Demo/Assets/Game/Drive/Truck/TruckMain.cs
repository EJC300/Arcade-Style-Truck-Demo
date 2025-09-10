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
    public Rigidbody rb {  get;  set; }
  
    float currentTime;
    float oldTime;
    Vector3 CurrentVelocity;
    Vector3 LastVelocity;
    private bool Grounded;
    Vector3 acceleration;
    float lastAccel;
    public void Init()
    {  
     
        rb = GetComponent<Rigidbody>();
        rb.mass = truck.Mass;
        rb.centerOfMass = Vector3.down;
        
        foreach (WheelSuspension wheel in truck.wheels)
        {
            wheel.truck = this;
            Grounded = wheel.isGrounded;
        }
    }


    void CalculateAcceleration()
    {
        currentTime = Time.time;
        CurrentVelocity = rb.velocity;

        acceleration = (rb.velocity - LastVelocity) / Time.fixedDeltaTime;
        LastVelocity = rb.velocity;
        oldTime = currentTime;
    }
    public float GetAccel()
    {

        float accel = acceleration.magnitude;


        return accel;

        
    }
    void AntiRoll()
    {
        //If the vehicle begins to rotate on the z or x axis reset the rotation its not realistic but thats the point
        Vector3 Roll = transform.localEulerAngles;
        foreach (WheelSuspension wheel in truck.wheels)
        {

            if (rb.velocity.magnitude > 1f &&  wheel.isGrounded)
            {
                if (Mathf.Abs(Roll.x) >= 0)
                {
                    transform.localEulerAngles = new Vector3(0, Roll.y, Roll.z);
                }
                if (Mathf.Abs(Roll.z) >= 0)
                {
                    transform.localEulerAngles = new Vector3(Roll.x, Roll.y, 0);
                }
            }
        }
      

    }
    void ShiftWeight()
    {
        CurrentVelocity = transform.TransformDirection(rb.velocity);
        
        if (GetAccel() > lastAccel && CurrentVelocity.z < 0)
        {
            cg.localPosition = Vector3.MoveTowards(cg.localPosition, RearWeight.localPosition, GetAccel() * Time.deltaTime);
        }
        else if (GetAccel() > lastAccel && CurrentVelocity.z >0)
        {

            cg.localPosition = Vector3.MoveTowards(cg.localPosition, FrontWeight.localPosition, GetAccel() * Time.deltaTime);
        }

        else
        {
            cg.localPosition = Vector3.down;

        }
        lastAccel = GetAccel();
        rb.centerOfMass = cg.localPosition;
    }
    void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
       CalculateAcceleration();
       ShiftWeight();
       AntiRoll();
    
        Debug.Log(GetAccel());

    }




}
