using System;
using UnityEngine;

namespace SOE.Elements {
  [CreateAssetMenu(fileName = "ElBool", menuName = "ScriptableData/SOE/Elements/ElBool")]
  public class ElBool : ElementBase {
    public bool Bool;

    public override object GetValue() {
      return Bool;
    }

    public override Type GetType() {
      return Bool.GetType();
    }

    public override void SetValue(object obj) {
      Bool = (bool) obj;
    }
    
  }
}