using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    [SerializeField] NavMeshAgent navMeshAgent;

    private IState currentState;

    private Vector3 targetPosition;

    protected override void Update()
    {
        SpawnBricks();
        if(currentState!=null)
        {
            currentState.OnExecute(this);
        }
        else
        {
            ChangeState(new IdleState());
        }
    }

    public void ChangeState(IState newState)
    {
        if(currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if(currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    public void StopMoving()
    {
        rigidbody.velocity = Vector3.zero;
    }

    public void Moving()
    {
        
    }
}
