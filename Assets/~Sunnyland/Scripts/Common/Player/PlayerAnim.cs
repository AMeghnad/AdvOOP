﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sunnyland
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAnim : MonoBehaviour
    {
        private PlayerController player;
        private Animator anim;
        private Rigidbody2D rigid;

        #region Unity Functions
        // Use this for initialization
        void Start()
        {
            anim = GetComponent<Animator>();
            player = GetComponent<PlayerController>();
            // Subscribe animator to player events
            player.onGroundedChanged += OnGroundedChanged;
            player.onJump += OnJump;
            player.onHurt += OnHurt;
            player.onMove += OnMove;
            player.onClimb += OnClimb;
        }
        void Update()
        {
            anim.SetBool("IsGrounded", player.isGrounded);
            anim.SetBool("IsClimbing", player.isCrouching);
            anim.SetBool("IsCrouching", player.isCrouching);
            anim.SetFloat("JumpY", rigid.velocity.normalized.y);
        }
        #endregion
        #region Custom Functions
        void OnJump()
        {

        }
        void OnHurt()
        {
            anim.SetTrigger("Hurt");
        }
        void OnMove(float input)
        {
            anim.SetBool("IsRunning", input != 0);
        }
        void OnClimb(float input)
        {
            anim.SetFloat("ClimbY", Mathf.Abs(input));
        }
        void OnGroundedChanged(bool isGrounded)
        {
            // Update is grounded in animator
            if (isGrounded)
            {
                print("I'm grounded :C");
            }
            else
            {
                print("I'm not grounded! :D");
            }
        }
        #endregion
    }
}
