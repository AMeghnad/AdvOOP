using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sunnyland
{
    public class UserInput : MonoBehaviour
    {
        private PlayerController player;

        private float inputH, inputV;
        private bool isJumping;
        private bool isCrouching;

        // Use this for initialization
        void Start()
        {
            player = GetComponent<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
            GetInput();
            player.Move(inputH);
            player.Climb(inputV);
            if (isJumping)
            {
                player.Jump();
            }
            if (isCrouching)
            {
                player.Crouch();
            }
            else
            {
                player.UnCrouch();
            }
        }

        void GetInput()
        {
            inputH = Input.GetAxis("Horizontal");
            inputV = Input.GetAxis("Vertical");
            isJumping = Input.GetKeyDown(KeyCode.Space);
            isCrouching = Input.GetKeyDown(KeyCode.LeftControl);
        }
    }
}
