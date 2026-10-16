/************************************************************************
// File Name : ScreenFadeManager.cs
// Author : Arcadia Koederitz
// Creation Date : 10/16/2026
// Last Modified : 10/16/2026
//
// Brief Description : Crossfades the screen to cover up transitions.
*****************************************************************************/
using NaughtyAttributes;
using TFOOL.ManagersAndServices;
using UnityEngine;
using UnityEngine.UI;

namespace TFOOL.UI
{
    public class ScreenFadeService : Service
    {
        [SerializeField, BoxGroup("References")] private Image fadeImage;

        private static ScreenFadeService instance;

        public override async Awaitable Initialize()
        {
            if (SingletonUtility.SetSingleton(ref instance, this))
            {

            }
        }
    }
}