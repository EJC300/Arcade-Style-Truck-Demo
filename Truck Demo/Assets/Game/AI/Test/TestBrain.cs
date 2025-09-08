using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBrain
{


    //StateMachine

    string CurrentState;

    public string GetCurrentState()
    {
        return CurrentState;
    }
    public void SetState(int state)
    {
        if(state == 1)
        {
            CurrentState = "FollowWaypoint";
        }
        else if (state == 2)
        {
            CurrentState = "Stop";
        }
        else if (state == 3)
        {
            CurrentState = "SlowDown";
        }
        else if (state == 4)
        {
            CurrentState = "AvoidFront";
        }
        else if (state == 5)
        {
            CurrentState = "AvoidNextToMe";
        }
        else if (state == 6)
        {
            CurrentState = "AdjustThrottle";
        }
        else if (state == 7)
        {
            CurrentState = "Ram";
        }

    }

}
