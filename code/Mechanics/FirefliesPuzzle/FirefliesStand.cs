using System;

namespace Sandbox.Mechanics.FirefliesPuzzle;

public class FirefliesStand: Component
{
    [Property] public List<FirefliesRow> Rows { get; set; }
    
    [Description("Сколько секунд даётся на выполнение головоломки? (укажите с учетом показа светлячков)")]
    [Property] public int TimeToComplete { get; set; }
    
    [Description("Сколько секунд будут отображаться светлячки")]
    [Property] public int TimeToFirefliesShow { get; set; }
    
    
    [Property] public Action OnComplete { get; set; }
    [Property] public Action OnFail { get; set; }


    public bool IsCompleted => _isCompleted;
    

    private float _timer;
    private bool _timerStarted;
    private bool _firefliesPlaying;

    private bool _isCompleted;

    public bool TimerStarted => _timerStarted;


    /// <summary>
    /// Запускает светлячков которые показывают правильные варианты
    /// </summary>
    public void Play()
    {
        foreach (var row in Rows)
        {
            row.StopChangeCells(true);
            
            row.RandomizeCorrectCell();
            row.PlayShowRightCell(TimeToFirefliesShow);
        }

        _timer += TimeToComplete;

        _timerStarted = true;
        _firefliesPlaying = true;
    }

    /// <summary>
    /// Переключает лампочку выбора ячейки в указанной строке
    /// </summary>
    public void SwitchChangeCells(int rowIndex)
    {
        if (!_timerStarted)
            return;
        
        var row = Rows[rowIndex];
        if (row.ChangeCurrentCellEnable)
        {
            row.StopChangeCells();
        }
        else
        {
            row.PlayChangeCells();
        }
    }


    private bool CheckRowsAllRight()
    {
        var rowsCount = Rows.Count;

        foreach (var row in Rows)
        {
            if (row.CurrentCellIndex == row.RightCellIndex && !row.ChangeCurrentCellEnable)
            {
                rowsCount -= 1;
            }
        }
        
        return rowsCount == 0;
    }

    
    protected override void OnUpdate()
    {
        if (_timerStarted)
        {
            if (_timer > 0)
            {
                _timer -= Time.Delta;
                
                if (CheckRowsAllRight())
                {
                    _timerStarted = false;
                    
                    _isCompleted = true;
                    OnComplete?.Invoke();
                    
                    Log.Info("Fireflies Puzzle Is Completed!");
                    
                    foreach (var row in Rows)
                    {
                        row.StopChangeCells(false);
                    }
                }
            }
            else
            {
                _timerStarted = false;

                if (CheckRowsAllRight())
                {
                    _isCompleted = true;
                    OnComplete?.Invoke();
                    
                    Log.Info("Fireflies Puzzle Is Completed!");
                    
                    foreach (var row in Rows)
                    {
                        row.StopChangeCells(false);
                    }
                }
                else
                {
                    _isCompleted = false;
                    OnFail?.Invoke();
                    
                    Log.Info("Fireflies Puzzle Is Failed!");
                    
                    foreach (var row in Rows)
                    {
                        row.StopChangeCells(true);
                    }
                }
            }
        }
    }
}