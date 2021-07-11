using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecEmetter : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Ok, trigger " + collision.name);
        ElecReceiver elec = collision.GetComponent<ElecReceiver>();
        if(elec != null)
        {
            //Trigger when electrecity
            //if(electrecity)
            elec.ElecReceiving();
        }
    }
}
