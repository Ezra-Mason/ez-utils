using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ezutils.Editor
{

    public class NodeConnection : IGraphElement
    {
        private NodeSocket _in;
        private NodeSocket _out;
        private Action<NodeConnection> _onClick;
        private readonly float _size = 4;
        private readonly float _pickSize = 8;
        public NodeConnection(NodeSocket inSoc, NodeSocket outSoc, Action<NodeConnection> onClick)
        {
            _in = inSoc;
            _out = outSoc;
            _onClick = onClick;
        }
        // TODO: add option for bezier or straight lines
        public void DrawElement()
        {
            Handles.DrawBezier(
                _in.Rect.center,
                _out.Rect.center,
                _in.Rect.center + Vector2.left * 10f,
                _out.Rect.center - Vector2.left * 10f,
                Color.white,
                null,
                2f);

            var clicked = Handles.Button(
                (_in.Rect.center + _out.Rect.center) * 0.5f, 
                Quaternion.identity, 
                _size, 
                _pickSize,
                Handles.DotHandleCap);
            
            if (clicked && _onClick != null)
            {
                _onClick(this);
            }
        }
    }
}