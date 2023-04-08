using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlBricks : MonoBehaviour
{
    [SerializeField] protected bool isImportant;
    [SerializeField] protected LayerMask stepBrickLayer;
    [SerializeField] protected LayerMask stepBrickChangeColorLayer;
    internal List<GameObject> bricks = new List<GameObject>();

    private void Update()
    {
        CheckMoveOnStair();
    }

    protected void CheckMoveOnStair()
    {
        if(!isImportant)
        {
            ChangeColorStep();
            return;
        }
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.forward + Vector3.down * 0.4f, out hit, 1.5f, stepBrickLayer);
        Debug.DrawRay(transform.position, (Vector3.forward + Vector3.down * 0.4f)*1.5f, Color.green, 1f);
        if (hit.collider != null)
        {
            GameObject block = hit.collider.gameObject;

            //Debug.Log(block.transform.parent.GetComponent<Renderer>().sharedMaterial + " " + transform.GetComponent<Renderer>().sharedMaterial);
            if (block.transform.parent.GetComponent<Renderer>().sharedMaterial == transform.GetComponent<Renderer>().sharedMaterial)
            {
                    block.GetComponent<BoxCollider>().isTrigger = true;
                
            }
            else if(block.transform.parent.GetComponent<Renderer>().sharedMaterial != transform.GetComponent<Renderer>().sharedMaterial)
            {
                
                if (bricks.Count > 0)
                    block.GetComponent<BoxCollider>().isTrigger = true;
                else
                {
                    block.GetComponent<BoxCollider>().isTrigger = false;
                }
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
            if(block.transform.GetComponent<Renderer>().sharedMaterial != transform.GetComponent<Renderer>().sharedMaterial)
            {
                if(bricks.Count>0)
                {
                    Destroy(bricks[bricks.Count - 1]);
                    bricks.RemoveAt(bricks.Count - 1);
                    block.transform.GetComponent<Renderer>().sharedMaterial = transform.GetComponent<Renderer>().sharedMaterial;
                }
            }
            
        }
    }

    internal void EatingBrick(Collider other)
    {
        if (other.GetComponent<Renderer>().sharedMaterial == transform.GetComponent<MeshRenderer>().sharedMaterial)
        {
            int x = other.GetComponentInParent<Brick>().x;
            int y = other.GetComponentInParent<Brick>().y;
            other.transform.parent.parent.parent.GetComponent<SpawnBricks>().haveSpawnThisCoordinate[x, y] = false;
            other.transform.parent.SetParent(transform);
            bricks.Add(other.transform.parent.gameObject);
            other.transform.parent.localPosition = Vector3.back * 0.4f + Vector3.up * (bricks.Count + 1) * 0.2f;
            other.transform.parent.localRotation = Quaternion.identity;
            
            
        }
    }
}
