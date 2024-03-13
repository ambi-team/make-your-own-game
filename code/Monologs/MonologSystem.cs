using System;

namespace Monologs;

public class MonologSystem: Component
{
    // ! Обычные C# ивенты блять не работают между компонентами, и не ясно почему.
    // public event EventHandler<MonologResource> OnMonologAdded;
    // public event EventHandler<MonologResource> OnMonologStarted;
    // public event EventHandler OnMonologEnded; 
    
    
    [Property] private MonologUI _monologUI { get; set; }
    
    private Queue<MonologResource> _queue = new ();

    private SoundHandle _currentSoundHandle = null;
    private bool _isPlaying;


    public int QueueLength => _queue.Count;
    
    
    /// <summary>
    /// Добавляет монолог в очередь
    /// </summary>
    public void AddToQueue(MonologResource monolog)
    {
        _queue.Enqueue(monolog);
        Log.Info($"Monolog {monolog.VoiceSound.ResourceName} added to Queue.");
    }
    
    /// <summary>
    /// Удаляет все монологи из очереди
    /// </summary>
    public void ClearQueue(MonologResource monolog)
    {
        _queue.Clear();
    }
    

    /// <summary>
    /// Запускает вопроизведение очереди монологов
    /// </summary>
    public void Play()
    {
        _isPlaying = true;
    }
    
    /// <summary>
    /// Перебивает воспроизведение очереди и воспроизводит переданный монолог 
    /// </summary>
    public void PlayForce(MonologResource monolog)
    {
        if (_queue.Count == 0)
        {
            PlayMonolog(monolog);
        }
        else
        {
            if (_currentSoundHandle != null)
                _currentSoundHandle.Stop();
            
            PlayMonolog(monolog);
            
            Log.Info($"Monolog {monolog.VoiceSound.ResourceName} force played.");
        }
    }
    
    /// <summary>
    /// Останавливает воспроизведение очереди, не очищает очередь.
    /// </summary>
    /// <param name="force">Остановить ли воспроизведение очереди, не дав текущему монологу завершиться</param>
    public void Stop(bool force)
    {
        if (force && _currentSoundHandle != null && _currentSoundHandle.IsPlaying)
            _currentSoundHandle.Stop();
        
        _isPlaying = false;
    }
    
    
    private void PlayMonolog(MonologResource monolog)
    {
        _monologUI.Show(monolog.SubtitleText);
        _currentSoundHandle = Sound.Play(monolog.VoiceSound);
        
        Log.Info($"Monolog {monolog.VoiceSound.ResourceName} played.");
    }


    protected override void OnUpdate()
    {
        if (!_isPlaying)
            return;
        
        if (_currentSoundHandle != null && !_currentSoundHandle.IsPlaying)
        {
            if (QueueLength == 0)
            {
                _monologUI.Hide();
                _isPlaying = false;
                
                return;
            }
            
            _monologUI.Hide();
            
            var monolog = _queue.Dequeue();
            PlayMonolog(monolog);
        }
        else if (_currentSoundHandle == null && QueueLength > 0)
        {
            var monolog = _queue.Dequeue();
            PlayMonolog(monolog);
        }
    }
}