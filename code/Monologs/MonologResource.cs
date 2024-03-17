using System.Text.Json.Serialization;
using Sandbox.Localization;

[GameResource("Monolog", "monolog", "Звуковой файл озвучки с субтитрами.")]
public class MonologResource: GameResource
{
    public SoundEvent VoiceSound { get; init; }
    public LocalizedStrings SubtitleText { get; init; }
}