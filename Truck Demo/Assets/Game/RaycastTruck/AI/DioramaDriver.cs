using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DioramaDriver : MonoBehaviour
{
    Rigidbody body;
    public SimpleEngine motor;
    public int TopSpeed;
    public TruckMain truck;
    float engineForce;
    float Distance;
    public Vector3 startPosition;
    void Awake()
    {
        truck = GetComponent<TruckMain>();
        body = GetComponent<Rigidbody>();
        motor = new SimpleEngine();

        motor.MaxPower = truck.truck.Power * 5252;
        motor.gearRatios = truck.truck.gearRatios;
        startPosition = transform.position;
    }
    public float getCurrentSpeed()
    {
        return body.velocity.sqrMagnitude;
    }
    void Drive()
    {
        //Debug.Log(getCurrentSpeed());
        
        if (getCurrentSpeed() < TopSpeed)
        {
           engineForce = motor.EngineForce(1.0f, truck.truck);
        }
        else
        {
           engineForce = motor.EngineForce(-0.5f, truck.truck);
        }

        foreach (WheelSuspension wheel in truck.truck.wheels)
        {
            wheel.EngineForce = engineForce;       
        
        }
        Distance = transform.position.x;
        if(Distance > 19716)
        {
            transform.position = startPosition;
        }
    }

    void FixedUpdate()
    {
        Drive();
    }
}
