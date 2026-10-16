/*****************************************************************************
// File Name : TemplateCreator.cs
// Author : Arcadia Koederitz
// Creation Date : August 26, 2025
// Last Modified : September 16, 2025
//
// Brief Description : Allows for the creation of custom script templates in the assets window
*****************************************************************************/
using UnityEditor;

namespace AriUtils
{
    public static class TemplateCreator
    {
        #region CONSTS
        private const string MONOBEHAVIOUR_PATH = "Assets/Scripts/AriUtils/Editor/Templates/MonoBehaviour.cs.txt";
        private const string SCRIPTABLEOBJECT_PATH = "Assets/Scripts/AriUtils/Editor/Templates/ScriptableObject.cs.txt";
        private const string ENUM_PATH = "Assets/Scripts/AriUtils/Editor/Templates/Enum.cs.txt";
        private const string RAWCLASS_PATH = "Assets/Scripts/AriUtils/Editor/Templates/RawC#Class.cs.txt";
        private const string CUSTOMEDITOR_PATH = "Assets/Scripts/AriUtils/Editor/Templates/CustomEditor.cs.txt";
        private const string INTERFACE_PATH = "Assets/Scripts/AriUtils/Editor/Templates/Interface.cs.txt";
        #endregion

        /// <summary>
        /// Functions to be run by the Unity create menu that allows scripts to be created from templates.
        /// </summary>
        [MenuItem("Assets/Create/Scripting/AriUtils/MonoBehaviour")]
        public static void CreateMonoBehaviour()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(MONOBEHAVIOUR_PATH, "NewMonoBehaviour.cs");
        }

        [MenuItem("Assets/Create/Scripting/AriUtils/ScriptableObject")]
        public static void CreateScriptableObject()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(SCRIPTABLEOBJECT_PATH, "NewScriptableObject.cs");
        }

        [MenuItem("Assets/Create/Scripting/AriUtils/Enum")]
        public static void CreateEnum()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(ENUM_PATH, "NewEnum.cs");
        }

        [MenuItem("Assets/Create/Scripting/AriUtils/Raw C# Class")]
        public static void CreateRawClass()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(RAWCLASS_PATH, "NewRawC#Class.cs");
        }

        [MenuItem("Assets/Create/Scripting/AriUtils/Custom Editor")]
        public static void CreateCustomEditor()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(CUSTOMEDITOR_PATH, "NewCustomEditor.cs");
        }

        [MenuItem("Assets/Create/Scripting/AriUtils/Interface")]
        public static void CreateInterface()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(INTERFACE_PATH, "NewInterface.cs");
        }
    }
}
