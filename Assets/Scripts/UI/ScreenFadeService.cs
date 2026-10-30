/************************************************************************
// File Name : ScreenFadeManager.cs
// Author : Arcadia Koederitz
// Creation Date : 10/16/2026
// Last Modified : 10/16/2026
//
// Brief Description : Crossfades the screen to cover up transitions.
*****************************************************************************/
using AriUtils;
using NaughtyAttributes;
using TFOOL.ManagersAndServices;
using UnityEngine;
using UnityEngine.UI;

namespace TFOOL.UI
{
    public class ScreenFadeService : Service
    {
        [SerializeField, BoxGroup("References")] private Image fadeImage;
        [SerializeField] private AsyncTimeline fadeTimeline;

        private static ScreenFadeService instance;
        private bool isTransitioning;


        public override async Awaitable Initialize()
        {
            SingletonUtility.SetSingleton(ref instance, this);
        }

        /// <summary>
        /// Fades the screen to a given color.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static async Awaitable FadeScreenTo(Color color)
        {
            if (instance == null) { return; }
            await instance.FadeAsync(color);
        }
         
        /// <summary>
        /// Fades the screen back to transparent.
        /// </summary>
        /// <returns></returns>
        public static async Awaitable FadeScreenOut()
        {
            if (instance == null) { return; }
            await instance.FadeAsync(Color.clear);
        }

        private async Awaitable FadeAsync(Color targetColor)
        {
            if (!isTransitioning)
            {

                Color baseColor = fadeImage.color;
                if (baseColor == targetColor) { return; }

                void UpdateColor(float val)
                {
                    fadeImage.color = Color.Lerp(baseColor, targetColor, val);
                }

                isTransitioning = true;
                fadeTimeline.OnUpdateEvent += UpdateColor;
                await fadeTimeline.PlayFromStartAsync();
                fadeTimeline.OnUpdateEvent -= UpdateColor;
                isTransitioning = false;
            }
        }
    }
}