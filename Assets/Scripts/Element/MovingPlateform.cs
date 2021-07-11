using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlateform : MonoBehaviour
{
    public List<Transform> pointToGoTo = new List<Transform>(2);
    public float timingBetween = 1f;
    public float pauseOnEdge = 1f;

    Coroutine cyclingOn = null;

    //Add a change of parent (if jump == parent null)


    private void Start()
    {
        Activate();
    }

    void Activate()
    {
        if (cyclingOn != null)
            return;

        cyclingOn = StartCoroutine(Cycling());
    }

    void Deactivate()
    {
        if (cyclingOn == null)
            return;

        StopCoroutine(cyclingOn);
    }

    public IEnumerator Cycling()
    {
        float timingInEachPose = pointToGoTo.Count / timingBetween;
        while (true)
        {
            float lerp = 0;
            yield return new WaitForSeconds(pauseOnEdge);
            while (lerp < pointToGoTo.Count - 1)
            {
                int step = (int)lerp;
                this.transform.position = Vector3.Lerp(pointToGoTo[step].position, pointToGoTo[step + 1].position, lerp - step);
                lerp += Time.deltaTime * timingInEachPose;
                yield return new WaitForSeconds(1 / 60);
            }
            lerp = pointToGoTo.Count - 1;
            this.transform.position = pointToGoTo[(int)lerp].position;
            yield return new WaitForSeconds(pauseOnEdge);
            while (lerp > 0)
            {
                lerp -= Time.deltaTime * timingInEachPose;
                if (lerp < 0)
                    lerp = 0;
                int step = (int)lerp;
                this.transform.position = Vector3.Lerp(pointToGoTo[step].position, pointToGoTo[step + 1].position, lerp - step);
                yield return new WaitForSeconds(1 / 60);
            }
        }

    }
}
