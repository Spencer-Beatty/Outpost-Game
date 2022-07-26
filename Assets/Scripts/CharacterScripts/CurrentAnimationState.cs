using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CurrentAnimationState : MonoBehaviour
{
    private string[] possibleStates;
    private string currentState = "idle";
    

    private void Start()
    {

        possibleStates = new string[]
        {
            "idle", "attack", "block"
        };
        currentState = possibleStates[0];
        
    }

    public void SetState(AnimatorStateInfo stateInfo)
    {
        if(stateInfo.IsTag("attack"))
        {
            currentState = possibleStates[1];
        }
        else if (stateInfo.IsTag("block"))
        {
            currentState = possibleStates[2];
        }else if (stateInfo.IsTag("idle"))
        {
            currentState = possibleStates[0];
        }
        
    }

    public string GetCurrentState()
    {
        return currentState;
    }
    
    
}
