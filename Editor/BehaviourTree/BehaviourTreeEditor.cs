using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ezutils.Runtime.BehaviourTree;
namespace ezutils.Editor
{
    [CustomEditor(typeof(BehaviourTree))]
    public class BehaviourTreeEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            BehaviourTree tree = (BehaviourTree)target;
            base.OnInspectorGUI();


            if (GUILayout.Button("Edit Tree"))
            {
                BehaviourTreeGraph.OpenWindow(tree);
            }
        }
    }
}
