using System;
using UnityEngine;

namespace SOE.Elements {
  public abstract class ElementBase : ScriptableObject {

    public bool RestrictBlackBoard = false;
    
    public abstract object GetValue();
    public abstract Type GetType();
    public abstract void SetValue(object obj);
  }
}