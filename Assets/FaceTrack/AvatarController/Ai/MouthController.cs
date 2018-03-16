using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthController : MonoBehaviour
{
    private SkinnedMeshRenderer FaceA;

    private int blendShapeCount;
    private Mesh skinnedMesh;


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
        FaceA.SetBlendShapeWeight(0,MouthOpen);
    }
}
