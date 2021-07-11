using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecEmetter : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Ok, " + this.gameObject.name + " trigger " + collision.name, collision.gameObject);
        ElecReceiver elec = collision.GetComponent<ElecReceiver>();
        if(elec != null)
        {
            //Trigger when electrecity
            //if(electrecity)
            elec.ElecReceiving();
        }
    }
}
