using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SOE.Elements;
using SOE.GameActions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace SOE.Core {

  [Serializable]
  [ExecuteInEditMode]
  public class SoeAction : SoeAction<GameAction> {
    public static SoeAction MakeCopy<T>(SoeAction<T> action) where T : GameAction {
      SoeAction newAction = new SoeAction();
      newAction.SaveDataList = action.SaveDataList;
      newAction.GameActionRef = action.GameActionRef;
      newAction.ConditionList = action.ConditionList;
      newAction.ElementsDataList = action.ElementsDataList;
      return newAction;
    }
  }

  [Serializable] [ExecuteInEditMode]
  public class SoeAction<T> where T : GameAction {
    
    [HideInInspector]
    public List<string> SaveDataList;
    
    [HideLabel]
    [HorizontalGroup("Action")]
    [OnValueChanged("@RecreateData()")]
    public T GameActionRef;
    
    [HideLabel][ShowIf("@CheckShowCondition()")]
    public SoeConditionList ConditionList;
    
    [OdinSerialize] public List<ElementData> ElementsDataList = new();
    
    public IEnumerable <string> GetSlotList() {
      /*List<string> returnList = new List<string>();
      SaveDataList.ForEach(e=>returnList.Add(e.Name));*/
      return SaveDataList;
    }

    public bool GetStoredData(ElementField el, ref ElementData outEl) {
      if (outEl != null) return false;
      outEl = ElementsDataList.Find(e => el.Id.name == e.Id.name);
      return true;
    }   
    
    public string GetName() {
      return GameActionRef == null ? "Null" : GameActionRef.name;
      //return action;
    }
    
    private bool CheckShowCondition() {
      return GameActionRef != null && GameActionRef.IsConditional;
    }
    
    public bool Execute(BlackBoard bBoard) {
      return GameActionRef.Invoke(this, bBoard);
    }

    public async Task IsFinished(BlackBoard bBoard, CancellationTokenSource source) {
      await GameActionRef.IsFinished(this, bBoard, source);
    }

    public void RefreshData() {
      ElementsDataList = Utility.RefreshElements(ElementsDataList, ConditionList, GameActionRef,SaveDataList);
    }
    
    public void RecreateData() {
      ElementsDataList.Clear();
      if (GameActionRef == null) return;
      foreach (var el in GameActionRef.GetElements()) {
        ElementData element = new ElementData(el.ElementRef.GetType(), el.Id, el.ElementRef.GetValue());
        element.RestrictBlackBoard = el.ElementRef.RestrictBlackBoard;
        element.BlackBoardFields = SaveDataList;
        ElementsDataList.Add(element);
      }

      ConditionList.BlackBoard = SaveDataList;
      ConditionList.ConditionList.ForEach(e=>e.RefreshData());
    }
  }
}