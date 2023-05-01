using Photon.Deterministic;

namespace Quantum.Platformer
{
    public unsafe class PlatformFollowSystem : SystemMainThreadFilter<PlatformFollowSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform3D;
            public PlayerPlatformController* PlatformController;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            // if we are colliding with a platform this frame
            if (filter.PlatformController->CollidingWithPlatform)
            {
                FollowPlatform(f, filter);
            }

            // reset for next collision
            filter.PlatformController->CollidingWithPlatform = false;
        }

        private static void FollowPlatform(Frame f, Filter filter)
        {
            FPVector3 platformPosition = f.Get<Transform3D>(filter.PlatformController->Entity).Position;
            FPVector3 positionDelta    = f.Get<Platform>(filter.PlatformController->Entity).PositionDelta;
       
            filter.PlatformController->ApplyPosition(filter.Transform3D, positionDelta);
            filter.PlatformController->ApplyRotation(filter.Transform3D, platformPosition);
        }
    }
}