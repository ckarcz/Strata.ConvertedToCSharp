using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertUtilityClass;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Pair = com.opengamma.strata.collect.tuple.Pair;

	/// <summary>
	/// Test Messages.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class MessagesTest
	public class MessagesTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "formatMessageSingle") public static Object[][] data_formatMessageSingle()
		public static object[][] data_formatMessageSingle()
		{
		return new object[][]
		{
			new object[] {null, null, "", " - [null]"},
			new object[] {null, "", "", " - []"},
			new object[] {"", null, "", " - [null]"},
			new object[] {"{}", null, "null", ""},
			new object[] {"{}{}", null, "null{}", ""},
			new object[] {"{} and {}", null, "null and {}", ""},
			new object[] {"", "", "", " - []"},
			new object[] {"{}", "", "", ""},
			new object[] {"{}{}", "", "{}", ""},
			new object[] {"{} and {}", "", " and {}", ""},
			new object[] {"{}", 67, "67", ""},
			new object[] {"{}{}", 67, "67{}", ""},
			new object[] {"{} and {}", 67, "67 and {}", ""}
		};
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "formatMessageSingle") public void test_formatMessageSingle(String template, Object arg, String expMain, String expExcess)
	  public virtual void test_formatMessageSingle(string template, object arg, string expMain, string expExcess)
	  {
		assertEquals(Messages.format(template, arg), expMain + expExcess);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "formatMessageSingle") public void test_formatMessageSingle_prefix(String template, Object arg, String expMain, String expExcess)
	  public virtual void test_formatMessageSingle_prefix(string template, object arg, string expMain, string expExcess)
	  {
		assertEquals(Messages.format("::" + Objects.ToString(template, ""), arg), "::" + expMain + expExcess);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "formatMessageSingle") public void test_formatMessageSingle_suffix(String template, Object arg, String expMain, String expExcess)
	  public virtual void test_formatMessageSingle_suffix(string template, object arg, string expMain, string expExcess)
	  {
		assertEquals(Messages.format(Objects.ToString(template, "") + "@@", arg), expMain + "@@" + expExcess);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "formatMessageSingle") public void test_formatMessageSingle_prefixSuffix(String template, Object arg, String expMain, String expExcess)
	  public virtual void test_formatMessageSingle_prefixSuffix(string template, object arg, string expMain, string expExcess)
	  {
		assertEquals(Messages.format("::" + Objects.ToString(template, "") + "@@", arg), "::" + expMain + "@@" + expExcess);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "formatMessage") public static Object[][] data_formatMessage()
	  public static object[][] data_formatMessage()
	  {
		return new object[][]
		{
			new object[] {null, null, "", ""},
			new object[] {null, new object[] {}, "", ""},
			new object[] {null, new object[] {null}, "", " - [null]"},
			new object[] {null, new object[] {67}, "", " - [67]"},
			new object[] {"", null, "", ""},
			new object[] {"{}", null, "{}", ""},
			new object[] {"{}{}", null, "{}{}", ""},
			new object[] {"{} and {}", null, "{} and {}", ""},
			new object[] {"", new object[] {null}, "", " - [null]"},
			new object[] {"{}", new object[] {null}, "null", ""},
			new object[] {"{}{}", new object[] {null}, "null{}", ""},
			new object[] {"{} and {}", new object[] {null}, "null and {}", ""},
			new object[] {"", new object[] {}, "", ""},
			new object[] {"{}", null, "{}", ""},
			new object[] {"{}", new object[] {}, "{}", ""},
			new object[] {"{}", new object[] {67}, "67", ""},
			new object[] {"{}", new object[] {67, 78}, "67", " - [78]"},
			new object[] {"{}", new object[] {67, 78, 89}, "67", " - [78, 89]"},
			new object[] {"{}", new object[] {67, 78, 89, 90}, "67", " - [78, 89, 90]"},
			new object[] {"{}{}", null, "{}{}", ""},
			new object[] {"{}{}", new object[] {}, "{}{}", ""},
			new object[] {"{}{}", new object[] {67}, "67{}", ""},
			new object[] {"{}{}", new object[] {67, 78}, "6778", ""},
			new object[] {"{}{}", new object[] {67, 78, 89}, "6778", " - [89]"},
			new object[] {"{}{}", new object[] {67, 78, 89, 90}, "6778", " - [89, 90]"},
			new object[] {"{} and {}", null, "{} and {}", ""},
			new object[] {"{} and {}", new object[] {}, "{} and {}", ""},
			new object[] {"{} and {}", new object[] {67}, "67 and {}", ""},
			new object[] {"{} and {}", new object[] {67, 78}, "67 and 78", ""},
			new object[] {"{} and {}", new object[] {67, 78, 89}, "67 and 78", " - [89]"},
			new object[] {"{} and {}", new object[] {67, 78, 89, 90}, "67 and 78", " - [89, 90]"},
			new object[] {"{}, {} and {}", new object[] {}, "{}, {} and {}", ""},
			new object[] {"{}, {} and {}", new object[] {67}, "67, {} and {}", ""},
			new object[] {"{}, {} and {}", new object[] {67, 78}, "67, 78 and {}", ""},
			new object[] {"{}, {} and {}", new object[] {67, 78, 89}, "67, 78 and 89", ""},
			new object[] {"{}, {} and {}", new object[] {67, 78, 89, 90}, "67, 78 and 89", " - [90]"},
			new object[] {"ABC", new object[] {}, "ABC", ""},
			new object[] {"Message {}, {}, {}", new object[] {"A", 2, 3.0}, "Message A, 2, 3.0", ""},
			new object[] {"Message {}, {} blah", new object[] {"A", 2, 3.0}, "Message A, 2 blah", " - [3.0]"},
			new object[] {"Message {}, {}", new object[] {"A", 2, 3.0, true}, "Message A, 2", " - [3.0, true]"},
			new object[] {"Message {}, {}, {}, {} blah", new object[] {"A", 2, 3.0}, "Message A, 2, 3.0, {} blah", ""}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "formatMessage") public void test_formatMessage(String template, Object[] args, String expMain, String expExcess)
	  public virtual void test_formatMessage(string template, object[] args, string expMain, string expExcess)
	  {
		assertEquals(Messages.format(template, args), expMain + expExcess);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "formatMessage") public void test_formatMessage_prefix(String template, Object[] args, String expMain, String expExcess)
	  public virtual void test_formatMessage_prefix(string template, object[] args, string expMain, string expExcess)
	  {
		assertEquals(Messages.format("::" + Objects.ToString(template, ""), args), "::" + expMain + expExcess);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "formatMessage") public void test_formatMessage_suffix(String template, Object[] args, String expMain, String expExcess)
	  public virtual void test_formatMessage_suffix(string template, object[] args, string expMain, string expExcess)
	  {
		assertEquals(Messages.format(Objects.ToString(template, "") + "@@", args), expMain + "@@" + expExcess);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "formatMessage") public void test_formatMessage_prefixSuffix(String template, Object[] args, String expMain, String expExcess)
	  public virtual void test_formatMessage_prefixSuffix(string template, object[] args, string expMain, string expExcess)
	  {
		assertEquals(Messages.format("::" + Objects.ToString(template, "") + "@@", args), "::" + expMain + "@@" + expExcess);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "formatMessageWithAttributes") public static Object[][] data_formatMessageWithAttributes()
	  public static object[][] data_formatMessageWithAttributes()
	  {
		return new object[][]
		{
			new object[] {null, null, Pair.of("", ImmutableMap.of())},
			new object[] {null, new object[] {}, Pair.of("", ImmutableMap.of())},
			new object[] {"", new object[] {"testValueMissingKey"}, Pair.of(" - [testValueMissingKey]", ImmutableMap.of())},
			new object[] {"{}", new object[] {"testValue"}, Pair.of("testValue", ImmutableMap.of())},
			new object[] {"{}", new object[] {null}, Pair.of("null", ImmutableMap.of())},
			new object[] {"{a}", new object[] {"testValue"}, Pair.of("testValue", ImmutableMap.of("a", "testValue"))},
			new object[] {"{a} bcd", new object[] {"testValue"}, Pair.of("testValue bcd", ImmutableMap.of("a", "testValue"))},
			new object[] {"Test {abc} test2 {def} test3", new object[] {"abcValue", 123456}, Pair.of("Test abcValue test2 123456 test3", ImmutableMap.of("abc", "abcValue", "def", "123456"))},
			new object[] {"Test {abc} test2 {} test3", new object[] {"abcValue", 123456}, Pair.of("Test abcValue test2 123456 test3", ImmutableMap.of("abc", "abcValue"))},
			new object[] {"Test {abc} test2 {} test3 {} test4", new object[] {"abcValue", 123456, 789}, Pair.of("Test abcValue test2 123456 test3 789 test4", ImmutableMap.of("abc", "abcValue"))},
			new object[] {"Test {abc} test2 {def} test3", new object[] {"abcValue", 123456, 789}, Pair.of("Test abcValue test2 123456 test3 - [789]", ImmutableMap.of("abc", "abcValue", "def", "123456"))},
			new object[] {"Test {abc} test2 {abc} test3", new object[] {"abcValue", 123456, 789}, Pair.of("Test abcValue test2 123456 test3 - [789]", ImmutableMap.of("abc", "123456"))},
			new object[] {"Test {abc} test2 {def} test3", new object[] {"abcValue"}, Pair.of("Test abcValue test2 {def} test3", ImmutableMap.of("abc", "abcValue"))},
			new object[] {"{a} bcd", new object[] {"$testValue"}, Pair.of("$testValue bcd", ImmutableMap.of("a", "\\$testValue"))},
			new object[] {"Test {abc} test2 {def} test3 {ghi} test4", new object[] {"abcValue"}, Pair.of("Test abcValue test2 {def} test3 {ghi} test4", ImmutableMap.of("abc", "abcValue"))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "formatMessageWithAttributes") public void test_formatMessageWithAttributes(String template, Object[] args, com.opengamma.strata.collect.tuple.Pair<String, java.util.Map<String, String>> expectedOutput)
	  public virtual void test_formatMessageWithAttributes(string template, object[] args, Pair<string, IDictionary<string, string>> expectedOutput)
	  {

		assertEquals(Messages.formatWithAttributes(template, args), expectedOutput);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_validUtilityClass()
	  {
		assertUtilityClass(typeof(Messages));
	  }

	}

}