using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Quantum;

public unsafe class FsmView : MonoBehaviour
{
    [SerializeField]
    private AnimancerComponent _Animancer;
    public AnimancerComponent Animancer => _Animancer;

    private EntityView _view;
    public EntityRef _entityRef = default;
    private QuantumGame _game = null;

    public void Start() {
        _Animancer = GetComponent<AnimancerComponent>();
;
        }

    public void Initialize(QuantumGame gme) {
        _view = GetComponent<EntityView>();
        _entityRef = _view.EntityRef;
        _game = QuantumRunner.Default.Game;
        }

    public void Update() {
        UpdateAnimation();
        }

    public void UpdateAnimation() {
        var ent = _game.Frames.Verified.Unsafe.GetPointer<AbilityEntity>(_entityRef);
        StateAsset state = UnityDB.FindAsset<StateAsset>(ent->FSM.CurrentState.Id);

        var anim = _Animancer.Play(state.Animation);
        anim.NormalizedTime = (float)ent->AnimationFrame / (float)state.Settings.Length;

        }
}
