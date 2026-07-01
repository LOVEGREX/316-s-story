using UnityEngine;

namespace Case316
{
    public static class SpriteFactory
    {
        static Sprite square;

        public static Sprite Square
        {
            get
            {
                if (square != null)
                {
                    return square;
                }

                var texture = new Texture2D(1, 1)
                {
                    filterMode = FilterMode.Point
                };
                texture.SetPixel(0, 0, Color.white);
                texture.Apply();
                square = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
                return square;
            }
        }
    }
}
