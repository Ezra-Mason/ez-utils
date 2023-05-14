using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace ezutils.Editor
{
    public class GraphNode
    {
        private Rect _rect;
        private GUIStyle _style;
        private string _title;
        public GraphNode(Vector2 position, float width, float height, GUIStyle nodeStyle)
        {
            _rect = new Rect(position.x, position.y, width, height);
            _style = nodeStyle;
        }

        public void Move(Vector2 delta)
        {
            _rect.position += delta;
        }

        public void Draw()
        {
            GUI.Box(_rect, _title, _style);
        }
        /// <summary>
        /// Process input events
        /// </summary>
        /// <param name="e"></param>
        /// <returns>Should the GUI be repainted</returns>
        public bool ProcessEvents(Event e)
        {
            return false;
        }
    }
}
