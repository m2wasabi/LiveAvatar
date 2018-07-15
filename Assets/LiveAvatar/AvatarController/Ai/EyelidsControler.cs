using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyelidsControler : MonoBehaviour
{

    private SkinnedMeshRenderer _faceB;
    private SkinnedMeshRenderer _lash;
    private SkinnedMeshRenderer _brow;

    [Range(0, 100)]
    public float BrowSmile = 0;

    [Range(0, 100)]
    public float BrowSurprize = 0;

    [Range(0, 100)]
    public float BrowSad = 0;

    [Range(0, 100)]
    public float BrowAnger3 = 0;

    [Range(0, 100)]
    public float BrowAnger2 = 0;

    [Range(0, 100)]
    public float BrowAnger1 = 0;

    [Range(0, 100)]
    public float EyeSmile = 0;

    [Range(0, 100)]
    public float EyeSurprize = 0;

    [Range(0, 100)]
    public float EyeSad = 0;

    [Range(0, 100)]
    public float EyeAnger3 = 0;

    [Range(0, 100)]
    public float EyeAnger2 = 0;

    [Range(0, 100)]
    public float EyeAnger1;

    [Range(0, 100)]
    public float EyeClose;

    void Awake()
    {
        _faceB = transform.Find("FaceB").transform.GetComponent<SkinnedMeshRenderer>();
        _lash = transform.Find("FaceB/Lash").transform.GetComponent<SkinnedMeshRenderer>();
        _brow = transform.Find("Brow").transform.GetComponent<SkinnedMeshRenderer>();

    }

    void Update()
    {
        UpdateBrows();
        UpdateEyelids();
    }

    void SetWeightEyelids(int index, float value)
    {
        _faceB.SetBlendShapeWeight(index, value);
        _lash.SetBlendShapeWeight(index, value);
    }

    void UpdateBrows()
    {
        _brow.SetBlendShapeWeight(0, BrowSmile);
        _brow.SetBlendShapeWeight(1, BrowSurprize);
        _brow.SetBlendShapeWeight(2, BrowSad);
        _brow.SetBlendShapeWeight(3, BrowAnger3);
        _brow.SetBlendShapeWeight(4, BrowAnger2);
        _brow.SetBlendShapeWeight(5, BrowAnger1);
    }
    void UpdateEyelids()
    {
        SetWeightEyelids(0, EyeSmile);
        SetWeightEyelids(1, EyeSurprize);
        SetWeightEyelids(2, EyeSad);
        SetWeightEyelids(3, EyeAnger3);
        SetWeightEyelids(4, EyeAnger2);
        SetWeightEyelids(5, EyeAnger1);
        SetWeightEyelids(6, EyeClose);
    }
}
