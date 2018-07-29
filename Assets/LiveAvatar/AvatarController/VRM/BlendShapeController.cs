using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

namespace LiveAvatar.AvatarController.VRM
{
    public class BlendShapeController : MonoBehaviour {

        public float MouthOpen { get; set; }

        private VRMBlendShapeProxy m_blendShapePloxy;
        void Start () {
		
        }
	
        void Update () {
		
        }

        public void setup()
        {
            m_blendShapePloxy = gameObject.GetComponentInChildren<VRMBlendShapeProxy>();
        }
    }
}
