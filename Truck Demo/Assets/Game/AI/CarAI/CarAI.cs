using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    public TruckMain truck;
    public bool Avoid;
    public GameObject brakelight2;
    public GameObject brakelight;
    public SimpleEngine motor;
    //light
    public GameObject light;
    public GameObject light2;
    public Transform waypoint;
    public Vector3 currentPoint;
    float gas;
    float steer;

    void Init()
    {
        motor = new SimpleEngine();
        motor.MaxPower = truck.truck.Power * 5252;
        motor.gearRatios = truck.truck.gearRatios;
    }
    void MoveToWaypoint()
    {
        if (!Avoid)
        {
            
            Vector3 RelativeWaypoint = transform.InverseTransformPoint(currentPoint.x, transform.position.y, currentPoint.z);
            steer = RelativeWaypoint.x / RelativeWaypoint.magnitude;
            gas = RelativeWaypoint.z;

            if (gas < 0.5f)
            {
                gas = RelativeWaypoint.z / RelativeWaypoint.magnitude;
                gas = gas - Mathf.Abs(steer);
            }
        }
    }

    //Move Towards waypoint
    void MoveAwayFromWaypoint()
    {
        if (Avoid)
        {
            Vector3 RelativeWaypoint = transform.InverseTransformPoint(currentPoint.x, transform.position.y, currentPoint.z);
            Vector3 RelativePosition = transform.position;
            if (Vector3.Dot(transform.forward, RelativeWaypoint) < 0)
            {
                steer = RelativeWaypoint.x + RelativePosition.x;
            }
           else     if (Vector3.Dot(transform.forward, RelativeWaypoint)   > 0)
            {

                
                    steer = RelativeWaypoint.x - RelativePosition.x;
            }
            if (Vector3.Dot(transform.right, RelativeWaypoint) < 0)
            {
                steer = RelativeWaypoint.x  - RelativePosition.x;
            }
        else  if (Vector3.Dot(transform.right, RelativeWaypoint) > 0)
            {
               
                steer = RelativeWaypoint.x + RelativePosition.x;
            }
            gas = RelativeWaypoint.z;
          
                if (gas < 0.5f)
                {
                    gas = RelativeWaypoint.z / RelativeWaypoint.magnitude;
                    gas = gas - Mathf.Abs(steer);
                }

            Debug.DrawLine(transform.position, currentPoint, Color.red);
        }


    }
    //Drive Method
    void Drive()
    {

        gas = Mathf.Clamp(gas, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1);
        if(gas < 0)
        {
            brakelight.GetComponent<Light>().intensity = 10.5f;
            brakelight2.GetComponent<Light>().intensity = 10.5f;
        }
        else
        {
            brakelight.GetComponent<Light>().intensity = 0.5f;
            brakelight2.GetComponent<Light>().intensity = 0.5f;
        }
        foreach (WheelSuspension wheel in truck.truck.wheels)
        {
            wheel.EngineForce = (motor.CalculateEngineForce(gas, truck.truck));

            if (wheel.Steering)
            {

                wheel.Steer(steer);
            }

        }
    }
    void Awake()
    {
        Init();
    }
    void FixedUpdate()
    {
    
        MoveToWaypoint();
        MoveAwayFromWaypoint();
        Drive();
    }
}
