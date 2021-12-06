using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : Enemy
{
    private void FixedUpdate()
    {
        if(Active)
        {
            //just go to the player
            var dir = room.player.position - transform.position;
            dir.Normalize();
            rb.velocity = dir * Speed;
        }    
    }
}
