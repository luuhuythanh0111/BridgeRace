using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBricks : MonoBehaviour
{
    [SerializeField] private GameObject prefabBrick;

    private bool[] haveSpawnBrickColor = new bool[10];

    public void SpawnBrick(int colorIndex)
    {
        if (haveSpawnBrickColor[colorIndex] ==false)
        {
            for(int i=1; i<=20; i++)
            {
                for (int j = 1; j <= 20; j++)
                {
                    if (Random.Range(0, 2) == 0) 
                        continue;
                    GameObject brick = Instantiate(prefabBrick, Vector3.zero, Quaternion.identity, transform.GetChild(0));
                    brick.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial = FindObjectOfType<LevelManager>().GetMaterialFromNumber(colorIndex);
                    brick.transform.localPosition = new Vector3(10 - i, 0, 10 - j);
                }
            }
            haveSpawnBrickColor[colorIndex] = true;
        }
    }
}
