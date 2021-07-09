using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AnimationHandler : MonoBehaviour
{
    public List<Sprite> listFace = new List<Sprite>();
    public SpriteRenderer masqueDown;
    public SpriteRenderer masqueUp;
    public Transform masquePoint;

    [Header("Blob material")]
    public Material mat;
    public List<Transform> positionList = new List<Transform>();

    private void Start()
    {
        if (!Application.isPlaying)
            return;

        masqueDown.transform.SetParent(null);
    }

    public void Update()
    {
        for (int index = 0; index < positionList.Count; index++)
        {
            Vector3 positionInUV = positionList[index].localPosition;
            positionInUV.x = 1 - positionInUV.x;
            mat.SetVector("_Pos" + (index + 1), positionInUV);
        }
    }

    public void ChangeDirection(bool directionRight)
    {
        masqueDown.flipX = !directionRight;
        masqueUp.flipX = !directionRight;

        masquePoint.transform.localPosition = new Vector3((directionRight ? 1 : -1) * 0.06f, 0);
        //Invert the position OR ratioed the position local ?
        masqueUp.transform.localPosition = new Vector3((directionRight ? 1 : -1) * 0.25f, 0.1f);
    }

    public void ChangeOutch(bool on)
    {
        if (on)
        {
            masqueDown.sprite = listFace[1];
            masqueUp.sprite = listFace[1];
        }
        else
        {
            masqueDown.sprite = listFace[0];
            masqueUp.sprite = listFace[0];

        }
    }
}
