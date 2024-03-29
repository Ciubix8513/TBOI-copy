using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;


[Serializable]
class PlayerData
{
    public int Score = 0;
    [SerializeField]
    int health = 6;
    [HideInInspector]
    public int Health { get { return health; } set { health = value; } }
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
    [Range(0.1f, 4.0f)]
    float sSpeed = 1.0f;
    public float ShotSpeed { get { return sSpeed; } set { sSpeed = Mathf.Clamp(value, 0.1f, 4.0f); } }
    [SerializeField]
    [Range(0.1f, 10.0f)]
    float r = 5.0f;
    public float Range { get { return r; } set { r = Mathf.Clamp(value, 0.1f, 10.0f); } }

}
static class Data 
{
    public static bool HasData = false;
    public static PlayerData d;
}
[RequireComponent(typeof(Rigidbody))]

public class Player : MonoBehaviour
{
    
    Rigidbody rb;
    [Header("TechData")]
    [SerializeField]
    Text t;
[SerializeField]
    GameObject GOt;
    [SerializeField]
    Text GOtS;
    [SerializeField]
    float tearCNST = 2.0f;
    [SerializeField]
    float speedCnst = 4.0f; //Got this thru trial and error
    [SerializeField]
    Transform bParrent;
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    int bulletInstCount = 1000;
    [SerializeField]
    List<Bullet> Bullets;

    [Header("Stats")]
    [SerializeField]
    PlayerData data;
 

    [SerializeField]
    [Range(0,2.0f)]
    float Itime = .5f;
    bool InVulnerable;
    bool s = false;
    bool stoppedShooting = true;



    bool Shooting { get { return s; } 
        set 
        {
            if (value == s) return;
            s = value;
            if (!value)
                StopCoroutine(Shoot());
            else
                try
                {
                    StartCoroutine(Shoot());
                }catch(System.Exception e)
                {
                    Debug.Log(e.Message);
                }
        } 
    }
    Vector3 ShootingDir = Vector3.zero;
    [Header("Events")]
    public UnityEvent OnDamage;
    public UnityEvent OnShoot;
    [Header("Controls")]
    public KeyCode UpShoot = KeyCode.UpArrow; //1
    public KeyCode DownShoot = KeyCode.DownArrow; //2
    public KeyCode LeftShoot = KeyCode.LeftArrow; //3
    public KeyCode RightShoot = KeyCode.RightArrow; //4
    private void Death()
    {
        Data.HasData = false;
        t.gameObject.SetActive(false);
        GOt.gameObject.SetActive(true);
        GOtS.text += data.Score;
        if (HighScoreManager.GetHighScore() < data.Score)
            HighScoreManager.SetHighscore(data.Score);
        SeedSetting.seed = 0;
        gameObject.SetActive(false);

        
    }
    Bullet GetBullet()
    {
        for (int i = 0; i < Bullets.Count; i++)
            if (!Bullets[i].gameObject.activeInHierarchy)
                return Bullets[i];
        Bullets.Add(Instantiate(bullet,bParrent).GetComponent<Bullet>());
        var c = Bullets.Count - 1;
        Bullets[c].gameObject.SetActive(false);

        return Bullets[c];
    }

    IEnumerator Iframes() 
    {
        yield return new WaitForSeconds(Itime);//Wait for Itim and then make player vulnerable
        InVulnerable = false;
    }
    IEnumerator Shoot()  //I sure hope this works, I hav NO idea if it will tho
    {
        while(!stoppedShooting) yield return new WaitForEndOfFrame();        
        while (Shooting) 
        {
        stoppedShooting = false;
            shoot();
            yield return new WaitForSeconds(data.TearDelay);
            stoppedShooting = true;
        }
    }
    private void Start()
    {
        if (Data.HasData)
            data = Data.d;
        t.text = "Health: " + data.Health;
        rb = GetComponent<Rigidbody>();
        Bullets = new List<Bullet>();
        for (int i = 0; i < bulletInstCount; i++) 
        {
            Bullets.Add(Instantiate(bullet,bParrent).GetComponent<Bullet>());
                Bullets[i].gameObject.SetActive(false); 
        }

    }
    public void TakeDamage(int dmg = 1)
    {
        if (!InVulnerable)
        {
            InVulnerable = true;
            data.Health -= dmg;
            t.text = "Health: " + data.Health;
            if (data.Health <= 0) Death();
            OnDamage.Invoke();
          if(gameObject.activeInHierarchy)
                StartCoroutine(Iframes());
            

        }
    }
   public void NextLevel() 
    {
        Data.HasData = true;
        Data.d = data;
        data.Score++;
        SeedSetting.Depth++;
        SeedSetting.seed++;
        SceneManager.LoadGame(SeedSetting.Depth);
    }

    void shoot()
    {
        OnShoot.Invoke();
        var b = GetBullet();
        b.gameObject.SetActive(true);
        b.dmg = data.Damage;
        b.transform.position = transform.position;
        b.c.localScale *= data.Damage / 3.5f;
        b.rb.velocity = ShootingDir * data.ShotSpeed * tearCNST;
          b.StartCoroutine(b.Life(data.Range / data.ShotSpeed));
        
      
    }

    void Update()
    {
        Shooting = Input.GetKey(UpShoot) || Input.GetKey(DownShoot) || Input.GetKey(LeftShoot) || Input.GetKey(RightShoot);
        //Fix these
        if (Input.GetKeyDown(UpShoot))ShootingDir =  Vector3.forward;
        if (Input.GetKeyDown(DownShoot))ShootingDir = Vector3.back;
        if (Input.GetKeyDown(LeftShoot))ShootingDir = Vector3.left;
        if (Input.GetKeyDown(RightShoot)) ShootingDir = Vector3.right ;
    }

    void FixedUpdate()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        Vector3 force = new Vector3(h, 0, v) * data.Speed * speedCnst;
        //rb.AddForce(force);
        rb.velocity = force;
    }
}
