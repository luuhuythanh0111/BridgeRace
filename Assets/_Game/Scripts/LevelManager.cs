using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] internal List<GameObject> platforms = new List<GameObject>();

    private bool[] usedColor = new bool[10];
    private void Start()
    {

    }

    public bool CheckUsedColor(int index)
    {
        return !usedColor[index];
    }
    public Material GetMaterialFromNumber(int number)
    {
        usedColor[number] = true;
        return materials[number];
    }

}
