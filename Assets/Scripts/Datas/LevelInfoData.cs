public class LevelInfoData
{
    public int currentLevel { get; private set; }
    public int nextExp { get; private set; }
    public int currentExp { get; private set; }

    public readonly int maxLevel;

    public LevelInfoData(int maxLevel)
    {
        this.maxLevel = maxLevel;
    }
    
    public void SaveLevelInfoData(int currentLevel, int nextExp, int currentExp)
    {
        this.currentLevel = currentLevel;
        this.nextExp = nextExp;
        this.currentExp = currentExp;
    }
}