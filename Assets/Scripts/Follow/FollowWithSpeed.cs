using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWithSpeed : MonoBehaviour
{
    public Transform target;
    public Vector3 speed;

    public float epsilon = 0.01f;
    public float maxDist = 2f;

    public Vector3 maxSpeed = Vector3.one;
    public float ratio = 5;
    [Range(0, 1)]
    public float damping = 0.2f;

    public void Update()
    {
        if (target.position == this.transform.position)
            return;

        Vector3 diff = (target.position - this.transform.position);
        float distSqr = diff.sqrMagnitude;

        if (Mathf.Abs(speed.sqrMagnitude) < epsilon * epsilon 
            && Mathf.Abs(distSqr) < epsilon * epsilon)
        {
            this.transform.position = target.position;
            speed = Vector3.zero;
            return;
        }

        if (distSqr > maxDist * maxDist)
        {
            //Do something
            Vector3 pos = (this.transform.position - target.position);
            this.transform.position = target.position + (pos.normalized * maxDist);
        }
        else
        {
            //Apply speed
            this.transform.position += speed * Time.deltaTime;
        }

        //Compute speed

        speed += diff / ratio;
        speed *= damping;


        speed.x = Mathf.Clamp(speed.x, -maxSpeed.x, maxSpeed.x);
        speed.y = Mathf.Clamp(speed.y, -maxSpeed.y, maxSpeed.y);
    }
}
