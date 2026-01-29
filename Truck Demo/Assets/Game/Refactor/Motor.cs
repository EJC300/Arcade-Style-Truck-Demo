using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{
    /*
        Motor are both a torque curve with a braking curve as well. Im skipping gear shifting for demo 
        purposes. The truck reads all wheels and applies the force to it. The motor reads the 
        wheel force(faked RPM) then uses it with the torque power curve. 
    */
    #region DriveWheels
    [SerializeField] List<RaycastTire> Tires;
    #endregion
    #region EngineValues
    [SerializeField] private AnimationCurve EngineTorqueCurve;
    [Range(0.1f, 1f)]
    [SerializeField] private float Acceleration;
    [SerializeField] private float MaxTorque = 3000;
    [SerializeField] private int numberOfGears = 4;
    [SerializeField] private float maxSpeedPerGear = 30f; // mph per gear
    [SerializeField] private float maxReverseSpeed = 15;
    private bool reversing;

    private int currentGear = 1;
    private float TotalAcceleration;
    [SerializeField] private float Power;
    #endregion

    #region RigidBody
    private Rigidbody rb;
    #endregion
    #region BrakeValues

    [SerializeField] private float MaxBrake = 13000;
    #endregion


    public void DriverBrakeToWheels(float input)
    {


        for (int i = 0; i < Tires.Count; i++)
        {
            if (input < 0f)
            {
                Tires[i].ApplyBrakes(input, MaxBrake);
            }
        }
    }

    public void DriverPowerToDriveWheels(float throttle)
    {



        float currentSpeed = Vector3.Dot(transform.forward, rb.velocity) * 2.237f;
        reversing = currentSpeed < 0.1f && throttle < 0f;
        
        if (!reversing)
        {
            currentGear = Mathf.Clamp(Mathf.FloorToInt(currentSpeed / maxSpeedPerGear), 1, numberOfGears);
            float gearMinSpeed = (currentGear - 1) * maxSpeedPerGear;
            float gearMaxSpeed = currentGear * maxSpeedPerGear;
            float normalizedSpeedInGear = Mathf.InverseLerp(gearMinSpeed, gearMaxSpeed, currentSpeed);
            float torqueMultiplier = EngineTorqueCurve.Evaluate(normalizedSpeedInGear);
            float torqueForce = throttle * torqueMultiplier * MaxTorque;

            for (int i = 0; i < Tires.Count; i++)
            {


                Tires[i].DriveWheels(throttle, EngineTorqueCurve, torqueForce);

            }
        }
        else
        {
            currentGear = Mathf.FloorToInt(currentSpeed / maxReverseSpeed);
            float gearMinSpeed = (currentGear - 1) * maxReverseSpeed;
            float gearMaxSpeed = currentGear *  maxReverseSpeed;
            float normalizedSpeedInGear = Mathf.InverseLerp(gearMinSpeed, gearMaxSpeed, currentSpeed);
            float torqueMultiplier = EngineTorqueCurve.Evaluate(normalizedSpeedInGear);
            float torqueForce = throttle * torqueMultiplier * MaxTorque * 0.25f;

            for (int i = 0; i < Tires.Count; i++)
            {


                Tires[i].DriveWheels(throttle, EngineTorqueCurve, torqueForce);

            }
        }



    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        //For Now Just do this.
        DriverBrakeToWheels(Input.GetAxis("Vertical"));
        DriverPowerToDriveWheels(Input.GetAxis("Vertical"));

    }
    void FixedUpdate()
    {
        //Anti Roll Force 
        float tilt = Vector3.Dot(transform.right, Vector3.up);


        if (Mathf.Abs(tilt) > 0.003f)
        {

            Vector3 counterTorque = -transform.forward * tilt * 600000;
            rb.AddTorque(counterTorque);
        }
    }
}
