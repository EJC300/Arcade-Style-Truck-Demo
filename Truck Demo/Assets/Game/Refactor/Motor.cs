using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    [SerializeField] private float MinGearShiftInput = 0.5f;
    [SerializeField] private int MaxGearCount;
 
    private float TotalAcceleration;
    [SerializeField] private float Power;
    #endregion


    #region BrakeValues

    [SerializeField] private float MaxBrake = 13000;
    [SerializeField] private AnimationCurve BrakeTorqueCurve;
    #endregion

    public void DriverBrakeToWheels(float Input)
    {


        for (int i = 0; i < Tires.Count; i++)
        {
            if (Input < 0)
            {
                Power -= MaxBrake * 0.1f;


            }
            else
            {
                Power = Mathf.MoveTowards(Power, 0, Tires[i].GetWheelForce() * Time.deltaTime * 0.01f);
            }
            if (Tires[i].GetWheelForce() < 0.1f)
            {
                Power = 0;
            }
        }
    }

    public void DriverPowerToDriveWheels(float Input)
    {





        for (int i = 0; i < Tires.Count; i++)
        {
            if (Input > 0)
            {
                TotalAcceleration = Mathf.Lerp(TotalAcceleration, 1, Acceleration * Time.deltaTime);
                Power = (EngineTorqueCurve.Evaluate(Tires[i].GetWheelForce()) * TotalAcceleration * MaxTorque + 1);
            }
            Tires[i].DriveWheels(Power);

        }

    }

    void Update()
    {
        DriverBrakeToWheels(Input.GetAxis("Vertical"));
        DriverPowerToDriveWheels(Input.GetAxis("Vertical"));

    }
}
