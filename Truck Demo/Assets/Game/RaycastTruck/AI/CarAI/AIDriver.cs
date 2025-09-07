using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDriver : MonoBehaviour
{
    //Driver type

    public AIpersonalities driver;
    public CarAI car;
    //Waypoints
   public Transform[] waypoints;
    public int index;
    //Collision Detection
    void init()
    {
       
    }
    public void CollisionAvoidence()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, driver.Carefulness, transform.forward, out hit))
        {
            Debug.DrawLine(transform.position, hit.point);
            car.currentPoint = hit.point;
            car.Avoid = true;

        }

        else if (Physics.SphereCast(transform.position, driver.Carefulness, -transform.forward, out hit))

        {

            Debug.DrawLine(transform.position, hit.point);
            car.currentPoint = hit.point;
            car.Avoid = true;


        }

        else if (Physics.SphereCast(transform.position, driver.Carefulness, transform.right, out hit))
        {

            Debug.DrawLine(transform.position, hit.point);

            car.currentPoint = hit.point;
            car.Avoid = true;

        }

        else if (Physics.SphereCast(transform.position, driver.Carefulness, -transform.right, out hit))
        {
            Debug.DrawLine(transform.position, hit.point);

            car.Avoid = true;


        }

        else
        {
            car.currentPoint = waypoints[index].position;
            car.Avoid = false;
        }
        if (car.Avoid == true)
        {

            car.currentPoint = hit.point;

        }





    }

    //Attack
    public void Ram()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, driver.Aggression, transform.forward, out hit))
        {
            car.Avoid = false;
        }

        else if (Physics.SphereCast(transform.position, driver.Aggression, -transform.forward, out hit))

        {
            car.Avoid = false;
        }

        else if (Physics.SphereCast(transform.position, driver.Aggression, transform.right, out hit))
        {
            car.Avoid = false;
        }

        else if (Physics.SphereCast(transform.position, driver.Aggression, -transform.right, out hit))
        {
            car.Avoid = false;
        }
        if (hit.collider.tag == "Opponent" | hit.collider.tag == "Chaser")
        {
            car.currentPoint = hit.collider.transform.position;
        }
        else
        {
            car.currentPoint = waypoints[index].position;
        }
    }

   //FollowWaypoint
   public void MoveToWaypoint()
    {
        
        Vector3 RelativeWaypoint = transform.InverseTransformPoint(waypoints[index].position.x, transform.position.y, waypoints[index].position.z);
        float distance = Mathf.Abs( RelativeWaypoint.magnitude);

        if (distance < driver.Caution)
        {
            index++;
           if(index > waypoints.Length - 1)
            {
                index = 0;
            }
         
        }
        car.waypoint = waypoints[index];
    }

        //Reverse

    void test()
    {
        MoveToWaypoint();
        CollisionAvoidence();
    }
    void Update()
    {
        test();
    }
}

