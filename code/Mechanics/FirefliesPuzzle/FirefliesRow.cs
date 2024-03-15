using System;
public class FirefliesRow: Component
{
    [Property] public List<FirefliesCell> Cells { get; set; }

    [Description("Раз во сколько секунд будет переключаться ячейка на следующую")]
    [Property] public float CellChangeCooldown { get; set; }
    
    private int _currentCellIndex;
    private int _rightCellIndex;

    private float _changeCurrentCellTimer;
    private float _showRightCellTimer;

    private bool _changeCurrentCellEnable;
    private bool _showRightCellEnable;

    private Random _random = new Random();
    
    
    public int CurrentCellIndex => _currentCellIndex;
    public int RightCellIndex => _rightCellIndex;

    public bool ChangeCurrentCellEnable => _changeCurrentCellEnable;
    public bool ShowRightCellEnable => _showRightCellEnable;

    
    public void RandomizeCorrectCell()
    {
        _rightCellIndex = _random.Next(0, Cells.Count);
    }
    
    
    public void PlayShowRightCell(float showCorrectCellCooldown)
    {
        _showRightCellTimer = showCorrectCellCooldown;
        _showRightCellEnable = true;
        
        Cells[_rightCellIndex].SwitchParticles(true);
    }
    
    
    public void PlayChangeCells()
    {
        _currentCellIndex = -1;
        
        _changeCurrentCellTimer = 0;
        _changeCurrentCellEnable = true;
    }
    
    public void StopChangeCells(bool clearCurrentCell = false)
    {
        _changeCurrentCellTimer = 0;
        _changeCurrentCellEnable = false;

        if (clearCurrentCell)
        {
            _currentCellIndex = -1;
            
            foreach (var cell in Cells)
            {
                cell.SwitchLight(false);
            }
        }
    }


    protected override void OnStart()
    {
        foreach (var cell in Cells)
        {
            cell.SwitchLight(false);
            cell.SwitchParticles(false);
        }
    }

    
    protected override void OnUpdate()
    {
        if (_showRightCellEnable)
        {
            _showRightCellTimer -= Time.Delta;
            if (_showRightCellTimer < 0)
            {
                _showRightCellEnable = false;

                Cells[_rightCellIndex].SwitchParticles(false);
            }
        }

        if (_changeCurrentCellEnable)
        {
            _changeCurrentCellTimer -= Time.Delta;
            if (_changeCurrentCellTimer < 0)
            {
                if (_currentCellIndex != -1)
                    Cells[_currentCellIndex].SwitchLight(false);
                
                _currentCellIndex += 1;
                if (_currentCellIndex == Cells.Count)
                {
                    _currentCellIndex = 0;
                }
                
                Cells[_currentCellIndex].SwitchLight(true);
                
                _changeCurrentCellTimer = CellChangeCooldown;
            }
        }
    }
}