/*<FILE_LICENSE>
 * Azos (A to Z Application Operating System) Framework
 * The A to Z Foundation (a.k.a. Azist) licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
</FILE_LICENSE>*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Azos.IO;
using Azos.Data;

namespace Azos.Serialization.Arow
{
  /// <summary>
  /// Designates classes that register their single instance via a call to ArowSerializer.Register().
  /// These classes are generated by cli arow compiler
  /// </summary>
  public interface ITypeSerializationCore
  {
    void Register();
    void Serialize(TypedDoc doc, WritingStreamer streamer);
    void Deserialize(TypedDoc doc, ReadingStreamer streamer);
  }

  /// <summary>
  /// Facade for performing Arow serialization.
  /// Arow format is purposely designed for "[a]daptable [row]"/version tolerant serialization that eschews creating extra copies and
  /// object instances. The serializer is used in conjunction with cli compiler that generates type-specific static serializer cores
  /// for every type that supports the format.
  /// The serializer supports primitives, Azos intrinsics like GDID, PilePointer, NLSMap, custom TypedDocs and List/arrays of the aforementioned types
  /// </summary>
  /// <remarks>
  /// By design this serializer does not support polymorphic TypedDoc types in this version
  /// </remarks>
  public static class ArowSerializer
  {
    public const string AROW_TARGET = "AROW-SERIALIZER";

    private static object s_Lock = new object();
    private static volatile Dictionary<Type, ITypeSerializationCore> s_Serializers = new Dictionary<Type, ITypeSerializationCore>();


    /// <summary>
    /// Registers all entities in the specified assembly which implement ITypeSerializationCore interface
    /// </summary>
    public static void RegisterTypeSerializationCores(Assembly asm)
    {
      var allTypes = asm.NonNull(nameof(asm)).GetTypes();
      var allSerCores = allTypes.Where(t => t.IsClass && !t.IsAbstract && typeof(ITypeSerializationCore).IsAssignableFrom(t));

      foreach (var t in allSerCores)
      {
        var core = Activator.CreateInstance(t) as ITypeSerializationCore;
        core.Register();
      }
    }


    public static void Serialize(TypedDoc doc, WritingStreamer streamer, bool header = true)
    {
      ITypeSerializationCore core;
      var tRow = doc.GetType();
      if (!s_Serializers.TryGetValue(tRow, out core))
        throw new ArowException(StringConsts.AROW_TYPE_NOT_SUPPORTED_ERROR.Args(tRow.FullName));

      var ar = doc as IAmorphousData;
      if (ar!=null)
      {
        if (ar.AmorphousDataEnabled) ar.BeforeSave(AROW_TARGET);
      }

      //1 Header
      if (header) Writer.WriteHeader(streamer);

          //2 Body
          core.Serialize(doc, streamer);

      //3 EORow
      Writer.WriteEORow(streamer);
    }


    public static void Deserialize(TypedDoc doc, ReadingStreamer streamer, bool header = true)
    {
      var ok = TryDeserialize(doc, streamer, header);
      if (!ok)
        throw new ArowException(StringConsts.AROW_TYPE_NOT_SUPPORTED_ERROR.Args(doc.GetType().FullName));
    }

    public static bool TryDeserialize(TypedDoc doc, ReadingStreamer streamer, bool header = true)
    {
      ITypeSerializationCore core;
      var tDoc = doc.GetType();
      if (!s_Serializers.TryGetValue(tDoc, out core))
        return false;

      //1 Header
      if (header) Reader.ReadHeader(streamer);

      //2 Body
      core.Deserialize(doc, streamer);

      var ar = doc as IAmorphousData;
      if (ar!=null)
      {
        if (ar.AmorphousDataEnabled) ar.AfterLoad(AROW_TARGET);
      }

      return true;
    }

    public static bool IsDocTypeSupported(Type tDoc)
    {
      return s_Serializers.ContainsKey(tDoc);
    }

    /// <summary>
    /// Registers ITypeSerializationCore so it can be used globally to serialize TypedRows in Arow format
    /// </summary>
    public static bool Register(Type tRow, ITypeSerializationCore core)
    {
      lock(s_Lock)
      {
        if (s_Serializers.ContainsKey(tRow)) return false;
        var dict = new Dictionary<Type, ITypeSerializationCore>(s_Serializers);
        dict.Add(tRow, core);
        s_Serializers = dict;//atomic
        return true;
      }
    }
  }
}
