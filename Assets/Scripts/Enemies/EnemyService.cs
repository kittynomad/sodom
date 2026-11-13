/************************************************************************
// File Name : EnemyService.cs
// Author : Arcadia Koederitz
// Creation Date : 10/13/2026
// Last Modified : 10/13/2026
//
// Brief Description : Manages tracking enemies and enemy respawning.
*****************************************************************************/
using System.Collections.Generic;
using TFOOL.ManagersAndServices;

namespace TFOOL.Enemies
{
    public class EnemyService : Service
    {
        private static HashSet<string> deadEnemies = new HashSet<string>();
    }
}