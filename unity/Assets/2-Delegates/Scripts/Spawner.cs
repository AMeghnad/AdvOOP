/*=============================================
-----------------------------------
Copyright (c) 2018 Anshuman Meghnad
-----------------------------------
@file: Spawner.cs
@date: 2018/02/19
@author: Anshuman Meghnad
@brief: Script for spawning objects via delegates
===============================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Delegates
{
    public class Spawner : MonoBehaviour
    {
        public Transform target;
        public GameObject trollPrefab, orcPrefab;
        public float spawnAmount = 10;  // Spawn amount for each prefab
        public float spawnRate = 0.5f;

        private float spawnTimer = 0f;

        public delegate void SpawnDelegate();
        public SpawnDelegate spawnCallback;

        // Use this for initialization
        void Start()
        {
            // Subscribe all functions to delegate
            spawnCallback += SpawnOrc;
            spawnCallback += SpawnTroll;
        }

        // Update is called once per frame
        void Update()
        {
            // Count up the timer
            spawnTimer += Time.deltaTime;
            // Has timer reached spawn rate?
            if (spawnTimer >= spawnRate)
            {
                for (int i = 0; i < spawnAmount; i++)
                {
                    spawnCallback.Invoke();
                }
                // Reset spawn timer
                spawnTimer = 0f;
            }
        }

        // Function for spawning Orcs
        void SpawnOrc()
        {
            GameObject clone = Instantiate(orcPrefab, transform.position, transform.rotation);

            // Do orc stuff
            FollowTarget agent = clone.GetComponent<FollowTarget>();
            agent.target = target;
        }

        // Function for spawning Trolls
        void SpawnTroll()
        {
            GameObject clone = Instantiate(trollPrefab, transform.position, transform.rotation);

            // Do troll stuff
            FollowTarget agent = clone.GetComponent<FollowTarget>();
            agent.target = target;
        }
    }
}
