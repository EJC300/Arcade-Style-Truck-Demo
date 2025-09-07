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
    [SerializeField]
    public float DriveWheelRPM;
    public   float power = 0;
    public float lastPower;
    [SerializeField]
    float EnginePower;
    float EngineTorque;
    float CurrentGear;
    [SerializeField]
    float CurrentPower;
    public int appropiateGear = 0;
    public int gearIndex = 0;
    public float EnginePitch;
    public int Reverse;

    

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
    void ShiftGears()
    {
        CurrentGear = gearRatios[gearIndex];

        if (Reverse < 1)
        {

            if (EnginePower >= 1000)
            {

                for (int i = 0; i < gearRatios.Count; i++)
                {

                    if (DriveWheelRPM * gearRatios[i] < 1000)
                    {

                        appropiateGear = i;

                        break;
                    }
                }
                gearIndex = appropiateGear;
            }
            if (EnginePower <= 1000)
            {

                for (int i = 0; i < gearRatios.Count; i++)
                {
                    if (DriveWheelRPM * gearRatios[i] > 1000)
                    {
                        appropiateGear = i;

                        break;
                    }
                }
                gearIndex = appropiateGear;
            }


        }
       
    }
   

    public float ReverseEngineForce(float pedal, TruckType truck)
    {
        EngineTorque = (DriveWheelRPM / 60 / 2 * Mathf.PI) * CurrentGear + 15;
        if (pedal < -0.5)
        {

            EnginePower += truck.BrakeForce * Time.deltaTime;

            EnginePower = Mathf.Clamp(EnginePower, -MaxPower/2,0);
        }

        if (pedal > 0.9f)
        {

            EnginePower -= (((EngineTorque) / -7) * -pedal);

            EnginePitch += pedal * -((EnginePower / 5252 / CurrentGear)) / (DriveWheelRPM / 5252) * (Time.deltaTime);
        }
        else if (pedal < 0.9f)
        {
            EnginePower -= DriveWheelRPM * Time.deltaTime;

            EnginePitch -= ((CurrentGear / DriveWheelRPM)) * Time.deltaTime;
        }
        EnginePower = Mathf.Clamp(EnginePower, -MaxPower, MaxPower);
        PlayEngineSnd();
        engineSound.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", EnginePitch);
        return EnginePower;
    }

    public float EngineForce(float pedal,TruckType truck)
    {



        

        EngineTorque = (DriveWheelRPM / 60 / 2 * Mathf.PI) * CurrentGear + 15;

        ShiftGears();
 
            if (pedal < -0.5)
            {

                EnginePower -= truck.BrakeForce * Time.deltaTime;


            }
   
            if (pedal > 0.9f)
            {
           
                EnginePower += (((EngineTorque) / CurrentGear) * pedal);

                EnginePitch += pedal * ((EnginePower / 5252 / CurrentGear)) / (MaxPower / 5252) * Time.deltaTime;
            }
            else if (pedal < 0.9f)
            {
                EnginePower -= DriveWheelRPM * Time.deltaTime;

                EnginePitch -= ((CurrentGear / DriveWheelRPM)) * Time.deltaTime;
            }
        
       
        DriveWheelRPM = Mathf.Clamp(DriveWheelRPM, 0, 3000);
        float DrivePitch = Mathf.Clamp(DriveWheelRPM, 4.64f, 7.00f);
        
        EnginePitch = Mathf.Clamp(EnginePitch, 4.64f,6.00f);

     


        gearIndex = Mathf.Clamp(gearIndex, 0, gearRatios.Count - 1);

        EnginePower = Mathf.Clamp(EnginePower, -MaxPower, MaxPower);
        
     
        EnginePower = Mathf.Clamp(EnginePower, 0, MaxPower);
        
    

        DriveWheelRPM = Mathf.Clamp(DriveWheelRPM, -2000, 2000);
        PlayEngineSnd();
        //engineSound.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", EnginePitch);
        return EnginePower + EnginePower;
    }

    



}
