namespace Riftcore.Gameplay.Skills.Stats
{
    public interface IStatProvider
    {
        bool TryGetStatValue(StatType statType, out float value);
    }
}