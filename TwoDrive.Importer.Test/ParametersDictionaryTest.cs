using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Importer.Interface;
using TwoDrive.Importer.Interface.Exceptions;

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

        [TestMethod]
        [ExpectedException(typeof(ParameterException))]
        public void TheresNoValueInDictionary ()
        {
            var key = "Paramenter";
            var dictionary = new ParameterDictionary();
            var parameter = dictionary.GetParameterValue<string>(key);
        }
    }
}
