using System;
using System.Collections.Generic;
using SOE.Elements;
using Sirenix.Serialization;

namespace SOE.Core {
  [Serializable]
  public class BlackBoard  {
    
    [OdinSerialize]
    public List<BlackBoardData> Fields = new();
    
    public void SaveBlackboardValue(string key, object value) {
      Fields.Find(e => e.Name.Equals(key)).SetObj(value);
    }   
    private object GetBlackboardValue(string key) {
      return Fields.Find(e => e.Name.Equals(key)).Obj;
    }    
    public object GetBlackboardValue(ElementData el) {
      if (el == null) return null;
      return GetBlackboardValue(el.GetBBoardName());
    }

    public static void InitBlackBoard(BlackBoard bBoard, SoeActionList actionList) {
      bBoard.Fields.Clear();
      foreach (var s in actionList.BlackBoard) {
        bBoard.Fields.Add(new BlackBoardData(s));
      }
    }
  }
}