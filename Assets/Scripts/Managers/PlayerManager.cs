using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerManager : MonoBehaviour
{
    [Header("Mapping")]
    [FoldoutGroup("Mapping")]
    public string moveXAxisName = "Horizontal";
    [FoldoutGroup("Mapping")]
    public string moveYAxisName = "Vertical";
    [FoldoutGroup("Mapping")]
    public List<KeyCode> jumpKey = new List<KeyCode> { KeyCode.Z        , KeyCode.UpArrow       };
    [FoldoutGroup("Mapping")]
    public List<KeyCode> dropKey = new List<KeyCode> { KeyCode.S        , KeyCode.DownArrow     };
    [FoldoutGroup("Mapping")]
    public List<KeyCode> dashKey = new List<KeyCode> { KeyCode.Space    };
    [FoldoutGroup("Mapping")]
    public List<KeyCode> skipKey = new List<KeyCode> { KeyCode.Return   };


    [Header("Upgrades")]
    [FoldoutGroup("Upgrades")]
    public bool refuelBattery_Upgrade = false;

    [Header("Battery")]
    public float currentBattery = 1;
    public float maxBattery = 1;

    [Header("Movement")]
    [FoldoutGroup("Movement")]
    public float verticalSpeedDelta = 1;
    [FoldoutGroup("Movement")]
    public float verticalAerialSpeedDelta = 1;
    [FoldoutGroup("Movement")]
    public float verticalSpeedMax = 200;
    [FoldoutGroup("Movement")]
    public float maximalDownSpeed = -5;
    [FoldoutGroup("Movement")]
    public float dragValue = 0.05f;
    [FoldoutGroup("Movement")]
    public float dragAerialValue = 0.01f;

    public bool directionRight = true;
    public bool onTheAir = true;

    [FoldoutGroup("Movement")]
    public float positionOfYourFeet = 0;

    [FoldoutGroup("Movement")]
    public float basicGravity = 1;
    [FoldoutGroup("Movement")]
    public float fallGravity = 2;

    [Header("Jump")]
    [FoldoutGroup("Jump")]
    public Vector2 jumpForce = new Vector2(0,5);
    [FoldoutGroup("Jump")]
    public GameObject jumpElecGO;
    [Header("Dash")]
    [FoldoutGroup("Dash")]
    public Vector2 dashForce = new Vector2(5,0);
    [FoldoutGroup("Dash")]
    public List<GameObject> dashElecGO = new List<GameObject>();
    [FoldoutGroup("Dash")]
    public float dashDuration = 1f;
    [FoldoutGroup("Dash")]
    public bool dashOn = false;
    [Header("Drop")]
    [FoldoutGroup("Drop")]
    public bool dropOn = false;
    [FoldoutGroup("Drop")]
    public Vector2 dropForce = new Vector2(0, -3);
    [Header("Skip")]
    [Range(0,1)]
    [FoldoutGroup("Skip")]
    public float skipBattery = 1;
    [FoldoutGroup("Skip")]
    public bool skipOn = false;
    
    [Header("Part")]
    public Rigidbody2D _rgbd;
    public AnimationHandler animationHandler;

    [Header("Debug")]
    public GameObject debugTriangleRight;
    public GameObject debugTriangleLeft;

    // Start is called before the first frame update
    void Start()
    {
        UpdateBatteryUI();
    }

    // Update is called once per frame
    void Update()
    {
        InputManagement();
    }

    public void InputManagement()
    {
        float xVal = Input.GetAxisRaw(moveXAxisName);
        float yVal = Input.GetAxisRaw(moveYAxisName);

        //maybe add "yVal" in "canJump"
        if (CanJump() && Input_Utils.GetKeyDown(jumpKey))
        {
            Jump(xVal);//need xVal to know if we stop the Xmomentum or not
        }

        if (CanDash() && Input_Utils.GetKeyDown(dashKey))
        {
            Dash(xVal);//to know the direction to go
        }

        if (CanSkip() && Input_Utils.GetKeyDown(skipKey))
        {
            Skip();
        }
        else if(skipOn && Input_Utils.GetKeyUp(skipKey))
        {
            QuitSkip();
        }

        //maybe add "yVal" in "canDrop"
        if (CanDrop() && Input_Utils.GetKeyDown(dropKey))
        {
            Drop();
        }
        
        MovementManagement(xVal);
    }

    #region Jump
    public bool CanJump()
    {
        return currentBattery >= 1;
    }
    public void Jump(float xVal)
    {
//        Debug.Log("Jumping");
        currentBattery -= 1; //use one full battery 
        UpdateBatteryUI();

        Instantiate(jumpElecGO, this.transform.position, Quaternion.identity, null);

        onTheAir = true;

        if (dashOn)
            QuitDash();

        _StopYMove();
        //If : the player press the opposite direction while pressing jump, stop the X move
        if (xVal != 0)
        {
            if (directionRight != (xVal > 0))
            {
                _StopXMove();
            }
        }
        _rgbd.AddForce(jumpForce, ForceMode2D.Impulse);
        
    }
    #endregion

    #region Dash
    public bool CanDash()
    {
        return !dashOn && currentBattery > 0;
    }

    public void Dash(float xValPress)
    {
        dashOn = true;
        bool right = (xValPress == 0 ? directionRight : (xValPress > 0));//we check if the player press a direction. Else, the last direction inputed.
        (right ? debugTriangleRight : debugTriangleLeft).SetActive(true);

        if (skipOn)
            QuitSkip();
        if (dropOn)
            QuitDrop();

        currentBattery -= 1;
        UpdateBatteryUI();

        foreach (GameObject gO in dashElecGO)
        {
            gO.SetActive(true);
        }

        _StopMove();
        _rgbd.AddForce(dashForce * (right ? 1 : -1), ForceMode2D.Impulse);
        //_ChangeXMove(dashForce * (right ? 1 : -1));
        _rgbd.gravityScale = 0;

        StartCoroutine(DashTimer(dashDuration));
    }

    public IEnumerator DashTimer(float duration)
    {
        float timer = 0;
        while (timer < duration )
        {
            if (!dashOn)
            {
                //Abort timer
                yield break;
            }
            timer += Time.deltaTime;
            yield return new WaitForSeconds(1 / 60f);
        }
        QuitDash();
    }

    public void QuitDash()
    {
        dashOn = false;
        debugTriangleRight.SetActive(false);
        debugTriangleLeft.SetActive(false);
        _rgbd.gravityScale = basicGravity;
        Debug.Log("Dash end : "+_rgbd.velocity);

        foreach(GameObject gO in dashElecGO)
        {
            gO.SetActive(false);
        }

        //Update onTheAir before
        if (!onTheAir)
        {
            OnGround();
        }

    }
    #endregion

    #region Stop
    public bool CanSkip()
    {
        //need to add a delay after a rebound (can't skip too close to a rebound)
        //+ reboun quitSkip
        return !skipOn && currentBattery > 1;
    }
    public void Skip()
    {
        if (dashOn)
            QuitDash();
        if(dropOn)
            QuitDrop();

        currentBattery -= 1;
        UpdateBatteryUI();

        _StopMove();
        _rgbd.gravityScale = 0;
        skipOn = true;
        //add a timer
    }
    public void QuitSkip()
    {
        skipOn = false;
        _rgbd.gravityScale = basicGravity;
    }
    #endregion

    #region Drop
    public bool CanDrop()
    {
        return !dropOn && onTheAir;
    }
    public void Drop()
    {
        if (dashOn)
            QuitDash();

        if (dropOn)
            QuitDrop();

        _StopMove();
        _rgbd.AddForce(dropForce, ForceMode2D.Impulse);

        dropOn = true;
        //add a timer
    }
    public void QuitDrop()
    {
        dropOn = false;
        _rgbd.gravityScale = basicGravity;
    }
    #endregion

    public void MovementManagement(float xVal)
    {
        if (skipOn)
            return;

        if (dashOn)
        {
            //just don't slow down
            //_ChangeXMove(dashForce * (directionRight ? 1 : -1));
            return;
        }

        if (dropOn)
        {
            //just don't slow down
            //_ChangeXMove(dashForce * (directionRight ? 1 : -1));
            return;
        }

        if (xVal > 0)
        {
            float newSpeed = Mathf.Min(_rgbd.velocity.x + (onTheAir ? verticalAerialSpeedDelta : verticalSpeedDelta), verticalSpeedMax);
            _ChangeXMove(newSpeed);
            //Debug.Log("Move by (right) "+_rgbd.velocity.x);
        }
        else if(xVal < 0)
        {
            float newSpeed = Mathf.Max(_rgbd.velocity.x - (onTheAir ? verticalAerialSpeedDelta : verticalSpeedDelta), -verticalSpeedMax);
            _ChangeXMove(newSpeed);
            //Debug.Log("Move by (left) " + _rgbd.velocity.x);
        }
        else
        {
            //slowdown
            if (_rgbd.velocity.x != 0)
            {
                float newSpeed = GetSlowDownValue(_rgbd.velocity.x, (onTheAir? dragAerialValue : dragValue));
                _ChangeXMove(newSpeed);
            }
            //end of slowdown
        }

        GravityManagement();

        if (_rgbd.velocity.y < maximalDownSpeed)
        {
            _ChangeYMove(maximalDownSpeed);
        }


        //Visual data
        if (/*_rgbd.velocity.x*/xVal > 0)
        {
            directionRight = true;
            animationHandler.ChangeDirection(directionRight);
        }
        else if (/*_rgbd.velocity.x*/xVal < 0)
        {
            directionRight = false;
            animationHandler.ChangeDirection(directionRight);
        }
    }

    public static float GetSlowDownValue(float currentSpeed, float slowDelta)
    {
        float res = 0;
        if (Mathf.Abs(currentSpeed) < slowDelta)
            return res;

        if (currentSpeed > 0)
        {
            res = currentSpeed - slowDelta;
        }
        else
        {
            res = currentSpeed + slowDelta;
        }
        return res;
    }

    public void GravityManagement()
    {
        if (dashOn)
            return;


        if (_rgbd.velocity.y < 0 && _rgbd.gravityScale != fallGravity)
        {
            //Add gravity
            _rgbd.gravityScale = fallGravity;
        }
        else if (_rgbd.velocity.y >= 0 && _rgbd.gravityScale != basicGravity)
        {
            //Add gravity
            _rgbd.gravityScale = basicGravity;
        }
    }

    #region ToUi
    public void UpdateBatteryUI()
    {
        GameManager._instance._uiMng.UpdateBatteryUI((int)currentBattery, (int)maxBattery);
    }
    #endregion

    #region ToAnimation
    public void AnimationManagement()
    {

    }

    public void EndOutch()
    {
        animationHandler.ChangeOutch(false);
    }
    #endregion

    #region Collision
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.GetComponent<Ennemy>() != null)
        //    return;

        Rebound rebound = collision.gameObject.GetComponent<Rebound>();
        if (rebound != null) 
        {
            return;
        }

        if (collision.contacts[0].point.y < this.transform.position.y + positionOfYourFeet)
        {
            OnGround();
        }
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    public void OnGround()
    {
        if (dropOn)
            QuitDrop();

        currentBattery = maxBattery;
        UpdateBatteryUI();
        onTheAir = false;
    }

    public void Rebound(Vector2 impulse)
    {
        //Decide if goes trough or rebound ? Rebound for now
        if (dropOn)
            QuitDrop();

        Rebound(impulse, refuelBattery_Upgrade);
        //wait for the upgravde
    }

    public void Rebound(Vector2 impulse, bool refuelBattery)
    {
        if (refuelBattery)
        {
            currentBattery += 1;
            UpdateBatteryUI();
        }

        if(dashOn)
            QuitDash();
        if (skipOn)
            QuitSkip();

        animationHandler.ChangeOutch(true);

        _StopYMove();
        //+ if opposite direction, stop the XMove 
        _rgbd.AddForce(new Vector2(/*impulse.x * (directionRight ? -1 : 1)*/0, impulse.y), ForceMode2D.Impulse);

        Invoke("EndOutch", 0.4f);
    }

    #endregion

    #region Utils
    private void _StopMove()
    {
        Rgdbd_Utils.ChangeVelocityTo(_rgbd, 0, 0);
    }
    private void _StopXMove()
    {
        Rgdbd_Utils.ChangeVelocityXTo(_rgbd, 0);
    }
    private void _StopYMove()
    {
        Rgdbd_Utils.ChangeVelocityYTo(_rgbd, 0);
    }
    private void _ChangeXMove(float xVal)
    {
        Rgdbd_Utils.ChangeVelocityXTo(_rgbd, xVal);
    }
    private void _ChangeYMove(float yVal)
    {
        Rgdbd_Utils.ChangeVelocityYTo(_rgbd, yVal);
    }

    #endregion

}
