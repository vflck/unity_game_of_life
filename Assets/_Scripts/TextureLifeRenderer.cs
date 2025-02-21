using UnityEngine;
using UnityEngine.UI;

public class TextureLifeRenderer : LifeRenderer
{
    [SerializeField] private RectTransform fieldHolder;
    [SerializeField] private RawImage fieldRender;
    private Texture2D texture;
    private byte[] textureData;

    public override void Init(Vector2Int fieldSize)
    {
        textureData = new byte[fieldSize.x * fieldSize.y];
        texture = new Texture2D(fieldSize.x, fieldSize.y, TextureFormat.Alpha8, false);
        texture.filterMode = FilterMode.Point;
        fieldRender.texture = texture;

        fieldHolder.sizeDelta = fieldSize;

        DisplayGen(null);
    }

    public override void DisplayGen(bool[,] data)
    {
        if (data != null)
            updateTextureData();
        
        texture.SetPixelData(textureData, 0);
        texture.Apply();

        void updateTextureData()
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    textureData[x+y*height] = data[x,y] ? byte.MaxValue : byte.MinValue;
                }
            }
        }
    }

    public override void CheckClick(Vector2 pointerPos)
    {
        Rect r = fieldRender.rectTransform.rect;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(fieldRender.rectTransform, pointerPos, null, out var localPoint);

        int px = (int)((localPoint.x - r.x) * texture.width / r.width);
        int py = (int)((localPoint.y - r.y) * texture.height / r.height);

        if (onClick != null)
            onClick.Invoke(new (px,py));
    }
}
