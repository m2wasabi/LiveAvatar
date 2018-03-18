using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesController : MonoBehaviour
{

    private SkinnedMeshRenderer skinnedMeshRenderer;

    [Range(-100, 100)]
    public float LookAtHolizontal;

    [Range(-100, 100)]
    public float LookAtVertical;

    [Range(0, 100)]
    public float EmoAnger;

    [Range(0, 100)]
    public float EmoSad;

    [Range(0, 100)]
    public float EmoSurprize;

    public bool EmoShirome = false;

    public bool EyeTrace = false;

    public GameObject TraceTarget;
    private GameObject GuideTarget;

    void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        GuideTarget = transform.parent.gameObject.transform.Find("HeadEnd_M/Hair_Root").gameObject;
    }

    void Update()
    {
        if (EyeTrace && TraceTarget != null)
        {
            LookAtTraceObject();
        }
        LookAt(LookAtHolizontal, LookAtVertical);
        UpdateEmoLevel();
        if(EmoShirome) Shirome();
    }

    void LookAtTraceObject()
    {
        var LookWay = TraceTarget.transform.position - GuideTarget.transform.position;
        LookWay = LookWay.normalized;
        var localLookAt = GuideTarget.transform.worldToLocalMatrix *  LookWay;
        // 目線が 外すぎる or 背後に回った場合 目で追いかけない
        if (localLookAt.z < 0 || Mathf.Abs(localLookAt.x) + Mathf.Abs(localLookAt.y) * 1.5 > 1.2)
        {
            LookAtHolizontal = 0;
            LookAtVertical   = 0;
        }
        else
        {
            LookAtHolizontal = -localLookAt.x * 150;
            LookAtVertical   =  localLookAt.y * 100;
        }
    }
    void LookAt(float horizontal, float vertical)
    {
        if (horizontal == 0)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(0, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(1, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(4, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(5, 0);
        }
        else if (horizontal > 0)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(0,horizontal);
            skinnedMeshRenderer.SetBlendShapeWeight(1, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(4, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(5, 0);
        }
        else if(horizontal < 0)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(0, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(1, - horizontal);
            skinnedMeshRenderer.SetBlendShapeWeight(4, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(5, 0);
        }

        if (vertical == 0)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(2, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(3, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(6, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(7, 0);
        }
        else if (vertical > 0)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(2, vertical);
            skinnedMeshRenderer.SetBlendShapeWeight(3, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(6, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(7, 0);
        }
        else if (vertical < 0)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(2, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(3, -vertical);
            skinnedMeshRenderer.SetBlendShapeWeight(6, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(7, 0);
        }
    }

    void UpdateEmoLevel()
    {
        skinnedMeshRenderer.SetBlendShapeWeight(8, EmoAnger);
        skinnedMeshRenderer.SetBlendShapeWeight(9, EmoSad);
        skinnedMeshRenderer.SetBlendShapeWeight(10, EmoSurprize);
    }
    void Shirome()
    {
        skinnedMeshRenderer.SetBlendShapeWeight(0, 100);
        skinnedMeshRenderer.SetBlendShapeWeight(1, 0);
        skinnedMeshRenderer.SetBlendShapeWeight(2, 0);
        skinnedMeshRenderer.SetBlendShapeWeight(3, 0);
        skinnedMeshRenderer.SetBlendShapeWeight(4, 100);
        skinnedMeshRenderer.SetBlendShapeWeight(5, 0);
        skinnedMeshRenderer.SetBlendShapeWeight(6, 0);
        skinnedMeshRenderer.SetBlendShapeWeight(7, 0);
    }
}
