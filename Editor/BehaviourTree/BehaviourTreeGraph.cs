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
        private Dictionary<GraphNode, Node> _nodeMap = new Dictionary<GraphNode, Node>();
        public static void OpenWindow(BehaviourTree treeAsset)
        {
            BehaviourTreeGraph window = GetWindow<BehaviourTreeGraph>();
            window.titleContent = new GUIContent($"{treeAsset.name}");
            window.Init(treeAsset);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (_nodeMap == null)
            {
                _nodeMap = new Dictionary<GraphNode, Node>();
            }

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
            for (int i = 0; i < _treeAsset.EDITOR_Nodes.Count; i++)
            {
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
        /// Create a <see cref="ezutils.Editor.GraphNode"/> representing the given <see cref="ezutils.Runtime.BehaviourTree.Node"/>
        /// </summary>
        /// <param name="node">The node to be visualised</param>
        private void CreateNodeElement(Node node)
        {
            var pos = _mousePosition;
            if (_treeAsset.EDITOR_NodePositions.ContainsKey(node))
            {
                pos = _treeAsset.EDITOR_NodePositions[node];
            }

            var graphNode = new BTGraphNode(
                pos,
                _inSocketStyle,
                _outSocketStyle,
                OnClickInSocket,
                OnClickOutSocket,
                OnClickRemove);
            _nodes.Add(graphNode);
            _nodeMap[graphNode] = node;
        }

        protected override void OnClickRemove(GraphNode node)
        {
            base.OnClickRemove(node);
            _treeAsset.DeleteNode(_nodeMap[node]);
        }

        public void OnDisable()
        {

            //save the positions of the nodes when closing the graph window
            for (int i = 0; i < _nodes.Count; i++)
            {
                Node n = _nodeMap[_nodes[i]];
                _treeAsset.EDITOR_NodePositions[n] = _nodes[i].Rect.position;
            }
        }
    }
}