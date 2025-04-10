

public interface ISkill
{
    public int Level
    {
        get;
    }

    public bool IsReady
    {
        get;
    }

    void Perform(IAttacker attacker, IDamagerable target);
    void UpgradeLevel();
}
