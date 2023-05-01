using Photon.Deterministic;

namespace Quantum.Platformer
{
    // simple system to prevent platform from moving if it's in the way of a player
    public class PlatformCollisionQueryInjectionSystem 
        : SystemMainThreadFilter<PlatformCollisionQueryInjectionSystem.Filter>
    {
        public unsafe struct Filter
        {
            public EntityRef EntityRef;
            public Transform3D* Transform3D;
            public Platform* Platform;
            public PhysicsCollider3D* Collider;
        }

        public override unsafe void Update(Frame f, ref Filter filter)
        {
            int mask = f.Layers.GetLayerMask("PlatformCharacter");

            FP yCorrected = filter.Transform3D->Position.Y - filter.Collider->Shape.Box.Extents.Y * 2;

            Shape3D shape = filter.Collider->Shape;

            Shape3D shape3D =
                Shape3D.CreateBox(new FPVector3(shape.Box.Extents.X, shape.Box.Extents.Y / 2, shape.Box.Extents.Z));

            FPVector3 position = new FPVector3(filter.Transform3D->Position.X, yCorrected,
                                               filter.Transform3D->Position.Z);

            Draw.WireBox(position, shape3D.Box.Extents, filter.Transform3D->Rotation, ColorRGBA.Magenta);

            var index = f.Physics3D.AddOverlapShapeQuery(position, filter.Transform3D->Rotation,
                                                         shape3D, mask);

            filter.Platform->QueryIndex = index;
        }
    }
}