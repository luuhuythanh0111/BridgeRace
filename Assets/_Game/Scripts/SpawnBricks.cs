using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnBricks : MonoBehaviour
{
    [SerializeField] private GameObject prefabBrick;
    private bool haveSpawn = false;
    private bool[] haveSpawnBrickColor = new bool[10];
    internal bool[,] haveSpawnThisCoordinate = new bool[21, 21];
    private float[,] randomSpawnTime = new float[21, 21];
    private float[,] timer = new float[21, 21];
    private List<int> usedColorIndex = new List<int>();
    public int randomRange;
    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            haveSpawnBrickColor[i] = false;
        }

        for (int i = 1; i <= 10; i++)
        {
            for (int j = 1; j <= 10; j++)
            {
                haveSpawnThisCoordinate[i, j] = false;
                randomSpawnTime[i, j] = Random.Range(5f, 8f);
            }
        }
    }
    public void SpawnBrick(int colorIndex)
    {
        if (haveSpawnBrickColor[colorIndex] == false)
        {
            usedColorIndex.Add(colorIndex);

            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    if (haveSpawnThisCoordinate[i, j])
                    {
                        continue;
                    }
                    
                    int x = Random.Range(0, FindObjectOfType<LevelManager>().usedCololIndex.Count + 1);
                    if (x == FindObjectOfType<LevelManager>().usedCololIndex.Count)
                    {
                        continue;
                    }
                    x = FindObjectOfType<LevelManager>().usedCololIndex[x];
                    
                    if (x != colorIndex)
                    {
                        continue;
                    }
                    //Debug.Log(colorIndex + " " + x);
                    GameObject brick = Instantiate(prefabBrick, Vector3.zero, Quaternion.identity, transform.GetChild(0));
                    brick.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial = FindObjectOfType<LevelManager>().GetMaterialFromNumber(colorIndex);
                    brick.transform.localPosition = new Vector3(10 - 2 * i, 0, 10 - 2 * j);
                    brick.GetComponent<Brick>().x = i;
                    brick.GetComponent<Brick>().y = j;
                    haveSpawnThisCoordinate[i, j] = true;
                }
            }
            haveSpawnBrickColor[colorIndex] = true;
        }
        else
        {
            SpawnOnUpdate();
        }
    }
    void SpawnOnUpdate()
    {
        for (int i = 1; i <= 10; i++)
        {
            for (int j = 1; j <= 10; j++)
            {
                if (haveSpawnThisCoordinate[i, j])
                    continue;
                timer[i, j] += Time.deltaTime/(usedColorIndex.Count*1f);
                if (timer[i, j] > randomSpawnTime[i, j])
                {
                    timer[i, j] = 0;
                    randomSpawnTime[i, j] = Random.Range(5f, 8f);
                    int x = Random.Range(0, usedColorIndex.Count);
                    x = usedColorIndex[x];
                    GameObject brick = Instantiate(prefabBrick, Vector3.zero, Quaternion.identity, transform.GetChild(0));
                    brick.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial = FindObjectOfType<LevelManager>().GetMaterialFromNumber(x);
                    brick.transform.localPosition = new Vector3(10 - 2 * i, 0, 10 - 2 * j);
                    brick.GetComponent<Brick>().x = i;
                    brick.GetComponent<Brick>().y = j;
                    haveSpawnThisCoordinate[i, j] = true;
                }
            }
        }
        
    }
}
