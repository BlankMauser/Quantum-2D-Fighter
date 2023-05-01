using Photon.Deterministic;

namespace Quantum
{
    public partial struct PlayerPlatformController
    {
        public unsafe void ApplyPosition(Transform3D* transform3D, FPVector3 delta)
        {
            transform3D->Position += delta;
        }
        
        public unsafe void ApplyRotation(Transform3D* transform3D, FPVector3 pivot)
        {
            // delta rotation
            FPQuaternion platformDeltaRotation = PlatformDeltaRotation;

            // move + rotate around the given pivot as the center (the platform center usually)
            transform3D->Position =
                platformDeltaRotation * (transform3D->Position - pivot) +
                pivot;

            transform3D->Rotation =
                platformDeltaRotation * transform3D->Rotation;
        }
    }
}