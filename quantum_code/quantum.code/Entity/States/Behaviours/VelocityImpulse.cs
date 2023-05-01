using System;
using Photon.Deterministic;

namespace Quantum {
    public unsafe partial class VelocityImpulse : StateBehaviour {
        public FP Velocity;
        public FP Duration;

        public override unsafe void OnEnter(Frame f, AbilityEntity* abilityEnt, EntityRef ent) {
            f.Unsafe.TryGetPointer<CharacterController3D>(ent, out CharacterController3D* c);
            c->Velocity *= 2;
            c->MaxSpeed += 25;
            Log.Debug("Woosh");
            }

        public override void Update(Frame f, AbilityEntity* abilityEnt, EntityRef ent) {
            if (abilityEnt->controllerType == ControllerType.Platformer3D) {
                f.Unsafe.TryGetPointer<CharacterController3D>(ent, out CharacterController3D* c);
                c->Velocity.Y += Velocity * Duration;
                }



            }

        }
    }
