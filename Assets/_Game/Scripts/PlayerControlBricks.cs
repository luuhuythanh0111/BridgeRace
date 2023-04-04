using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlBricks : MonoBehaviour
{
    [SerializeField] protected LayerMask stepBrickLayer;
    [SerializeField] protected LayerMask stepBrickChangeColorLayer;
    internal List<GameObject> bricks = new List<GameObject>();

    private void Update()
    {
        CheckMoveOnStair();
    }

    protected void CheckMoveOnStair()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.forward + Vector3.down * 0.4f, out hit, 1.5f, stepBrickLayer);
        Debug.DrawRay(transform.position, (Vector3.forward + Vector3.down * 0.4f)*1.5f, Color.green, 1f);
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
                block.GetComponent<BoxCollider>().enabled = false;
            }
            else
            {
                block.GetComponent<BoxCollider>().enabled = true;
            }
        }
        ChangeColorStep();
    }

    protected void ChangeColorStep()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 2f, stepBrickChangeColorLayer);
        

        if (hit.collider != null)
        {
            //Debug.DrawRay(transform.position, Vector3.down * 2f, Color.red, 1f);
            GameObject block = hit.collider.gameObject;
            block.transform.GetComponent<Renderer>().sharedMaterial = transform.GetComponent<Renderer>().sharedMaterial;
        }
    }

    internal void EatingBrick(Collider other)
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
