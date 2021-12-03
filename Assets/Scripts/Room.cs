using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{    
    //Directions are the same as in minecraft lol
    [SerializeField]
    GameObject doorN;
    [SerializeField]
    GameObject doorS;
    [SerializeField]
    GameObject doorW;
    [SerializeField]
    GameObject doorE;
    //N = 1  S = 2 W = 4 E = 8 (Use binary operators to set)
    public byte permClosed; 
   
}
