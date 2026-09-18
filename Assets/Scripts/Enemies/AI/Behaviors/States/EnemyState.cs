/*****************************************************************************
// File Name : EnemyState.cs
// Author : Arcadia Koederitz
// Creation Date : 6/7/2026
// Last Modified : 6/7/2026
//
// Brief Description : A form of enemy behavior that is used at the top level of the state machine and controls a continuous behavior.
*****************************************************************************/
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public abstract class EnemyState : EnemyBehavior
    {
        public virtual bool IsCancellable => true;
        public virtual Color DebugColor => Color.white;

        public override Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            //if (enemy.transform.GetChild(0).TryGetComponent(out SpriteRenderer rend))
            //{
            //    rend.color = debugColor;
            //}
            return Awaitable.NextFrameAsync();
        }
    }

}