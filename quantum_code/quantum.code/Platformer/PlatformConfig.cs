using Photon.Deterministic;

namespace Quantum
{
    partial class PlatformConfig
    {
        // multipliers for movement and rotation curves
        public FP MovementAmplitude;
        public FP RotationAmplitude;
        
        // what axis' we should be operating on
        public PlatformAxis MovementAxis;

        // individual curves for each axis
        public FPAnimationCurve XMovementCurve;
        public FPAnimationCurve YMovementCurve;
        public FPAnimationCurve ZMovementCurve;
        
        // rotation Y axis curve
        public FPAnimationCurve RotationCurve;
    }
}