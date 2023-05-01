namespace Quantum
{
    public partial class PlatformControllerConfig
    {
        public bool ApplyRotationInertia = true;
        
        // what axis' we should apply inertia to 
        public PlatformAxis PlatformAxisInertia = PlatformAxis.X | PlatformAxis.Y | PlatformAxis.Z;
    }
}