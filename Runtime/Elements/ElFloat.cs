using System;
using UnityEngine;

namespace SOE.Elements {
  [CreateAssetMenu(fileName = "ElFloat", menuName = "ScriptableData/SOE/Elements/Float")]
  public class ElFloat : ElementBase{
    public float Float;

    public override object GetValue() {
      return Float;
    }

    public override Type GetType() {
      return Float.GetType();
    }

    public override void SetValue(object obj) {
      Float = (float) obj;
    }
    
  }
}