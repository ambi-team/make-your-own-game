using System.Text.Json.Serialization;

namespace Monologs;

[GameResource("Monolog", "monolog", "Звуковой файл озвучки с субтитрами.")]
public class MonologResource: GameResource
{
    public SoundEvent VoiceSound { get; init; }
    public string SubtitleText { get; init; }
}