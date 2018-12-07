using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SelfManagementOfTrackedDevices : MonoBehaviour
{
    public GameObject[] targetObjs;
    public ETrackedDeviceClass targetClass = ETrackedDeviceClass.GenericTracker;
    public KeyCode resetDeviceIds = KeyCode.Tab;
    public Transform origin;

    CVRSystem _vrSystem;
    [SerializeField]
    List<int> _validDeviceIds = new List<int>();

    void Awake()
    {
        var error = EVRInitError.None;
        _vrSystem = OpenVR.Init(ref error, EVRApplicationType.VRApplication_Other);

        if (error != EVRInitError.None) { Debug.LogWarning("Init error: " + error); }

        else
        {
            Debug.Log("init done");
            foreach (var item in targetObjs) { item.SetActive(false); }
            SetDeviceIds();
        }
    }

    void SetDeviceIds()
    {
        _validDeviceIds.Clear();
        for (uint i = 0; i < OpenVR.k_unMaxTrackedDeviceCount; i++)
        {
            var deviceClass = _vrSystem.GetTrackedDeviceClass(i);
            if (deviceClass != ETrackedDeviceClass.Invalid && deviceClass == targetClass)
            {
                Debug.Log("OpenVR device at " + i + ": " + deviceClass);
                _validDeviceIds.Add((int)i);
                var idx = _validDeviceIds.Count - 1;
                targetObjs[idx].SetActive(true);
                var to = targetObjs[idx].GetComponent<SteamVR_TrackedObject>();
                if (to != null) { to.index = (SteamVR_TrackedObject.EIndex)i; }
            }
        }
    }

    void UpdateTrackedObj()
    {
        TrackedDevicePose_t[] allPoses = new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount];

        _vrSystem.GetDeviceToAbsoluteTrackingPose(ETrackingUniverseOrigin.TrackingUniverseStanding, 0, allPoses);

        for (int i = 0; i < _validDeviceIds.Count; i++)
        {
            if (i < targetObjs.Length)
            {
                var pose = allPoses[_validDeviceIds[i]];
                var absTracking = pose.mDeviceToAbsoluteTracking;
                var mat = new SteamVR_Utils.RigidTransform(absTracking);
                if (origin != null)
                {
                    targetObjs[i].transform.SetPositionAndRotation(origin.TransformPoint(mat.pos), origin.rotation * mat.rot);
                }
                else
                {
                    targetObjs[i].transform.SetPositionAndRotation(mat.pos, mat.rot);
                }
            }
        }
    }

    void Update()
    {
        UpdateTrackedObj();

        if (Input.GetKeyDown(resetDeviceIds))
        {
            SetDeviceIds();
        }
    }
}
