using UnityEngine;
using Valve.VR;

public class HandPoseController : MonoBehaviour {

    public GameObject leftHandController;
    public GameObject rightHandController;

    public FingerControlType controlType = FingerControlType.TypeA;

    public enum FingerControlType
    {
        TypeA,
        TypeB
    }

    private SteamVR_Controller.Device _device_L, _device_R;

    private Animator _animator;

    private class PoseDegreeConfig
    {
        public HandPose pose;
        public float degree;

        public PoseDegreeConfig(HandPose Pose, float Degree)
        {
            pose = Pose;
            degree = Degree;
        }
    }

    private enum HandPose
    {
        none,
        fist,
        v,
        open,
        like,
        ok,
        point,
        rock,
        otome
    }
    private enum Hand
    {
        left,
        right
    }

    private PoseDegreeConfig[] _poseDegreeConfigsTypeA = new PoseDegreeConfig[9]
    {
        new PoseDegreeConfig(HandPose.fist, 30),
        new PoseDegreeConfig(HandPose.like, 60),
        new PoseDegreeConfig(HandPose.otome, 120),
        new PoseDegreeConfig(HandPose.ok, 150),
        new PoseDegreeConfig(HandPose.open, 210),
        new PoseDegreeConfig(HandPose.rock, 240),
        new PoseDegreeConfig(HandPose.v, 300),
        new PoseDegreeConfig(HandPose.point, 330),
        new PoseDegreeConfig(HandPose.fist, 360)
    };

    private PoseDegreeConfig[,] _poseDegreeConfigsTypeB = new PoseDegreeConfig[2,5]
    {
        {
            new PoseDegreeConfig(HandPose.fist, 45),
            new PoseDegreeConfig(HandPose.otome, 135),
            new PoseDegreeConfig(HandPose.open, 225),
            new PoseDegreeConfig(HandPose.v, 315),
            new PoseDegreeConfig(HandPose.fist, 360)

        },
        {
            new PoseDegreeConfig(HandPose.like, 45),
            new PoseDegreeConfig(HandPose.ok, 135),
            new PoseDegreeConfig(HandPose.rock, 225),
            new PoseDegreeConfig(HandPose.point, 315),
            new PoseDegreeConfig(HandPose.like, 360),
        }
    };

    void Start () {
        InitAnimator();

        if (leftHandController != null)
        {
            var controller_l = leftHandController.GetComponent<SteamVR_TrackedObject>();
            _device_L = SteamVR_Controller.Input((int)controller_l.index);
        }
        if (rightHandController != null)
        {
            var controller_r = rightHandController.GetComponent<SteamVR_TrackedObject>();
            _device_R = SteamVR_Controller.Input((int)controller_r.index);
        }        
    }

    public void InitAnimator()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void SetAnimaor(Animator target)
    {
        _animator = target;
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            controlType = (controlType == FingerControlType.TypeA) ? FingerControlType.TypeB : FingerControlType.TypeA;
        }
        if (_animator != null)
        {
            if (_device_L != null)
            {
                if (_device_L.GetPress(SteamVR_Controller.ButtonMask.Grip))
                {
                    _animator.SetInteger("LeftHandPose", updateHandPose(_device_L, Hand.left));
                }
            }
            if (_device_R != null)
            {
                if (_device_R.GetPress(SteamVR_Controller.ButtonMask.Grip))
                {
                    _animator.SetInteger("RightHandPose", updateHandPose(_device_R, Hand.right));
                }
            }

        }
    }

    private int updateHandPose(SteamVR_Controller.Device device, Hand hand)
    {
        var pos = device.GetAxis();

        if (pos.magnitude < 0.5f)
        {
            return 0;
        }
        else
        {
            Vector2 zero;
            if (hand == Hand.left)
            {
                zero = new Vector2(1,0);
            }
            else
            {
                zero = new Vector2(-1, 0);
            }
            var deg = Vector2.Angle(zero, pos);
            if (pos.y < 0)
            {
                deg = 360 - deg;
            }
            Debug.Log(deg);

            if (controlType == FingerControlType.TypeA)
            {
                return getPoseByDegree(deg);
            }
            else
            {
                return getPoseByDegreeWithShift(deg, device.GetPress(SteamVR_Controller.ButtonMask.Trigger));
            }

        }
    }

    private int getPoseByDegree(float deg)
    {
        for (int i = 0; i < _poseDegreeConfigsTypeA.Length; i++)
        {
            if (_poseDegreeConfigsTypeA[i].degree > deg)
            {
                return (int) _poseDegreeConfigsTypeA[i].pose;
            }
        }
        return (int)HandPose.none;
    }

    private int getPoseByDegreeWithShift(float deg, bool shift)
    {
        int shiftId = (shift)? 1:0;
        for (int i = 0; i < _poseDegreeConfigsTypeB.GetLength(1); i++)
        {
            if (_poseDegreeConfigsTypeB[shiftId, i].degree > deg)
            {
                return (int)_poseDegreeConfigsTypeB[shiftId, i].pose;
            }
        }
        return (int)HandPose.none;
    }
}
