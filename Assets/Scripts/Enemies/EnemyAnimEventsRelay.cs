/*****************************************************************************
// File Name : EnemyAnimEventsRelay.cs
// Author : Arcadia Koederitz
// Creation Date : 8/22/2026
// Last Modified : 8/22/2026
//
// Brief Description : Reroute script that handles enemy animation events.
*****************************************************************************/
using CustomAttributes;
using NaughtyAttributes;
using TFOOL.Enemies.AI;
using UnityEngine;

namespace TFOOL.Enemies
{
    public class EnemyAnimEventsRelay : MonoBehaviour
    {
        [SerializeField, ShowIfNull] private EnemyController enemyController;

        private void Reset()
        {
            enemyController = GetComponentInParent<EnemyController>();
        }

        /// <summary>
        /// Sends a string signal to the current AI state to advance it based on actions in the animation.
        /// </summary>
        /// <param name="signal"></param>
        public void SetAnimationSignal(string signal)
        {
            enemyController.SetSignal(signal);
        }
    }
}