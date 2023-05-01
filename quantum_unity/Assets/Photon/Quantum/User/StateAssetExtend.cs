using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quantum;
using Animancer;

[System.Serializable]
public class ConditionBlock {

    [SerializeReference]
    [SerializeReferenceDropdown]
    public T_Condition[] transitions;

    }

public partial class StateAsset
{
    [SerializeReference] [SerializeReferenceDropdown]
    public StateBehaviour[] behaviours;

    public ConditionBlock[] entryConditions;
    public TransitionCheck[] buffer;

    [SerializeField]
    public ClipTransition Animation;

    [SerializeField]
    public string Name;

    public override void PrepareAsset() {
        var changer = (State)AssetObject;
        changer.Behaviours = behaviours;

        int index = 0;
        foreach(ConditionBlock cb in entryConditions) {
            TransitionCheck tc;
            tc.Conditions = cb.transitions;
            buffer[index] = tc;
            index++;
            }
        changer.EntryChecks = buffer;
        changer.Name = Name;
        /*        Debug.Log(Settings.Name);*/


        }

    }
