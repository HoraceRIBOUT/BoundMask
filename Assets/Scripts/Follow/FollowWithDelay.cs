using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWithDelay : MonoBehaviour
{
    public Transform target;
    [Range(0,1)]
    public float delay = 0.5f;
    public float distanceMax = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(distanceMax != -1)
        {
            if(distanceMax < (target.transform.position - transform.position).magnitude)
            {
                this.transform.position = target.position + (this.transform.position - target.position) * distanceMax;
                return;
            }
        }

        this.transform.position = Vector3.Lerp(this.transform.position, target.position, delay);
    }
}
