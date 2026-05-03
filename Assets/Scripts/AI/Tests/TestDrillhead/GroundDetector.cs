/*****************************************************************************
// File Name : GroundDetector.cs
// Author : Arcadia Koederitz
// Creation Date : 5/3/2026
// Last Modified : 5/3/2026
//
// Brief Description : Detects if the drillhead is in the ground.
*****************************************************************************/
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [field: SerializeField] public bool InGround { get; private set; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 7 is the ground layer.
        if (collision.gameObject.layer == 7)
        {
            InGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 7 is the ground layer.
        if (collision.gameObject.layer == 7)
        {
            InGround = false;
        }
    }
}
