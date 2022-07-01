using System.Collections.Generic;
using System.Linq;
using SOE.Elements;
using SOE.GameActions;
using UnityEngine;

namespace SOE.Core {
  public static class Utility {
    
    //private Dictionary<string, int> nameType = new ();
    public static List<ElementData> RefreshElements(List<ElementData> sourceL, SoeConditionList condition, GameAction action, List<string> blackboard) {
      
      //sourceL.ForEach(e => e.ParseLegacy());
      //foreach (var c in condition.ConditionList)
      //  foreach (var e in c.ElementsDataList)
      //    e.ParseLegacy();
      
      sourceL.RemoveAll(e => e == null);
      
      //clear legacy
      sourceL.RemoveAll(e => action.GetElements().All(f => !ValidateType(f.ElementRef.GetType() ,e.ValueType.type)));
      if (action == null) {
        Debug.LogError("Action is null");
        return sourceL;
      }
      
      //add all missing
      action.GetElements().ForEach(e => {
        if (sourceL.All(f => !ValidateType(f.ValueType.type ,e.ElementRef.GetType()))) {
          ElementData data = new ElementData(e.ElementRef.GetType(), e.Id, e.ElementRef.GetValue());
          data.RestrictBlackBoard = e.ElementRef.RestrictBlackBoard;
          sourceL.Add(data);
        }
      });
      
      for (int i = 0; i < action.GetElements().Count; i++) {
        //if (i >= sourceL.Count) break;
        sourceL[i].Id = action.GetElements()[i].Id;
        sourceL[i].BlackBoardFields = blackboard;
        sourceL[i].RestrictBlackBoard = action.GetElements()[i].ElementRef.RestrictBlackBoard;
      }
      return sourceL;
    }

    private static bool ValidateType(System.Type a, System.Type b) {
      if (a == typeof(ElBValue)) return true;
      return a.IsDerivedTypeOf(b);
    }

    public static bool IsDerivedTypeOf(this System.Type type, System.Type baseType) {
      return baseType.IsAssignableFrom(type);
    }

  }
}