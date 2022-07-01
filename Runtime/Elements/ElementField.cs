using System;
using SOE.Core;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace SOE.Elements {
  [Serializable]
  public class ElementField {
    [HorizontalGroup("Element")][HideLabel]
    public ElementBase ElementRef;
    [FormerlySerializedAs("Name")] [HorizontalGroup("Element")][HideLabel]
    public FieldID Id;
  }
}