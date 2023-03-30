using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    float randomTime;
    float timer;

    public void OnEnter(Bot bot)
    {
        timer = 0;
        randomTime = Random.Range(3f, 6f);
    }

    public void OnExecute(Bot bot)
    {
        timer += Time.deltaTime;



        
    }

    public void OnExit(Bot bot)
    {

    }
}
