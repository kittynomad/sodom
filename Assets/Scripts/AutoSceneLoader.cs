/*****************************************************************************
// File Name : AutoSceneLoader.cs
// Author : Arcadia Koederitz
// Creation Date : 10/04/2026
// Last Modified : 10/04/2026
//
// Brief Description : Automatically loads a scene called "BootstrapScene" when the project starts.
*****************************************************************************/
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AriUtils
{
    public static class AutoSceneLoader
    {
        private const string AUTO_SCENE_NAME = "AutoLoadedObjects";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadBootstrapScene()
        {
            if (CheckIfSceneExists(AUTO_SCENE_NAME))
            {
                SceneManager.LoadScene(AUTO_SCENE_NAME, LoadSceneMode.Additive);
            }
        }

        private static bool CheckIfSceneExists(string sceneName)
        {
            // Loop through all scenes in the Build ScenesList and check their path.
            // If the file with a given name exists, then return true.
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                string name = Path.GetFileNameWithoutExtension(path);
                if (sceneName == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
