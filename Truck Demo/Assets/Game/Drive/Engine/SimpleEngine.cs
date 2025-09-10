using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SimpleEngine
{

    /// <summary>
    /// Over Steer - done
    /// Proper AutoGear - done
    /// Engine Sound - done
    /// Reverse - done
    /// Basic Driving AI
    /// Realistic Measurments - done
    /// Horn 
    /// Trailer
    /// Demo Level
    /// Tire Effects
    /// Special Effects
    /// Basic Damage Model
    /// Improved Tire 
    /// </summary>


    private float bright = 3.4f;
    private float brakeLight = 4.5f;
    public AudioSource engineSound;
    public List<float> gearRatios = new List<float>();
    public int MaxPower;
    public float DriveWheelRPM;
    public float power = 0;
    public float lastPower;
    private float delayTime;

    [SerializeField]
    float EngineForce;
    float EngineTorque;
    float CurrentGear;
    [SerializeField]
    float CurrentPower;
    public int appropiateGear = 0;
    public int gearIndex = 0;
    public float EnginePitch;
    public bool Reverse;
    float shiftTime;



    void PlayEngineSnd()
    {
        if (engineSound != null)
        {
            if (engineSound.time > engineSound.clip.length / 2)
            {
                engineSound.time = 2;
            }
        }
    }
    void ShiftGears(float maxWheelRPM)
    {





        if (EngineForce >= maxWheelRPM * CurrentGear && appropiateGear < gearRatios.Count - 1 && Time.time > shiftTime)
        {

            appropiateGear++;
            shiftTime = Time.time + delayTime;
            EnginePitch *= 0.8f;


        }

        if (EngineForce <= maxWheelRPM * CurrentGear * 0.7f && appropiateGear > 0 && Time.time > shiftTime)
        {
            appropiateGear--;

            shiftTime = Time.time + delayTime * 0.2f;
            EnginePitch *= 0.25f;
        }
        gearIndex = appropiateGear;



        CurrentGear = gearRatios[gearIndex];
        EnginePitch = Mathf.Lerp(DriveWheelRPM, maxWheelRPM, EngineForce / 100);







    }


    public float ReverseEngineForce(float pedal, TruckType truck)
    {
        float engineRPM = CalculateEngineTorque(pedal, truck);

        delayTime = truck.Transmission;

    

        EngineTorque = engineRPM * ((DriveWheelRPM / truck.MaxWheelRPM) * truck.MaxWheelRPM * 0.1f)  * truck.Transmission;
        if (pedal < -0.5)
        {

            EngineForce += truck.BrakeForce * Time.deltaTime;

            EngineForce = Mathf.Clamp(EngineForce, -MaxPower / 2, 0);
        }

        else if (pedal > 0.9f)
        {

            EngineForce -= EngineTorque / truck.MaxWheelRPM * truck.accelRate * Time.deltaTime;

            EnginePitch = pedal * EnginePitch;
        }
        else if (pedal < 0.9f)
        {
            EngineForce += EngineForce / truck.MaxWheelRPM;

            EnginePitch -= ((CurrentGear / DriveWheelRPM)) * Time.deltaTime;
        }

        EngineForce = Mathf.Clamp(EngineForce, 0, MaxPower * 120);
        PlayEngineSnd();
        engineSound.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", EnginePitch);
        return EngineForce * truck.Power/ truck.Mass;
    }
    //Generate Fake Power
    //use as the max Power To lerp towards
    public float CalculateEngineTorque(float pedal,TruckType truck)
    {
       return truck.Power * 30 * pedal;
        
    }


    public float CalculateEngineForce(float pedal, TruckType truck)
    {
        float engineRPM= CalculateEngineTorque(pedal, truck);

        delayTime = truck.Transmission;

        ShiftGears(truck.MaxWheelRPM);

        EngineTorque =engineRPM * ((DriveWheelRPM/truck.MaxWheelRPM) * truck.MaxWheelRPM * 0.1f) * CurrentGear * truck.Transmission;
    

      

        if (pedal < -0.5)
        {

            EngineForce -= truck.BrakeForce;


        }

        if (pedal > 0.9f)
        {

            EngineForce += EngineTorque/ truck.MaxWheelRPM * truck.accelRate * Time.deltaTime;


        }
        else if (EngineForce > 0)
        {
            EngineForce -= EngineForce/truck.MaxWheelRPM;


        }


       
      float DrivePitch = Mathf.Clamp(DriveWheelRPM, 4.64f, 7.00f);

      EnginePitch = Mathf.Clamp(EnginePitch, 4.64f, 6.00f);

      EngineForce = Mathf.Clamp(EngineForce,0,MaxPower *120);


      gearIndex = Mathf.Clamp(gearIndex, 0, gearRatios.Count - 1);


      //  EnginePower = Mathf.Clamp(EnginePower,0,Mathf.Pow(truck.Power,2));

      // EnginePower = Mathf.Clamp(EnginePower,-truck.MaxWheelRPM,truck.MaxWheelRPM);
       
   
        
        PlayEngineSnd();
        engineSound.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", EnginePitch);
        return EngineForce;
    }





}

