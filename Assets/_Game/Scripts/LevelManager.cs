using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    protected enum Color
    {
        Red,
        Green,
        Blue,
        Purple,
        Cyan,
        Yellow,
        Pink,
        Magenta,
        DarkBlue,
        Orange
    }

    [SerializeField] internal List <Material> materials = new List <Material> ();
<<<<<<< Updated upstream
    [SerializeField] internal List<GameObject> platforms = new List<GameObject>();

    internal List<int> usedCololIndex = new List<int>();
    private bool[] usedColor = new bool[10];
    public bool CheckUsedColor(int index)
=======
    private bool[] usedColor = new bool[10];
    private void Start()
>>>>>>> Stashed changes
    {
        if(!usedColor[index])
            usedCololIndex.Add(index);
        return !usedColor[index];
    }
<<<<<<< Updated upstream
=======

    public bool CheckUsedColor(int index)
    {
        return !usedColor[index];
    }
>>>>>>> Stashed changes
    public Material GetMaterialFromNumber(int number)
    {
        usedColor[number] = true;
        return materials[number];
    }

}
