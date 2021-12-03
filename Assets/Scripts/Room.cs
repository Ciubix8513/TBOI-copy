using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{    
    //Directions are the same as in minecraft lol
    //N = -Z S = +Z W = -X E = +X
    [Header("Room Data")]
    [SerializeField]
    GameObject doorN;
    [SerializeField]
    GameObject doorS;
    [SerializeField]
    GameObject doorW;
    [SerializeField]
    GameObject doorE;
    [SerializeField]
    GameObject passageN;
    [SerializeField]
    GameObject passageS;
    [SerializeField]
    GameObject passageW;
    [SerializeField]
    GameObject passageE;
    [HideInInspector]
    public byte Opened;//N = 1  S = 2 W = 4 E = 8 (Use binary operators to set)
    [Header("Room events")]
    public UnityEvent OnRoomEnter;
    public UnityEvent OnRoomClear;
    [Header("Room data")]
    public bool Cleared = false;
    public bool InProgress = false;
    public List<Enemy> enemies;


    public void OpenDoors()
    {
        doorN.SetActive((Opened & 1) == 0);
        doorS.SetActive((Opened & 2) == 0);
        doorW.SetActive((Opened & 4) == 0);
        doorE.SetActive((Opened & 8) == 0);

        passageN.SetActive((Opened & 1) != 0);
        passageS.SetActive((Opened & 2) != 0);
        passageW.SetActive((Opened & 4) != 0);
        passageE.SetActive((Opened & 8) != 0);
    }
    public void CloseDoors() 
    {
        doorN.SetActive(true);
        doorS.SetActive(true);
        doorW.SetActive(true);
        doorE.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!Cleared)
            {
                OnRoomEnter.Invoke();
                InProgress = true;
            }
        }
        
          

    }
    private void Start()
    {
        OpenDoors();
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].room = this;
            enemies[i].Active = false;
        }
    }
    private void Update()
    {
        if (InProgress)
            for (int i = 0; i < enemies.Count; i++)
                if (enemies[i].Alive)
                    return;
        Cleared = true;
        InProgress = false;
        OnRoomClear.Invoke();
    }
}
