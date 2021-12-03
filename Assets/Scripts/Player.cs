using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    int health = 6;
    [HideInInspector]
    public int Health { get { return health; } }
    [SerializeField]
    [Range(0.01f, 1000.0f)]
    float dmg = 3.5f;
    public float Damage { get { return dmg; } set { dmg = Mathf.Clamp(value, 0.01f, 1000.0f); } }
    [SerializeField]
    [Range(0.01f, 2.0f)]
    float speed = 1.0f;
    public float Speed { get { return speed; } set { speed = Mathf.Clamp(value,0.01f,2.0f); } }
    [SerializeField]
    [Range(0.01f, 10.0f)]
    float tears = 1.0f;
    public float TearDelay { get { return tears; } set { tears = Mathf.Clamp(value, 0.01f, 10.0f); } }


    public void TakeDamage() 
    { }
}
