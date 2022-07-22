using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SOE.GameActions;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

namespace SOE.Core {
  [Serializable]
  public class SoeActionList : SoeActionList<GameAction> {
    public SoeActionList() { }
    public SoeActionList(SoeActionList actionList) {
      BlackBoard = new List<string>(actionList.BlackBoard);
      ActionList = new List<SoeAction<GameAction>>(actionList.ActionList);
    }
  }

  [Serializable]
  public class SoeActionList<T> where T : GameAction {
    [GUIColor(0.9f, 1f, 1f, 1f)] public List<string> BlackBoard = new();

    [ListDrawerSettings(ShowIndexLabels = false, Expanded = true, ListElementLabelName = "@GetName()",
      OnTitleBarGUI = "DrawRefreshButton", CustomAddFunction = "CustomAddFunction")]
    public List<SoeAction<T>> ActionList = new();

    private void CustomAddFunction() {
      SoeAction<T> act = new SoeAction<T>();
      act.SaveDataList = BlackBoard;
      ActionList.Add(act);
    }

    public SoeActionList() { }

    public SoeActionList(SoeActionList<T> actionList) {
      BlackBoard = new List<string>(actionList.BlackBoard);
      ActionList = new List<SoeAction<T>>(actionList.ActionList);
    }

#if UNITY_EDITOR
    private void DrawRefreshButton() {
      if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh)) {
        ActionList.ForEach(e => {
          e.SaveDataList = BlackBoard;
          e.RefreshData();

          e.ConditionList.BlackBoard = BlackBoard;
          e.ConditionList.ConditionList.ForEach(f => {
            f.BlackBoard = BlackBoard;
            f.RefreshData();
          });
        });
      }
    }
#endif

    public void ExecuteSync(BlackBoard bBoard, Func<SoeAction<T>, Boolean> condition = null,
      Action<SoeAction<T>> onExecute = null) {
      try {
        foreach (var action in ActionList) {
          if (condition != null && !condition.Invoke(action)) continue;
          if (!action.Execute(bBoard)) break;
          onExecute?.Invoke(action);
        }
      }
      catch (Exception e) {
        Console.WriteLine(e);
        throw;
      }
    }

    public async Task Execute(BlackBoard bBoard) {
      try {
        foreach (var action in ActionList) {
          bool result = action.Execute(bBoard);
          if (!result) break;
          while (!action.IsFinished(bBoard)) {
            await Task.Delay(25);
          }
        }
      }
      catch (Exception e) {
        Console.WriteLine(e);
        throw;
      }
    }
  }
}