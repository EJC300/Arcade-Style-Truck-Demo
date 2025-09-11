using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerControls : MonoBehaviour
{
    //Dirty NonePersistant Singleton 

    public static PlayerControls instance;

    //Event for player acceleration

    public delegate void AccelerationEvent(float pedal);

    public event AccelerationEvent Acceleration;

    
    //Event for player braking
    public delegate void BrakingEvent(float pedal);

    public event BrakingEvent Braking;
    //Event for player gear shift

    

    //Event for player turning

    public delegate void TurningEvent(float steer);
    public event TurningEvent Turning;
    //Update to use new player Input system
    private void OnEnable()
    {
        instance = this;
    }

    private void Update()
    {
        float pedal = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");
        if (pedal > 0)
        {
            Acceleration?.Invoke(pedal);
        }
      
        if(pedal < 0)
        {
            Braking?.Invoke(pedal);
        }

        if(Mathf.Abs(steer) > 0)
        {
            Turning?.Invoke(steer);
        }
    }



}
