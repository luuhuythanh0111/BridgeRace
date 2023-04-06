using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBridgeState : IState
{
    public void OnEnter(Bot bot)
    {
        bot.StopMoving();
        bot.GoToBridge();
    }

    public void OnExecute(Bot bot)
    {
        if (bot.GoToNextPlatform())
            return;
        if (bot.GetComponentInChildren<PlayerControlBricks>().bricks.Count==0)
        {
            bot.GetDownBridge();
            //Debug.Log("GET DOWN");
        }
        

    }

    public void OnExit(Bot bot)
    {
        bot.StopMoving();
    }
}
