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

namespace Sunnyland
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public float speed = 5f;
        public float maxVelocity = 5f;
        [Header("Grounding")]
        public float rayDistance = 0.5f;
        public float maxSlopeAngle = 45f;
        public bool isGrounded = false;
        [Header("Crouch")]
        public bool isCrouching = false;
        [Header("Jump")]
        public float jumpHeight = 2f;
        public int maxJumpCount = 2;
        public bool isJumping = false;
        [Header("Climb")]
        public float climbSpeed = 5f;
        public bool isClimbing = false;
        public bool isOnSlope = false;
        [Header("References")]
        public Collider2D defaultCollider;
        public Collider2D crouchCollider;

        // Delegates
        public EventCallback onJump;
        public EventCallback onHurt;
        public BoolCallback onCrouchChanged;
        public BoolCallback onGroundedChanged;
        public BoolCallback onSlopeChanged;
        public BoolCallback onClimbChanged;
        public FloatCallback onMove;
        public FloatCallback onClimb;

        private Vector3 groundNormal = Vector3.up;
        private Vector3 moveDirection;
        private int currentJump = 0;

        private float vertical, horizontal;

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
        // Check to see if ray hit object is ground
        bool CheckSlope(RaycastHit2D hit)
        {
            // Grab the angle in degrees of the surface we're standing on 
            float slopeAngle = Vector3.Angle(Vector3.up, hit.normal);
            // If the angle is greater than max
            if (slopeAngle >= maxSlopeAngle)
            {
                // Make player slide down surface
                rigid.AddForce(Physics.gravity);
            }
            if (slopeAngle > 0 && slopeAngle < maxSlopeAngle)
            {
                return true;
            }
            return false;
        }
        bool CheckGround(RaycastHit2D hit)
        {
            // Check if
            // it hit something AND
            // It didn't hit me AND
            // It didn't hit a trigger
            if (hit.collider != null && hit.collider.name != name && !hit.collider.isTrigger)
            {
                // Reset jump count
                currentJump = 0;
                // Is grounded!
                isGrounded = true;
                // Set ground normal now that we're grounded
                groundNormal = -hit.normal;

                //Record 'isOnSlope value
                bool wasOnSlope = isOnSlope;
                // Check if we're on a slope!
                isOnSlope = CheckSlope(hit);
                // Has the 'isOnSlope' value changed?
                if (wasOnSlope != isOnSlope)
                {
                    // Invoke event
                    if (onSlopeChanged != null)
                        onSlopeChanged.Invoke(isOnSlope);
                }

                // We have found our ground so exit the function
                // No need to check any more hits
                return true;
            }
            else
            {
                // We are no longer grounded
                isGrounded = false;
            }
            return false;
        }
        void DetectGround()
        {
            // Record a copy of what isGrounded was
            bool wasGrounded = isGrounded;

            #region Ground Detection Logic            
            // Create a ray going down
            Ray groundRay = new Ray(transform.position, Vector3.down);
            // Set Hit to 2D Raycast
            RaycastHit2D[] groundHits = Physics2D.RaycastAll(groundRay.origin, groundRay.direction, rayDistance);
            // If hit collider is not null
            foreach (var hit in groundHits)
            {
                if (CheckGround(hit))
                {
                    // We found the ground! So exit the function
                    break;
                }


                // If hit collider is not null
                if (hit.collider != null)
                {
                    // Reset currentJump
                    currentJump = 0;
                }
            }
            #endregion

            // Check if:
            // isGrounded has changed before the detection AND
            // Something is subscribed to this event
            if (wasGrounded != isGrounded && onGroundedChanged != null)
            {
                // Run all the things subscribed to event and give it "isGrounded" value
                onGroundedChanged.Invoke(isGrounded);
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
        public void Move(float horizontal)
        {
            // If horizontal > 0
            if (horizontal > 0)
            {
                // Flip Character
                rend.flipX = false;
            }
            // If horizontal < 0
            if (horizontal < 0)
            {
                // Flip character
                rend.flipX = true;
            }
            // Add force to player in the right direction
            rigid.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
            // Limit velocity
            LimitVelocity();
        }
        public void Climb()
        {

        }
        #endregion
    }
}
