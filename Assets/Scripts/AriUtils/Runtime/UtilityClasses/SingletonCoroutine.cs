/*****************************************************************************
// File Name : SingletonCoroutine.cs
// Author : Arcadia Koederitz
// Creation Date : 6/12/2026
// Last Modified : 6/12/2026
//
// Brief Description : Manager class for having a coroutine that can only run 1 instance.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace AriUtils
{
    public class SingletonCoroutine
    {
        private readonly MonoBehaviour source;

        private Coroutine singletonRoutine;

        public enum InterruptMode
        {
            Cancel,
            Ignore
        }

        public SingletonCoroutine(MonoBehaviour source)
        {
            this.source = source;
        }

        /// <summary>
        /// Starts a coroutine and cancels the existing routine.
        /// </summary>
        /// <param name="coroutine"></param>
        public void StartCoroutineCancel(IEnumerator coroutine)
        {
            if (singletonRoutine != null)
            {
                source.StopCoroutine(singletonRoutine);
                singletonRoutine = null;
            }
            singletonRoutine = source.StartCoroutine(CoroutineWrapper(coroutine));
        }

        /// <summary>
        /// Starts a coroutine only if no coroutine is running.
        /// </summary>
        /// <param name="coroutine"></param>
        public void StartCoroutineIgnore(IEnumerator coroutine)
        {
            // Only start the coroutine if the singleton routine ref is null.
            if (singletonRoutine == null)
            {
                singletonRoutine = source.StartCoroutine(CoroutineWrapper(coroutine));
            }
        }

        private IEnumerator CoroutineWrapper(IEnumerator coroutine)
        {
            yield return coroutine;
            singletonRoutine = null;
        }

        /// <summary>
        /// Stops a currently ongoing singleton coroutine.
        /// </summary>
        public void StopCoroutine()
        {
            if (singletonRoutine != null)
            {
                source.StopCoroutine(singletonRoutine);
                singletonRoutine = null;
            }
        }
    }
}