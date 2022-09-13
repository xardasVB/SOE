using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SOE.Core;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif

namespace SOE.Elements {
  
  [Serializable]
  public class ElementData {
    protected internal class IgnoreDraw : Attribute { }

    [SerializeField]
    [IgnoreDraw] public SerializableType ValueType;
    [IgnoreDraw] public FieldID Id;

    [IgnoreDraw] public bool IsBlackboard => ValueType.type?.Name == "ElBValue";
    [IgnoreDraw] public bool RestrictBlackBoard = false;
    [IgnoreDraw] public bool UseBlackboard = false;
    [IgnoreDraw] public int DropDownValue = 0;
    [IgnoreDraw] public List<string> BlackBoardFields = new();

    [IgnoreDraw]
    public int EnumValue;
    public int Int; 
    public bool Bool;
    public float Float;
    public String String; 
    public Vector2 Vector2;
    public Vector3 Vector3;
    public AnimationCurve AnimationCurve;
    public Color Color;
    public Object Object;

    [SerializeField] [HideInInspector]
    protected Dictionary<Type, Action<string>> _drawFields;
    
    public ElementData(Type type, FieldID id, object value) {
      ValueType = new(type);
      Id = id;
      SetValue(value);
      InitDrawFields();
    }
    
#if UNITY_EDITOR
    
    public void DrawEditor() {
      EditorGUILayout.BeginHorizontal();

      if (UseBlackboard || IsBlackboard)
        DropDownValue = SirenixEditorFields.Dropdown(Id.name, DropDownValue, BlackBoardFields?.ToArray());
      else
        DrawDefaultEditor();

      if (!IsBlackboard && !RestrictBlackBoard)
        UseBlackboard = EditorGUILayout.Toggle(UseBlackboard, GUILayout.MaxWidth(30.0f));
      else
        UseBlackboard = !RestrictBlackBoard;

      EditorGUILayout.EndHorizontal();
    }
    
    private void DrawDefaultEditor() {
      if (ValueType.type == null) return;
      if (_drawFields == null) InitDrawFields();
      
      string name = Id == null ? "" : Id.name;

      if (_drawFields.ContainsKey(ValueType.type))
        _drawFields[ValueType.type].Invoke(name);
      else if (ValueType.type.IsEnum)
        EnumValue = SirenixEditorFields.Dropdown(name, EnumValue, ValueType.type.GetEnumValues().OfType<object>().Select(o => o.ToString()).ToArray());
      else if (typeof(Object).IsAssignableFrom(ValueType.type))
        Object = EditorGUILayout.ObjectField(name, Object, ValueType.type, false);
    }
#endif
    
    public string GetBBoardName() {
      return BlackBoardFields[DropDownValue];
    }

    public void SetValue(object value) {
      if (ValueType.type.IsEnum) {
        EnumValue = (int)value;
        return;
      }
      
      GetDrawFields().FirstOrDefault(f => ValueType.type.IsAssignableFrom(f.FieldType))?.SetValue(this, value);
    }
    
    public T GetValue<T>() {
      if (Object is T)
        return (T)(object)Object;
      if (typeof(T).IsEnum)
        return (T)Enum.GetValues(typeof(T)).GetValue(EnumValue);
      return (T)GetDrawFields().FirstOrDefault(f => typeof(T).IsAssignableFrom(f.FieldType))?.GetValue(this);
    }
    
    public object GetDefaultValue() {
      return GetType()
        .GetMethod("GetValue", BindingFlags.Instance | BindingFlags.Public)?
        .MakeGenericMethod(ValueType.type)
        .Invoke(this, Array.Empty<object>());
    }

    private void InitDrawFields() {
#if UNITY_EDITOR
      _drawFields = new() {
        { typeof(int),            (name) => Int = EditorGUILayout.IntField(name, Int)                         },
        { typeof(bool),           (name) => Bool = EditorGUILayout.Toggle(name, Bool)                         },
        { typeof(float),          (name) => Float = EditorGUILayout.FloatField(name, Float)                   },
        { typeof(string),         (name) => String = EditorGUILayout.TextField(name, String)          },
        { typeof(Vector2),        (name) => Vector2 = EditorGUILayout.Vector2Field(name, Vector2)             },
        { typeof(Vector3),        (name) => Vector3 = EditorGUILayout.Vector3Field(name, Vector3)             },
        { typeof(AnimationCurve), (name) => AnimationCurve = EditorGUILayout.CurveField(name, AnimationCurve) },
        { typeof(Color), (name) => Color = EditorGUILayout.ColorField(name, Color) }
      };
#endif
    }

    private IEnumerable<FieldInfo> GetDrawFields() {
      return GetType().GetFields().Where(method => !Attribute.IsDefined(method, typeof(IgnoreDraw)));
    }

    #region Legacy
    //private class Fix : System.Attribute { }
    
    //[IgnoreDraw] [Fix] public ChunkType ChunkType;
    //[IgnoreDraw] [Fix] public BiomeType BiomeType;
    //[IgnoreDraw] [Fix] public RuneData RuneData;
    //[IgnoreDraw] [Fix] public SpellData SpellData;
    //[IgnoreDraw] [Fix] public SpellModifier SpellModifier;
    //[IgnoreDraw] [Fix] public UOData UobjectData;
    //[IgnoreDraw] [Fix] public WindowData WindowData;
    //[IgnoreDraw] [Fix] public EnumTypes.OperationE Operation;
    //[IgnoreDraw] [Fix] public EnumTypes.SignE Sign;
    //[IgnoreDraw] [Fix] public EnumTypes.MathOperationE MathOperation;
    //[IgnoreDraw] [Fix] public PerkData PerkData;
    //[IgnoreDraw] [Fix] public CharacterDataEx.LoyaltyE Loyalty;
    //[IgnoreDraw] [Fix] public CharacterData CharacterData;
    //[IgnoreDraw] [Fix] public DamageData DamageData;
    //[IgnoreDraw] [Fix] public PoolDataBase PoolData;
    //[IgnoreDraw] [Fix] public RuneGroup RuneGroup;
    //[IgnoreDraw] [Fix] public SpellGroup SpellGroup;
    //[IgnoreDraw] [Fix] public ScenarioGroup ScenarioGroup;
    
    //public void ParseLegacy() {
      //if (System.String.IsNullOrEmpty(Type)) return;
      //
      //ValueType = new(System.Type.GetType("System." + Type));
      //if (ValueType.type == null)
      //  ValueType = new(typeof(Vector3).Assembly.GetType("UnityEngine." + Type));
      //if (ValueType.type == null)
      //  ValueType = new(System.Type.GetType("Modules.SOE.Elements." + Type));
      //if (ValueType.type == null) {
      //  
      //  var fixFields = GetType().GetFields().Where(method => System.Attribute.IsDefined(method, typeof(Fix)));
      //  foreach (var field in fixFields) {
      //    if (Type != field.FieldType.Name) continue;
      //    ValueType = new(field.FieldType);
      //    if (field.FieldType.IsEnum) EnumValue = (int)field.GetValue(this);
      //    else Object = (Object) field.GetValue(this);
      //    Debug.LogError(field.FieldType + " ||| " + Type);
      //    break;
      //  }
      //}
      //if (ValueType.type != null) {
      //  Type = null; //Debug.LogError(ValueType.type.Name + " ||| " + Type);
      //}
      //else
      //  Debug.LogError("NULL ||| " + Type);

    //}
    
    //private bool IsDirty = true;
    //public void OnBeforeSerialize() {
    //  if (!IsDirty) return;
    //  foreach (var ef in _action.ActionList.ActionList) {
    //    foreach (var c in ef.ConditionList.ConditionList)
    //      foreach (var el in c.ElementsDataList)
    //        el.ParseLegacy();
    //    foreach (var el in ef.ElementsDataList)
    //      el.ParseLegacy();
    //  }
    //  EditorUtility.SetDirty(this);
    //  IsDirty = false;
    //}
    //public void OnAfterDeserialize() { }
    
    #endregion
    
  }
}
