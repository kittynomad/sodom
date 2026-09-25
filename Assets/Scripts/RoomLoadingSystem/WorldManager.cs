/************************************************************************
// File Name : WorldManager.cs
// Author : Arcadia Koederitz
// Creation Date : 9/25/2026
// Last Modified : 9/25/2026
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private LevelStreamingService levelStreamingService;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        levelStreamingService.Initialize();
    }

    private void OnDestroy()
    {
        levelStreamingService.Deinitialize();
    }
}
