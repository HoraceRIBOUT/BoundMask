using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebound_Temp : Rebound
{
    public List<GameObject> gameObjectOn;
    public List<GameObject> gameObjectOff;

    public float timingBeforeReactivate = 5f;

    protected override void Boing(PlayerManager player)
    {
        base.Boing(player);

        Deactivate();
    }

    public void Deactivate()
    {
        foreach (GameObject gO in gameObjectOn)
            gO.SetActive(false);
        foreach (GameObject gO in gameObjectOff)
            gO.SetActive(true);

        StartCoroutine(WaitToReactivate());
    }

    public IEnumerator WaitToReactivate()
    {
        yield return new WaitForSeconds(timingBeforeReactivate);
        Reactivate();
    }

    public void Reactivate()
    {
        foreach (GameObject gO in gameObjectOn)
            gO.SetActive(true);
        foreach (GameObject gO in gameObjectOff)
            gO.SetActive(false);
    }
}
