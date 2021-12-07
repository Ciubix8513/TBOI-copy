using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{

    public float dmg;
    public Rigidbody rb;
    public Transform c;
    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            return;
        }
        var e = collision.gameObject.GetComponent<Enemy>();
        e.Health -= dmg;
        gameObject.SetActive(false);



    }

    public IEnumerator Life(float time) 
    {
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(false);
    }

}

