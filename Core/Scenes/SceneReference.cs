using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

namespace ezutils
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
        private int BuildIndexFromAsset
        {
            get
            {
                var id = -1;
                if (_sceneAsset == null) return id;
                if (!(_sceneAsset is SceneAsset)) return id;
                
                var assetPath = AssetDatabase.GetAssetPath(_sceneAsset);
                var assetGUID = new GUID(AssetDatabase.AssetPathToGUID(assetPath));

                var scenes = EditorBuildSettings.scenes;
                for (int i = 0; i < scenes.Length; i++)
                {
                    if (!assetGUID.Equals(scenes[i].guid))
                        continue;
                    id = i;
                    break;
                }
                return id;
            }
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
}
