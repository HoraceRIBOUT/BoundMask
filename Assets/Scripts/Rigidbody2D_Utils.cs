using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rgdbd_Utils
{
    public static void ChangeVelocityXTo(Rigidbody2D _rgbd, float xVal)
    {
        ChangeVelocityTo(_rgbd, xVal, _rgbd.velocity.y);
    }
    public static void ChangeVelocityYTo(Rigidbody2D _rgbd, float yVal)
    {
        ChangeVelocityTo(_rgbd, _rgbd.velocity.x, yVal);
    }
    public static void ChangeVelocityTo(Rigidbody2D _rgbd, float xVal, float yVal)
    {
        _rgbd.velocity = new Vector2(xVal, yVal);
        
    }

}
