using System;
using Photon.Deterministic;

namespace Quantum {

    [System.Serializable]
    public class ButtonCheck {
        public InputButtons Button;
        public HoldType holdType;

        public bool Check(Input inp) {

            if (holdType == HoldType.Pressed) {
                switch (Button) {
                    case InputButtons.Jump:
                        return inp.Jump.WasPressed;
                    case InputButtons.Attack:
                        return inp.Attack.WasPressed;
                    case 0:
                        return false;

                    }

                }

            return false;
            }

        }

    public enum HoldType {
        Pressed,
        Held,
        Released,
        }
    public static unsafe class InputManager {

        //Used for Angle Calculation
        public static FP FP_90 { get { return FP._9 * FP._10; } }
        public static FP FP_180 { get { return FP_90 * FP._2; } }
        public static FP FP_360 { get { return FP_180 * FP._2; } }

        public static Input InputLink(Frame f, EntityRef e) {
            Input input = default;
            if (f.Unsafe.TryGetPointer(e, out PlayerLink* playerLink)) {
                input = *f.GetPlayerInput(playerLink->Player);
                }
            return input;
            }

        public static FP LsAngle(Input inp) {
            return FPMath.Atan2(inp.LeftStick.X, inp.LeftStick.Y);
            }

        public static FP RsAngle(Input inp) {
            return FPMath.Atan2(inp.RightStick.X, inp.RightStick.Y);
            }

        // Loops the value t, so that it is never larger than length and never smaller than 0.
        public static FP Repeat(FP t, FP length) {
            return FPMath.Clamp(t - FPMath.Floor(t / length) * length, FP._0, length);
            }

        // Calculates the shortest difference between two given angles. in degrees
        public static FP DeltaAngle(FP current, FP target) {
            FP delta = Repeat((target - current), FP_360);
            if (delta > FP_180)
                delta -= FP_360;
            return delta;
            }

        public static bool WithinRange(FP inputAngle, FP quadrantAngle, FP range) {
            return FPMath.Abs(DeltaAngle(inputAngle * FP.Rad2Deg, quadrantAngle)) <= range;
            }

        public static bool StickInQuadrant(FP axisAngle, FP quadrantAngle, FP range) {
            return WithinRange(axisAngle, quadrantAngle, range);
            }

        public static FP SlopeAngleRange(FPAnimationCurve slope, FP distance, FP angleMin, FP angleMax) {
            return (slope.Evaluate(distance) * (angleMax - angleMin)) + angleMin;

            }

        }
    }
