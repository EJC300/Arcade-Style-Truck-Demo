using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    private PlayerControls playerControls
    {
        get { return PlayerControls.instance; }
    }
    [SerializeField]
    public Light redlight;
    [SerializeField]
    public Light redlight2;
    public TruckMain truck;
    [SerializeField]
    private SimpleEngine motor;
    void Init()
    {
        truck = GetComponent<TruckMain>();


        motor.gearRatios = truck.truck.gearRatios;
        motor.MaxPower = (int)(((truck.truck.Power * 5252) / truck.truck.Power) * motor.gearRatios.Count);

    }
    void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        if (playerControls != null)
        {
            playerControls.Acceleration += Gas;
            playerControls.Braking += Brake;
            playerControls.Turning += Steer;
        }
    }


    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Acceleration -= Gas;
            playerControls.Braking -= Brake;
            playerControls.Turning -= Steer;
        }
    }
    private void OnDestroy()
    {
        if (playerControls != null)
        {
            playerControls.Acceleration -= Gas;
            playerControls.Braking -= Brake;
            playerControls.Turning -= Steer;
        }
    }
    private void Gas(float gasPedal)
    {
        foreach (WheelSuspension wheel in truck.truck.wheels)
        {
            wheel.EngineForce = (motor.CalculateEngineForce(gasPedal, truck.truck));
            motor.DriveWheelRPM = wheel.GetWheelRPM();
            redlight.intensity = 2.5f;
            redlight2.intensity = 2.5f;
        }
    }
    void Brake(float brakePedal)
    {
        foreach (WheelSuspension wheel in truck.truck.wheels)
        {
            redlight.intensity = 6.5f;
            redlight2.intensity = 6.5f;

            wheel.EngineForce = (motor.CalculateEngineForce(brakePedal, truck.truck));
            motor.DriveWheelRPM = wheel.GetWheelRPM();
        }
    }

    void Steer(float steer)
    {
        foreach (WheelSuspension wheel in truck.truck.wheels)
        {
            if (wheel.Steering)
            {
                wheel.Steer(steer);
            }
            motor.DriveWheelRPM = wheel.GetWheelRPM();
        }
    }
    void DriveVehicle()
    {
        motor.engineSound = truck.engineSound;
        /*
     
        if (Input.GetAxis("Vertical") < -0.5)
        {
            redlight.intensity = 6.5f;
            redlight2.intensity = 6.5f;
        }
        else
        {
            redlight.intensity = 2.5f;
            redlight2.intensity = 2.5f;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            motor.Reverse =true;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            motor.Reverse = false;
        }
   
        foreach (WheelSuspension wheel in truck.truck.wheels)
        {

            if (!motor.Reverse)
            {
                wheel.EngineForce = (motor.CalculateEngineForce(Input.GetAxis("Vertical"), truck.truck));
            }
            if (motor.Reverse)
            {
                wheel.EngineForce = (motor.ReverseEngineForce(Input.GetAxis("Vertical"), truck.truck));
            }



            if (wheel.Steering)
            {
                wheel.Steer(Input.GetAxis("Horizontal"));
            }

        
            motor.DriveWheelRPM = wheel.GetWheelRPM();
             */

    }
    void FixedUpdate()
    {
        
        DriveVehicle();

    }
}
