using System;
using UnityEngine;

namespace SOE.Elements {
  [CreateAssetMenu(fileName = "Vector2", menuName = "ScriptableData/SOE/Elements/Vector2")]
  public class ElVector2 : ElementBase {
    public Vector2 Vector2 = Vector2.zero;

    public override object GetValue() {
      return Vector2;
    }
    
    public override Type GetType() {
      return Vector2.GetType();
    }
    
    public override void SetValue(object obj) {
      Vector2 = (Vector2) obj;
    }
    
  }
}