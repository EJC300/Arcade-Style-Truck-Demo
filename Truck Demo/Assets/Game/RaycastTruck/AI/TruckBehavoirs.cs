using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckBehavoirs 
{
    //about the ai:
    //The car follows a moving waypoint.
public static void FollowWaypoint(TestAI driver,float dist)
    {


        //The waypoint is the when following the path
        Vector3 direction = (driver.transform.position - driver.waypoint);
        // Use a path finding algorithm to find the quickest route on nodes of the road 
        if(direction.magnitude < dist )
        {
            driver.index++;
        }
        //As the car follows the path it follows a few rules
        if(driver.index > driver.Waypoints.Length - 1)
        {
            driver.index = 0;
        }

        driver.waypoint = driver.Waypoints[driver.index].transform.position;
    }

    public static void CollisionAvoidence(TestAI driver, float caution)
    {
        //they are
        RaycastHit hit;
        
        float steer = (driver.transform.InverseTransformPoint(driver.transform.position).x - driver.transform.InverseTransformPoint(driver.MostThreatening()).x);
        float pedal = (driver.transform.InverseTransformPoint(driver.MostThreatening()).z / driver.transform.InverseTransformPoint(driver.MostThreatening()).z);
        pedal = Mathf.Clamp(pedal, -1, 1);

        //Collision avoidence is based on a raycast within a sphere collider
        driver.adjustGas = pedal;
        driver.steer = steer;
       //The raycast gets the speed and distance of the obstacle
       //This makes the car slow down or steer out of the way
        
    }
    //FollowPath
    public static void FollowPath()
    {

    }
    //Adjusts throttle and steer based on distance to the waypoint and a value like aggression or caution
    public static void AdjustThrottle(TestAI driver,float caution)
    {
        RaycastHit hit;
        bool slowDown = false;
        if (Physics.SphereCast(driver.transform.position,caution,driver.transform.right,out hit))
        {
            slowDown = true;
        }
        if (Physics.SphereCast(driver.transform.position, caution, -driver.transform.right, out hit))
        {
            slowDown = true;
        }
        if (Physics.SphereCast(driver.transform.position, caution, -driver.transform.forward, out hit))
        {
            slowDown = true;
        }
        if (Physics.SphereCast(driver.transform.position, caution, driver.transform.forward, out hit))
        {
            slowDown = true;
        }
        if(slowDown)
        {
            Vector3 direction = driver.transform.InverseTransformDirection(hit.point) - driver.transform.InverseTransformPoint(driver.transform.position);
            float limit = direction.normalized.magnitude;

            driver.limit = limit;

        }
        else
        {
            driver.limit = 1;
        }
    }
    //Ram
    //If a target vehicle is adjacent next to you steer into it
    public static void SteerIntoIt(TestAI driver, float atckrnge)
    {

        RaycastHit hit;
        Transform tgt;
        bool ram = false;

        if (Physics.SphereCast(driver.transform.position, atckrnge, -driver.transform.right, out hit))
        {
            ram = true;
        }
        if (Physics.SphereCast(driver.transform.position, atckrnge, driver.transform.right, out hit))
        {
            ram = true;
        }

        if (Physics.SphereCast(driver.transform.position, 0.5f, -driver.transform.right, out hit))
        {
            ram = false;

        }
        if (Physics.SphereCast(driver.transform.position, 0.5f, driver.transform.right, out hit))
        {
            ram = false;
        }

        if (ram)
        {

            tgt = hit.collider.transform;
            if (tgt != null)
            {
                float steerForce = (tgt.position - driver.transform.position).magnitude;
                driver.waypoint = tgt.transform.position;
            }
        }
        else
        {
            driver.waypoint = driver.currentPoint.transform.position;

            //If rival is next adjacent and in range:

            //of course set the waypoint to the tgt
            //steer in to him
            //When contact is made or the he is no longer next to me return to normal 
            //Not all drivers do this but most do
        }
    }
    //Stop
    //Slow down or slam on the brakes
    public static void Stop(TestAI driver)
    {
        float pedal = -1;
        driver.adjustGas = pedal;
    }

  public static  void SlowDown(TestAI driver, float amount)
    {
        amount = Mathf.Clamp(amount, 0.01f, 0.5f);
        driver.limit = amount;
    }
    //Avoid
    public static void AvoidAdjacant(TestAI driver,float caution)
    {
        RaycastHit hit;
        bool next = false;

        if (Physics.SphereCast(driver.transform.position,caution, -driver.transform.right, out hit))
        {
            next = true;
        }
        if (Physics.SphereCast(driver.transform.position, caution, driver.transform.right, out hit))
        {
            next = true;
        }
        if (next)
        {
            float steer = (driver.transform.InverseTransformPoint( driver.transform.position).x - driver.transform.InverseTransformPoint(hit.point).x);
            float pedal = (driver.transform.InverseTransformPoint(driver.MostThreatening()).z / driver.transform.InverseTransformPoint(hit.point).z);

            driver.adjustGas = pedal;
            driver.steer = steer;
        }
       
    }
    //Adjust speed and steer based on the distance of an object
    //Reverse
    //Back up
    public static void Reverse(TestAI driver,Transform tgt)
    {
        SimpleEngine motor = driver.motor;
        motor.Reverse = Mathf.FloorToInt(-(driver.transform.InverseTransformPoint(driver.MostThreatening()).z / driver.transform.InverseTransformPoint(driver.currentPoint.transform.position).z));
        float steer = (driver.transform.InverseTransformPoint(driver.transform.position).x + driver.transform.InverseTransformPoint(driver.currentPoint.transform.position).x);
        driver.steer = steer;
    }
 
  

}
