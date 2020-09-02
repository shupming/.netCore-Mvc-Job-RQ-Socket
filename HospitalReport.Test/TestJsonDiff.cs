using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Test
{
    [TestClass]
    public class TestJsonDiff
    {
        [TestMethod]
        [Description(description: "测试数字序列化")]
        public void TestNumber()
        {
            object jsonObject = new { number = 123.456 };
            string aJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(value: jsonObject);
            string bJsonString = System.Text.Json.JsonSerializer.Serialize(value: jsonObject);

            Assert.AreEqual(expected: aJsonString, actual: bJsonString, message: "测试数字序列化失败");
        }

        [TestMethod]
        [Description(description: "测试英文序列化")]
        public void TestEnglish()
        {
            object jsonObject = new { english = "bla bla" };
            string aJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(value: jsonObject);
            string bJsonString = System.Text.Json.JsonSerializer.Serialize(value: jsonObject);

            Assert.AreEqual(expected: aJsonString, actual: bJsonString, message: "测试英文序列化失败");
        }

        [TestMethod]
        [Description(description: "测试中文序列化")]
        public void TestChinese()
        {
            object jsonObject = new { chinese = "灰长标准的布咚发" };
            string aJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(value: jsonObject);
            string bJsonString = System.Text.Json.JsonSerializer.Serialize(value: jsonObject);

            Assert.AreEqual(expected: aJsonString, actual: bJsonString, message: "测试中文序列化失败");
        }

        [TestMethod]
        [Description(description: "测试英文符号")]
        public void TestEnglishSymbol()
        {
            object jsonObject = new { symbol = @"~`!@#$%^&*()_-+={}[]:;'<>,.?/ " };
            string aJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(value: jsonObject);
            string bJsonString = System.Text.Json.JsonSerializer.Serialize(value: jsonObject);

            Assert.AreEqual(expected: aJsonString, actual: bJsonString, message: "测试英文符号失败");
        }

        [TestMethod]
        [Description(description: "测试中文符号")]
        public void TestChineseSymbol()
        {
            object jsonObject = new { chinese_symbol = @"~·@#￥%……&*（）—-+=｛｝【】；：“”‘’《》，。？、" };
            string aJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(value: jsonObject);
            string bJsonString = System.Text.Json.JsonSerializer.Serialize(value: jsonObject);

            Assert.AreEqual(expected: aJsonString, actual: bJsonString, message: "测试中文符号失败");
        }

        [TestMethod]
        [Description(description: "测试反序列化数值字符串隐式转换为数值类型")]
        public void TestDeserializeNumber()
        {
            string ajsonString = "{\"Number\":\"123\"}";

            TestClass aJsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<TestClass>(ajsonString);

            // 报错，The JSON value could not be converted to System.Int32. Path: $.number | LineNumber: 0 | BytePositionInLine: 15
            TestClass bJsonObject = System.Text.Json.JsonSerializer.Deserialize<TestClass>(json: ajsonString);

            Assert.AreEqual(expected: aJsonObject.Number, actual: bJsonObject.Number, message: "测试反序列化数值字符串隐式转换为数值类型失败");
        }

        public class TestClass
        {
            public int Number { get; set; }
        }
    }
}
