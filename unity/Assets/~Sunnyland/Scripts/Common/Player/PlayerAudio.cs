using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sunnyland
{
    public class PlayerAudio : MonoBehaviour
    {
        public AudioSource onHurtSound;

        private PlayerController player;

        #region Unity Functions
        // Use this for initialization
        void Start()
        {
            player = GetComponent<PlayerController>();
            // Subscribe to on hurt function
            player.onHurt += OnHurt;
        }

        // Update is called once per frame
        void Update()
        {
            // TEST
            if (Input.GetKeyDown(KeyCode.U))
            {
                // Hurt yourself when you press U
                player.Hurt(10);
            }
        }
        #endregion
        #region Custom Functions
        void OnHurt()
        {
            onHurtSound.Play();
        }
        #endregion
    }
}
