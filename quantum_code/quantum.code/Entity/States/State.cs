using System;
using Photon.Deterministic;
using System.Linq;
using System.Collections.Generic;

namespace Quantum {

    //Agnostic basic state template
    public unsafe partial class State {
        public string Name;
        public StateBehaviour[] Behaviours;
        public StateTags[] Tags;
        public TransitionCheck[] EntryChecks;
        public int PriorityLevel;

        #region Animation Parameters
        public int Length;
        public bool OverrideAnimation;
        public bool Loop;
        #endregion



        public virtual void UpdateBehaviours(Frame f, AbilityEntity* abilityEnt, EntityRef ent) {
            foreach (StateBehaviour sb in Behaviours) {
                sb.Update(f, abilityEnt, ent);
                }

            }

        public virtual void OnEnterState(Frame f, AbilityEntity* abilityEnt, EntityRef ent) {
            abilityEnt->AnimationFrame = 0;
            foreach (StateBehaviour sb in Behaviours) {
                sb.OnEnter(f, abilityEnt, ent);
                }


            }

        public virtual void UpdateState(Frame f, AbilityEntity* abilityEnt, EntityRef ent) {
            abilityEnt->AnimationFrame += 1;

            if (abilityEnt->AnimationFrame > Length) {
                if (Loop) {
                    abilityEnt->AnimationFrame = 0;
                    } else {
                    abilityEnt->AnimationFrame = Length;
                    OnAnimFinish(f, abilityEnt, ent);
                    }
                

                }

            }

        public virtual void OnExitState(Frame f, AbilityEntity* abilityEnt, EntityRef ent) {

            }

        public virtual void OnAnimFinish(Frame f, AbilityEntity* abilityEnt, EntityRef ent) {

            }

        public virtual bool CanEnter(Frame f, AbilityEntity* abilityEnt, EntityRef ent) {
            foreach(TransitionCheck tc in EntryChecks) {
                if(tc.AllConditionsTrue(f, abilityEnt, ent)) {
                    return true;
                    }
                }
            return false;
            }

        }






    }
