using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using ezutils.Runtime;
using ezutils.Runtime.BehaviourTree;

namespace ezutils.Editor
{
    public class BehaviourTreeGraph : GraphEditor
    {
        private BehaviourTree _treeAsset;
        private Dictionary<GraphNode, Node> _nodeMap = new Dictionary<GraphNode, Node>();
        private Dictionary<Node, GraphNode> _graphMap = new Dictionary<Node, GraphNode>();
        private bool _initialised = false;
        private Dictionary<Type, StackPool<IGraphNode>> _typeGraphNodeMap = new Dictionary<Type, StackPool<IGraphNode>>();
        private StackPool<BTGraphNode> _nodePool;
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
            AssetDatabase.Refresh();

            if (_nodeMap == null)
            {
                _nodeMap = new Dictionary<GraphNode, Node>();
            }

        }
        private void Init(BehaviourTree treeAsset)
        {
            _treeAsset = treeAsset;
            _nodes = new List<GraphNode>();
            _typeGraphNodeMap[typeof(RootNode)] = new StackPool<IGraphNode>(() => new RootGraphNode(OnClickInSocket, OnClickOutSocket, OnClickRemove), OnGetNode, OnPutNode, 10);
            _typeGraphNodeMap[typeof(DecoratorNode)] = new StackPool<IGraphNode>(() => new DecoratorGraphNode(OnClickInSocket, OnClickOutSocket, OnClickRemove), OnGetNode, OnPutNode, 10);
            _typeGraphNodeMap[typeof(SequenceComposite)] = new StackPool<IGraphNode>(() => new SequenceGraphNode(OnClickInSocket, OnClickOutSocket, OnClickRemove), OnGetNode, OnPutNode, 10);
            _typeGraphNodeMap[typeof(TaskNode)] = new StackPool<IGraphNode>(() => new TaskGraphNode(OnClickInSocket, OnClickOutSocket, OnClickRemove), OnGetNode, OnPutNode, 10);
            _nodePool = new StackPool<BTGraphNode>(() => new BTGraphNode(OnClickInSocket, OnClickOutSocket, OnClickRemove), OnGetNode, OnPutNode, 10);
            PopulateTree();
            _initialised = true;
        }
        private void OnGetNode(IGraphNode node)
        {
            //Debug.Log("Got node from pool");
            //_nodes.Add(node as GraphNode);
        }
        private void OnPutNode(IGraphNode node)
        {
            //Debug.Log("put Node in pool");
            //_nodes.Remove(node as GraphNode);
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

            BTGraphNode graphNode;
            if (node as RootNode)
            {
                graphNode = _typeGraphNodeMap[typeof(RootNode)].Get() as RootGraphNode;
            }
            else if (node as SequenceComposite)
            {
                graphNode = _typeGraphNodeMap[typeof(SequenceComposite)].Get() as SequenceGraphNode;
            }
            else if (node as DecoratorNode)
            {
                graphNode = _typeGraphNodeMap[typeof(DecoratorNode)].Get() as DecoratorGraphNode;
            }
            else if (node as TaskNode)
            {
                graphNode = _typeGraphNodeMap[typeof(TaskNode)].Get() as TaskGraphNode;
            }
            else
            {
                Debug.LogError("Couldnt find valid graphNode to represent node");
                return;
            }
            if (graphNode != null)
            {

                graphNode.Header = ObjectNames.NicifyVariableName(node.name);
                graphNode.Move(position);
                _nodes.Add(graphNode);
                _nodeMap[graphNode] = node;
                _graphMap[node] = graphNode;
            }
            else
            {
                Debug.LogError($"graph node is null {node.name}");
            }
        }

        protected override void OnClickRemove(GraphNode node)
        {
            base.OnClickRemove(node);
            _treeAsset.DeleteNode(_nodeMap[node]);
        }
        protected override void ConnectSelection()
        {
            base.ConnectSelection();
            var inNode = _nodeMap[_selectedTargetSocket.Node];
            var outNode = _nodeMap[_selectedSourceSocket.Node];
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
            insoc.Connect();
            outsoc.Connect();
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
            AssetDatabase.SaveAssets();
            //_treeAsset.Save();
        }
    }
}