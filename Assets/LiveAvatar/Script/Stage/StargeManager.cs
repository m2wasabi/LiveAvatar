using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LiveAvatar.Stage
{
    public class StargeManager : MonoBehaviour
    {

        private float speed = 1.0f;
        private bool movable = false;

        [SerializeField]
        private GameObject uipanel;
        void Start () {
        
        }

        void Update () {
            if (movable)
            {
                if (Input.GetKey (KeyCode.W)) {
                    transform.position -= transform.forward * speed * Time.deltaTime;
                }
                if (Input.GetKey (KeyCode.S)) {
                    transform.position += transform.forward * speed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift)) {
                    transform.Rotate(0,50 * Time.deltaTime,0);
                }
                else if (Input.GetKey(KeyCode.D)) {
                    transform.position += transform.right * speed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift)) {
                    transform.Rotate(0,-50 * Time.deltaTime,0);
                }
                else if (Input.GetKey(KeyCode.A)) {
                    transform.position -= transform.right * speed * Time.deltaTime;
                }
            }
        }

        public void SetEnableStageMove()
        {
            uipanel.SetActive(true);
            movable = true;
        }

        public void SetDisableStageMove()
        {
            uipanel.SetActive(false);
            movable = false;
        }

        
    }
}
