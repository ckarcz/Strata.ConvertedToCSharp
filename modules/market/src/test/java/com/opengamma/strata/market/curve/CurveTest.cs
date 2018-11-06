/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LabelParameterMetadata = com.opengamma.strata.market.param.LabelParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// Test <seealso cref="Curve"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurveTest
	public class CurveTest
	{

	  private static readonly CurveName CURVE_NAME = CurveName.of("Curve");
	  private static readonly LabelParameterMetadata PARAM_META = LabelParameterMetadata.of("TestParam");

	  public virtual void test_withPerturbation()
	  {
		Curve test = new TestingCurve(2d);
		assertEquals(test.withPerturbation((i, v, m) => v + 1).getParameter(0), 3d);
	  }

	  public virtual void test_createParameterSensitivity_unit()
	  {
		Curve test = new TestingCurve(2d);
		assertEquals(test.createParameterSensitivity(DoubleArray.of(2d)).MarketDataName, CURVE_NAME);
		assertEquals(test.createParameterSensitivity(DoubleArray.of(2d)).ParameterCount, 1);
		assertEquals(test.createParameterSensitivity(DoubleArray.of(2d)).ParameterMetadata, ImmutableList.of(PARAM_META));
		assertEquals(test.createParameterSensitivity(DoubleArray.of(2d)).Sensitivity, DoubleArray.of(2d));
	  }

	  public virtual void test_createParameterSensitivity_currency()
	  {
		Curve test = new TestingCurve(2d);
		assertEquals(test.createParameterSensitivity(USD, DoubleArray.of(2d)).MarketDataName, CURVE_NAME);
		assertEquals(test.createParameterSensitivity(USD, DoubleArray.of(2d)).ParameterCount, 1);
		assertEquals(test.createParameterSensitivity(USD, DoubleArray.of(2d)).ParameterMetadata, ImmutableList.of(PARAM_META));
		assertEquals(test.createParameterSensitivity(USD, DoubleArray.of(2d)).Currency, USD);
		assertEquals(test.createParameterSensitivity(USD, DoubleArray.of(2d)).Sensitivity, DoubleArray.of(2d));
	  }

	  //-------------------------------------------------------------------------
	  internal class TestingCurve : Curve
	  {

		internal readonly double value;

		internal TestingCurve(double value)
		{
		  this.value = value;
		}

		public virtual int ParameterCount
		{
			get
			{
			  return 1;
			}
		}

		public virtual double getParameter(int parameterIndex)
		{
		  return value;
		}

		public virtual ParameterMetadata getParameterMetadata(int parameterIndex)
		{
		  return PARAM_META;
		}

		public virtual CurveMetadata Metadata
		{
			get
			{
			  return DefaultCurveMetadata.of(CURVE_NAME);
			}
		}

		public virtual Curve withMetadata(CurveMetadata metadata)
		{
		  return this;
		}

		public virtual Curve withParameter(int parameterIndex, double newValue)
		{
		  return new TestingCurve(newValue);
		}

		public virtual double yValue(double x)
		{
		  return value;
		}

		public virtual UnitParameterSensitivity yValueParameterSensitivity(double x)
		{
		  return UnitParameterSensitivity.of(CURVE_NAME, DoubleArray.filled(1));
		}

		public virtual double firstDerivative(double x)
		{
		  return 0;
		}
	  }

	}

}