using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected Joystick joystick;

    [SerializeField] protected new Rigidbody rigidbody;

    [SerializeField] protected float speed;

    [SerializeField] protected LayerMask stepBrickLayer;

    [SerializeField] protected LayerMask groundLayer;

    protected int bodyColorIndex;

    protected List<GameObject> bricks = new List<GameObject>();

    protected float previousVelocityY;
    protected float currentVeloctiyY;

    protected void Start()
    {
        bodyColorIndex = Random.Range(0, FindObjectOfType<LevelManager>().materials.Count);
        transform.GetComponent<Renderer>().sharedMaterial = FindObjectOfType<LevelManager>().GetMaterialFromNumber(bodyColorIndex);
    }

    protected virtual void Update()
    {
        CheckMoveOnStair();
        MoveDownStairSmooth();
        SpawnBricks();
    }

    protected void CheckMoveOnStair()
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

    protected void MoveDownStairSmooth()
    {
        currentVeloctiyY = rigidbody.velocity.y;
        if (previousVelocityY - currentVeloctiyY > 0.15f)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x,
                                             Mathf.Min(currentVeloctiyY, -5f),
                                             rigidbody.velocity.z);
        }

        previousVelocityY = currentVeloctiyY;
    }

    protected void SpawnBricks()
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, Vector3.down, out hit, 2f, groundLayer);
        Debug.DrawRay(transform.position, Vector3.down * 2f, Color.yellow, 2f);

        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponent<SpawnBricks>().SpawnBrick(bodyColorIndex);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Brick")
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
