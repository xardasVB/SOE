using System;

namespace SOE.GameEvents {
  [Serializable]
  public class EventContainer {
    public Action<object> OnComplete;
    public object Reciever;

    public EventContainer( object reciever, Action<object> act) {
      Reciever = reciever;
      OnComplete = act;
    }
    
  }
}