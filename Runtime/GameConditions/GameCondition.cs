using System.Collections.Generic;
using System.Linq;
using SOE.Core;
using SOE.Elements;

namespace SOE.GameConditions {
  public abstract class GameCondition : UniqueId {

    public ElementData GetLinked(List<ElementData> data) {
      return data.Find(e => GetElements().Any(f => f.Id.name == e.Id.name));
    }
    public abstract bool Check(SoeCondition condition, object obj, BlackBoard bBoard);
    public abstract List<ElementField> GetElements();
  }
}