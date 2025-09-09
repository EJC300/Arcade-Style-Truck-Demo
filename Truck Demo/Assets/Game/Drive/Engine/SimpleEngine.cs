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
    public   float power = 0;
    public float lastPower;
    private float delayTime;
    
    [SerializeField]
    float EnginePower;
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



         

            if (EnginePower>= maxWheelRPM * CurrentGear && appropiateGear < gearRatios.Count - 1 && Time.time > shiftTime)
            {

                appropiateGear++;
                shiftTime = Time.time + delayTime;
                EnginePitch *= 0.8f;


            }

            else if (EnginePower <= maxWheelRPM * CurrentGear * 0.7f && appropiateGear > 0 && Time.time > shiftTime)
            {
                appropiateGear--;

                shiftTime = Time.time + delayTime * 0.2f;
                EnginePitch *= 0.25f;
            }
            gearIndex = appropiateGear;



            CurrentGear = gearRatios[gearIndex];
            EnginePitch = Mathf.Lerp(DriveWheelRPM, maxWheelRPM, EnginePower / 100);







    }
   

    public float ReverseEngineForce(float pedal, TruckType truck)
    {
        EngineTorque = (DriveWheelRPM / 60 / 2 * Mathf.PI) * CurrentGear + 15;
        if (pedal < -0.5)
        {

            EnginePower += truck.BrakeForce * Time.deltaTime;

            EnginePower = Mathf.Clamp(EnginePower, -MaxPower/2,0);
        }

        else if (pedal > 0.9f)
        {

            EnginePower -= (((EngineTorque) / -7) * -pedal);

            EnginePitch = pedal * EnginePitch;
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

        delayTime = truck.Transmission;
         
        

            EngineTorque = (DriveWheelRPM / 60 / 2 * Mathf.PI) * CurrentGear + 15;

            ShiftGears(truck.MaxWheelRPM);
 
            if (pedal < -0.5)
            {

                EnginePower -= truck.BrakeForce * Time.deltaTime;


            }
   
            if (pedal > 0.9f)
            {
           
                EnginePower += (((EngineTorque) * CurrentGear) * pedal);

               
            }
            else if (pedal < 0.9f)
            {
                EnginePower -= DriveWheelRPM * CurrentGear * Time.deltaTime;

               
            }
        
       
        DriveWheelRPM = Mathf.Clamp(DriveWheelRPM, 0, truck.MaxWheelRPM);
        float DrivePitch = Mathf.Clamp(DriveWheelRPM, 4.64f, 7.00f);
        
        EnginePitch = Mathf.Clamp(EnginePitch, 4.64f,6.00f);

     


        gearIndex = Mathf.Clamp(gearIndex, 0, gearRatios.Count - 1);

        EnginePower = Mathf.Clamp(EnginePower, -MaxPower, MaxPower);
        
     
        EnginePower = Mathf.Clamp(EnginePower, 0, MaxPower);
        
    

        DriveWheelRPM = Mathf.Clamp(DriveWheelRPM, -truck.MaxWheelRPM,truck.MaxWheelRPM);
        PlayEngineSnd();
        engineSound.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", EnginePitch);
        return EnginePower + EnginePower * truck.Mass * 0.01f * Time.deltaTime;
    }

    



}
