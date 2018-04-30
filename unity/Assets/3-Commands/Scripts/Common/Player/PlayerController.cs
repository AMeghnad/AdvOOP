using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float lookSpeed = 5f;
        public float maxVelocity = 5f;
        [Header("References")]
        public Transform attachedCamera;

        // Private references
        private Rigidbody rigid;

        #region Unity Functions
        // Use this for initialization
        void Start()
        {
            rigid = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion
        #region Custom Functions
        void LimitVelocity()
        {
            Vector3 vel = rigid.velocity;
            if (vel.magnitude > maxVelocity)
            {
                vel = vel.normalized * maxVelocity;
            }
            rigid.velocity = vel;
        }
        public void Look(float horizontal, float vertical)
        {
            // Rotate player on the "horizontal" axis
            Vector3 euler = transform.localEulerAngles;
            euler.y += horizontal * lookSpeed * Time.deltaTime;
            transform.eulerAngles = euler;
            // Rotate camera on the "vertical" axis
            Vector3 camEuler = attachedCamera.localEulerAngles;
            camEuler.x -= vertical * lookSpeed * Time.deltaTime;
            attachedCamera.localEulerAngles = camEuler;
        }
        public void Move(float horizontal, float vertical)
        {
            Vector3 inputDir = new Vector3(horizontal, 0, vertical);
            rigid.AddForce(inputDir * moveSpeed);
            LimitVelocity();
        }
        #endregion
    }
}