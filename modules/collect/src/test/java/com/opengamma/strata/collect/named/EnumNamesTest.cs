/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.named
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThatThrownBy;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="EnumNames"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class EnumNamesTest
	public class EnumNamesTest
	{

	  public virtual void test_format()
	  {
		EnumNames<MockEnum> test = EnumNames.of(typeof(MockEnum));
		assertEquals(test.format(MockEnum.ONE), "One");
		assertEquals(test.format(MockEnum.TWENTY_ONE), "TwentyOne");
		assertEquals(test.format(MockEnum.FooBar), "Foobar");
		assertEquals(test.format(MockEnum.WOO_BAR_WAA), "WooBarWaa");
	  }

	  public virtual void test_parse_one()
	  {
		EnumNames<MockEnum> test = EnumNames.of(typeof(MockEnum));
		assertEquals(test.parse("One"), MockEnum.ONE);
		assertEquals(test.parse("ONE"), MockEnum.ONE);
		assertEquals(test.parse("one"), MockEnum.ONE);
	  }

	  public virtual void test_parse_twentyOne()
	  {
		EnumNames<MockEnum> test = EnumNames.of(typeof(MockEnum));
		assertEquals(test.parse("TwentyOne"), MockEnum.TWENTY_ONE);
		assertEquals(test.parse("TWENTYONE"), MockEnum.TWENTY_ONE);
		assertEquals(test.parse("twentyone"), MockEnum.TWENTY_ONE);
		assertEquals(test.parse("TWENTY_ONE"), MockEnum.TWENTY_ONE);
		assertEquals(test.parse("twenty_one"), MockEnum.TWENTY_ONE);
	  }

	  public virtual void test_parse_fooBar()
	  {
		EnumNames<MockEnum> test = EnumNames.of(typeof(MockEnum));
		assertEquals(test.parse("Foobar"), MockEnum.FooBar);
		assertEquals(test.parse("FOOBAR"), MockEnum.FooBar);
		assertEquals(test.parse("foobar"), MockEnum.FooBar);
		assertEquals(test.parse("FooBar"), MockEnum.FooBar);
	  }

	  public virtual void test_parse_wooBarWaa()
	  {
		EnumNames<MockEnum> test = EnumNames.of(typeof(MockEnum));
		assertEquals(test.parse("WooBarWaa"), MockEnum.WOO_BAR_WAA);
		assertEquals(test.parse("WOOBARWAA"), MockEnum.WOO_BAR_WAA);
		assertEquals(test.parse("woobarwaa"), MockEnum.WOO_BAR_WAA);
		assertEquals(test.parse("WOO_BAR_WAA"), MockEnum.WOO_BAR_WAA);
		assertEquals(test.parse("woo_bar_waa"), MockEnum.WOO_BAR_WAA);
	  }

	  public virtual void test_parse_invalid()
	  {
		EnumNames<MockEnum> test = EnumNames.of(typeof(MockEnum));
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		assertThatThrownBy(() => test.parse("unknown")).isInstanceOf(typeof(System.ArgumentException)).hasMessageContaining("Unknown enum name 'unknown' for type " + typeof(MockEnum).FullName);
	  }

	  internal enum MockEnum
	  {
		ONE,
		TWENTY_ONE,
		FooBar,
		WOO_BAR_WAA,
	  }

	}

}