using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTire : MonoBehaviour
{



    /*
      This is a simplistic raycast vehicle physics.

      Features:
          Arcade But Stable SuspensionSystem

          Different Friction(Drag) Values For Tires

          This is not very complicated it's a essentially a hovercraft with skid.

          Tire "rolls" based in the direction it is applied via engine force and wheel speed.




    */

    #region  UtilityValues
    [SerializeField] private bool Steer;
    [SerializeField] private LayerMask HitLayer;
    [SerializeField] private Rigidbody RB;
    [SerializeField] private GameObject WheelModel;
    #endregion 
    #region  TireForce Values
    [SerializeField] private float SideDrag;
    [SerializeField] private float ForwardDrag;
    [SerializeField] private float WheelStiffness;
    private Vector3 PreviousVelocity;

    #endregion
    #region  Suspension Values
    [SerializeField] float MinHeight;
    [SerializeField] float SuspensionHeight;
    [SerializeField] float WheelHeight;
    [SerializeField] float Damper;
    [SerializeField] float SuspensionStiffness;
    [SerializeField] float WheelMinHeight;
    #endregion

    #region Force Values
    private Vector3 TireForces;
    private Vector3 SuspensionForces;
    #endregion


    #region Wheel Values 
    private RaycastHit WheelRayHit;
    private Vector3 MotorPower;
    private Vector3 StartWheelModelPosition;

    [SerializeField] private bool Grounded;

    [SerializeField] private float MaxSteerWheelAngle;


    #endregion
    //Get Velocity At Point
    Vector3 VelocityAtWheelPoint()
    {

        return RB.GetPointVelocity(WheelRayHit.point);
    }
    //SuspensionForces

    void SuspensionForce()
    {



        Ray WheelRay = new Ray(transform.position, -transform.up);
        Grounded = Physics.Raycast(WheelRay, out
        WheelRayHit,
        SuspensionHeight + WheelHeight,
        HitLayer);
        if (Grounded)
        {
            float TotalDistance = SuspensionHeight + WheelHeight;
            float Spring = (TotalDistance - WheelRayHit.distance) / TotalDistance;
            float SpringNormalized = Mathf.Clamp(Spring, -1f, 1f);

            Vector3 UpForce = transform.up * SpringNormalized * SuspensionStiffness;
            UpForce = Vector3.Dot(UpForce, transform.up) * Vector3.up;
            Vector3 SuspensionLocalVelocity = transform.InverseTransformDirection(
                Vector3.Dot(transform.up,
                VelocityAtWheelPoint()) * transform.up);

            Vector3 SpringDamp = SuspensionLocalVelocity * -Damper;
            SpringDamp.x = 0;
            SpringDamp.z = 0;

            Vector3 TotalWheelForce = transform.TransformDirection(UpForce + SpringDamp);
            TotalWheelForce.x = 0;
            TotalWheelForce.z = 0;
            SuspensionForces = TotalWheelForce;
            TireForces = SuspensionForces;
        }
        else
        {
            TireForces = Vector3.zero;
        }



    }

    //Wheel Long Forces
    /*
        Since before the tire force is locked on the local x and z axis plane we 
        need to add a rolling force that is modified by the local wheel speed whether from the engine 
        or just a wheel spin.
        We can do it in two ways get the normal force and apply friction or we can seperate 
        the the two forces which are momentum transformed to local space and the engine power 
        we are doing the later which involves calculating manually. Not the most realistic
        But will work for that purpose
    */
    void WheelLongForce()
    {


        if (!Grounded) return;


        Vector3 forwardVelocity = Vector3.Dot(VelocityAtWheelPoint(), transform.forward) * transform.forward;
        Vector3 WheelDrag = -forwardVelocity * ForwardDrag;


        TireForces += MotorPower + WheelDrag;

        PreviousVelocity = VelocityAtWheelPoint();
    }
    public float GetWheelForce()
    {
        Vector3 ForwardRoll = transform.forward * PreviousVelocity.magnitude * RB.mass * 0.05f;
        float WheelForwardRoll = Mathf.Clamp(Vector3.Dot(ForwardRoll, transform.forward), Vector3.Dot(ForwardRoll, transform.forward), 1);
        return WheelForwardRoll;
    }

    public void ApplyBrakes(float input, float MaxBrakeForce)
    {
        if (!Grounded) return;
        MotorPower = Vector3.zero;
        Vector3 forwardVelocity = Vector3.Dot(VelocityAtWheelPoint(), transform.forward) * transform.forward;

        float speedFactor = Mathf.Clamp01(forwardVelocity.magnitude / 20f);
        float brakeForce = input * MaxBrakeForce * (0.5f + speedFactor * 0.5f);
        MotorPower = -forwardVelocity.normalized * -Mathf.Sign(forwardVelocity.magnitude) * brakeForce;

    }
    public void DriveWheels(float input, AnimationCurve torqueCurve, float maxTorque)
    {
        if(input > 0f)
        {
        float wheelSpeedNormalized = GetWheelForce();
        float torqueMultiplier = torqueCurve.Evaluate(wheelSpeedNormalized);
        float torqueForce = input * torqueMultiplier * maxTorque;
        MotorPower = transform.forward * torqueForce;
        }
    }
    public void SteerWheel(float input)
    {
        //Just a simple rotation more arcade than realistic.
        if (Steer)
        {
            float speedFactor = Mathf.Clamp01(1f - (RB.velocity.magnitude / 25f));
            float adjustedAngle = MaxSteerWheelAngle * speedFactor;

            transform.localRotation = Quaternion.Euler(0, Input.GetAxis("Horizontal") * adjustedAngle, 0);
        }
    }
    void WheelLateralForce()
    {


        if (!Grounded) return; // Add this safety check
                               // Get lateral (sideways) velocity
        Vector3 lateralVelocity = Vector3.Dot(VelocityAtWheelPoint(), transform.right) * transform.right;

        // Strong lateral grip - resists sideways sliding
        float lateralGripStrength = WheelStiffness * 2f; // Increase multiplier if needed
        Vector3 LateralGrip = -lateralVelocity * lateralGripStrength;

        // Lateral drag (additional resistance)
        Vector3 LateralDrag = -lateralVelocity.normalized * lateralVelocity.sqrMagnitude * SideDrag;

        // Add both lateral forces
        TireForces += LateralGrip + LateralDrag;

        PreviousVelocity = VelocityAtWheelPoint();

    }
    //Wheel Model Behavior
    void WheelModelMovement()
    {

        if (Grounded)
        {
            WheelModel.transform.Rotate(Vector3.right * PreviousVelocity.sqrMagnitude / 2 * Mathf.PI * WheelHeight);
            WheelModel.transform.localPosition = Vector3.up * ((SuspensionHeight + WheelMinHeight) - WheelRayHit.distance);
        }
        else
        {
            WheelModel.transform.localPosition = StartWheelModelPosition;
        }
    }

    //Apply All Forces
    void Start()
    {

        WheelModelMovement();
    }
    void FixedUpdate()
    {
        SteerWheel(0);
        SuspensionForce();
        WheelLongForce();
        WheelLateralForce();
        Debug.DrawRay(WheelRayHit.point, TireForces, Color.red);
        Debug.DrawRay(WheelRayHit.point, transform.right * 5f, Color.green); // Wheel direction
        Debug.DrawRay(WheelRayHit.point, VelocityAtWheelPoint(), Color.blue); // Velocity

        RB.AddForceAtPosition(TireForces, WheelRayHit.point);
        if (Grounded)
        {
            RB.AddForceAtPosition(TireForces, WheelRayHit.point);
        }
    }
    void Update()
    {
        WheelModelMovement();

    }
}
