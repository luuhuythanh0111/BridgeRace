using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Bot : Character
{
    [SerializeField] NavMeshAgent navMeshAgent;

    private IState currentState;

    private int currentPlatformIndex;

    internal Vector3 targetBrickPosition = Vector3.zero;
    internal Vector3 targetPosition = Vector3.zero;
    internal bool goingToTargert = false;
    internal bool haveTarget = false;
    internal bool haveGetDown = false;
    internal int randomTargetBrick;

    protected override void Start()
    {
        base.Start();
        randomTargetBrick = Random.Range(1, 21);
        currentPlatformIndex = 0;
        Debug.Log(randomTargetBrick);
    }

    

    protected override void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, groundLayer);
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
        if (randomTargetBrick <= playerBody.GetComponent<PlayerControlBricks>().bricks.Count)
        {
            ChangeState(new BuildBridgeState());
            return;
        }

        if (haveTarget==false)
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

    public void GoToBridge()
    {
        navMeshAgent.SetDestination(FindObjectOfType<LevelManager>().platforms[++currentPlatformIndex].transform.position);
    }
    
    public void GetDownBridge()
    {
        if (haveGetDown == false)
        {
            navMeshAgent.SetDestination(FindObjectOfType<LevelManager>().platforms[--currentPlatformIndex].transform.position);
            haveGetDown = true;
        }

        if (grounded)
        {
            ChangeState(new PatrolState());
            haveGetDown = false;
        }
    }
}
