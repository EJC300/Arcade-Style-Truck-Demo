using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class BasicTrafficCar : MonoBehaviour
{
    public int SpeedLimit;

     public TruckMain truck;

    public SimpleEngine motor;

    public Transform follower;

    float Gas;
    public float getCurrentSpeed()
    {
        return truck.rb.velocity.sqrMagnitude;
    }
    void Awake()
    {
        motor = new SimpleEngine();
        motor.MaxPower = truck.truck.Power * 5252;
        motor.gearRatios = truck.truck.gearRatios;
    }
    void Steering(float angle)
    {
        foreach (WheelSuspension wheel in truck.truck.wheels)
        {
            if (wheel.Steering)
            {

                wheel.Steer(angle);
            }
        }
    }
    void Motor()
    {
        foreach (WheelSuspension wheel in truck.truck.wheels)
        {

            wheel.EngineForce = motor.EngineForce(Gas, truck.truck);
        }
    }
   void Drive()
    {
        Vector3 towards = follower.transform.position;
        towards = transform.InverseTransformPoint( new Vector3( follower.position.x,transform.position.y,follower.position.z));
        float steer = towards.x / transform.InverseTransformPoint(follower.position.x, transform.position.y, follower.position.z).magnitude;
        Debug.Log(steer * 180);
        float speedDiff = towards.z/ towards.magnitude;
        Steering(180 * steer);
        if (getCurrentSpeed() < SpeedLimit)
        {
            Gas = speedDiff;
        }
        if(getCurrentSpeed() > SpeedLimit)
        {
            Gas = -0.75f;
        }

    }

    void FixedUpdate()
    {

        Drive();
        Motor();
    }
}
