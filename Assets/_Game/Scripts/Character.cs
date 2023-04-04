using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    protected float horizontalInput;
    protected float verticalInput;
    [SerializeField] protected Joystick joystick;
    [SerializeField] protected new Rigidbody rigidbody;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    bool grounded;
    public Transform orientation;
    Vector3 moveDirection;


    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Color and Change Direction")]
    [SerializeField] GameObject playerBody;
    protected int bodyColorIndex;


    protected void Start()
    {
        bodyColorIndex = Random.Range(0, FindObjectOfType<LevelManager>().materials.Count);
        playerBody.GetComponent<Renderer>().sharedMaterial = FindObjectOfType<LevelManager>().GetMaterialFromNumber(bodyColorIndex);
        rigidbody.drag = groundDrag;
    }

    protected virtual void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, groundLayer);

        MyInput();
        SpeedControl();
        SpawnBricks();
    }

    protected void FixedUpdate()
    {
        MovePlayer();
    }


    private void MyInput()
    {
        //horizontalInput = Input.GetAxis("Horizontal");
        //verticalInput = Input.GetAxis("Vertical");

        horizontalInput = joystick.Horizontal;
        verticalInput = joystick.Vertical;
    }

    protected void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope) 
        {
            RaycastHit hitLeft;
            RaycastHit hitRight;

            Physics.Raycast(transform.position, Vector3.left, out hitLeft,1f);
            Debug.DrawRay(transform.position, Vector3.left, Color.green, 2f);
            
            Physics.Raycast(transform.position, Vector3.right, out hitRight, 1f);
            Debug.DrawRay(transform.position, Vector3.right, Color.green, 2f);


            if(hitLeft.collider != null  && horizontalInput <0f)
            {
                if(verticalInput>0f)
                {
                    
                    moveDirection += moveSpeed * orientation.forward * (playerBody.transform.rotation.y+90)/90f;
                }
                else if(verticalInput<0f)
                {                  
                    moveDirection += -1f * orientation.forward * (playerBody.transform.rotation.y+90)/90f;
                }
            }

            else if (hitRight.collider != null && horizontalInput > 0f)
            {
                if (verticalInput > 0f)
                {

                    moveDirection += moveSpeed * orientation.forward * (playerBody.transform.rotation.y + 90) / 90f;
                }
                else if (verticalInput < 0f)
                {
                    moveDirection += -1f * orientation.forward * (playerBody.transform.rotation.y + 90) / 90f;
                }
            }

            rigidbody.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rigidbody.velocity.y > 0)
                rigidbody.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rigidbody.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        ChangeDirection();
    }

    private void ChangeDirection()
    {
        Vector3 direct2 = new Vector3(horizontalInput, 0, verticalInput);

        if (Vector3.Distance(direct2, Vector3.zero) > 0.1f)
            playerBody.transform.rotation = Quaternion.LookRotation(direct2);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rigidbody.velocity = new Vector3(limitedVel.x, rigidbody.velocity.y, limitedVel.z);
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    protected void SpawnBricks()
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, Vector3.down, out hit, 2f, groundLayer);
        //Debug.DrawRay(transform.position, Vector3.down * 2f, Color.yellow, 2f);

        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponent<SpawnBricks>().SpawnBrick(bodyColorIndex);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Brick")
        {

            playerBody.GetComponent<PlayerControlBricks>().EatingBrick(other);
        }

    }
}
