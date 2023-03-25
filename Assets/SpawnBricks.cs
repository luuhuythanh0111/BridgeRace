using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBricks : MonoBehaviour
{
    [SerializeField] private GameObject prefabBrick;

    private bool haveSpawnBrickColor = false;

    public void SpawnBrick(Material material)
    {
        if(haveSpawnBrickColor==false)
        {
            Debug.Log("da spawn");

            for(int i=1; i<=10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    GameObject brick = Instantiate(prefabBrick, Vector3.zero, Quaternion.identity, transform.GetChild(0));
                    brick.transform.localPosition = new Vector3(5 - i, 0, 5 - j);
                }

            }
            
            haveSpawnBrickColor = true;
        }
    }
}
