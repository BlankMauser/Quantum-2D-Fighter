using System;
using Photon.Deterministic;

namespace Quantum
{
    public unsafe partial struct Platform
    {
        // check if the curve should loop by checking if the accumulated time has exceeded the curve length
        private static bool ShouldLoopCurve(FP* accumulatedTime, FP curveLength)
        {
            return *accumulatedTime > curveLength;
        }

        // evaluate the given movement curve at a certain point in time, then multiply it's output by amplitude
        private static FP GetCurveEvaluation(FPAnimationCurve curve, FP accumulated, FP amplitude)
        {
            return curve.Evaluate(curve.StartTime + accumulated) * amplitude;
        }

        // get the delta between two curve evaluations
        private static FP GetDelta(FP result, FP lastEvaluation)
        {
            FP delta = result - lastEvaluation;
            return delta;
        }

        public static void Rotate(Frame f, Transform3D* transform3D, Platform* platform)
        {
            PlatformConfig config = f.FindAsset<PlatformConfig>(platform->Config.Id);
            
            FPAnimationCurve curve = config.RotationCurve;

            // if rotation amplitude is 0, then we won't move anyways, so just exit early
            if (config.RotationAmplitude == 0 || platform->CanMove == false)
                return;
            
            FP curveLength = curve.EndTime - curve.StartTime;
            
            if (ShouldLoopCurve(&platform->RotationAccumulatedTime, curveLength))
            {
                // reset the curve evaluations to the beginning
                platform->LastRotationCurveEvaluation = curve.Evaluate(curve.StartTime);
                platform->RotationAccumulatedTime     = f.DeltaTime;
            }

            // evaluate the new curve position
            FP result = curve.Evaluate(curve.StartTime + platform->RotationAccumulatedTime) * config.RotationAmplitude;
            FP delta  = result - platform->LastRotationCurveEvaluation;

            // euler angles so it's easier to understand
            FPVector3 eulerAngles = transform3D->Rotation.AsEuler;

            // apply the rotation delta to our rotation
            eulerAngles.Y         += delta;
            transform3D->Rotation =  FPQuaternion.Euler(eulerAngles);

            // save the current delta for next frame calculations
            FPQuaternion rotationDelta = transform3D->Rotation * FPQuaternion.Inverse(platform->PreviousRotation);

            platform->RotationDelta    = rotationDelta;
            platform->PreviousRotation = transform3D->Rotation;

            platform->LastRotationCurveEvaluation = result;
        }

        public static void Move(Frame f, Transform3D* transform3D, Platform* platform)
        {
            PlatformConfig config = f.FindAsset<PlatformConfig>(platform->Config.Id);
            
            PlatformAxis flags = config.MovementAxis; 

            // only calculate movement for the curves that are set in the config
            if (flags.IsFlagSet(PlatformAxis.X))
                MoveInternal(f, PlatformAxis.X, transform3D, platform, config);

            if (flags.IsFlagSet(PlatformAxis.Y))
                MoveInternal(f, PlatformAxis.Y, transform3D, platform, config);

            if (flags.IsFlagSet(PlatformAxis.Z))
                MoveInternal(f, PlatformAxis.Z, transform3D, platform, config);
            
            // save the position changes for next frame calculations
            platform->PositionDelta =
                transform3D->Position - platform->PreviousPosition;

            platform->PreviousPosition = transform3D->Position;
        }

        private static void MoveInternal(Frame f, PlatformAxis axis, Transform3D* transform3D, Platform* platform, PlatformConfig config)
        {
            // if movement amplitude is 0, we won't move anyways
            if (config.MovementAmplitude == 0 || platform->CanMove == false)
                return;
            
            FPAnimationCurve curve;

            QBoolean* reverseMovementPtr;
            FP*       accumulatedTimePtr;
            FP*       lastCurveEvaluationPtr;
            FP*       positionAxisPtr;

            // get the correct pointers for the axis we are operating on
            switch (axis)
            {
                case PlatformAxis.X:
                    curve                  = config.XMovementCurve;
                    accumulatedTimePtr     = &platform->XMovementAccumulatedTime;
                    reverseMovementPtr     = &platform->ReverseXMovement;
                    positionAxisPtr        = &transform3D->Position.X;
                    lastCurveEvaluationPtr = &platform->LastMovementCurveEvaluation.X;
                    break;

                case PlatformAxis.Y:
                    curve                  = config.YMovementCurve;
                    accumulatedTimePtr     = &platform->YMovementAccumulatedTime;
                    reverseMovementPtr     = &platform->ReverseYMovement;
                    positionAxisPtr        = &transform3D->Position.Y;
                    lastCurveEvaluationPtr = &platform->LastMovementCurveEvaluation.Y;
                    break;

                case PlatformAxis.Z:
                    curve                  = config.ZMovementCurve;
                    accumulatedTimePtr     = &platform->ZMovementAccumulatedTime;
                    reverseMovementPtr     = &platform->ReverseZMovement;
                    positionAxisPtr        = &transform3D->Position.Z;
                    lastCurveEvaluationPtr = &platform->LastMovementCurveEvaluation.Z;
                    break;
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }

            FP curveLength = curve.EndTime - curve.StartTime;

            if (ShouldLoopCurve(accumulatedTimePtr, curveLength))
            {
                // reset
                *accumulatedTimePtr = f.DeltaTime;

                // set bool flag to reverse movement
                QBoolean value = *reverseMovementPtr;
                *reverseMovementPtr = !value;

                // reset
                *lastCurveEvaluationPtr = curve.Evaluate(curve.StartTime);
            }

            FP result = GetCurveEvaluation(curve, *accumulatedTimePtr, config.MovementAmplitude);

            FP delta = GetDelta(result, *lastCurveEvaluationPtr);
            
            // reverse the delta before applying to reverse the movement once we've reached the end of the curve
            if (*reverseMovementPtr)
            {
                delta = -delta;
            }

            // store delta's for next frame calculations
            *positionAxisPtr        += delta;
            *lastCurveEvaluationPtr =  result;
        }
    }
}