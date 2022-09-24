using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.UI
{
    public static class SpriteExtensions
    {
        public static UnityEngine.Sprite none => UnityEngine.Sprite.Create(Texture2D.normalTexture, new Rect(0, 0, 1, 1), Vector2.zero);
    }
}
