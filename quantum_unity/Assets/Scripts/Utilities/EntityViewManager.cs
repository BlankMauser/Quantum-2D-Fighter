using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public unsafe sealed class EntityViewManager : EntityViewUpdater {
    public static EntityViewManager Instance => Singleton<EntityViewManager>.Instance;

    [Serializable]
    public sealed class PrototypePool {
        public EntityViewAsset Asset;
        public int DefaultCapacity;
        public ObjectPool<EntityView> ObjectPool;
        }

    [Serializable]
    public sealed class PrototypePoolGroup {
        public string Label;
        public List<PrototypePool> PrototypePools;
        }

    [SerializeField]
    private List<PrototypePoolGroup> _prototypePoolGroups = new();

    private Dictionary<EntityViewAsset, PrototypePool> _prototypePoolPairs = new();
    private Dictionary<EntityView, PrototypePool> _entityInstancePoolOwnershipPairs = new();

    private void OnEnable() {
        foreach (PrototypePoolGroup prototypePoolGroup in _prototypePoolGroups) {
            foreach (PrototypePool prototypePool in prototypePoolGroup.PrototypePools) {
                prototypePool.ObjectPool = new ObjectPool<EntityView>(
                createFunc: createFunc,
                actionOnGet: (entityViewInstance) => entityViewInstance.gameObject.SetActive(true),
                actionOnRelease: (networkObject) => networkObject.gameObject.SetActive(false),
                collectionCheck: false,
                defaultCapacity: prototypePool.DefaultCapacity);
                _prototypePoolPairs.Add(prototypePool.Asset, prototypePool);

                EntityView createFunc() {
                    var entityView = GameObject.Instantiate(prototypePool.Asset.View);
                    return entityView;
                    }
                }
            }
        }

    private void OnDisable() {
        DisposePools();

        void DisposePools() {
            foreach (PrototypePoolGroup prototypePoolGroup in _prototypePoolGroups)
                foreach (PrototypePool prefabPool in prototypePoolGroup.PrototypePools)
                    prefabPool.ObjectPool.Dispose();
            }
        }

    protected override EntityView CreateEntityViewInstance(EntityViewAsset asset, Vector3? position = null, Quaternion? rotation = null) {
        // Most object should be preloaded into Pools, with a default capacity set.
        // This is configured in the inspector in the Object Pool prefab in Resources.
        if (_prototypePoolPairs.TryGetValue(asset, out var prefabPool)) {
            var instance = prefabPool.ObjectPool.Get();
            _entityInstancePoolOwnershipPairs.TryAdd(instance, prefabPool);
            return instance;
            }
        // Just do the default create behaviour if pool does not exist.
        else
            return base.CreateEntityViewInstance(asset, position, rotation);
        }

    protected override void DestroyEntityViewInstance(EntityView instance) {
        if (_entityInstancePoolOwnershipPairs.TryGetValue(instance, out PrototypePool prefabPool)) {
            prefabPool.ObjectPool.Release(instance);
            _entityInstancePoolOwnershipPairs.Remove(instance);
            return;
            }
        else
            base.DestroyEntityViewInstance(instance);
        }
    }