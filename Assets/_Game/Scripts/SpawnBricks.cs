using System.Collections;
using System.Collections.Generic;
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
        for(int i=0; i<10; i++)
        {
            haveSpawnBrickColor[i] = false;
        }
    }
    public void SpawnBrick(int colorIndex)
    {
        
        if (haveSpawnBrickColor[colorIndex] ==false)
        {
            usedColorIndex.Add(colorIndex);
            for(int i=1; i<=10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    if (haveSpawnThisCoordinate[i, j] == true)
                    {
                        continue;
                    }
                    Debug.Log(colorIndex);
                    int x = Random.Range(0, FindObjectOfType<LevelManager>().usedCololIndex.Count+1);

                    if(x== FindObjectOfType<LevelManager>().usedCololIndex.Count)
                    {
                        continue;
                    }

                    x = FindObjectOfType<LevelManager>().usedCololIndex[x];

                    if (x != colorIndex)
                    {
                        haveSpawnThisCoordinate[i, j] = false;
                        continue;
                    }
                    GameObject brick = Instantiate(prefabBrick, Vector3.zero, Quaternion.identity, transform.GetChild(0));
                    brick.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial = FindObjectOfType<LevelManager>().GetMaterialFromNumber(colorIndex);
                    brick.transform.localPosition = new Vector3(20 - 4*i, 0, 20 - 4*j);
                    brick.GetComponent<Brick>().x = i;
                    brick.GetComponent<Brick>().y = j;
                    haveSpawnThisCoordinate[i, j] = true;

                    randomSpawnTime[i,j] = Random.Range(4f, 6f);
                }
            }
            haveSpawnBrickColor[colorIndex] = true;
        }
        if (haveSpawn)
            SpawnOnUpdate();
        haveSpawn = true;
    }

    void SpawnOnUpdate()
    {
        for (int i = 1; i <= 10; i++)
        {
            for (int j = 1; j <= 10; j++)
            {
                if (haveSpawnThisCoordinate[i, j])
                    continue;
                timer[i, j] += Time.deltaTime;

                if (timer[i, j] > randomSpawnTime[i, j])
                {
                    timer[i, j] = 0;
                    randomSpawnTime[i, j] = Random.Range(5f, 8f);
                    int x = Random.Range(0, usedColorIndex.Count);
                    x = usedColorIndex[x];
                    GameObject brick = Instantiate(prefabBrick, Vector3.zero, Quaternion.identity, transform.GetChild(0));
                    brick.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial = FindObjectOfType<LevelManager>().GetMaterialFromNumber(x);
                    brick.transform.localPosition = new Vector3(20 - 4 * i, 0, 20 - 4 * j);
                    brick.GetComponent<Brick>().x = i;
                    brick.GetComponent<Brick>().y = j;
                    haveSpawnThisCoordinate[i, j] = true;
                }
            }
        }
    }

    private void Update()
    {
        
    }
}
