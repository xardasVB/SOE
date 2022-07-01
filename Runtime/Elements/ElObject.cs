using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SOE.Elements {
  [CreateAssetMenu(fileName = "Object", menuName = "ScriptableData/SOE/Elements/Object")]
  public class ElObject : ElementBase {
    public Object obj;

    public override object GetValue() {
      return obj;
    }
    
    public override Type GetType() {
      return obj.GetType();
    }
    
    public override void SetValue(object value) {
      obj = (Object) value;
    }
  }
}