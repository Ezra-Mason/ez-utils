using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using ezutils.Runtime.BehaviourTree;

namespace ezutils.Editor
{
    public class BehaviourTreeGraph : GraphEditor
    {
        private BehaviourTree _treeAsset;
        private Dictionary<GraphNode, Node> _nodeMap = new Dictionary<GraphNode, Node>();
        private Dictionary<Node, GraphNode> _graphMap = new Dictionary<Node, GraphNode>();
        private bool _initialised = false;

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is BehaviourTree)
            {
                BehaviourTreeGraph.OpenWindow(Selection.activeObject as BehaviourTree);
                return true;
            }
            return false;
        }
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
            _initialised = true;
        }

        protected override void OnGUI()
        {
            base.OnGUI();

            if (!_initialised) return;

            for (int i = 0; i < _nodes.Count; i++)
            {
                _treeAsset.EDITOR_Nodes[i].GraphPosition = _nodes[i].Rect.position;
            }
        }

        private void PopulateTree()
        {
            if (_treeAsset.EDITOR_RootNode == null)
            {
                var root = _treeAsset.CreateNode(typeof(RootNode));
                _treeAsset.SetRootNode(root);
            }

            for (int i = 0; i < _treeAsset.EDITOR_Nodes.Count; i++)
            {
                var node = _treeAsset.EDITOR_Nodes[i];
                CreateNodeElement(node, node.GraphPosition);
            }

            for (int i = 0; i < _nodes.Count; i++)
            {
                var child = _treeAsset.EDITOR_Nodes[i];
                GraphNode c = _graphMap[child];

                var parent = child.Parent;
                if (parent == null) continue;

                GraphNode p = _graphMap[parent];

                ConnectNodes(c, p);
            }

        }

        protected override void ShowContextMenu()
        {
            var menu = new GenericMenu();
            if (_treeAsset.EDITOR_RootNode == null)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent($"Root Node"), false, () => CreateNodeOfType(typeof(RootNode)));
            }

            // composites
            menu.AddSeparator("");
            menu.AddItem(new GUIContent($"Sequence"), false, () => CreateNodeOfType(typeof(SequenceComposite)));

            // decorators
            menu.AddSeparator("");
            var decorators = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in decorators)
            {
                menu.AddItem(new GUIContent($"{type.Name}"), false, () => CreateNodeOfType(type));
            }

            // task
            menu.AddSeparator("");
            var taskNodes = TypeCache.GetTypesDerivedFrom<TaskNode>();
            foreach (var type in taskNodes)
            {
                menu.AddItem(new GUIContent($"{type.Name}"), false, () => CreateNodeOfType(type));
            }

            menu.ShowAsContext();
        }

        public void CreateNodeOfType(Type type)
        {
            var node = _treeAsset.CreateNode(type);
            CreateNodeElement(node, _mousePosition);
        }

        /// <summary>
        /// Create a <see cref="ezutils.Editor.GraphNode"/> representing the given <see cref="ezutils.Runtime.BehaviourTree.Node"/>
        /// </summary>
        /// <param name="node">The node to be visualised</param>
        private void CreateNodeElement(Node node, Vector2 position)
        {

            var comp = node as CompositeNode;
            BTGraphNode graphNode;
            if (comp)
            {
                graphNode = new CompositeGraphNode(
                    position,
                    _inSocketStyle,
                    _outSocketStyle,
                    OnClickInSocket,
                    OnClickOutSocket,
                    OnClickRemove);
            }
            else
            {

                graphNode = new BTGraphNode(
                    position,
                    _inSocketStyle,
                    _outSocketStyle,
                    OnClickInSocket,
                    OnClickOutSocket,
                    OnClickRemove);
            }
            _nodes.Add(graphNode);
            _nodeMap[graphNode] = node;
            _graphMap[node] = graphNode;
        }

        protected override void OnClickRemove(GraphNode node)
        {
            base.OnClickRemove(node);
            _treeAsset.DeleteNode(_nodeMap[node]);
        }
        protected override void ConnectSelection()
        {
            base.ConnectSelection();
            var inNode = _nodeMap[_selectedInSocket.Node];
            var outNode = _nodeMap[_selectedOutSocket.Node];
            Debug.Log($"in node {inNode} setting its parent to {outNode}");
            _treeAsset.Connect(inNode, outNode);
        }

        protected void ConnectNodes(GraphNode inNode, GraphNode outNode)
        {
            if (_connections == null)
            {
                _connections = new List<NodeConnection>();
            }
            var insoc = inNode.InSocket;
            var outsoc = outNode.OutSocket;
            _connections.Add(
                new NodeConnection(insoc, outsoc, OnClickConnection)
                );
        }

        protected override void OnClickConnection(NodeConnection connection)
        {
            Node node = _nodeMap[connection.In.Node];
            _treeAsset.Connect(node, null);
            Debug.Log("clicked connection");
            base.OnClickConnection(connection);
        }


        public void OnDisable()
        {
            //save the positions of the nodes when closing the graph window
            for (int i = 0; i < _nodes.Count; i++)
            {
                _treeAsset.EDITOR_Nodes[i].GraphPosition = _nodes[i].Rect.position;

            }
        }
    }
}