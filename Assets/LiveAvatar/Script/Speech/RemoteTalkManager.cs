using UnityEngine;
using IST.RemoteTalk;

namespace LiveAvatar.Speech
{
    public class RemoteTalkManager : MonoBehaviour
    {
        [SerializeField]
        private RemoteTalkClient _client = null;

        private void Awake()
        {
            if (_client == null)
                _client = GameObject.FindObjectOfType<RemoteTalkClient>();
        }

        public void Connect()
        {
            var ret = rtPlugin.LaunchVOICEROID2();
            if (ret > 0)
            {
                _client.serverPort = ret;
                _client.UpdateStats();
            }

        }
    }
}