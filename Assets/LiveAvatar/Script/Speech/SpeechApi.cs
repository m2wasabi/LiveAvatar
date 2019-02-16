using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace LiveAvatar.Speech
{
    public class SpeechApi
    {
        [System.Serializable]
        public class SeikaTalk{
            public string talktext;
            public float speed;
            public float volume;
            public float pitch;
            public float intonation;
            public Emotion[] emotions;

            public SeikaTalk()
            {
                speed = 1.0f;
                volume = 2.0f;
                pitch = 1.0f;
                intonation = 1.0f;
            }
        }

        [System.Serializable]
        public class Emotion
        {
            public string Key;
            public float Value;
        }
    
        private string url = "http://local:password@localhost:7180/PLAY/2000";

        public void SetUrl(string url)
        {
            this.url = url;
        }

        public IEnumerator  Speech(string text) {
            SeikaTalk talk = new SeikaTalk();
            talk.talktext = text;
            talk.emotions = new[] {new Emotion(){Key = "喜び", Value = 1.00f}, new Emotion(){Key = "悲しみ", Value = 0.20f}};
            var myjson = JsonUtility.ToJson(talk);

            byte[] postData = System.Text.Encoding.UTF8.GetBytes (myjson);
            var request = new UnityWebRequest(url, "POST");
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.Send();
        
            if(request.isNetworkError || request.isHttpError) {
                Debug.Log(request.error);
            }
            else {
                Debug.Log("Form upload complete!");
            }
        }
    }
}
