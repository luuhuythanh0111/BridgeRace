using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] private Joystick joystick;

    [SerializeField] private new Rigidbody rigidbody;

    [SerializeField] private float speed;

    [SerializeField] private LayerMask stepBrickLayer;

    [SerializeField] private LayerMask groundLayer;

    private List<GameObject> bricks = new List<GameObject>();

    private float previousVelocityY;
    private float currentVeloctiyY;

    void Update()
    {
        Move();
        CheckMoveOnStair();
        MoveDownStairSmooth();
        SpawnBricks();
    }

    private void CheckMoveOnStair()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.forward + Vector3.down * 0.4f, out hit, 2f, stepBrickLayer);
        //Debug.DrawLine(transform.position, transform.position + (Vector3.forward + Vector3.down * 0.4f) * 2f, Color.red, 2f);

        if (hit.collider != null)
        {
            GameObject block = hit.collider.gameObject;

            if (block.transform.parent.GetComponent<Renderer>().sharedMaterial == transform.GetComponent<Renderer>().sharedMaterial)
            {
                block.GetComponent<BoxCollider>().enabled = false;
            }
            else if (bricks.Count > 0)
            {
                Destroy(bricks[bricks.Count - 1]);
                bricks.RemoveAt(bricks.Count - 1);
                block.transform.parent.GetComponent<Renderer>().sharedMaterial = transform.GetComponent<Renderer>().sharedMaterial;
                block.GetComponent<BoxCollider>().enabled = false;
            }
            else
            {
                block.GetComponent<BoxCollider>().enabled = true;
            }
        }

    }

    private void MoveDownStairSmooth()
    {
        currentVeloctiyY = rigidbody.velocity.y;
        if(previousVelocityY-currentVeloctiyY > 0.15f)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x,
                                             Mathf.Min(currentVeloctiyY, -5f), 
                                             rigidbody.velocity.z);
        }

        previousVelocityY = currentVeloctiyY;
    }

    private void Move()
    {
        rigidbody.velocity = new Vector3(joystick.Horizontal * speed + Input.GetAxis("Horizontal") * speed,
                                         rigidbody.velocity.y,
                                         joystick.Vertical * speed + Input.GetAxis("Vertical") * speed);

        Vector3 direct1 = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        Vector3 direct2 = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (Vector3.Distance(direct1, Vector3.zero) > 0.1f) 
            transform.rotation = Quaternion.LookRotation(direct1);
        if (Vector3.Distance(direct2, Vector3.zero) > 0.1f)
            transform.rotation = Quaternion.LookRotation(direct2);
    }

    private void SpawnBricks()
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, Vector3.down, out hit, 2f,groundLayer);
        Debug.DrawRay(transform.position, Vector3.down*2f, Color.yellow, 2f);

        if(hit.collider != null)
        {
            hit.collider.gameObject.GetComponent<SpawnBricks>().SpawnBrick(transform.GetComponent<Renderer>().sharedMaterial);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Brick")
        {
            if (other.GetComponent<Renderer>().sharedMaterial == transform.GetComponent<MeshRenderer>().sharedMaterial)
            {
                other.transform.parent.SetParent(transform);
                bricks.Add(other.transform.parent.gameObject);
                other.transform.parent.localPosition = Vector3.back * 0.4f + Vector3.up * (bricks.Count + 1) * 0.2f;
                other.transform.parent.localRotation = Quaternion.identity;
            }
        }

    }
}
