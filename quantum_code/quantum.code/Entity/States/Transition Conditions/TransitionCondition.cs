using System;
using Photon.Deterministic;

namespace Quantum {

    [Serializable]
    public unsafe struct TransitionCheck {
        public T_Condition[] Conditions;

        public bool AllConditionsTrue(Frame f, AbilityEntity* abilityEnt, EntityRef ent) {
            bool pass = false;
            foreach(T_Condition c in Conditions) {
                if (c.TryCondition(f, abilityEnt, ent)) {
                    pass = true;
                    continue;
                    } else {
                    return false;
                    }
                }
            return pass;
            }

        }

    [Serializable]
    public unsafe partial class T_Condition {

        public virtual bool TryCondition(Frame f, AbilityEntity* abilityEnt, EntityRef ent) {
            return false;
            }

        }
    }
