using System.Collections.Generic;
using SOE.Core;
using SOE.Elements;

namespace SOE.GameActions {
  public abstract class GameAction : UniqueId {

    public bool IsConditional;
    
    public abstract bool Invoke<T>(SoeAction<T> actionRef, BlackBoard bBoard) where T : GameAction;
    public abstract List<ElementField> GetElements();
    public virtual bool IsFinished<T>(SoeAction<T> actionRef, BlackBoard bBoard) where T : GameAction { return true; }
  }
}