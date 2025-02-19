using UnityEngine;
using UnityEngine.UI;

public abstract class LifeRunner : MonoBehaviour
{
    public abstract void Init(Vector2Int fieldSize);
    public abstract void CheckClick(Vector2 pointerPos);
    public abstract void Run();
}
