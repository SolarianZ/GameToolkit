using System;
using System.Collections.Generic;

namespace GBG.Framework.View.ActorManagement
{
    public sealed class ViewActorServerProxy : IViewActor
    {
        public object ResKey { get; set; }


        public void OnRecycleActor() { }

        public void OnUseActor() { }

        public void OnDestroyActor() => ResKey = null;
    }

    public sealed class ViewActorManagerServerProxy : IViewActorManager
    {
        public IReadOnlyList<object> LoadedActorReses => _loadedActorReses;
        public IReadOnlyList<object> LoadingActorReses => _loadingActorReses;

        private readonly object[] _loadedActorReses = new object[0];
        private readonly object[] _loadingActorReses = new object[0];

        private readonly Stack<IViewActor> _pool = new();


        public object GetActorResKey(int actorId) =>
            throw new InvalidOperationException("You should not access the Actor Res Key in the Server.");

        public bool IsActorResLoaded(int actorId) => true;

        public void LoadActorRes(int actorId) { }

        public void UnloadActorRes(int actorId) { }

        public IViewActor GetOrCreateActorInstance(int actorId)
        {
            IViewActor actor;
            if (_pool.Count > 0)
            {
                actor = _pool.Pop();
            }
            else
            {
                actor = new ViewActorServerProxy();
            }

            actor.OnUseActor();

            return actor;
        }

        public void RecycleActorInstance(IViewActor actor)
        {
            if (_pool.Contains(actor))
            {
                return;
            }

            actor.OnRecycleActor();
            _pool.Push(actor);
        }

        public void ClearActorInstancePool()
        {
            foreach (var actor in _pool)
            {
                actor.OnDestroyActor();
            }

            _pool.Clear();
        }
    }
}
