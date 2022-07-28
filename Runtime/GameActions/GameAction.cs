using System;
using System.Collections.Generic;
using System.Linq;
using SOE.Core;
using SOE.Elements;
using UnityEngine;

namespace SOE.GameActions {
  public class GameAction : UniqueId {

    public bool IsConditional;
    
    [SerializeField] protected List<ElementField> ElementsSet;

    public virtual bool Invoke<T>(SoeAction<T> actionRef, BlackBoard bBoard) where T : GameAction => true;
    public virtual bool IsFinished<T>(SoeAction<T> actionRef, BlackBoard bBoard) where T : GameAction => true;

    private List<ElementData> _currElements = new();
    
    public virtual List<ElementField> GetElements() => ElementsSet;
    
    public virtual ElementData[] ReadElements<T>(SoeAction<T> actionRef) where T : GameAction { 
      ElementData[] result = new ElementData[ElementsSet.Count];
      for (int i = 0; i < ElementsSet.Count; i++) {
        ElementData data = null;
        if (actionRef.GetStoredData(ElementsSet[i], ref data))
          result[i] = data;
      }
      return result;
    }
    
    public virtual T ReadElement<T, K>(SoeAction<K> actionRef, BlackBoard bBoard) where K : GameAction {
      if (_currElements.Count == 0)
        _currElements = ReadElements(actionRef).ToList();
      
      try {
        var el = _currElements.First();
        _currElements.RemoveAt(0);
        T res = el.UseBlackboard || el.IsBlackboard ? (T) bBoard.GetBlackboardValue(el) : el.GetValue<T>();
        return res;
      }
      catch (Exception e) {
        Debug.LogError(e.Message);
        return default;
      }
    }
    
    public virtual T SaveElement<T, K>(SoeAction<K> actionRef, BlackBoard bBoard, int id, T value) where K : GameAction {
      var elements = ReadElements(actionRef).ToList();
      bBoard.SaveBlackboardValue(elements[id].GetBBoardName(), value);
      return default;
    }
    
    public virtual T SaveToLastElement<T>(BlackBoard bBoard, T value) {
      bBoard.SaveBlackboardValue(_currElements.First().GetBBoardName(), value);
      _currElements.RemoveAt(0);
      return default;
    }

  }
}