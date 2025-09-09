using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TruckType 
{
    public string TruckName;

    public int Mass;

    public int durability;

    public int Power;

    public int TopSpeed;

    public float BrakeForce;

    public float MaxSuspensionHeight;

    public float RestLength;

    public float WheelRadius;

    public int SupsensionForce;

    public float DownForce;

    public float accelRate;

    public float turnRadius;

    public float Transmission = 2.5f;

    public float WheelBaseHeight;

    public int RTrackLength;

    public int MaxWheelRPM;

    public int Damp;

    public float Drag;
    public List<float> gearRatios = new List<float>();

    //Wheel List
    public WheelSuspension[] wheels;
}
