using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using IST.RemoteTalk;
using UniRx;
using UniRx.Triggers;
using LiveAvatar.UI;

namespace LiveAvatar.Speech
{
    public class RemoteTalkManager : MonoBehaviour
    {
        [SerializeField]
        private RemoteTalkClient _client = null;

        [SerializeField]
        private Dropdown _castSelector;

        [SerializeField]
        private GameObject Sliders;
        [SerializeField]
        private GameObject SliderPrefab;

        private TalkParam[] _talkParams;
        private List<UI_SliderField> _sliderFields;
        private void Awake()
        {
            if (_client == null)
                _client = GameObject.FindObjectOfType<RemoteTalkClient>();

            _castSelector.onValueChanged.AddListener(_ => { OnhangeCast(_castSelector); });
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
                    SetParamsList(_client.talkParams);
                }).AddTo(gameObject);
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

        private void SetParamsList(TalkParam[] talkParams)
        {
            // ToDO: 音声パラメータを取得・更新する
            for (var i = 0; i < talkParams.Length; i++)
            {
                var slider = Instantiate(SliderPrefab, Sliders.transform) as GameObject;
                var sliderComponent = slider.GetComponent<UI_SliderField>(); 
                sliderComponent.Initialize(talkParams[i].name,talkParams[i].rangeMin,talkParams[i].rangeMax,talkParams[i].value);
            }

            _sliderFields = Sliders.GetComponentsInChildren<UI_SliderField>().ToList();
        }

        public void Speech(string text)
        {
            var talk = new Talk();
            talk.castName = _client.casts[_client.castID].name;
            
            var talkParams = new List<TalkParam>();
            foreach (var sliderField in _sliderFields)
            {
                talkParams.Add(sliderField.GetTalkParam());
            }
            talk.param = talkParams.ToArray();
            talk.text = text;
            _client.Play(talk);
        }
    }
}