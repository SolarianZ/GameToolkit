namespace GBG.GameToolkit.Ability.Buff
{
    public interface IBuffManager
    {
        int AttachBuffToTarget(int buffConfigId);
        bool DetachBuffFromTarget(int buffInstanceId);
        int GetBuffOverlapCount(int buffConfigId);
    }
}