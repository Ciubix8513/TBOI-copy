using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    protected Rigidbody rb;
    [Header("Enemy data")]
    public float Health;
    public float Speed;
    public bool Flying;
    public bool Alive;
    public bool ContactDamage;
    [SerializeField]
    bool active;
    public bool Active { get { return active; } set { active = value; if (value) OnSpawn.Invoke(); } }
    [HideInInspector]
    public Room room;
    [Header("Enemy events")]
    [SerializeField]
    UnityEvent OnPlayerDamage; //Hit by player
    [SerializeField]
    UnityEvent OnHit; //Hit player
    [SerializeField]
    UnityEvent OnDeath;
    [SerializeField]
    UnityEvent OnSpawn;
    

    private void OnCollisionStay(Collision collision)
    {
       
        if(ContactDamage
            && collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<Player>().TakeDamage();            
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Health <= 0)
        {
            Alive = false;
            OnDeath.Invoke();
            gameObject.SetActive(false);
        }
    }
}
