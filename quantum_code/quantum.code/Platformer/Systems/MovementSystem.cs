using Photon.Deterministic;
using Quantum.Core;
using Quantum.Physics3D;

namespace Quantum.Platformer
{
    public unsafe class MovementSystem : SystemMainThreadFilter<MovementSystem.Filter>, IKCCCallbacks3D
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform3D;
            public CharacterController3D* CharacterController;
            public PlayerPlatformController* PlatformController;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            Input input = default;

            if (f.Unsafe.TryGetPointer(filter.Entity, out PlayerLink* playerLink))
            {
                input = *f.GetPlayerInput(playerLink->Player);
            }

            FPVector3 direction = input.Direction.XOY;

            filter.CharacterController->Move(f, filter.Entity, direction, this);
            
            // note: pointer property access via -> instead of .
            if (input.Jump.WasPressed)
            {
                filter.CharacterController->Jump(f);
            }

            // look at the direction we are moving
            if (direction != default)
                filter.Transform3D->Rotation = FPQuaternion.LookRotation(direction);
        }

        public bool OnCharacterCollision3D(FrameBase f, EntityRef character, Hit3D hit)
        {
            PlayerPlatformController* controller = f.Unsafe.GetPointer<PlayerPlatformController>(character);

            // check if we are colliding with a platform
            if (f.Unsafe.TryGetPointer(hit.Entity, out Platform* platform))
            {
                Transform3D* transform3D = f.Unsafe.GetPointer<Transform3D>(character);

                CharacterController3D* kcc = f.Unsafe.GetPointer<CharacterController3D>(character);

                CharacterController3DConfig config = f.FindAsset(kcc->Config);

                FPVector3 contactToCenter = (transform3D->Position + config.Offset - hit.Point).Normalized;

                FP angle = FPVector3.Angle(-config.GravityNormalized, contactToCenter);
                
                // true = ground
                if (angle <= config.MaxSlope)
                {
                    controller->LastVelocity          = platform->PositionDelta;
                    controller->PlatformDeltaRotation = platform->RotationDelta;
                    controller->Entity                = hit.Entity;
                    
                    controller->LastFrameCollided = f.Number;
                    controller->LastPosition      = f.Get<Transform3D>(hit.Entity).Position;

                    controller->CollidingWithPlatform = true;
                }
            }
            else
            {
                // reset values if not actively colliding
                controller->PlatformDeltaRotation = FPQuaternion.Identity;
                controller->LastVelocity          = FPVector3.Zero;
            }

            return true;
        }

        public void OnCharacterTrigger3D(FrameBase f, EntityRef character, Hit3D hit)
        {
        }
    }
}