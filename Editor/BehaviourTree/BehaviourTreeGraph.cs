using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ezutils.Runtime.BehaviourTree;
using System;

namespace ezutils.Editor
{
    public class BehaviourTreeGraph : GraphEditor
    {
        private BehaviourTree _treeAsset;

        public static void OpenWindow(BehaviourTree treeAsset)
        {
            BehaviourTreeGraph window = GetWindow<BehaviourTreeGraph>();
            window.titleContent = new GUIContent($"{treeAsset.name}");
            window.Init(treeAsset);
        }

        private void Init(BehaviourTree treeAsset)
        {
            _treeAsset = treeAsset;
            _nodes = new List<GraphNode>();
            PopulateTree();
        }

        protected override void OnGUI()
        {
            base.OnGUI();
        }

        private void PopulateTree()
        {
            Debug.Log("Populating tree...");
            for (int i = 0; i < _treeAsset.EDITOR_Nodes.Count; i++)
            {
                Debug.Log($"Creating element {i}/{_treeAsset.EDITOR_Nodes.Count}");
                var node = _treeAsset.EDITOR_Nodes[i];
                CreateNodeElement(node);
            }
        }



        protected override void ShowContextMenu()
        {
            var menu = new GenericMenu();
            // composites
            menu.AddSeparator("");
            menu.AddItem(new GUIContent($"Sequence"), false, () => CreateNodeOfType<SequenceComposite>());

            // decorators
            menu.AddSeparator("");
            var decorators = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            for (int i = 0; i < decorators.Count; i++)
            {
                menu.AddItem(new GUIContent($"{decorators[i].Name}"), false, () => CreateNodeOfType(decorators[i]));
            }

            // task
            menu.AddSeparator("");
            var taskNodes = TypeCache.GetTypesDerivedFrom<TaskNode>();
            for (int i = 0; i < taskNodes.Count; i++)
            {
                menu.AddItem(new GUIContent($"{taskNodes[i].Name}"), false, () => CreateNodeOfType(taskNodes[i]));
            }

            menu.ShowAsContext();
        }

        public void CreateNodeOfType(Type type)
        {
            var node = _treeAsset.CreateNode(type);
            CreateNodeElement(node);
        }
        private void CreateNodeOfType<T>() where T : Node
        {
            CreateNodeOfType(typeof(T));
        }


        /// <summary>
        /// Create the GraphNode representing 
        /// </summary>
        /// <param name="node"></param>
        private void CreateNodeElement(Node node)
        {
            _nodes.Add(
    new BTGraphNode(
        _mousePosition,
        200,
        50,
        _nodeStyle,
        _nodeSelectedStyle,
        _inSocketStyle,
        _outSocketStyle,
        OnClickInSocket,
        OnClickOutSocket,
        OnClickRemove
        )
    );
            Debug.Log("Created visual node ");

        }
    }
}