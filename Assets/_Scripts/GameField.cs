using UnityEngine;

public class GameField : MonoBehaviour
{
    [SerializeField, Min(1)] private Vector2Int fieldSize = Vector2Int.one * 10;
    [SerializeField] private LifeRenderer lifeRenderer;
    [Space]
    [SerializeField, Min(1)] private int updateRate = 24;
    [SerializeField] private bool isAutoRun;

    private LifeRunner lifeRunner;

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
            lifeRenderer.CheckClick(Input.mousePosition);
    }
    private void FixedUpdate()
    {
        if (isAutoRun == false)
            return;
        
        MoveToNextGen();
    }
    private void MoveToNextGen()
    {
        lifeRunner.Run();
    }


    public void ClearField()
    {
        lifeRunner = new LifeRunner(fieldSize, lifeRenderer);
    }
    public void MakeStepAndSetPause()
    {
        MoveToNextGen();
        isAutoRun = false;
    }

    public void TogglePause()
    {
        isAutoRun = !isAutoRun;
    }
    public void SetUpdateRate()
    {
        Time.fixedDeltaTime = 1f / updateRate;
    }
}
