using System;
using UnityEngine;

namespace SOE.Elements {
  [CreateAssetMenu(fileName = "ElBValue", menuName = "ScriptableData/SOE/Elements/ElBValue")]
  public class ElBValue : ElementBase {
    //public string BValue = String.Empty;

    public override object GetValue() {
      return null;
    }
    
    public override Type GetType() {  
      return typeof(ElBValue);
    }
    
    public override void SetValue(object obj) {
      //String = (String) obj;
    }

  }
}