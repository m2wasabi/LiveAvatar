/**
FinalIKを使ったLeapMotion Orion用HandController
 (VRIKバージョン)
Author: MiyuMiyu
*/

using UnityEngine;
using Leap.Unity;
using RootMotion.FinalIK;

public class FinalIKOrionLeapHandController : LeapHandController
{
    [SerializeField]
    private VRIK vrIK;

    public bool ikActive = true;
    public bool leftActive = false;
    public bool rightActive = false;

    public GameObject avatarLeftHand;   //keeps track of the left hand gameobject for IK
    public GameObject avatarRightHand;  //keeps track of the right hand gameobject for IK
    [HideInInspector]
    public HandModel leftHand;
    [HideInInspector]
    public HandModel rightHand;

    [HideInInspector]
    public UpdateDuplicatedTransformRecursive leftTransform;
    [HideInInspector]
    public UpdateDuplicatedTransformRecursive rightTransform;


    protected virtual void Awake()
    {
        if (vrIK == null)
        {
            vrIK = gameObject.transform.root.GetComponent<VRIK>();
        }
        if (vrIK == null)
        {
            Debug.LogError("FinalIKOrionLeapHandController:: no FullBodyBipedIK found on GameObject or any of its parent transforms. ");
        }

        if (leftHand == null && avatarLeftHand != null)
            leftHand = avatarLeftHand.GetComponent<RiggedHand>();
        if (rightHand == null && avatarRightHand != null)
            rightHand = avatarRightHand.GetComponent<RiggedHand>();
        if (leftTransform == null && avatarLeftHand != null)
            leftTransform = avatarLeftHand.GetComponent<UpdateDuplicatedTransformRecursive>();
        if (rightTransform == null && avatarRightHand != null)
            rightTransform = avatarRightHand.GetComponent<UpdateDuplicatedTransformRecursive>();

        if (leftHand == null)
            Debug.LogError("IKOrionLeapHandController::Awake::No Rigged Hand set for left hand parameter. You have to set this in the inspector.");
        if (rightHand == null)
            Debug.LogError("IKOrionLeapHandController::Awake::No Rigged Hand set for right hand parameter. You have to set this in the inspector.");

        // Physic Handは使用しないのでDisableにする
        physicsEnabled = false;
    }

    //    protected override void Start()
    protected virtual void Start()
    {
        provider = GetComponent<LeapProvider>();
        if (provider == null)
        {
            Debug.LogError("IKOrionLeapHandController::Start::No Leap Provider component was present on " + gameObject.name);
            Debug.Log("Added a Leap Service Provider with default settings.");
            gameObject.AddComponent<LeapServiceProvider>();
        }
    }

    public void FixedUpdate()
    {
        if (graphicsEnabled)
        {
            UpdateHandRepresentations();

            if (ikActive)
            {
                if (leftActive && leftHand != null)
                {
                    RiggedHand l = leftHand as RiggedHand;

                    vrIK.solver.leftArm.IKPosition = leftHand.GetPalmPosition();
                    vrIK.solver.leftArm.IKRotation = leftHand.GetPalmRotation() * l.Reorientation();
                    vrIK.solver.leftArm.positionWeight = 1.0f;
                    vrIK.solver.leftArm.rotationWeight = 0.0f;
                    leftTransform.Enabled = true;
                }
                else
                {
                    vrIK.solver.leftArm.positionWeight = 0.0f;
                    vrIK.solver.leftArm.rotationWeight = 0.0f;
                    leftTransform.Enabled = false;
                }

                if (rightActive && rightHand != null)
                {
                    RiggedHand r = rightHand as RiggedHand;

                    vrIK.solver.rightArm.IKPosition = rightHand.GetPalmPosition();
                    vrIK.solver.rightArm.IKRotation = rightHand.GetPalmRotation() * r.Reorientation();
                    vrIK.solver.rightArm.positionWeight = 1.0f;
                    vrIK.solver.rightArm.rotationWeight = 0.0f;
                    rightTransform.Enabled = true;
                }
                else
                {
                    vrIK.solver.rightArm.positionWeight = 0.0f;
                    vrIK.solver.rightArm.rotationWeight = 0.0f;
                    rightTransform.Enabled = false;
                }
            }
        }
    }

    /// <summary>
    /// Tells the hands to update to match the new Leap Motion hand frame data. Also keeps track of
    /// which hands are currently active.
    /// </summary>
    void UpdateHandRepresentations()
    {
        leftActive = false;
        rightActive = false;
        foreach (Leap.Hand curHand in provider.CurrentFrame.Hands)
        {
            if (curHand.IsLeft)
            {
                leftHand.SetLeapHand(curHand);
                leftHand.UpdateHand();
                leftActive = true;
            }
            if (curHand.IsRight)
            {
                rightHand.SetLeapHand(curHand);
                rightHand.UpdateHand();
                rightActive = true;
            }
        }
    }
}