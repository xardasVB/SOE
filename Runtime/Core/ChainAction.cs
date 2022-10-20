using System;
using System.Threading;
using System.Threading.Tasks;
using Sirenix.OdinInspector;

namespace SOE.Core {
  [Serializable]
  public class ChainAction {

    [HideLabel] public SoeActionList ActionList;

    public async Task<bool> Execute(Action onFinish, BlackBoard bBoard, CancellationTokenSource source = null) {
      bool result = true;
      if (ActionList.ActionList.Count != 0) {
        result await ActionList.Execute(bBoard, source);
      }

      onFinish?.Invoke();
      return result;
    }
    
    public void ExecuteSync(BlackBoard bBoard) {
      ActionList.ExecuteSync(bBoard);
    }

    public string GetName() {
      return "ActionList";
    }
  }
}