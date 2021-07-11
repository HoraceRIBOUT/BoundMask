using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPlateform : ElecReceiver
{
    public List<GameObject> colliderOff = new List<GameObject>();
    public List<GameObject> colliderOn  = new List<GameObject>();

    public float delay = 1f;

    Coroutine timer = null;

    public void Start()
    {
        Close();
    }

    public override void ElecReceiving()
    {
        Debug.Log("Ok, trigger ");
        //Trigger when electrecity
        //if(electrecity)
        Open();
    }




    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Ok, trigger ");
        //Trigger when electrecity
        //if(electrecity)
        Open();
    }

    public void Open()
    {
        foreach (GameObject gO in colliderOff)
            gO.SetActive(false);
        foreach (GameObject gO in colliderOn)
            gO.SetActive(true);

        timer = StartCoroutine(Timing(delay));
    }

    public IEnumerator Timing(float timingDelay)
    {
        yield return new WaitForSeconds(delay);
        timer = null;
        Close();
    }

    public void Close()
    {
        foreach (GameObject gO in colliderOff)
            gO.SetActive(true);
        foreach (GameObject gO in colliderOn)
            gO.SetActive(false);
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }
    }

}
