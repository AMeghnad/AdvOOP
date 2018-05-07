using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generics
{
    public class GenericsTest : MonoBehaviour
    {
        public GameObject prefab;
        public int spawnAmount = 20;
        public float spawnRadius = 50f;
        public CustomList<int> gameObjects = new CustomList<int>();


        // Use this for initialization
        void Start()
        {
            gameObjects.Add(spawnAmount);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
