using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebound : MonoBehaviour
{
    public Vector2 reboundForce = new Vector2(15,3);

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision = " + collision.gameObject.name);
        PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();
        if (player != null)
        {
            Boing(player);
        }
    }

    protected virtual void Boing(PlayerManager player)
    {
        //            Debug.Log("Boing");
        player.Rebound(reboundForce);
    }
}
