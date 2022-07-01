using System;
using System.Collections.Generic;
using System.Text;
using SOE.Elements;
using SOE.GameConditions;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif
using UnityEngine;
using UnityEngine.Serialization;

namespace SOE.Core {
  [Serializable]
  [ExecuteInEditMode]
  public class SoeCondition {
    [OnValueChanged("@RecreateData()")] [HideLabel]
    [HorizontalGroup("Condition")] public GameCondition GameConditionRef;
    
    [FormerlySerializedAs("SaveDataList")] [HideInInspector]
    public List<string> BlackBoard;
    
    public List<ElementData> ElementsDataList = new();
    
    [EnumToggleButtons, HideLabel]public ReturnResultE ConditionsMet = ReturnResultE.True;
    [EnumToggleButtons, HideLabel]public CheckOperationE CheckOperator = CheckOperationE.And;
    
    public bool Check(object target, BlackBoard bBoard) {
      if (GameConditionRef == null) return false;

      bool result = GameConditionRef.Check(this, target, bBoard) == EnumToBoolean();
      return result;
    }

#if UNITY_EDITOR
    private void RefreshButton() {
      if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh)) {
        RefreshData();
      }
    }
#endif
    
    public bool GetStoredData(ElementField el, ref ElementData outEl) {
      if (outEl != null) return false;
      outEl = ElementsDataList.Find(e => el.Id.name == e.Id.name);
      return true;
    }  
    
    public void RefreshData() {
     //ElementsDataList = Utility.RefreshElements(ElementsDataList,GameConditionRef.GetElements());
    }

    public void RecreateData() {
      ElementsDataList.Clear();
      if (GameConditionRef == null) return;
      foreach (var el in GameConditionRef.GetElements()) {
        ElementData element = new ElementData(el.ElementRef.GetType(), el.Id, el.ElementRef.GetValue());
        element.RestrictBlackBoard = el.ElementRef.RestrictBlackBoard;
        element.BlackBoardFields = BlackBoard;
        ElementsDataList.Add(element);
      }
    }
    
    public bool EnumToBoolean() {
      return ConditionsMet == ReturnResultE.True;
    }
    
    public string GetName() {
      StringBuilder str = new StringBuilder();

      /*if (Inverted)
        str.Append("!");*/
      
      if (GameConditionRef == null)
        str.Append("Null");
      else {
        str.Append(GameConditionRef.name);
        str.Append(" == ");
        str.Append(ConditionsMet);

        if (CheckOperator == CheckOperationE.Or) {
          str.Append(" | OR ▾");
        }
      }
      return str.ToString();
    }

    public enum ReturnResultE {
      True,
      False
    }    
    public enum InvertedE {
      Inverted,
      Original
    }
    
    public enum CheckOperationE {
      And,
      Or
    }
  }
}