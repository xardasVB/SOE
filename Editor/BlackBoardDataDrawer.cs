#if UNITY_EDITOR
using System.Collections.Generic;
using SOE.Core;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace SOE.Editor {
  public class BlackBoardDataDrawer : OdinValueDrawer<List<BlackBoardData>> {
    protected override void DrawPropertyLayout(GUIContent label) {
      ValueEntry.SmartValue.ForEach(e => {
        e.DrawEditor();
      });
    }
    
  }
}
#endif