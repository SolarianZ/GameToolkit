namespace GBG.GameToolkit.Ability.Buff
{
    public interface IBuffManager
    {
        int AttachBuffToTarget(int buffConfigId);
        bool DetachBuffFromTarget(int buffInstanceId);
        int GetBuffOverlapCount(int buffConfigId);
    }

    public class DefaultBuffManager //: IBuffManager, IPropertiesContainer, IFlagsContainer, ITickable
    {
        private readonly IBuffFactory _buffFactory;


        public DefaultBuffManager(IBuffFactory buffFactory)
        {
            _buffFactory = buffFactory;
        }
    }
}