using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sunnyland
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAnim : MonoBehaviour
    {
        private PlayerController player;

        // Use this for initialization
        void Start()
        {
            player = GetComponent<PlayerController>();
        }

        void OnGroundedChanged(bool isGrounded)
        {
            // Update is grounded in animator
            if (isGrounded)
            {
                print("I'm grounded :(");
            }
            else
            {
                print("I'm not grounded! :D");
            }
        }
    }
}
