/*****************************************************************************
// File Name : TemplateCreator.cs
// Author : Brandon Koederitz
// Creation Date : August 26, 2025
// Last Modified : September 16, 2025
//
// Brief Description : Allows for the creation of custom script templates in the assets window
*****************************************************************************/
using UnityEditor;

namespace TFOOL.Enemies.AI.Editor
{
    public static class TemplateCreator
    {
        #region CONSTS
        private const string ENEMYBEHAVIOR_PATH = "Assets/Scripts/AI/Editor/TemplateFiles/EnemyBehavior.cs.txt";
        private const string ENEMYSTATE_PATH = "Assets/Scripts/AI/Editor/TemplateFiles/EnemyState.cs.txt";
        private const string DECISIONENGINE_PATH = "Assets/Scripts/AI/Editor/TemplateFiles/DecisionEngine.cs.txt";
        #endregion

        /// <summary>
        /// Functions to be run by the Unity create menu that allows scripts to be created from templates.
        /// </summary>
        [MenuItem("Assets/Create/Scripting/AI/EnemyBehavior")]
        public static void CreateEnemyBehavior()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(ENEMYBEHAVIOR_PATH, "NewEnemyBehavior.cs");
        }
        [MenuItem("Assets/Create/Scripting/AI/EnemyState")]
        public static void CreateEnemyState()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(ENEMYSTATE_PATH, "NewEnemyState.cs");
        }
        [MenuItem("Assets/Create/Scripting/AI/DecisionEngine")]
        public static void CreateDecisionEngine()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(DECISIONENGINE_PATH, "NewDecisionEngine.cs");
        }
    }
}
