namespace Quantum.Platformer
{
    // retrieves the queries injected by PlatformCollisionQueryInjectionSystem and applies them
    public class PlatformCollisionQueryRetrievalSystem : SystemMainThreadFilter<PlatformCollisionQueryRetrievalSystem.Filter>
    {
        public unsafe struct Filter
        {
            public EntityRef EntityRef;
            public Transform3D* Transform3D;
            public Platform* Platform;
            public PhysicsCollider3D* Collider;
        }

        public override unsafe void Update(Frame f, ref Filter filter)
        {
            var hits = f.Physics3D.GetQueryHits(filter.Platform->QueryIndex);

            filter.Platform->CanMove = hits.Count == 0;
        }
    }
}