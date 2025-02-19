using System;
using UnityEngine;
using UnityEngine.UI;

public class TextureLifeRunner : LifeRunner
{
    private readonly (sbyte,sbyte)[] neighbourOffsets = { (-1,1), (-1,0), (-1,-1), (0,1), (0,-1), (1,1), (1,0), (1,-1) };

    [SerializeField] private RawImage fieldRender;
    private Texture2D texture;
    private byte[] textureData;

    private int width, height;
    private bool[,] currentStepData;
    private byte[,] neighbours;

    public override void Init(Vector2Int fieldSize)
    {
        width = fieldSize.x;
        height = fieldSize.y;
        fieldRender.rectTransform.sizeDelta = fieldSize;

        currentStepData = new bool[width, height];
        neighbours = new byte[width, height];

        textureData = new byte[width * height];
        texture = new Texture2D(width, height, TextureFormat.Alpha8, false);
        texture.filterMode = FilterMode.Point;
        fieldRender.texture = texture;

        UpdateTexture();
    }
    private void UpdateTexture()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                textureData[x+y*height] = currentStepData[x,y] ? byte.MaxValue : byte.MinValue;
            }
        }
        
        texture.SetPixelData(textureData, 0);
        texture.Apply();
    }

    public override void Run()
    {
        CollectAllNeighboursCount();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                byte nCount = neighbours[x,y];
                if (currentStepData[x,y] )
                    currentStepData[x,y] = nCount >= 2 && nCount <= 3;
                else if (nCount == 3)
                    currentStepData[x,y] = true;
            }
        }
        
        Array.Clear(neighbours, 0, neighbours.Length);
        UpdateTexture();
    }
    private void CollectAllNeighboursCount()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (currentStepData[x,y])
                    incNeighbours(x, y);
            }
        }

        void incNeighbours(int x, int y)
        {
            foreach (var offset in neighbourOffsets)
            {
                int fX = x + offset.Item1;
                int fY = y + offset.Item2;
                if (fX >= 0 && fX < width && fY >= 0 && fY < height)
                    ++neighbours[x + offset.Item1, y + offset.Item2];
            }
        }
    }

    public override void CheckClick(Vector2 pointerPos)
    {
        Rect r = fieldRender.rectTransform.rect;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(fieldRender.rectTransform, pointerPos, null, out var localPoint);

        int px = (int)((localPoint.x - r.x) * width / r.width);
        int py = (int)((localPoint.y - r.y) * height / r.height);

        if (px >= 0 && px < r.width && py >= 0 && py < r.height)
        {
            currentStepData[px,py] = !currentStepData[px,py];
            UpdateTexture();
        }
    }
}
