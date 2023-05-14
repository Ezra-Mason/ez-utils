using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Editor
{
    public interface ISelectable
    {
        bool IsSelected { get; }
        void Select();
        void Deselct();
    }
}
