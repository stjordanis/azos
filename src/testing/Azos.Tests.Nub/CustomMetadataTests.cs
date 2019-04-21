/*<FILE_LICENSE>
 * Azos (A to Z Application Operating System) Framework
 * The A to Z Foundation (a.k.a. Azist) licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
</FILE_LICENSE>*/


using System;

using Azos.Conf;
using Azos.Scripting;

namespace Azos.Tests.Nub
{
  [Runnable]
  public class CustomMetadataTests
  {

    [CustomMetadata("a=123 b=789 score=100 description='Generic car' origin{_override=all country=world} z=0")]
    public class Car { }

    [CustomMetadata("score=75 description='Cars built in the US' origin{_override=stop country=usa}")]
    public class AmericanCar : Car { }

    [CustomMetadata("score=90 description='Very usable and decent quality' a=-900")]
    public class Buick : AmericanCar { }

    [CustomMetadata("score=40 description='Luxury item, but unreliable'  origin{country=XYZYZ/*this will never take effect*/}")]
    public class Cadillac : AmericanCar { }

    [CustomMetadata("score=110 description='Cars built in Japan' origin{_override=stop country=jap} z=1")]
    public class JapanesenCar : Car { }

    [CustomMetadata("description='Honda motors'")]
    public class Honda : JapanesenCar { }

    [CustomMetadata("description='Toyota motors' b=-1 score=137 z=7")]
    public class Toyota : JapanesenCar { }


    [Run]
    public void Car_1()
    {
      var data = Conf.Configuration.NewEmptyRoot();
      CustomMetadataAttribute.Apply(typeof(Car), this, data);

      Console.WriteLine(data.ToLaconicString(Azos.CodeAnalysis.Laconfig.LaconfigWritingOptions.PrettyPrint));

      Aver.AreEqual(123, data.AttrByName("a").ValueAsInt());
      Aver.AreEqual(789, data.AttrByName("b").ValueAsInt());
      Aver.AreEqual(100, data.AttrByName("score").ValueAsInt());
      Aver.AreEqual(0, data.AttrByName("z").ValueAsInt());
      Aver.AreEqual("Generic car", data.AttrByName("description").Value);
      Aver.AreEqual("world", data.Navigate("origin/$country").Value);
    }

    [Run]
    public void AmericanCar_1()
    {
      var data = Conf.Configuration.NewEmptyRoot();
      CustomMetadataAttribute.Apply(typeof(AmericanCar), this, data);

      Console.WriteLine(data.ToLaconicString(Azos.CodeAnalysis.Laconfig.LaconfigWritingOptions.PrettyPrint));

      Aver.AreEqual(123, data.AttrByName("a").ValueAsInt());
      Aver.AreEqual(789, data.AttrByName("b").ValueAsInt());
      Aver.AreEqual(75, data.AttrByName("score").ValueAsInt());
      Aver.AreEqual(0, data.AttrByName("z").ValueAsInt());
      Aver.AreEqual("Cars built in the US", data.AttrByName("description").Value);
      Aver.AreEqual("usa", data.Navigate("origin/$country").Value);
    }

    [Run]
    public void Buick_1()
    {
      var data = Conf.Configuration.NewEmptyRoot();
      CustomMetadataAttribute.Apply(typeof(Buick), this, data);

      Console.WriteLine(data.ToLaconicString(Azos.CodeAnalysis.Laconfig.LaconfigWritingOptions.PrettyPrint));

      Aver.AreEqual(-900, data.AttrByName("a").ValueAsInt());
      Aver.AreEqual(789, data.AttrByName("b").ValueAsInt());
      Aver.AreEqual(90, data.AttrByName("score").ValueAsInt());
      Aver.AreEqual(0, data.AttrByName("z").ValueAsInt());
      Aver.AreEqual("Very usable and decent quality", data.AttrByName("description").Value);
      Aver.AreEqual("usa", data.Navigate("origin/$country").Value);
    }

    [Run]
    public void Cadillac_1()
    {
      var data = Conf.Configuration.NewEmptyRoot();
      CustomMetadataAttribute.Apply(typeof(Cadillac), this, data);

      Console.WriteLine(data.ToLaconicString(Azos.CodeAnalysis.Laconfig.LaconfigWritingOptions.PrettyPrint));

      Aver.AreEqual(123, data.AttrByName("a").ValueAsInt());
      Aver.AreEqual(789, data.AttrByName("b").ValueAsInt());
      Aver.AreEqual(40, data.AttrByName("score").ValueAsInt());
      Aver.AreEqual(0, data.AttrByName("z").ValueAsInt());
      Aver.AreEqual("Luxury item, but unreliable", data.AttrByName("description").Value);
      Aver.AreEqual("usa", data.Navigate("origin/$country").Value); //in spite of XYZ set in config, the American-level override pragma set to stop
    }

    [Run]
    public void Honda_1()
    {
      var data = Conf.Configuration.NewEmptyRoot();
      CustomMetadataAttribute.Apply(typeof(Honda), this, data);

      Console.WriteLine(data.ToLaconicString(Azos.CodeAnalysis.Laconfig.LaconfigWritingOptions.PrettyPrint));

      Aver.AreEqual(123, data.AttrByName("a").ValueAsInt());
      Aver.AreEqual(789, data.AttrByName("b").ValueAsInt());
      Aver.AreEqual(110, data.AttrByName("score").ValueAsInt());
      Aver.AreEqual(1, data.AttrByName("z").ValueAsInt());
      Aver.AreEqual("Honda motors", data.AttrByName("description").Value);
      Aver.AreEqual("jap", data.Navigate("origin/$country").Value);
    }

    [Run]
    public void Toyota_1()
    {
      var data = Conf.Configuration.NewEmptyRoot();
      CustomMetadataAttribute.Apply(typeof(Toyota), this, data);

      Console.WriteLine(data.ToLaconicString(Azos.CodeAnalysis.Laconfig.LaconfigWritingOptions.PrettyPrint));

      Aver.AreEqual(123, data.AttrByName("a").ValueAsInt());
      Aver.AreEqual(-1, data.AttrByName("b").ValueAsInt());
      Aver.AreEqual(137, data.AttrByName("score").ValueAsInt());
      Aver.AreEqual(7, data.AttrByName("z").ValueAsInt());
      Aver.AreEqual("Toyota motors", data.AttrByName("description").Value);
      Aver.AreEqual("jap", data.Navigate("origin/$country").Value);
    }


  }
}