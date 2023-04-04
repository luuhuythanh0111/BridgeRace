using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    [SerializeField] NavMeshAgent navMeshAgent;

    private IState currentState;

    internal Vector3 targetBrickPosition = Vector3.zero;
    internal Vector3 targetPosition = Vector3.zero;
    internal bool goingToTargert = false;
    internal bool haveTarget = false;

    protected override void Update()
    {
        SpawnBricks();
        Debug.Log(currentState);
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
        if(haveTarget==false)
        {
            if (Vector3.Distance(transform.position, targetPosition) < 0.2f || Vector3.Distance(Vector3.zero, targetPosition) < 0.2f || (rigidbody.velocity.x == 0f && rigidbody.velocity.z == 0f))
            {
                targetPosition = new Vector3(Random.Range(transform.position.x - 10f, transform.position.x + 11f),
                                             transform.position.y,
                                             Random.Range(transform.position.z - 10f, transform.position.z + 11f));
                navMeshAgent.SetDestination(targetPosition);
            }
            else
            {
                
            }
        }
        else
        {
            
            if(goingToTargert==false)
            {
                navMeshAgent.SetDestination(targetBrickPosition);
                goingToTargert = true;
            }
            else
            {
                if(Vector3.Distance(targetBrickPosition,transform.position)<0.2f)
                {
                    haveTarget = false;
                }
            }
        }
        
    }
}
