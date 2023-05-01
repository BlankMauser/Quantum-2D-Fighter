using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum {
    public static unsafe class StateMachineManager {

        public static void UpdateStateMachine(Frame f, AbilityEntity* e, EntityRef ent) {
            f.Assets.State(e->FSM.CurrentState).UpdateState(f, e, ent);
            f.Assets.State(e->FSM.CurrentState).UpdateBehaviours(f, e, ent);

            }

        public static bool CheckTransitions(Frame f, AbilityEntity* e, EntityRef ent) {
            e->FSM.NextState = f.Assets.State(e->FSM.CurrentState);
            //Only return True if we're actually changing state.
            bool reset = false;
            foreach (AssetRefState s in f.Assets.StateSet(e->FSM.Moveset).BasicStates) {
                if (f.Assets.State(s).CanEnter(f, e, ent)) {
                    //Check For PriorityLevel before overwriting
                    if(f.Assets.State(s).PriorityLevel > f.Assets.State(e->FSM.NextState).PriorityLevel) {
                        e->FSM.NextState = s;
                        reset = true;
                        }
                    }
                }

            foreach (AssetRefState s in f.Assets.StateSet(e->FSM.Moveset).AttackStates) {
                if (f.Assets.State(s).CanEnter(f, e, ent)) {
                    //Check For PriorityLevel before overwriting
                    if (f.Assets.State(s).PriorityLevel > f.Assets.State(e->FSM.NextState).PriorityLevel) {
                        e->FSM.NextState = s;
                        reset = true;
                        }
                    }
                }

            return reset;
            }

        public static void StateChange(Frame f, AbilityEntity* e, EntityRef ent) {
            f.Assets.State(e->FSM.CurrentState).OnExitState(f, e, ent);
            e->FSM.PrevState = f.Assets.State(e->FSM.CurrentState);
            e->FSM.CurrentState = f.Assets.State(e->FSM.NextState);
            f.Assets.State(e->FSM.CurrentState).OnEnterState(f, e, ent);

            }


        }
    }
