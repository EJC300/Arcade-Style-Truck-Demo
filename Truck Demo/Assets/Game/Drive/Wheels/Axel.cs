using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axel : MonoBehaviour
{
    [SerializeField] private float SuspesnionHeight;
    [SerializeField] private float WheelRadius;
    [SerializeField] private float SuspensionStiffness;
    [SerializeField] private float SuspensionDamp;
    [SerializeField] private WheelSuspension LeftWheel;
    [SerializeField] private WheelSuspension RightWheel;
    [SerializeField] private bool PoweredWheel;
    [SerializeField] private bool SteerWheel;

    public float suspensionHeight { get { return SuspesnionHeight; } }
    public float wheelRadius { get { return WheelRadius; } }

    public float suspensionStiffness { get { return SuspensionStiffness; }}

    public float suspensionDamp { get { return SuspensionDamp; } }

    public WheelSuspension leftWheel { get { return LeftWheel; } }

    public WheelSuspension rightWheel { get { return RightWheel; } }

    public bool poweredWheel {  get { return PoweredWheel; } }

    public bool steerWheel { get { return  SteerWheel; } }

    private void Start()
    {
        leftWheel.SuspensionDamp = suspensionDamp;
        rightWheel.SuspensionDamp = suspensionDamp;
        rightWheel.SuspensionStiffness = SuspensionStiffness;
        leftWheel.SuspensionStiffness = suspensionStiffness;
        leftWheel.MaxSuspensionHeight = suspensionHeight;
        rightWheel.MaxSuspensionHeight = suspensionHeight;
        leftWheel.WheelRadius = wheelRadius;
        rightWheel.WheelRadius = wheelRadius;
        leftWheel.Steering = steerWheel;
        rightWheel.Steering = steerWheel;

        leftWheel.WheelOpposite = rightWheel;
        rightWheel.WheelOpposite = leftWheel;
    }
}
