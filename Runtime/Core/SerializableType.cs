using System.IO;
using Sirenix.Serialization;
using UnityEngine;

namespace SOE.Core {
  [System.Serializable]
  public class SerializableType {
    
    public System.Type type {
      get {
        if (m_type == null) {
          GetSystemType();
        }

        return m_type;
      }
    }
    
    [PreviouslySerializedAs("type")]
    private System.Type m_type;
    [SerializeField]
    private byte[] data;

    private void GetSystemType() {
      using (var stream = new MemoryStream(data))
      using (var r = new BinaryReader(stream)) {
        m_type = Read(r);
      }
    }
    
    public SerializableType(System.Type aType) {
      m_type = aType;
      using (var stream = new MemoryStream())
      using (var w = new BinaryWriter(stream)) {
        Write(w, m_type);
        data = stream.ToArray();
      }
    }

    public static System.Type Read(BinaryReader aReader) {
      var paramCount = aReader.ReadByte();
      if (paramCount == 0xFF)
        return null;
      var typeName = aReader.ReadString();
      var type = System.Type.GetType(typeName);
      if (type == null)
        throw new System.Exception("Can't find type; '" + typeName + "'");
      if (type.IsGenericTypeDefinition && paramCount > 0) {
        var p = new System.Type[paramCount];
        for (int i = 0; i < paramCount; i++) {
          p[i] = Read(aReader);
        }

        type = type.MakeGenericType(p);
      }

      return type;
    }

    public static void Write(BinaryWriter aWriter, System.Type aType) {
      if (aType == null) {
        aWriter.Write((byte) 0xFF);
        return;
      }

      if (aType.IsGenericType) {
        var t = aType.GetGenericTypeDefinition();
        var p = aType.GetGenericArguments();
        aWriter.Write((byte) p.Length);
        aWriter.Write(t.AssemblyQualifiedName);
        for (int i = 0; i < p.Length; i++) {
          Write(aWriter, p[i]);
        }

        return;
      }

      aWriter.Write((byte) 0);
      aWriter.Write(aType.AssemblyQualifiedName);
    }

  }
}