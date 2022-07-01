using System;
using UnityEngine;

namespace SOE.Elements {
  [CreateAssetMenu(fileName = "Vector3", menuName = "ScriptableData/SOE/Elements/Vector3")]
  public class ElVector3 : ElementBase {
    public Vector3 Vector3 = Vector3.zero;

    public override object GetValue() {
      return Vector3;
    }
    
    public override Type GetType() {
      return Vector3.GetType();
    }
    
    public override void SetValue(object obj) {
      Vector3 = (Vector3) obj;
    }
    
  }
}