using System;
using UnityEngine;

namespace SOE.Elements {
  [CreateAssetMenu(fileName = "String", menuName = "ScriptableData/SOE/Elements/String")]
  public class ElString : ElementBase {
    public string String = String.Empty;

    public override object GetValue() {
      return String;
    }
    
    public override Type GetType() {
      return String.GetType();
    }
    
    public override void SetValue(object obj) {
      String = (String) obj;
    }

  }
}