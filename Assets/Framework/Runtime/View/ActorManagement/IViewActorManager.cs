using System.Collections.Generic;
using System.Linq;

namespace GBG.Framework.View.ActorManagement
{
    public interface IViewActorManager
    {
        IReadOnlyList<int> LoadedActorReses { get; }
        IReadOnlyList<int> LoadingActorReses { get; }
        bool IsLoadingAnyActorReses => LoadingActorReses != null && LoadingActorReses.Count > 0;

        bool IsActorResLoaded(int actorId) => LoadedActorReses?.Contains(actorId) ?? false;
        bool IsActorResLoading(int actorId) => LoadingActorReses?.Contains(actorId) ?? false;
        void LoadActorRes(int actorId);
        void UnloadActorRes(int actorId);
       
        IViewActor GetOrCreateActorInstance(int actorId);
        void RecycleActorInstance(IViewActor actor);
        void ClearActorInstancePool();
    }
}
