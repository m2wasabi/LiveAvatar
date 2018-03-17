using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthController : MonoBehaviour
{
    private SkinnedMeshRenderer FaceA;

    private int blendShapeCount;
    private Mesh skinnedMesh;

    [Range(0, 100)]
    public float MouthSmile = 0;

    [Range(0, 100)]
    public float MouthSurprize = 0;

    [Range(0, 100)]
    public float MouthSad = 0;

    [Range(0, 100)]
    public float MouthAnger3 = 0;

    [Range(0, 100)]
    public float MouthAnger2 = 0;

    [Range(0, 100)]
    public float MouthAnger1 = 0;

    public bool MouthCapture = true;

    public enum Emotion
    {
        Smile,
        Surprize,
        Sad,
        Ang3,
        Ang2,
        Ang1
    }

    public Emotion MouthEmotion = Emotion.Smile;

    [Range(0,100)]
    public float MouthOpen = 0;

    void Awake()
    {
        FaceA = GetComponent<SkinnedMeshRenderer>();
        skinnedMesh = FaceA.sharedMesh;
    }
    // Use this for initialization
    void Start ()
    {
        blendShapeCount = skinnedMesh.blendShapeCount;
    }

    // Update is called once per frame
    void Update () {
        if (MouthCapture)
        {
            UpdateMouthCapture();
        }
        else
        {
            UpdateBrend();
        }
    }

    void UpdateBrend()
    {
        FaceA.SetBlendShapeWeight(0, MouthSmile);
        FaceA.SetBlendShapeWeight(1, MouthSurprize);
        FaceA.SetBlendShapeWeight(2, MouthSad);
        FaceA.SetBlendShapeWeight(3, MouthAnger3);
        FaceA.SetBlendShapeWeight(4, MouthAnger2);
        FaceA.SetBlendShapeWeight(5, MouthAnger1);
    }
    void UpdateMouthCapture()
    {
        for (var i = 0; i < blendShapeCount; i++)
        {
            FaceA.SetBlendShapeWeight(i, 0);
        }
        switch (MouthEmotion)
        {
            case Emotion.Smile:
                FaceA.SetBlendShapeWeight(0, MouthOpen);
                break;
            case Emotion.Surprize:
                FaceA.SetBlendShapeWeight(4, MouthOpen / 2);
                FaceA.SetBlendShapeWeight(1, MouthOpen);
                break;
            case Emotion.Sad:
                FaceA.SetBlendShapeWeight(1, MouthOpen);
                FaceA.SetBlendShapeWeight(2, 100);
                break;
            case Emotion.Ang3:
                FaceA.SetBlendShapeWeight(3, 50);
                FaceA.SetBlendShapeWeight(4, MouthOpen * 4 / 5);
                break;
            case Emotion.Ang2:
                FaceA.SetBlendShapeWeight(4, MouthOpen);
                break;
            case Emotion.Ang1:
                FaceA.SetBlendShapeWeight(4, MouthOpen);
                FaceA.SetBlendShapeWeight(5, 100 - MouthOpen);
                break;
        }
    }

}
