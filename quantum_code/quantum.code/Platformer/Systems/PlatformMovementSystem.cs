namespace Quantum.Platformer
{
    public unsafe class PlatformMovementSystem : SystemMainThreadFilter<PlatformMovementSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Platform* Platform;
            public Transform3D* Transform3D;
        }
        
        public override void Update(Frame f, ref Filter filter)
        {
            Platform.Move(f, filter.Transform3D, filter.Platform);
        }
    }
}