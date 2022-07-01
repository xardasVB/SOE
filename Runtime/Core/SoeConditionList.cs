using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif
using UnityEngine;

namespace SOE.Core {
  [Serializable]
  public class SoeConditionList {
    
    [HideInInspector]
    public List<string> BlackBoard;
    
    [ListDrawerSettings(ListElementLabelName = "@GetName()",NumberOfItemsPerPage = 100,CustomAddFunction = "CustomAddFunction")]
    public List<SoeCondition> ConditionList = new();
    
    public bool Check(object target, BlackBoard bBoard){
      bool isPostOrCheck = false;
      foreach (var condition in ConditionList){
        if (!condition.Check(target, bBoard)){
          if (!isPostOrCheck && (condition.CheckOperator == SoeCondition.CheckOperationE.And || ConditionList.IndexOf(condition) == ConditionList.Count - 1))
            return false;
        }
        else{
          isPostOrCheck = condition.CheckOperator == SoeCondition.CheckOperationE.Or;
        }
      }

      return true;
    }
    
    private void CustomAddFunction() {
      SoeCondition act = new SoeCondition();
      act.BlackBoard = BlackBoard;
      ConditionList.Add(act);
    }

    private void RefreshData() {
      ConditionList.ForEach(e=> {
        e.BlackBoard = BlackBoard;
        e.RefreshData();
      });
    }

#if UNITY_EDITOR
    private void DrawRefreshButton() {
      if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh)) {
        RefreshData();
      }
    }
#endif
    
  }
}