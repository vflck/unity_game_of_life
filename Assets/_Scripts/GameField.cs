using UnityEngine;

public class GameField : MonoBehaviour
{
    [SerializeField, Min(1)] private Vector2Int fieldSize = Vector2Int.one * 10;
    [SerializeField] private LifeRunner lifeRunner;
    [Space]
    [SerializeField, Min(1)] private int updateRate = 24;
    [SerializeField] private bool isRunning;
    private uint currentGen = 0;

    private void OnValidate()
    {
        if (Application.isPlaying)
            SetUpdateRate();
    }
    private void Start()
    {
        ClearField();
        SetUpdateRate();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            lifeRunner.CheckClick(Input.mousePosition);
    }
    private void FixedUpdate()
    {
        if (isRunning == false)
            return;

        lifeRunner.Run();
        currentGen++;
    }



    public void ClearField()
    {
        lifeRunner.Init(fieldSize);
        currentGen = 0;
    }

    public void SetUpdateRate()
    {
        Time.fixedDeltaTime = 1f / updateRate;
    }
}
