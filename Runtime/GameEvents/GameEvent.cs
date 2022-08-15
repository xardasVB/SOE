using System;
using System.Collections.Generic;
using SOE.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SOE.GameEvents {
  [CreateAssetMenu(fileName = "newGameEvent", menuName = "ScriptableData/SOE/Game Events/GameEvent")]
  public partial class GameEvent : ScriptableObject {
    
    //check all already subscribed 
    [ShowInInspector] [NonSerialized] [ReadOnly]
    private List<EventContainer> EventContainerList = new();

    public void Check(object reciever, object data) {
      for (int i = EventContainerList.Count - 1; i >= 0; i--) {
        if(i > EventContainerList.Count - 1) continue;
        var eventContainer = EventContainerList[i];
        if (eventContainer.Reciever == null || reciever == eventContainer.Reciever || reciever == null) {
          eventContainer.OnComplete.Invoke(data);
        }
      }
    }

    public void Subscribe(object reciever, Action<object> act) {
      EventContainerList.Add(new EventContainer(reciever, act));
    }

    public void Unsubscribe(object reciever, Action<object> act) {
      EventContainerList.RemoveAll(e => e.Reciever == reciever && e.OnComplete == act);
    }

    public void Clear() {
      EventContainerList.Clear();
    }

  } 
}