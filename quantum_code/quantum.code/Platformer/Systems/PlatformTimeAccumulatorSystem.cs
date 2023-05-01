namespace Quantum.Platformer
{
    // simple system to track time for each platform
    // used to calculate curve evaluations
    public unsafe class PlatformTimeAccumulatorSystem : SystemMainThreadFilter<PlatformTimeAccumulatorSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Platform* Platform;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            // if we cannot move, do not accumulate time
            // if we did, it would cause incorrect movement due to the curve evaluations being wrong
            if(filter.Platform->CanMove == false)
                return;
            
            filter.Platform->XMovementAccumulatedTime += f.DeltaTime;
            filter.Platform->YMovementAccumulatedTime += f.DeltaTime;
            filter.Platform->ZMovementAccumulatedTime += f.DeltaTime;
            
            filter.Platform->RotationAccumulatedTime  += f.DeltaTime;
        }
    }
}