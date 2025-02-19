using UnityEngine;
using UnityEngine.UI;

public class TextureLifeRunner : LifeRunner
{
    [SerializeField] private RawImage fieldRender;

    private int width, height;
    private byte[] currentStepData, nextStepData;
    private Texture2D texture;

    public override void Init(Vector2Int fieldSize)
    {
        width = fieldSize.x;
        height = fieldSize.y;
        fieldRender.rectTransform.sizeDelta = fieldSize;

        currentStepData = new byte[width * height];
        nextStepData = new byte[width * height];

        texture = new Texture2D(width, height, TextureFormat.Alpha8, false);
        texture.filterMode = FilterMode.Point;
        SetTexture(currentStepData);
        fieldRender.texture = texture;
    }
    private void SetTexture(byte[] data)
    {
        texture.SetPixelData(data, 0);
        texture.Apply();
    }

    public override void Run()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                byte neighbourCount = GetNeighbourCount(x, y);

                int index = GetIndex(x, y);
                if (IsCurrentCellAlive(index))
                    SetNextCellAlive(index, neighbourCount >= 2 && neighbourCount <= 3);
                else if (neighbourCount == 3)
                    SetNextCellAlive(index, true);
            }
        }

        System.Array.Copy(nextStepData, currentStepData, currentStepData.Length);
        SetTexture(currentStepData);
    }
    private byte GetNeighbourCount(int x, int y)
    {
        byte count = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int index = GetIndex(x + i, y + j);
                if (index < 0 || index >= currentStepData.Length)
                    continue;

                if (IsCurrentCellAlive(index))
                    count++;
            }
        }
        if (IsCurrentCellAlive(GetIndex(x, y)))
            count--;

        return count;
    }
    private int GetIndex(int x, int y) => x + y * height;

    private bool IsCurrentCellAlive(int i)
        => currentStepData[i] == byte.MaxValue;


    public override void CheckClick(Vector2 pointerPos)
    {
        Rect r = fieldRender.rectTransform.rect;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(fieldRender.rectTransform, pointerPos, null, out var localPoint);

        int px = (int)((localPoint.x - r.x) * width / r.width);
        int py = (int)((localPoint.y - r.y) * height / r.height);

        if (px >= 0 && px < r.width && py >= 0 && py < r.height)
            SetCurrentCellAlive(GetIndex(px, py), true);
    }
    public void SetCurrentCellAlive(int i, bool alive)
    {
        currentStepData[i] = alive ? byte.MaxValue : byte.MinValue;
        SetTexture(currentStepData);
        SetNextCellAlive(i, alive);
    }
    private void SetNextCellAlive(int i, bool alive)
        => nextStepData[i] = alive ? byte.MaxValue : byte.MinValue;
}
