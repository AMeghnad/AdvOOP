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
        public int health = 100;
        public int damage = 50;
        [Header("Movement")]
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
        private float inputH, inputV;
        [Header("Climb")]
        public float climbSpeed = 5f;
        public bool isClimbing = false;
        public bool isOnSlope = false;
        [Header("References")]
        public Collider2D defaultCollider;
        public Collider2D crouchCollider;
        private Collider2D currentCollider;

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
        private int currentJump = 0;

        // References
        private SpriteRenderer rend;
        private Animator anim;
        private Rigidbody2D rigid;
        public Climbable climbObject;

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
            PerformClimb();
            PerformMove();
            PerformJump();
        }
        void FixedUpdate()
        {
            UpdateCollider();
            // Check for the ground
            DetectGround();
            DetectClimbable();
        }
        void OnDrawGizmos()
        {
            // Drawing the ground ray
            Ray groundRay = new Ray(transform.position, Vector3.down);
            Gizmos.DrawLine(groundRay.origin, groundRay.origin + groundRay.direction * rayDistance);
            // Drawing the 'right' ray
            Vector3 right = Vector3.Cross(groundNormal, Vector3.forward);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position - right, transform.position + right);
        }
        #endregion
        #region Custom Functions

        void PerformClimb()
        {
            // Check if:
            if (inputV != 0 && // Player is pressing up or down
                climbObject != null) // Player is in front of a climbable
            {
                // Get bools
                bool isAtTop = climbObject.IsAtTop(transform.position);
                bool isAtBottom = climbObject.IsAtBottom(transform.position);
                bool isAtTopAndPressingUp = isAtTop && inputV > 0;
                bool isAtBottomAndPressingDown = isAtBottom && inputV < 0;

                // Check if the player is:
                if (!isAtTopAndPressingUp && // not trying to climb up
                    !isAtBottomAndPressingDown && // not trying to climb down
                    !isClimbing) // not climbing
                {
                    // Start climbing
                    isClimbing = true;
                    // Invoke event
                    if (onClimbChanged != null)
                    {
                        onClimbChanged.Invoke(isClimbing);
                    }
                }
                if (isAtTopAndPressingUp || isAtBottomAndPressingDown)
                {
                    // Stop climbing
                    StopClimbing();
                }
            }
            if (isClimbing && climbObject != null) // if this is null there's an error
            {
                // Make sure physics is disabled before moving the player
                DisablePhysics();
                // logic for moving the player up and down climbable
                float x = climbObject.GetX();
                Vector3 position = transform.position;
                position.x = x;
                position.y += inputV * climbSpeed * Time.deltaTime;
                transform.position = position;
            }
        }
        void PerformMove()
        {
            if (isOnSlope && inputH == 0 && isGrounded)
            {
                // Cancel the velocity 
                rigid.velocity = Vector3.zero;
            }

            Vector3 right = Vector3.Cross(groundNormal, Vector3.back);
            rigid.AddForce(right * inputH * speed);

            // Limit velocity to max velocity
            LimitVelocity();
        }

        void PerformJump()
        {
            if (isJumping)
            {
                // if currentJump is less than maxJumpCount
                if (currentJump < maxJumpCount)
                {
                    // Increment currentJump
                    currentJump++;
                    // Add Force to player (using Impulse)
                    rigid.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                }
                // Reset jump input
                isJumping = false;
            }
        }

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
                // Detect if on a slope
                if (Mathf.Abs(hit.normal.x) > 0.1f)
                {
                    // Set gravity to zero
                    rigid.gravityScale = 0;
                }
                else
                {
                    // Set gravity to one
                    rigid.gravityScale = 1;
                }

                if (CheckGround(hit))
                {
                    // We found the ground! So exit the function
                    break;
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
        void DetectClimbable()
        {
            // If we're already climbing, return
            if (isClimbing)
            {
                return;
            }
            // Otherwise, detect climbable
            // Perform a box cast that is the size of the player's collision
            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, currentCollider.bounds.size, 0);
            // Loop through all hits
            foreach (var hit in hits)
            {
                // Check if:
                if (hit != null && // Something was hit
                    hit.isTrigger)// The hit object is a trigger
                {
                    // We have found our climbable!
                    climbObject = hit.GetComponent<Climbable>();
                    return;
                }

                // No climbable's were found
                climbObject = null;
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
        void EnablePhysics()
        {
            rigid.simulated = true;
            rigid.gravityScale = 1;
        }
        void DisablePhysics()
        {
            rigid.simulated = false;
            rigid.gravityScale = 0;
        }
        void StopClimbing()
        {
            climbObject = null;

            // We are no longer climbing
            isClimbing = false;
            if (onClimbChanged != null)
            {
                onClimbChanged.Invoke(isClimbing);
            }
            // Re-enable physics
            EnablePhysics();
        }
        void UpdateCollider()
        {
            // If crouching
            if (isCrouching)
            {
                defaultCollider.enabled = false;
                currentCollider = crouchCollider;
            }
            else
            {
                // Set current to default collider
                crouchCollider.enabled = false;
                currentCollider = defaultCollider;
            }
            currentCollider.enabled = true;
        }

        public void Jump()
        {
            isJumping = true;

            if (onJump != null)
            {
                onJump.Invoke();
            }
        }

        public void Move(float horizontal)
        {
            if (horizontal != 0)
            {
                rend.flipX = horizontal < 0;
            }

            inputH = horizontal;

            // Invoke event 
            if (onMove != null)
            {
                onMove.Invoke(inputH);
            }
        }
        public void Climb(float vertical)
        {
            inputV = vertical;

            // Invoke event
            if (onClimb != null)
            {
                onClimb.Invoke(vertical);
            }
        }
        public void Crouch()
        {
            isCrouching = true;
            // Invoke event
            if (onCrouchChanged != null)
            {
                onCrouchChanged.Invoke(isCrouching);
            }
        }
        public void UnCrouch()
        {
            isCrouching = false;
            // Invoke event
            if (onCrouchChanged != null)
            {
                onCrouchChanged.Invoke(isCrouching);
            }
        }
        public void Hurt(int damage, Vector2? hitNormal = null)
        {
            // Set a default hit direction
            Vector2 force = Vector2.up;
            if (hitNormal != null) // If a hitNormal exists
            {
                // Use the hitNormal as a direction
                force = hitNormal.Value;
            }

            // Deal damage to player
            health -= damage;

            // Add force in the hit direction
            rigid.AddForce(force * damage, ForceMode2D.Impulse);

            // Invoke event
            if (onHurt != null)
            {
                // Play hurt sound or animation
                onHurt.Invoke();
            }
        }
        #endregion
    }
}
