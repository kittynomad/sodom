/************************************************************************
// File Name : UIManager.cs
// Author : Arcadia Koederitz
// Creation Date : 10/16/2026
// Last Modified : 10/16/2026
//
// Brief Description : 
*****************************************************************************/
using TFOOL.ManagersAndServices;
using UnityEngine;

namespace TFOOL.UI
{
    public class UIManager : Manager
    {
        public override Awaitable Initialize()
        {
            DontDestroyOnLoad(this);
            return base.Initialize();
        }
    }
}