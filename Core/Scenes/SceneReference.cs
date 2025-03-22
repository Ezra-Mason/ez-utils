using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

namespace ezutils.Scenes
{
    /// <summary>
    /// Wrapper for a runtime-safe serialization for <see cref="SceneAsset"/>
    /// </summary>
    [System.Serializable]
    public class SceneReference : ISerializationCallbackReceiver
    {
        [SerializeField] private Object _sceneAsset;
        [SerializeField] private string _scenePath = string.Empty;
        [SerializeField] private int _buildIndex = -1;
#if UNITY_EDITOR
        private bool IsValidSceneAsset => _sceneAsset != null && _sceneAsset is SceneAsset;
        private SceneAsset SceneAssetFromPath => string.IsNullOrEmpty(_scenePath) ? null : AssetDatabase.LoadAssetAtPath<SceneAsset>(_scenePath);
        private string ScenePathFromAsset => _sceneAsset == null ? string.Empty : AssetDatabase.GetAssetPath(_sceneAsset);
        private int BuildIndexFromAsset => _sceneAsset == null ? -1 : GetAssetBuildData(_sceneAsset).buildIndex;

        public struct AssetBuildData
        {
            public int buildIndex;
            public string assetPath;
            public GUID assetGUID;
            public EditorBuildSettingsScene buildSettingsScene;
        }
        public static AssetBuildData GetAssetBuildData(Object sceneAsset)
        {
            var buildData = new AssetBuildData { buildIndex = -1, assetGUID = new GUID(string.Empty) };

            if (sceneAsset == null) return buildData;
            if (!(sceneAsset is SceneAsset)) return buildData;

            buildData.assetPath = AssetDatabase.GetAssetPath(sceneAsset);
            buildData.assetGUID = new GUID(AssetDatabase.AssetPathToGUID(buildData.assetPath));

            var scenes = EditorBuildSettings.scenes;
            for (int i = 0; i < scenes.Length; i++)
            {
                if (!buildData.assetGUID.Equals(scenes[i].guid))
                    continue;
                buildData.buildSettingsScene = scenes[i];
                buildData.buildIndex = i;
                break;
            }
            return buildData;
        }
#endif

        //this is essentially called after any changes are made to the SceneReference in the editor
        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            // we cannot access AssetDatabase while serialization is in progress as its on another thread
            // so we delay the processing to the next editor update on the main thread
            EditorApplication.update += HandleAfterSerialization;
#endif
        }

        //this is called before the SceneReference in displayed in the editor
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            //is the asset invalid but we have a valid path to look it up with
            if (IsValidSceneAsset == false && string.IsNullOrEmpty(_scenePath) == false)
            {
                _sceneAsset = SceneAssetFromPath;
                if (!_sceneAsset)
                {
                    _scenePath = string.Empty;
                }
                EditorSceneManager.MarkAllScenesDirty();
            }
            else
            {
                _scenePath = ScenePathFromAsset;
                _buildIndex = BuildIndexFromAsset;
            }
#endif
        }

#if UNITY_EDITOR
        private void HandleAfterSerialization()
        {

            EditorApplication.update -= HandleAfterSerialization;

            if (IsValidSceneAsset) return;
            if (string.IsNullOrEmpty(_scenePath)) return;

            //the asset isnt valid but we can use the path to recover from
            _sceneAsset = SceneAssetFromPath;
            _buildIndex = BuildIndexFromAsset;

            //if the path was also invalid so we should discard it
            if (!_sceneAsset)
            {
                _scenePath = string.Empty;
            }

            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkAllScenesDirty();
            }
        }
#endif
    }


#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferencePropertyDrawer : PropertyDrawer
    {
        private const string SCENE_ASSET_FIELD = "_sceneAsset";
        private const string SCENE_PATH_FIELD = "_scenePath";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.isEditingMultipleObjects)
            {
                GUI.Label(position, "SceneReference does not support editing multiple objects");
                return;
            }

            SerializedProperty assetProperty = property.FindPropertyRelative(SCENE_ASSET_FIELD);

            Rect scenePos = new Rect(position);

            scenePos.height = position.height - EditorGUIUtility.standardVerticalSpacing * 2f;
            if (SceneReference.GetAssetBuildData(assetProperty.objectReferenceValue).assetGUID.Empty())
            {
                scenePos.height = position.height - EditorGUIUtility.standardVerticalSpacing * 2f;
            }
            else
            {
                scenePos.height = position.height / 2f - EditorGUIUtility.standardVerticalSpacing;
            }
            scenePos.y += EditorGUIUtility.standardVerticalSpacing;
            label = EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();
            //we only need to render the object field, the scene path and build index can be hidden from the user
            assetProperty.objectReferenceValue = EditorGUI.ObjectField(
                scenePos, 
                label, 
                assetProperty.objectReferenceValue,
                typeof(SceneAsset),
                false);

            var buildData = SceneReference.GetAssetBuildData(assetProperty.objectReferenceValue);
            if (EditorGUI.EndChangeCheck())
            {
                //if asset was changed to null, we need to discard the scene path to prevent the 
                //scene from before the change from being restored from the path
                if (buildData.buildSettingsScene == null)
                {
                    property.FindPropertyRelative(SCENE_PATH_FIELD).stringValue = string.Empty;
                }
            }


            if (!buildData.assetGUID.Empty())
            {
                Rect contentPosition = new Rect(scenePos);
                contentPosition.height -= EditorGUIUtility.standardVerticalSpacing;
                contentPosition.y += scenePos.height + EditorGUIUtility.standardVerticalSpacing;
                DrawSceneBuildData(contentPosition, buildData);
            }

        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty assetProperty = property.FindPropertyRelative(SCENE_ASSET_FIELD);

            var buildData = SceneReference.GetAssetBuildData(assetProperty.objectReferenceValue);
            if (!buildData.assetGUID.Empty())
            {
                return EditorGUIUtility.singleLineHeight * 2f + EditorGUIUtility.standardVerticalSpacing * 3f;
            }
            else
            {
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2f;
            }

        }
        /// <summary>
        /// Draw helpboxes to alert user about the state of the <see cref="SceneReference"/>'s asset in te build settings
        /// </summary>
        /// <param name="position"></param>
        /// <param name="buildData"></param>
        private void DrawSceneBuildData(Rect position, SceneReference.AssetBuildData buildData)
        {
            var labelContent = new GUIContent();
            int style = 0;
            if (buildData.buildIndex == -1)
            {
                labelContent.text = "SceneReference is for a Scene not in the build list";
                style = (int)MessageType.Error;
            }
            else if (buildData.buildSettingsScene.enabled)
            {
                labelContent.text = $"Build index: {buildData.buildIndex}";
                style = (int)MessageType.Info;
            }
            else
            {
                labelContent.text = $"Scene in build list with index: {buildData.buildIndex} but is disabled";
                style = (int)MessageType.Warning;
            }

            EditorGUI.HelpBox(position, labelContent.text, (MessageType)style);
        }

    }
#endif

}
