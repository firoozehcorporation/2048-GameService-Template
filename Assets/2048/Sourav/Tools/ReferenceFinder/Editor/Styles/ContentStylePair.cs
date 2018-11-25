using System;
using UnityEngine;

namespace Sourav.Utilities.Tools
{
    [Serializable]
    internal class ContentStylePair
    {
        public GUIContent Content = new GUIContent();
        public GUIStyle Style = new GUIStyle();
    }
}