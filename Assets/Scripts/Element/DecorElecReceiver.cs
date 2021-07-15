using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorElecReceiver : ElecReceiver
{
    public Animator _ani;

    public bool definitiveActive = false;
    [Sirenix.OdinInspector.HideIf("definitiveActive")]
    public float deactivateTiming = 1f;

    private bool active = false;
    private Coroutine deactivateAfter;

    public override void ElecReceiving()
    {
        Debug.Log("Received !");
        _ani.SetBool("Elec", true);

        if (definitiveActive)
        {
            active = true;
            return;
        }
        if (active)
        {
            if (deactivateAfter != null)
            {
                StopCoroutine(deactivateAfter);
            }
        }
        else
        {
            active = true;
        }
        deactivateAfter = StartCoroutine(DeactiveAfterTiming());
    }

    public IEnumerator DeactiveAfterTiming()
    {
        yield return new WaitForSeconds(deactivateTiming);
        active = false;
        _ani.SetBool("Elec", false);
        deactivateAfter = null;
    }

}
