using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleElec : MonoBehaviour
{
    public float duration = 0.4f;
    public AnimationCurve elecCurve = AnimationCurve.Linear(0, 1, 1, 0);

    public Transform transf;

    public void Start()
    {
        StartCoroutine(ElecLife());
    }

    public IEnumerator ElecLife()
    {
        float lerp = 0;
        while(lerp < duration)
        {
            lerp += Time.deltaTime;
            if (transf == null)
                this.transform.localScale = Vector3.one * elecCurve.Evaluate(lerp / duration);
            else
                transf.localScale = Vector3.one * elecCurve.Evaluate(lerp / duration);

            yield return new WaitForSeconds(1f / 60f);
        }
    }

}
