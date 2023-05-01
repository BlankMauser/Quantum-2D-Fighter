using System;
using Photon.Deterministic;

namespace Quantum {
    class GroundedCheck : T_Condition {

        public override unsafe bool TryCondition(Frame f, AbilityEntity* abilityEnt, EntityRef ent) {
            if (abilityEnt->controllerType == ControllerType.Platformer3D) {
                f.Unsafe.TryGetPointer<CharacterController3D>(ent, out CharacterController3D* c);
                return c->Grounded;
                } else {
                //placeholder
                return false;

                }
            }

        }
    }
