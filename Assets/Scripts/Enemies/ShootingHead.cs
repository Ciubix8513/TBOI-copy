using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingHead : Enemy
{
    [SerializeField]
    Transform bParrent;
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    int bulletInstCount = 1000;
    [SerializeField]
    List<Bullet> Bullets;

    [SerializeField]
    float spCNST = 2.0f;
    [SerializeField]
    float BulletDelay = 1.0f;
    [SerializeField]
    float Range = 4.0f;
    [SerializeField]
    float ShotSpeed = 2.0f;
    [SerializeField]
    int dmg = 1;

    bool s;
    bool Shooting { get { return s; } set { s = value; if (value) StartCoroutine(Attack()); else StopCoroutine(Attack()); } }
    bool stoppedShooting = true;

    private void Start()
    {
        Bullets = new List<Bullet>();
        for (int i = 0; i < bulletInstCount; i++)
        {
            Bullets.Add(Instantiate(bullet, bParrent).GetComponent<Bullet>());
            Bullets[i].gameObject.SetActive(false);
        }
    }
    IEnumerator Attack() 
    {
        while (!stoppedShooting)yield return new WaitForEndOfFrame();
        while (Shooting)
        {
            stoppedShooting = false;
            shoot();
            yield return new WaitForSeconds(BulletDelay);
            stoppedShooting = true;
        }
    }
    Bullet GetBullet()
    {
        for (int i = 0; i < Bullets.Count; i++)
            if (!Bullets[i].gameObject.activeInHierarchy)
                return Bullets[i];
        Bullets.Add(Instantiate(bullet, bParrent).GetComponent<Bullet>());
        var c = Bullets.Count - 1;
        Bullets[c].gameObject.SetActive(false);

        return Bullets[c];
    }
    void shoot() 
    {
        var dir = room.player.position - transform.position;
        dir.Normalize();
        var b = GetBullet();
        b.gameObject.SetActive(true);
        b.dmg = dmg;
        b.transform.position = transform.position;
        b.rb.velocity = dir * ShotSpeed * spCNST;
        b.StartCoroutine(b.Life(Range / ShotSpeed));

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Shooting = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            Shooting = false;
    }
}
