using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBricks : MonoBehaviour
{
    [SerializeField] private GameObject prefabBrick;

    private bool[] haveSpawnBrickColor = new bool[10];
    private bool[,] haveSpawnThisCoordinate = new bool[21, 21];
    private float[,] randomSpawnTime = new float[21, 21];
    private float[,] timer = new float[21, 21];

    public void SpawnBrick(int colorIndex)
    {
        if (haveSpawnBrickColor[colorIndex] ==false)
        {
            for(int i=1; i<=20; i++)
            {
                for (int j = 1; j <= 20; j++)
                {
                    if (haveSpawnThisCoordinate[i, j] == true)
                        continue;
                    int x = Random.Range(0, FindObjectOfType<LevelManager>().indexUsingColor.Count);

                    if (FindObjectOfType<LevelManager>().indexUsingColor[x] != colorIndex) 
                        continue;
                    GameObject brick = Instantiate(prefabBrick, Vector3.zero, Quaternion.identity, transform.GetChild(0));
                    brick.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial = FindObjectOfType<LevelManager>().GetMaterialFromNumber(colorIndex);
                    brick.transform.localPosition = new Vector3(25 - 2*i, 0, 25 - 2*j);
                    haveSpawnThisCoordinate[i, j] = true;

                    randomSpawnTime[i,j] = Random.Range(2f, 5f);
                }
            }
            haveSpawnBrickColor[colorIndex] = true;
        }
    }

    void CreateRandomTime()
    {
        for (int i = 1; i <= 20; i++)
        {
            for (int j = 1; j <= 20; j++)
                randomSpawnTime[i,j] = Random.Range(2f, 5f);
        }
    }

    void SpawnOnUpdate()
    {
        for (int i = 1; i <= 20; i++)
        {
            for (int j = 1; j <= 20; j++)
            {
                timer[i, j] += Time.deltaTime;

                if (timer[i, j] > randomSpawnTime[i, j])
                {
                    timer[i, j] = 0;
                    randomSpawnTime[i, j] = Random.Range(5f, 10f);
                    int x = FindObjectOfType<LevelManager>().indexUsingColor[
                        Random.Range(0, FindObjectOfType<LevelManager>().indexUsingColor.Count)];

                    Debug.Log(x);

                    GameObject brick = Instantiate(prefabBrick, Vector3.zero, Quaternion.identity, transform.GetChild(0));
                    brick.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial = FindObjectOfType<LevelManager>().GetMaterialFromNumber(x);
                    brick.transform.localPosition = new Vector3(25 - 2 * i, 0, 25 - 2 * j);
                }
            }
        }
    }

    private void Update()
    {
        SpawnOnUpdate();
    }
}
