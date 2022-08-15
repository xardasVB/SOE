using System;
using System.Threading.Tasks;
using SOE.GameActions;
using Sirenix.OdinInspector;

namespace SOE.Core {
  [Serializable]
  public class ChainAction {

    [HideLabel] public SoeActionList ActionList;

    public async Task Execute(Action onFinish, BlackBoard bBoard) {

      if (ActionList.ActionList.Count != 0) {
        await ActionList.Execute(bBoard);
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