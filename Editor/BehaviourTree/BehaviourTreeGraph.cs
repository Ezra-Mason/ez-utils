using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            if (_treeAsset.EDITOR_RootNode != null)
            {
                _nodes.Add(
                    new GraphNode(
                       new Vector2(0f, 0f),
                       200,
                       50,
                       _nodeStyle,
                       _nodeSelectedStyle,
                       _inSocketStyle,
                       _outSocketStyle,
                       OnClickInSocket,
                       OnClickOutSocket,
                       OnClickRemove));

            }
        }

        protected override void OnGUI()
        {
            base.OnGUI();
        }
    }
}