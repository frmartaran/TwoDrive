using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Importer.Interface;

namespace TwoDrive.Importer.Test
{
    [TestClass]
    public class ParametersDictionaryTest
    {

        [TestMethod]
        public void AddKeyAndValue()
        {
            var key = "Paramenter";
            var value = "Value";
            var dictionary = new ParameterDictionary();
            dictionary.AddParameter(key, value);
            var parameter = dictionary.GetParameterValue<string>(key);

            Assert.AreEqual(value, parameter);
        }
    }
}
