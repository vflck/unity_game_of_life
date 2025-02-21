using System;
using UnityEngine;

public struct LifeInfo
{
    public uint AliveCount;
    public uint CurrentGen;
}

public class LifeRunner
{
    public LifeInfo Info => liveInfo;

    private readonly (sbyte,sbyte)[] neighbourOffsets = { (-1,1), (-1,0), (-1,-1), (0,1), (0,-1), (1,1), (1,0), (1,-1) };
    private readonly int width, height;
    private readonly bool[,] currentGenData;
    private readonly byte[,] neighbours;

    private LifeInfo liveInfo;
    private readonly LifeRenderer lifeRenderer;

    public LifeRunner(Vector2Int fieldSize, LifeRenderer renderer)
    {
        liveInfo = new LifeInfo();

        width = fieldSize.x;
        height = fieldSize.y;
        lifeRenderer = renderer;

        currentGenData = new bool[width, height];
        neighbours = new byte[width, height];
        lifeRenderer.Init(fieldSize);
        lifeRenderer.onClick += TrySwitchCell;
    }

    public void Run()
    {
        liveInfo.AliveCount = 0;

        CollectAllNeighboursCount();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                byte nCount = neighbours[x,y];
                if (currentGenData[x,y])
                {
                    currentGenData[x,y] = nCount >= 2 && nCount <= 3;
                    liveInfo.AliveCount++;
                }
                else if (nCount == 3)
                {
                    currentGenData[x,y] = true;
                }
            }
        }

        liveInfo.CurrentGen++;
        lifeRenderer.DisplayGen(currentGenData);
        Array.Clear(neighbours, 0, neighbours.Length);
    }
    private void CollectAllNeighboursCount()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (currentGenData[x,y])
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

    public void TrySwitchCell(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height)
        {
            currentGenData[pos.x, pos.y] = !currentGenData[pos.x, pos.y];
            lifeRenderer.DisplayGen(currentGenData);
        }
    }
}
