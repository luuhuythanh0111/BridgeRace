using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : Character
{
    private IState currentState;

    private Vector3 targetPosition;

    protected override void Update()
    {
        base.Update();
 
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

        if (Vector3.Distance(targetPosition, transform.position) < 2f)
        {
            targetPosition = new Vector3(Random.Range(transform.position.x - 10f, transform.position.x + 10f),
                                     transform.position.y,
                                     Random.Range(transform.position.z - 10f, transform.position.z + 10f));
        }

        transform.position = Vector3.MoveTowards(transform.position,targetPosition,speed * Time.deltaTime);
    }
}
