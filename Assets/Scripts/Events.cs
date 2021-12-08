using UnityEngine;
using UnityEngine.Events;

public class Events : MonoBehaviour
{
    public UnityEvent OnPlayerCollision;  
    public UnityEvent OnPlayerTrigger;
    public UnityEvent OnCollision;
    public UnityEvent OnTrigger;
    public Player player;
    public void NextLevel() =>player.NextLevel();
    public void HealPlayer(int h) => player.TakeDamage(-h);
    private void Start() 
    { 
        var o = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < o.Length; i++)
        {
            player = o[i].GetComponent<Player>();
            if (player != null) { Debug.Log("Found player at " + i); return; }
            
        }
        Debug.Log(player == null);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            OnPlayerCollision.Invoke();
        else
            OnCollision.Invoke();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            OnPlayerTrigger.Invoke();
        else
            OnTrigger.Invoke();
    }
}