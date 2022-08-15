using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using SOE.Core;
using SOE.Elements;
using UnityEngine;

namespace SOE.GameConditions {
  public class GameCondition : ScriptableObject {

    [LabelText("Game Object")] public List<ElementField> ElementsSet = new ();
    private List<ElementData> _currElements = new();
    
    public ElementData GetLinked(List<ElementData> data) {
      return data.Find(e => GetElements().Any(f => f.Id.name == e.Id.name));
    }
    public virtual bool Check(SoeCondition condition, object obj, BlackBoard bBoard) => true;
    public virtual List<ElementField> GetElements() => ElementsSet;
    
    public virtual ElementData[] ReadElements(SoeCondition condition) { 
      ElementData[] result = new ElementData[ElementsSet.Count];
      for (int i = 0; i < ElementsSet.Count; i++) {
        ElementData data = null;
        if (condition.GetStoredData(ElementsSet[i], ref data))
          result[i] = data;
      }
      return result;
    }
    
    public virtual T ReadElement<T>(SoeCondition condition, BlackBoard bBoard) {
      if (_currElements.Count == 0)
        _currElements = ReadElements(condition).ToList();
      
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
    
    public virtual T SaveElement<T>(SoeCondition condition, BlackBoard bBoard, int id, T value) {
      var elements = ReadElements(condition).ToList();
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