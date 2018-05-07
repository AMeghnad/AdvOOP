using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sunnyland
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Climbable : MonoBehaviour
    {
        public Vector2 offset;
        public Vector2 zoneCentre;
        public Vector2 zoneSize;
        [Header("Debug")]
        public Color zoneColour = new Color(1, 0, 0, 0.5f);
        public Color lineColour = new Color(1, 1, 1, 1f);

        private Bounds zone;
        private Bounds box;
        private BoxCollider2D col;
        private Vector2 size, top, bottom;

        // Use this for initialization
        void Start()
        {

        }

        void OnDrawGizmos()
        {
            // If we are in the application editor
            if (Application.isEditor)
            {
                // Draw the debug lines
                RecalculateBounds();

                Gizmos.color = zoneColour;
                Gizmos.DrawCube(zone.center, zone.size);

                Gizmos.color = lineColour;
                Gizmos.DrawLine(top, bottom);
            }
        }

        void RecalculateBounds()
        {
            col = GetComponent<BoxCollider2D>();
            box = col.bounds;
            size = box.size;
            // Create a new bounds basedd on boxCollider
            zone = new Bounds(box.center + (Vector3)zoneCentre, box.size + (Vector3)zoneSize);

            // Get position of transform
            Vector2 position = transform.position;
            // Calculate top and bottom
            top = position + new Vector2(offset.x, 0);
            bottom = position + new Vector2(offset.x, -size.y + offset.y);
        }

        public float GetX()
        {
            return zone.center.x;
        }

        public bool IsAtTop(Vector3 point)
        {
            // Return true if player is at top
            return point.y > zone.max.y;
        }

        public bool IsAtBottom(Vector3 point)
        {
            // Return true if player is at bottom
            return point.y < zone.min.y;
        }
    }
}
