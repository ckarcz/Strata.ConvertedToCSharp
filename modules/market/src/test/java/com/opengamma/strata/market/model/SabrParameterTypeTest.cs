/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.model
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverEnum;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="SabrParameterType"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrParameterTypeTest
	public class SabrParameterTypeTest
	{

	  public virtual void test_basics()
	  {
		assertEquals(SabrParameterType.ALPHA.name(), "ALPHA");
		assertEquals(SabrParameterType.ALPHA.ToString(), "Alpha");
		assertEquals(SabrParameterType.BETA.ToString(), "Beta");
	  }

	  public virtual void coverage()
	  {
		coverEnum(typeof(SabrParameterType));
	  }

	}

}