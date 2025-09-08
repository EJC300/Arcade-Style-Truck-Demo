using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSuspension : MonoBehaviour
{
    public WheelSuspension WheelOpposite { get; set; }
   
    public TruckMain truck;
    [SerializeField] bool LeftWheel;
    [SerializeField] bool RightWheel;
    public bool Steering { get; set; }
    private RaycastHit hit;
    public bool isGrounded {  get; set; }
    public float EngineForce { get; set; }

    public float MaxSuspensionHeight { get; set; }
    
    public float SuspensionStiffness {  get; set; }

    public float SuspensionDamp {  get; set; }

    public float WheelRadius {  get; set; }


    float SteerAngle = 0;
    float AckerManLeft = 0;
    float AckerManRight = 0;



    public void Awake()
    {
      
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

   
        if (LeftWheel)
        {
            AckerManLeft = Mathf.Rad2Deg * Mathf.Atan((truck.truck.WheelBaseHeight / (truck.truck.turnRadius + truck.truck.RTrackLength / 2)) * steering);
            AckerManRight = Mathf.Rad2Deg * Mathf.Atan((truck.truck.WheelBaseHeight / (truck.truck.turnRadius - truck.truck.RTrackLength / 2)) * steering) ;
        }
        else if(RightWheel)
        {
            AckerManLeft = Mathf.Rad2Deg * Mathf.Atan((truck.truck.WheelBaseHeight / (truck.truck.turnRadius - truck.truck.RTrackLength / 2)) * steering);
            AckerManRight = Mathf.Rad2Deg * Mathf.Atan((truck.truck.WheelBaseHeight / (truck.truck.turnRadius + truck.truck.RTrackLength / 2)) * steering) ;
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
            Vector3 wheelRight = (hit.point - WheelOpposite .hit.point ).normalized;

            Vector3 wheelForward =  Vector3.Cross(hit.normal,wheelRight);
            Vector3 WheelVelocity = truck.rb.GetPointVelocity(hit.point);
            
            Vector3 lateralVelocity = Vector3.Dot(WheelVelocity, wheelRight) * wheelRight;

            Vector3 flatVelocity = Vector3.Dot(WheelVelocity, wheelForward) * wheelForward;


            Vector3 DownForce = -transform.up * WheelVelocity.normalized.magnitude * truck.truck.DownForce ;

            Vector3 slidingForce = (0.5f * (flatVelocity + lateralVelocity));

            Vector3 Fwd =(-slidingForce * truck.truck.Mass * 0.15f /Time.fixedDeltaTime);
            
        
            
            Vector3 Force = Vector3.Dot(Fwd,wheelForward) * wheelForward;

            float cornerForce  = -Mathf.Atan ( Mathf.Deg2Rad*((WheelVelocity.magnitude *WheelRadius * Mathf.Deg2Rad)) -Force.magnitude/(Force.magnitude));
            Debug.Log(cornerForce);


           // truck.rb.AddForceAtPosition(DownForce,hit.point);
            truck.rb.AddForceAtPosition(EngineForce  * transform.forward,hit.point);
            //add hit.point when downforce and drag is in there!
           
            truck.rb.AddForceAtPosition(Fwd * cornerForce, hit.point);
            Fwd -= Force;
          
            /*
                Vector3 wheelRight = (oppositeWheel.hit.point - hit.point).normalized;
                Vector3 wheelForward = Vector3.Cross(wheelRight, hit.normal);
                Vector3 WheelVelocity = truck.rb.GetPointVelocity(hit.point);

                Vector3 lateralVelocity = Vector3.Dot(wheelRight, WheelVelocity) * wheelRight;

                Vector3 forwardVelocity = Vector3.Dot(wheelForward,WheelVelocity) * wheelForward;

                Vector3 slidingVelocity = (forwardVelocity + lateralVelocity) * 0.5f;

                Vector3 slidingForce = Vector3.Dot(slidingVelocity, wheelForward) * wheelForward * truck.truck.Mass;

                Vector3 frictionForce =-slidingVelocity * 1.0f * truck.truck.Mass;

                Debug.Log(slidingForce);


                Vector3 longForce = Vector3.Dot(frictionForce, wheelForward) * wheelForward;
                Vector3 engineForce = wheelForward * EngineForce;
                frictionForce -= longForce;
                truck.rb.AddForceAtPosition(frictionForce, hit.point);








                if (EngineForce == 0)
                {
                    longForce *= 1.0f;
                }

                truck.rb.AddForceAtPosition(engineForce, hit.point);
            */
        }

    }
    void Suspension()
    {
        Vector3 DownDir =-transform.up;
        if (Physics.Raycast(transform.position, DownDir, out hit, MaxSuspensionHeight - WheelRadius))
        {
            if (!hit.collider.gameObject != truck.gameObject)
            {
                isGrounded = true;

                float compressionRatio = ((hit.distance) / MaxSuspensionHeight) + WheelRadius;
                compressionRatio = -compressionRatio + 1;
                Vector3 WheelVelocity = truck.rb.GetPointVelocity(hit.point);

                float lastCompressionRatio = compressionRatio;


                Vector3 UpForce = transform.up * compressionRatio * SuspensionStiffness;
                Vector3 DampForce = (-WheelVelocity) * truck.truck.Damp;

                Vector3 SupsensionForce = UpForce + DampForce;

              

                SupsensionForce = Vector3.Dot(SupsensionForce, transform.up) * transform.up;
                truck.rb.AddForceAtPosition(SupsensionForce, hit.point);

                transform.GetChild(0).transform.position = transform.position + Vector3.down * (compressionRatio);
                compressionRatio = lastCompressionRatio;


            }
        }
        else
        {
            isGrounded = false;
            transform.GetChild(0).transform.position = transform.position + new Vector3(0, -MaxSuspensionHeight * 0.5f, 0);
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
