using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using IST.RemoteTalk;
using UniRx;
using UniRx.Triggers;

namespace LiveAvatar.Speech
{
    public class RemoteTalkManager : MonoBehaviour
    {
        [SerializeField]
        private RemoteTalkClient _client = null;

        [SerializeField]
        private Dropdown _castSelector;

        private void Awake()
        {
            if (_client == null)
                _client = GameObject.FindObjectOfType<RemoteTalkClient>();

            _castSelector.onValueChanged.AddListener(delegate { OnhangeCast(_castSelector); });
        }

        public void Connect()
        {
            var ret = rtPlugin.LaunchVOICEROID2();
            if (ret > 0)
            {
                _client.serverPort = ret;
                _client.UpdateStats();
                this.UpdateAsObservable().First(_ => _client.isReady).Subscribe(_ =>
                {
                    SetCastList(_client.casts);
                });
            }
        }

        private void SetCastList(Cast[] casts)
        {
            // Castリストをドロップダウンに更新する
            _castSelector.ClearOptions();
            _castSelector.AddOptions(casts.Select(a => a.name).ToList());
        }

        public void OnhangeCast(Dropdown change)
        {
            if (_client.casts.Length > 0)
            {
                _client.castID = change.value;
            }
        }

        private void setParamsList(TalkParam[] talkParams)
        {
            // ToDO: 音声パラメータを取得・更新する
        }

        public void OnChangeTalkParams()
        {
            // ToDo: 音声パラメータを変更した処理・記録・更新する
        }

        public void OnSpeech()
        {
            // ToDO: 発声する
        }
    }
}