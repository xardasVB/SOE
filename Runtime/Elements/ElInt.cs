using System;
using UnityEngine;

namespace SOE.Elements {
  [CreateAssetMenu(fileName = "ElInt", menuName = "ScriptableData/SOE/Elements/ElInt")]
  public class ElInt : ElementBase {
    public int Int;

    public override object GetValue() {
      return Int;
    }

    public override Type GetType() {
      return Int.GetType();
    }

    public override void SetValue(object obj) {
      Int = (int) obj;
    }
    
  }
}