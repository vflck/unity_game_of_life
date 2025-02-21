using System;
using UnityEngine;

public abstract class LifeRenderer : MonoBehaviour
{
    public Action<Vector2Int> onClick;
    public abstract void Init(Vector2Int fieldSize);
    public abstract void DisplayGen(bool[,] genData);
    public abstract void CheckClick(Vector2 pointerPos);
}
