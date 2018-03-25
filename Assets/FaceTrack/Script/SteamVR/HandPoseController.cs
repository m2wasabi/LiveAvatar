using UnityEngine;
using Valve.VR;

public class HandPoseController : MonoBehaviour {

    public GameObject leftHandController;
    public GameObject rightHandController;

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

    private PoseDegreeConfig[] _poseDegreeConfigs = new PoseDegreeConfig[9];

    void Start () {
        _animator = GetComponent<Animator>();

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

        _poseDegreeConfigs[0] = new PoseDegreeConfig(HandPose.fist, 30);
        _poseDegreeConfigs[1] = new PoseDegreeConfig(HandPose.like, 60);
        _poseDegreeConfigs[2] = new PoseDegreeConfig(HandPose.otome, 120);
        _poseDegreeConfigs[3] = new PoseDegreeConfig(HandPose.ok, 150);
        _poseDegreeConfigs[4] = new PoseDegreeConfig(HandPose.open, 210);
        _poseDegreeConfigs[5] = new PoseDegreeConfig(HandPose.rock, 240);
        _poseDegreeConfigs[6] = new PoseDegreeConfig(HandPose.v, 300);
        _poseDegreeConfigs[7] = new PoseDegreeConfig(HandPose.point, 330);
        _poseDegreeConfigs[8] = new PoseDegreeConfig(HandPose.fist, 360);
    }

    void Update () {
        if (_animator != null)
        {
            if (_device_L != null)
            {
                _animator.SetInteger("LeftHandPose", updateHandPose(_device_L, Hand.left));
            }
            if (_device_R != null)
            {
                _animator.SetInteger("RightHandPose", updateHandPose(_device_R, Hand.right));
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

            return getPoseByDegree(deg);
        }
    }

    private int getPoseByDegree(float deg)
    {
        for (int i = 0; i < _poseDegreeConfigs.Length; i++)
        {
            if (_poseDegreeConfigs[i].degree > deg)
            {
                return (int) _poseDegreeConfigs[i].pose;
            }
        }
        return 0;
    }
}
