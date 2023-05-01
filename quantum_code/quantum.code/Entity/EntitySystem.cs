using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum {
    public unsafe class EntitySystem : SystemMainThreadFilter<EntitySystem.Filter> {

        public struct Filter {
            public EntityRef Entity;
            public AbilityEntity* abilityEntity;
            }

        public override void Update(Frame f, ref Filter filter) {

/*            f.Assets.State(filter.abilityEntity->FSM.CurrentState).UpdateBehaviours(f, filter.abilityEntity, filter.Entity);*/
            StateMachineManager.UpdateStateMachine(f, filter.abilityEntity, filter.Entity);

            //Check if any transitions are true
            if (StateMachineManager.CheckTransitions(f, filter.abilityEntity, filter.Entity)) {
                StateMachineManager.StateChange(f, filter.abilityEntity, filter.Entity);
                }

            }

        }
    }
