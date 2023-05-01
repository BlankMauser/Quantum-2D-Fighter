using Quantum.Platformer;

namespace Quantum
{
    public static class SystemSetup
    {
        public static SystemBase[] CreateSystems(RuntimeConfig gameConfig, SimulationConfig simulationConfig)
        {
            return new SystemBase[]
            {
                // all platform movement and rotation needs to happen BEFORE the physics system runs
                // this is to ensure that the KCC callbacks use the correct and up-to-date values.
                
                // platform systems
                new PlatformTimeAccumulatorSystem(),
                new PlatformMovementSystem(),
                new PlatformRotationSystem(),
                
                // player platform systems
                new PlatformInertiaSystem(),
                new PlatformFollowSystem(),

                new Core.CullingSystem3D(),
                new PlatformCollisionQueryInjectionSystem(),

                new Core.PhysicsSystem3D(),
                Core.DebugCommand.CreateSystem(),
                new Core.EntityPrototypeSystem(),
                new Core.PlayerConnectedSystem(),

                new PlatformCollisionQueryRetrievalSystem(),

                new PlayerSpawnSystem(),

                //Ability System Tentatively Goes Here
                new EntitySystem(),

                new MovementSystem(),
            };
        }
    }
}