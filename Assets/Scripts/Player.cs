using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    
    float speedCnst = 4.0f; //Got this thru trial and error
    Rigidbody rb;
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
    public float Speed { get { return speed; } set { speed = Mathf.Clamp(value, 0.01f, 2.0f); } }
    [SerializeField]
    [Range(0.01f, 10.0f)]
    float tears = 1.0f;
    public float TearDelay { get { return tears; } set { tears = Mathf.Clamp(value, 0.01f, 10.0f); } }
    [SerializeField]
    [Range(0,2.0f)]
    float Itime = .5f;
    bool InVulnerable;
    bool s = false; 
    bool Shooting { get { return s; } 
        set 
        {
            if (value == s) return;
            s = value;
            if (!value)
                StopCoroutine(Shoot());
            else
                StartCoroutine(Shoot());
        } 
    }
    int ShootingDir = 0;
    [Header("Events")]
    public UnityEvent OnDamage;
    public UnityEvent OnShoot;
    [Header("Controls")]
    public KeyCode UpShoot = KeyCode.UpArrow; //1
    public KeyCode DownShoot = KeyCode.DownArrow; //2
    public KeyCode LeftShoot = KeyCode.LeftArrow; //3
    public KeyCode RightShoot = KeyCode.RightArrow; //4



    IEnumerator Iframes() 
    {
        yield return new WaitForSeconds(Itime);//Wait for Itim and then make player vulnerable
        InVulnerable = false;
    }
    IEnumerator Shoot()  //I sure hope this works, I hav NO idea if it will tho
    {
        while (true) 
        {
            yield return new WaitForSeconds(TearDelay);
            shoot();
        }
    }
    private void Start() => rb = GetComponent<Rigidbody>();
    
    public void TakeDamage(int dmg = 1) 
    {       
        if (!InVulnerable) 
        {
            InVulnerable = true;
            health -= dmg;
            OnDamage.Invoke();
            StartCoroutine(Iframes());
        }
    }
    void shoot() 
    {
        OnShoot.Invoke();
        //TODO implement
    }

    void Update()
    {
        Shooting = Input.GetKey(UpShoot) || Input.GetKey(DownShoot) || Input.GetKey(LeftShoot) || Input.GetKey(RightShoot);
        if (Input.GetKeyDown(UpShoot))ShootingDir = 1;
        if (Input.GetKeyDown(DownShoot))ShootingDir = 2;
        if (Input.GetKeyDown(LeftShoot))ShootingDir = 3;
        if (Input.GetKeyDown(RightShoot)) ShootingDir = 4;
    }

    void FixedUpdate()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        Vector3 force = new Vector3(h, 0, v) * speed * speedCnst;
        //rb.AddForce(force);
        rb.velocity = force;
    }
}
