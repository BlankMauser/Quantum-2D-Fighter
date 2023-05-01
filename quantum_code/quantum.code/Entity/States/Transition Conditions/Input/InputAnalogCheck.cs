using System;
using Photon.Deterministic;

namespace Quantum {

    public class InputAnalogCheck : T_Condition {

        public bool CheckLeftStick;
        public FP LeftAxisDirection;
        public FP LeftAxisAngleMax;
        public FP LeftAxisAngleMin;
        public FP LeftThreshholdMin;
        public FP LeftThreshholdMax;
        public FPAnimationCurve LeftSlope;
        public bool LeftMirror_x;

        public bool CheckRightStick;
        public FP RightAxisDirection;
        public FP RightAxisAngleMax;
        public FP RightAxisAngleMin;
        public FP RightThreshholdMin;
        public FP RightThreshholdMax;
        public FPAnimationCurve RightSlope;
        public bool RightMirror_x;

        public ButtonCheck[] Buttons;


        public override unsafe bool TryCondition(Frame f, AbilityEntity* abilityEnt, EntityRef ent) {
            Input input = InputManager.InputLink(f, ent);
            bool AllInputs = false;

            if (CheckLeftStick) {
                var length = input.LeftStick.SqrMagnitude;
                //Check if within threshhold
                if (length > LeftThreshholdMin && length < LeftThreshholdMax) {
                    FP slopedAngle = InputManager.SlopeAngleRange(LeftSlope, (length-LeftThreshholdMin)/(LeftThreshholdMax-LeftThreshholdMin), LeftAxisAngleMin, LeftAxisAngleMax);
                    if (InputManager.StickInQuadrant(InputManager.LsAngle(input), LeftAxisDirection, slopedAngle)) {
                        AllInputs = true;
                        } else {
                        return false;
                        }
                    } else {
                    return false;
                    }
                }

            if (CheckRightStick) {
                var length = input.RightStick.SqrMagnitude;
                //Check if within threshhold
                if (length > RightThreshholdMin && length < RightThreshholdMax) {
                    FP slopedAngle = InputManager.SlopeAngleRange(RightSlope, (length - RightThreshholdMin) / (RightThreshholdMax - RightThreshholdMin), RightAxisAngleMin, RightAxisAngleMax);
                    if (InputManager.StickInQuadrant(InputManager.RsAngle(input), RightAxisDirection, slopedAngle)) {
                        AllInputs = true;
                        }
                    else {
                        return false;
                        }
                    }
                else {
                    return false;
                    }
                }

            foreach(ButtonCheck bc in Buttons) {
                if (bc.Check(input)) {
                    AllInputs = true;
                    Log.Debug("Attack=0");
                    } else {
                    return false;
                    }
                }


            return AllInputs;
            }

        }
    }
