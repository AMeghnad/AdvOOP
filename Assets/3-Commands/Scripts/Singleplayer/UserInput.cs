using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    [RequireComponent(typeof(PlayerController))]
    public class UserInput : MonoBehaviour
    {
        public float inputH, inputV;
        public float mouseX, mouseY;

        private PlayerController player;
        #region Unity Functions
        // Use this for initialization
        void Start()
        {
            player = GetComponent<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
            GetInput();
            player.Move(inputH, inputV);
            player.Look(mouseX, mouseY);
        }
        #endregion
        #region CustomFunctions
        void GetInput()
        {
            inputH = Input.GetAxis("Horizontal");
            inputV = Input.GetAxis("Vertical");
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }
        #endregion
    }
}
