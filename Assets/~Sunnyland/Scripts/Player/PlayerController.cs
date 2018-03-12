/*=============================================
-----------------------------------
Copyright (c) 2018 Anshuman Meghnad
-----------------------------------
@file: PlayerController.cs
@date: 12/03/2018
@author: Anshuman Meghnad
@brief: Script to control the Player
===============================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sunnyland.Player
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public float speed = 5f;
        public float maxVelocity = 5f;
        public float rayDistance = 0.5f;
        public float jumpHeight = 2f;
        public int maxJumpCount = 2;
        public LayerMask groundLayer;

        private Vector3 moveDirection;
        private int currentJump = 0;

        // References
        private SpriteRenderer rend;
        private Animator anim;
        private Rigidbody2D rigid;

        #region Unity Functions
        // Use this for initialization
        void Start()
        {
            rend = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody2D>();
        }
        // Update is called once per frame
        void Update()
        {
            moveDirection.y += Physics.gravity.y * Time.deltaTime;
        }
        void FixedUpdate()
        {
            // Check for the ground
            DetectGround();
        }
        void OnDrawGizmos()
        {
            Ray groundRay = new Ray(transform.position, Vector3.down);
            Gizmos.DrawLine(groundRay.origin, groundRay.origin + groundRay.direction * rayDistance);
        }
        #endregion
        #region Custom Functions
        void DetectGround()
        {
            // Create a ray going down
            Ray groundRay = new Ray(transform.position, Vector3.down);
            // Set Hit to 2D Raycast
            RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down);
            // If hit collider is not null
            if (groundHit.collider != null)
            {
                // Reset currentJump
                currentJump = 0;
            }
        }
        void LimitVelocity()
        {
            // If rigid's velocity (magnitude) is greater than maxVelocity
            if (rigid.velocity.magnitude > maxVelocity)
            {
                // Set rigid velocity to velocity normalized x maxVelocity
                rigid.velocity = rigid.velocity.normalized * maxVelocity;
            }
        }

        public void Jump()
        {
            // if currentJump is less than maxJumpCount
            if (currentJump < maxJumpCount)
            {
                // Increment currentJump
                currentJump++;
                // Add Force to player (using Impulse)
                rigid.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            }
        }
        public void Climb()
        {

        }
        public void Move(float horizontal)
        {

        }
        #endregion
    }
}
