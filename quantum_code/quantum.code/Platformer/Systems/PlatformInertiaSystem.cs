using Photon.Deterministic;

namespace Quantum.Platformer
{
    public unsafe class PlatformInertiaSystem : SystemMainThreadFilter<PlatformInertiaSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public CharacterController3D* KCC;
            public Transform3D* Transform3D;
            public PlayerPlatformController* Controller;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            // calculate the amount of frames it's been since we've touched a platform
            var diff = f.Number - filter.Controller->LastFrameCollided;

            PlatformControllerConfig config = f.FindAsset<PlatformControllerConfig>(filter.Controller->Config.Id);

            // if it's been less than one frame
            // or if we are grounded
            // or if we are still colliding
            // -> do nothing, we don't need to apply inertia yet
            if (diff < 1 || filter.KCC->Grounded || filter.Controller->CollidingWithPlatform)
                return;
            
            FPVector3 velocity = filter.Controller->LastVelocity;

            FPVector3 delta = FPVector3.Zero;

            // calculate the delta movement from the last known velocity
            // only apply the specified axis' that are set in our config
            
            if (config.PlatformAxisInertia.IsFlagSet(PlatformAxis.X))
                delta.X += velocity.X;

            if (config.PlatformAxisInertia.IsFlagSet(PlatformAxis.Y))
                delta.Y += velocity.Y;

            if (config.PlatformAxisInertia.IsFlagSet(PlatformAxis.Z))
                delta.Z += velocity.Z;
            
            // modify our rotation and position 
            filter.Controller->ApplyPosition(filter.Transform3D, delta);

            // check if config specifies rotation inertia 
            // ensure that the platform entity still exists and is valid
            if (config.ApplyRotationInertia && filter.Controller->Entity.IsValid)
                filter.Controller->ApplyRotation(filter.Transform3D, filter.Controller->LastPosition);
        }
    }
}