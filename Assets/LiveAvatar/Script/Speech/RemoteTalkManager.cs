using System.Collections.Generic;
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
                setCastList(_client.casts);
            }
        }

        private void setCastList(Cast[] casts)
        {
            // ToDo: Castリストをドロップダウンに更新する
            
            // ToDo: Onchangeをチェックする
            
        }

        public void OnhangeCast()
        {
            // ToDo: 引数ちゃんとする
            
            // ToDO: Cast変更したときに今のCastを記憶する
        }

        private void setParamsList()
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