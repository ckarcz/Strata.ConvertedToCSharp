/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.sensitivity
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// Test <seealso cref="PointSensitivityBuilder"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PointSensitivityBuilderTest
	public class PointSensitivityBuilderTest
	{

	  private static readonly DummyPointSensitivity SENS = DummyPointSensitivity.of(Currency.GBP, date(2015, 6, 30), 12);

	  public virtual void test_of_array_size0()
	  {
		PointSensitivities test = PointSensitivityBuilder.of().build();
		assertEquals(test.size(), 0);
	  }

	  public virtual void test_of_array_size1()
	  {
		PointSensitivities test = PointSensitivityBuilder.of(SENS).build();
		assertEquals(test.size(), 1);
		assertEquals(test.Sensitivities.get(0), SENS);
	  }

	  public virtual void test_of_array_size2()
	  {
		PointSensitivities test = PointSensitivityBuilder.of(SENS, SENS).build();
		assertEquals(test.size(), 2);
		assertEquals(test.Sensitivities.get(0), SENS);
		assertEquals(test.Sensitivities.get(1), SENS);
	  }

	  public virtual void test_of_list_size0()
	  {
		PointSensitivities test = PointSensitivityBuilder.of(ImmutableList.of()).build();
		assertEquals(test.size(), 0);
	  }

	  public virtual void test_of_list_size1()
	  {
		PointSensitivities test = PointSensitivityBuilder.of(ImmutableList.of(SENS)).build();
		assertEquals(test.size(), 1);
		assertEquals(test.Sensitivities.get(0), SENS);
	  }

	  public virtual void test_of_list_size2()
	  {
		PointSensitivities test = PointSensitivityBuilder.of(ImmutableList.of(SENS, SENS)).build();
		assertEquals(test.size(), 2);
		assertEquals(test.Sensitivities.get(0), SENS);
		assertEquals(test.Sensitivities.get(1), SENS);
	  }

	  public virtual void test_multipliedBy()
	  {
		TestingPointSensitivityBuilder test = new TestingPointSensitivityBuilder();
		test.multipliedBy(6);
		assertEquals(test.value, 12d * 6);
	  }

	  private sealed class TestingPointSensitivityBuilder : PointSensitivityBuilder
	  {
		internal double value = 12d;

		public PointSensitivityBuilder withCurrency(Currency currency)
		{
		  throw new System.NotSupportedException();
		}

		public PointSensitivityBuilder mapSensitivity(System.Func<double, double> @operator)
		{
		  value = @operator(value);
		  return this;
		}

		public PointSensitivityBuilder normalize()
		{
		  throw new System.NotSupportedException();
		}

		public MutablePointSensitivities buildInto(MutablePointSensitivities combination)
		{
		  return combination;
		}

		public PointSensitivityBuilder cloned()
		{
		  throw new System.NotSupportedException();
		}
	  }

	}

}