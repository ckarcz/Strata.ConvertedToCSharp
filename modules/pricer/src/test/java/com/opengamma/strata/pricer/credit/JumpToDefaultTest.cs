/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using StandardId = com.opengamma.strata.basics.StandardId;

	/// <summary>
	/// Test <seealso cref="JumpToDefault"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class JumpToDefaultTest
	public class JumpToDefaultTest
	{

	  private static readonly StandardId ID_ABC = StandardId.of("OG", "ABC");
	  private static readonly StandardId ID_DEF = StandardId.of("OG", "DEF");

	  public virtual void test_of()
	  {
		JumpToDefault test = JumpToDefault.of(GBP, ImmutableMap.of(ID_ABC, 1.1d, ID_DEF, 2.2d));
		assertEquals(test.Currency, GBP);
		assertEquals(test.Amounts.size(), 2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		JumpToDefault test = JumpToDefault.of(GBP, ImmutableMap.of(ID_ABC, 1.1d, ID_DEF, 2.2d));
		coverImmutableBean(test);
		JumpToDefault test2 = JumpToDefault.of(USD, ImmutableMap.of(ID_DEF, 2.3d));
		coverBeanEquals(test, test2);
	  }

	}

}