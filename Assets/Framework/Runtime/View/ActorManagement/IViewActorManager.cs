using System.Collections.Generic;

namespace GBG.Framework.View.ActorManagement
{
    public interface IViewActorManager
    {
        IReadOnlyList<object> LoadedActorReses { get; } // res key
        IReadOnlyList<object> LoadingActorReses { get; } // res key
        bool IsLoadingAnyActorReses => LoadingActorReses != null && LoadingActorReses.Count > 0;

        object GetActorResKey(int actorId);

        bool IsActorResLoaded(int actorId)
        {
            if (LoadedActorReses == null || LoadedActorReses.Count == 0)
            {
                return false;
            }

            var resKey = GetActorResKey(actorId);
            if (resKey == null)
            {
                return false;
            }

            for (int i = 0; i < LoadedActorReses.Count; i++)
            {
                if (LoadedActorReses[i] == resKey)
                {
                    return true;
                }
            }

            return false;
        }
        bool IsActorResLoading(int actorId)
        {
            if (LoadingActorReses == null || LoadingActorReses.Count == 0)
            {
                return false;
            }

            var resKey = GetActorResKey(actorId);
            if (resKey == null)
            {
                return false;
            }

            for (int i = 0; i < LoadingActorReses.Count; i++)
            {
                if (LoadingActorReses[i] == resKey)
                {
                    return true;
                }
            }

            return false;
        }
        void LoadActorRes(int actorId);
        void UnloadActorRes(int actorId);

        IViewActor GetOrCreateActorInstance(int actorId);
        void RecycleActorInstance(IViewActor actor);
        void ClearActorInstancePool();
    }
}
