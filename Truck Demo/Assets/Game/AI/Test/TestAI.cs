using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI : MonoBehaviour
{

    //This is just a test for finding out how I can manipulate the AI to drive a vehicle

    public AIpersonalities ai;

    public float caution;

    public Light redlight;
   
    public Light redlight2;
    public TruckMain truck;

    public SimpleEngine motor;
    
    public GameObject currentPoint;

    public Vector3 waypoint;
 
    public int index;
    public GameObject[] Waypoints;

    public  RaycastHit hit;

    public  float steer;
    public float limit;
    public  float adjustGas;
    [SerializeField]
    float threshold;
    float distance;
    [SerializeField]
    float pedal;
    TestBrain statemachine;


  public  bool stopper;


    float Speed()
    {
        return -truck.rb.transform.TransformDirection(truck.rb.velocity).z;
    }

    


    void Init()
    {
        statemachine = new TestBrain();
        Waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        truck = GetComponent<TruckMain>();

        motor = new SimpleEngine();

        motor.MaxPower = truck.truck.Power * 5252;
        motor.gearRatios = truck.truck.gearRatios;
    }
    void StateMachine()
    {
        if(statemachine.GetCurrentState() == "FollowWaypoint")
        {
            TruckBehavoirs.FollowWaypoint(this, ai.Carefulness);
            limit = 1;
        }
        else if (statemachine.GetCurrentState() == "Stop")
        {
            limit = 1;
            TruckBehavoirs.Stop(this);
        }
        else if (statemachine.GetCurrentState() == "SlowDown")
        {
          
            TruckBehavoirs.SlowDown(this, ai.Caution);
        }
        else if (statemachine.GetCurrentState() == "AvoidFront")
        {
            limit = 1;
            TruckBehavoirs.CollisionAvoidence(this, ai.Caution);
        }
        else if (statemachine.GetCurrentState() == "AvoidNextToMe")
        {

            TruckBehavoirs.AvoidAdjacant(this, ai.Caution);
        }
        else if (statemachine.GetCurrentState() == "AdjustThrottle")
        {
            TruckBehavoirs.AdjustThrottle(this, ai.Carefulness);
        }
        else if (statemachine.GetCurrentState() == "Ram")
        {
              limit = 1;
            TruckBehavoirs.SteerIntoIt(this, ai.Caution);
        }
       
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Ground")
        {
            distance = transform.InverseTransformDirection(other.transform.position - transform.position).magnitude;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag != "Ground")
        {
            distance =  transform.InverseTransformDirection(other.transform.position - transform.position).magnitude;
        }
    }
    void Decision()
    {
        threshold = ((distance / Speed()) + (Speed() - ai.Carefulness) + ai.Aggression);
        threshold = threshold / 1000;
        threshold = Mathf.FloorToInt( threshold * 100);
        threshold = Mathf.Clamp(threshold, 1, 7);
        Debug.Log(threshold);
        statemachine.SetState((int)threshold);
      
    }

    void Awake()
    {
        //use a threshold based on speed of the vehicle, the distance between anything within its influence (sphere trigger) along with carefulness, and aggression.
        // This has a range from 1 to 8;
        //threshold = (((distance / speed) + (speed - carefullness) + aggression)/1000) * 100 - round off and clamp between 1 and 8
        
        Init();
    }

   public Vector3 MostThreatening()
    {
        if (Physics.SphereCast(transform.position,5,transform.forward,out hit))
        {
            Vector3 towards = (hit.point - transform.position);
            Vector3 Ahead = transform.position.normalized+ Vector3.Normalize(truck.rb.velocity) * 30;
            if (towards.magnitude < Ahead.magnitude)
            {
                stopper = true;
                return hit.point;
            }
        }
        
        stopper = false;
        currentPoint = Waypoints[index];
        return currentPoint.transform.position;
    }

    void Drive()
    {

            if (!stopper)
            {
                steer = transform.InverseTransformPoint(waypoint.x, transform.position.y, waypoint.z).x / transform.InverseTransformPoint(waypoint.x, transform.position.y, waypoint.z).magnitude;
                pedal = transform.InverseTransformPoint(waypoint.x, transform.position.y, waypoint.z).z;
            }
            if (Mathf.Abs(steer) < 0.5f)
            {
                adjustGas = pedal - Mathf.Abs(steer);
            }
            else
            {
                adjustGas = pedal;
            
            }
            if (waypoint.magnitude > 5 && !stopper)
            {
           
                 adjustGas = pedal;
            }
            else
            {
            adjustGas = pedal;
            }
        
            
            if (adjustGas < 0)
            {
                redlight.intensity = 6.5f;
                redlight2.intensity = 6.5f;
            }
        
        else
        {
            redlight.intensity = 2.5f;
            redlight2.intensity = 2.5f;
        }
            adjustGas = Mathf.Clamp(adjustGas,-1,limit);
        
        foreach (WheelSuspension wheel in truck.truck.wheels)
        {
            wheel.EngineForce = (motor.CalculateEngineForce(adjustGas, truck.truck));

            if (wheel.Steering)
            {
           
                     wheel.Steer(steer);
            }

            motor.DriveWheelRPM = wheel.GetWheelRPM();
            
        }
      
    }
    void FixedUpdate()
    {
        Debug.DrawLine(transform.position, MostThreatening());
        Decision();
        StateMachine();
        
        Drive();
 
    }


}
