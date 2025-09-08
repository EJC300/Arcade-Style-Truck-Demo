using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSuspension : MonoBehaviour
{

    public TruckMain truck;
    public bool LeftWheel;
    public bool RightWheel;
    public bool Steering;
    private RaycastHit hit;
    private bool isGrounded;
    public float EngineForce;
    [SerializeField]
    float SteerAngle = 0;
    float SteerResistance;
    public void Init()
    {
       
    }
    float timer = 60;
    float AckerManLeft = 0;
    float AckerManRight = 0;
    public void Awake()
    {
        Init();
    }

    public  float GetWheelRPM()
    {
        if (isGrounded)
        {
            Vector3 WheelVelocity = truck.rb.GetPointVelocity(hit.point);
            float z = (new Vector3(truck.rb.velocity.x, 0, truck.rb.velocity.z).magnitude * 0.6213711922f);
            z = z * 1609;
            z = z / (60 / 2 * Mathf.PI * 4.5f);
            z = z * Mathf.Rad2Deg;
            return z;
        }
        return 0;
    }
    void WheelSpin()
    {
        transform.GetChild(0).Rotate(Vector3.right  * (0 + GetWheelRPM()) * Time.deltaTime);
    }
    public void Steer(float steering)
    {

        if (GetWheelRPM() > 2000)
        {
            SteerResistance = (GetWheelRPM() + 1);
            SteerResistance = Mathf.Clamp(SteerResistance, -4, 4);

        }
        else
        {
            SteerResistance = (GetWheelRPM() + 1);
            SteerResistance = Mathf.Clamp(SteerResistance, -1, 1);
        }
        if (LeftWheel)
        {
            AckerManLeft = Mathf.Rad2Deg * Mathf.Atan((truck.truck.WheelBaseHeight / (truck.truck.turnRadius + truck.truck.RTrackLength / 2)) * steering) / SteerResistance;
            AckerManRight = Mathf.Rad2Deg * Mathf.Atan((truck.truck.WheelBaseHeight / (truck.truck.turnRadius - truck.truck.RTrackLength / 2)) * steering) / SteerResistance; ;
        }
        else if(RightWheel)
        {
            AckerManLeft = Mathf.Rad2Deg * Mathf.Atan((truck.truck.WheelBaseHeight / (truck.truck.turnRadius - truck.truck.RTrackLength / 2)) * steering) / SteerResistance; ;
            AckerManRight = Mathf.Rad2Deg * Mathf.Atan((truck.truck.WheelBaseHeight / (truck.truck.turnRadius + truck.truck.RTrackLength / 2)) * steering) / SteerResistance; ;
        }

        if(RightWheel)
        {
            SteerAngle = AckerManRight;
        }
        else if(LeftWheel)
        {
            SteerAngle = AckerManLeft;
        }
        //SteerAngle = Mathf.Clamp(SteerAngle, -truck.truck.turnRadius, truck.truck.WheelRadius);
        
    }


    public void Engine(float EngineForce)
    {
      
        if (isGrounded)
        {
            /*
            Vector3 WheelVelocity = truck.rb.GetPointVelocity(hit.point);
            
            Vector3 RollingResitance = Vector3.Dot(transform.forward,WheelVelocity) * transform.forward * (-truck.truck.Drag);
       
            Vector3 FreeWheel = Vector3.Dot(transform.forward, WheelVelocity) * transform.forward * truck.truck.Mass ;
           
            Vector3 DownForce = -transform.up * WheelVelocity.normalized.magnitude * truck.truck.DownForce ;

            Vector3 Fwd = transform.TransformDirection(Vector3.forward) * (EngineForce);
       
            Vector3 Force = Fwd + RollingResitance + FreeWheel;
           
      
            truck.rb.AddForceAtPosition(DownForce,hit.point);
        
            //add hit.point when downforce and drag is in there!

            truck.rb.AddForceAtPosition(Force, hit.point);
        */

            Vector3 WheelVelocity = truck.rb.GetPointVelocity(hit.point);

            Vector3 WheelForwardVelocity = Vector3.Dot(WheelVelocity,transform.forward) * transform.forward;

            Vector3 lateralForce = Vector3.Cross(hit.normal, transform.forward).normalized * Vector3.Dot(WheelVelocity, Vector3.Cross(hit.normal, transform.forward).normalized) * -truck.truck.Drag;

            Vector3 fwd = -Vector3.Cross(hit.normal,transform.right).normalized * EngineForce;
            Vector3 rollingResisitance = WheelForwardVelocity * -truck.truck.Drag;
            Vector3 sideVelocity = Vector3.Dot(Vector3.Cross(hit.normal, transform.forward).normalized, WheelVelocity) * Vector3.Cross(hit.normal, transform.forward).normalized * -truck.truck.Drag;
            Vector3 forwardVelocity = -Vector3.Dot(sideVelocity,transform.forward) * transform.forward * truck.truck.Mass;
            Vector3 wheelForce = fwd + forwardVelocity + sideVelocity+ rollingResisitance + lateralForce;
            truck.rb.AddForceAtPosition(wheelForce,hit.point);
        }

    }
    void Suspension()
    {
        Vector3 DownDir =-transform.up;
        if(Physics.Raycast(transform.position,DownDir,out hit,truck.truck.MaxSuspensionHeight -truck.truck.WheelRadius))
        {
            isGrounded = true;
            
            float compressionRatio = ((hit.distance) / truck.truck.MaxSuspensionHeight) + truck.truck.WheelRadius;
            compressionRatio = -compressionRatio + 1;
            Vector3 WheelVelocity = truck.rb.GetPointVelocity(hit.point);

            float lastCompressionRatio = compressionRatio;

   
            Vector3 UpForce =  transform.up * compressionRatio* truck.truck.SupsensionForce;
            Vector3 DampForce = (-WheelVelocity) * truck.truck.Damp;
            
            Vector3 SupsensionForce = UpForce + DampForce;
         
            if (compressionRatio > truck.truck.RestLength)
            {
                compressionRatio = truck.truck.RestLength;
            }
       
            SupsensionForce =Vector3.Dot(SupsensionForce,-DownDir) * hit.normal;
            truck.rb.AddForceAtPosition(SupsensionForce, hit.point);

            transform.GetChild(0).transform.position = transform.position + Vector3.down * (compressionRatio);
            compressionRatio = lastCompressionRatio;

            
        }
        else
        {
            isGrounded = false;  
            transform.GetChild(0).transform.position = transform.position + new Vector3(0,-truck.truck.MaxSuspensionHeight * 0.5f,0);
        }
        if(isGrounded)
        {
            Engine(EngineForce);
        }
        else
        {
            Engine(0);
        }
        Vector3 Steer = transform.localEulerAngles;
        Steer = new Vector3(0, SteerAngle, 0);

        transform.localRotation = Quaternion.Euler( Steer);
    }

    void FixedUpdate()
    {
        Suspension();
        WheelSpin();
    }
}
