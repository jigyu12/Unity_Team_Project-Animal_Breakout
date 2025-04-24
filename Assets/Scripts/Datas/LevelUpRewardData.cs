public class LevelUpRewardData
{
    public int staminaToAdd { get; private set; }
    public long goldToAdd { get; private set; }
    
    public void SaveLevelUpRewardData(int staminaToAdd, int goldToAdd)
    {
        this.staminaToAdd = staminaToAdd;
        this.goldToAdd = goldToAdd;
    }
}