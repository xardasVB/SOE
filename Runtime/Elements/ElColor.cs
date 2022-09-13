using System;
using UnityEngine;

namespace SOE.Elements {
  [CreateAssetMenu(fileName = "ElColor", menuName = "ScriptableData/SOE/Elements/ElColor")]
  public class ElColor : ElementBase {
    public Color Color;

    public override object GetValue() {
      return Color;
    }

    public override Type GetType() {
      return Color.GetType();
    }

    public override void SetValue(object obj) {
      Color = (Color) obj;
    }
    
  }
}