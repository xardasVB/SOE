using System;
using System.Threading;
using System.Threading.Tasks;
using Sirenix.OdinInspector;

namespace SOE.Core {
  [Serializable]
  public class ChainAction {

    [HideLabel] public SoeActionList ActionList;

    public async Task Execute(Action onFinish, BlackBoard bBoard, CancellationTokenSource source = null) {

      if (ActionList.ActionList.Count != 0) {
        await ActionList.Execute(bBoard, source);
      }

      onFinish?.Invoke();
    }
    
    public void ExecuteSync(BlackBoard bBoard) {
      ActionList.ExecuteSync(bBoard);
    }

    public string GetName() {
      return "ActionList";
    }
  }
}