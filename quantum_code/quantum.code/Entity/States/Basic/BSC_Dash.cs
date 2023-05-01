using System;
using Photon.Deterministic;

namespace Quantum {
    [System.SerializableAttribute()]
    public unsafe partial class BSC_Dash : State {

        public FP Initial_Velocity;

        public override unsafe void OnEnterState(Frame f, AbilityEntity* abilityEnt, EntityRef ent) {
            base.OnEnterState(f, abilityEnt, ent);
            f.Unsafe.TryGetPointer<CharacterController3D>(ent, out CharacterController3D* c);
            c->Velocity *= Initial_Velocity;
            c->MaxSpeed = Initial_Velocity;

            }
        public override void UpdateState(Frame f, AbilityEntity* abilityEnt, EntityRef ent) {
            base.UpdateState(f, abilityEnt, ent);
            }



        }
    }
