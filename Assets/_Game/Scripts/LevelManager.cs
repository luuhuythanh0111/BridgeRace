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

    private void Start()
    {
        
    }

    public Material GetMaterialFromNumber(int number)
    {
        return materials[number];
    }

}
