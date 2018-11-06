/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.value
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertNotNull;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="ValueDerivatives"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ValueDerivativesTest
	public class ValueDerivativesTest
	{

	  private const double VALUE = 123.4;
	  private static readonly DoubleArray DERIVATIVES = DoubleArray.of(1.0, 2.0, 3.0);

	  public virtual void test_of()
	  {
		ValueDerivatives test = ValueDerivatives.of(VALUE, DERIVATIVES);
		assertEquals(test.Value, VALUE, 0);
		assertEquals(test.Derivatives, DERIVATIVES);
		assertEquals(test.getDerivative(0), DERIVATIVES.get(0));
		assertEquals(test.getDerivative(1), DERIVATIVES.get(1));
		assertEquals(test.getDerivative(2), DERIVATIVES.get(2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ValueDerivatives test = ValueDerivatives.of(VALUE, DERIVATIVES);
		coverImmutableBean(test);
		assertNotNull(ValueDerivatives.meta());
		ValueDerivatives test2 = ValueDerivatives.of(123.4, DoubleArray.of(1.0, 2.0, 3.0));
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ValueDerivatives test = ValueDerivatives.of(VALUE, DERIVATIVES);
		assertSerialization(test);
	  }

	}

}