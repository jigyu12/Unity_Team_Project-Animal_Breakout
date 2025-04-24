public class LevelUpInfoData
{
    public int currentLevel { get; private set; }
    public int nextExp { get; private set; }
    public int currentExp { get; private set; }

    public readonly int maxLevel;

    public LevelUpInfoData(int maxLevel)
    {
        this.maxLevel = maxLevel;
    }
    
    public void SaveLevelUpInfoData(int currentLevel, int nextExp, int currentExp)
    {
        this.currentLevel = currentLevel;
        this.nextExp = nextExp;
        this.currentExp = currentExp;
    }
}