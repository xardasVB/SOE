using UnityEngine;
using System;

namespace SOE.Elements {
  [CreateAssetMenu(fileName = "ElAnimCurve", menuName = "ScriptableData/SOE/Elements/AnimCurve")]
  public class ElAnimCurve : ElementBase{
    public AnimationCurve value;

    public override object GetValue() {
      return value;
    }

    public override Type GetType() {
      return typeof(AnimationCurve);
    }

    public override void SetValue(object obj) {
      value = (AnimationCurve) obj;
    }
    
  }
}