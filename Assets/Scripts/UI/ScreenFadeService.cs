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

        public static async Awaitable FadeScreenTo(Color color, float duration)
        {
            if (instance == null) { return; }
            await instance.FadeAsync(color, duration);
        }
         
        public static async Awaitable FadeScreenOut(float duration)
        {
            if (instance == null) { return; }
            await instance.FadeAsync(Color.clear, duration);
        }

        private async Awaitable FadeAsync(Color targetColor, float duration)
        {
            if (!isTransitioning)
            {
                Color baseColor = fadeImage.color;
                fadeTimeline.Duration = duration;

                void UpdateColor(float val)
                {
                    fadeImage.color = Color.Lerp(baseColor, targetColor, val);
                }

                fadeTimeline.OnUpdateEvent += UpdateColor;
                await fadeTimeline.PlayFromStartAsync();
                fadeTimeline.OnUpdateEvent -= UpdateColor;
            }
        }
    }
}