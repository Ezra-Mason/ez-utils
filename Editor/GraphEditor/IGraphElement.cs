using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Editor
{
    public interface IGraphElement
    {

        /// <summary>
        /// Render this element to the window
        /// </summary>
        void DrawElement();
    }
}
