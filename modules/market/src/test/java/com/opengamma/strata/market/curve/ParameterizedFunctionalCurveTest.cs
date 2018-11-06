using System;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using TenorParameterMetadata = com.opengamma.strata.market.param.TenorParameterMetadata;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// Test <seealso cref="ParameterizedFunctionalCurve"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ParameterizedFunctionalCurveTest
	public class ParameterizedFunctionalCurveTest
	{
	  private static readonly DoubleArray PARAMETERS = DoubleArray.of(1.2, -10.4, 8.9);
	  private static readonly CurveMetadata METADATA;
	  static ParameterizedFunctionalCurveTest()
	  {
		TenorParameterMetadata param1 = TenorParameterMetadata.of(Tenor.TENOR_1Y);
		TenorParameterMetadata param2 = TenorParameterMetadata.of(Tenor.TENOR_5Y);
		TenorParameterMetadata param3 = TenorParameterMetadata.of(Tenor.TENOR_10Y);
		METADATA = DefaultCurveMetadata.builder().curveName("test").yValueType(ValueType.DISCOUNT_FACTOR).xValueType(ValueType.YEAR_FRACTION).parameterMetadata(param1, param2, param3).build();
	  }
	  private static readonly System.Func<DoubleArray, double, double> VALUE_FUNCTION = (DoubleArray t, double? u) =>
	  {
	  return t.get(0) + Math.Sin(t.get(1) + t.get(2) * u);
	  };
	  private static readonly System.Func<DoubleArray, double, double> DERIVATIVE_FUNCTION = (DoubleArray t, double? u) =>
	  {
	  return t.get(2) * Math.Cos(t.get(1) + t.get(2) * u);
	  };
	  private static readonly System.Func<DoubleArray, double, DoubleArray> SENSITIVITY_FUNCTION = (DoubleArray t, double? u) =>
	  {
	  return DoubleArray.of(1d, Math.Cos(t.get(1) + t.get(2) * u), u * Math.Cos(t.get(1) + t.get(2) * u));
	  };

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ParameterizedFunctionalCurve test = ParameterizedFunctionalCurve.of(METADATA, PARAMETERS, VALUE_FUNCTION, DERIVATIVE_FUNCTION, SENSITIVITY_FUNCTION);
		assertEquals(test.ValueFunction, VALUE_FUNCTION);
		assertEquals(test.DerivativeFunction, DERIVATIVE_FUNCTION);
		assertEquals(test.SensitivityFunction, SENSITIVITY_FUNCTION);
		assertEquals(test.Metadata, METADATA);
		assertEquals(test.Name, METADATA.CurveName);
		assertEquals(test.getParameter(2), PARAMETERS.get(2));
		assertEquals(test.ParameterCount, PARAMETERS.size());
		assertEquals(test.getParameterMetadata(1), METADATA.getParameterMetadata(1));
		assertEquals(test.Parameters, PARAMETERS);
	  }

	  //-------------------------------------------------------------------------

	  public virtual void test_withParameter()
	  {
		ParameterizedFunctionalCurve @base = ParameterizedFunctionalCurve.of(METADATA, PARAMETERS, VALUE_FUNCTION, DERIVATIVE_FUNCTION, SENSITIVITY_FUNCTION);
		DoubleArray parameters = DoubleArray.of(1.2, 1d, 8.9);
		ParameterizedFunctionalCurve expected = ParameterizedFunctionalCurve.of(METADATA, parameters, VALUE_FUNCTION, DERIVATIVE_FUNCTION, SENSITIVITY_FUNCTION);
		assertEquals(@base.withParameter(1, 1d), expected);
	  }

	  public virtual void test_withPerturbation()
	  {
		ParameterizedFunctionalCurve @base = ParameterizedFunctionalCurve.of(METADATA, PARAMETERS, VALUE_FUNCTION, DERIVATIVE_FUNCTION, SENSITIVITY_FUNCTION);
		DoubleArray parameters = PARAMETERS.minus(2d);
		ParameterizedFunctionalCurve expected = ParameterizedFunctionalCurve.of(METADATA, parameters, VALUE_FUNCTION, DERIVATIVE_FUNCTION, SENSITIVITY_FUNCTION);
		assertEquals(@base.withPerturbation((i, v, m) => v - 2d), expected);
	  }

	  public virtual void test_withMetadata()
	  {
		ParameterizedFunctionalCurve @base = ParameterizedFunctionalCurve.of(METADATA, PARAMETERS, VALUE_FUNCTION, DERIVATIVE_FUNCTION, SENSITIVITY_FUNCTION);
		CurveMetadata metadata = DefaultCurveMetadata.builder().curveName("test").yValueType(ValueType.DISCOUNT_FACTOR).xValueType(ValueType.YEAR_FRACTION).build();
		ParameterizedFunctionalCurve expected = ParameterizedFunctionalCurve.of(metadata, PARAMETERS, VALUE_FUNCTION, DERIVATIVE_FUNCTION, SENSITIVITY_FUNCTION);
		assertEquals(@base.withMetadata(metadata), expected);
	  }

	  public virtual void test_withParameters()
	  {
		ParameterizedFunctionalCurve @base = ParameterizedFunctionalCurve.of(METADATA, PARAMETERS, VALUE_FUNCTION, DERIVATIVE_FUNCTION, SENSITIVITY_FUNCTION);
		DoubleArray parameters = DoubleArray.of(1d, 2d, 3d);
		ParameterizedFunctionalCurve expected = ParameterizedFunctionalCurve.of(METADATA, parameters, VALUE_FUNCTION, DERIVATIVE_FUNCTION, SENSITIVITY_FUNCTION);
		assertEquals(@base.withParameters(parameters), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_values()
	  {
		ParameterizedFunctionalCurve test = ParameterizedFunctionalCurve.of(METADATA, PARAMETERS, VALUE_FUNCTION, DERIVATIVE_FUNCTION, SENSITIVITY_FUNCTION);
		double x = 5.2;
		assertEquals(test.yValue(x), VALUE_FUNCTION.apply(PARAMETERS, x));
		assertEquals(test.firstDerivative(x), DERIVATIVE_FUNCTION.apply(PARAMETERS, x));
		assertEquals(test.yValueParameterSensitivity(x), UnitParameterSensitivity.of(METADATA.CurveName, METADATA.ParameterMetadata.get(), SENSITIVITY_FUNCTION.apply(PARAMETERS, x)));
	  }

	  public virtual void test_sensitivities()
	  {
		ParameterizedFunctionalCurve test = ParameterizedFunctionalCurve.of(METADATA, PARAMETERS, VALUE_FUNCTION, DERIVATIVE_FUNCTION, SENSITIVITY_FUNCTION);
		DoubleArray sensiVal = DoubleArray.of(1d, 2d, 3d);
		assertEquals(test.createParameterSensitivity(sensiVal), UnitParameterSensitivity.of(METADATA.CurveName, METADATA.ParameterMetadata.get(), sensiVal));
		assertEquals(test.createParameterSensitivity(USD, sensiVal), CurrencyParameterSensitivity.of(METADATA.CurveName, METADATA.ParameterMetadata.get(), USD, sensiVal));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ParameterizedFunctionalCurve test1 = ParameterizedFunctionalCurve.of(METADATA, PARAMETERS, VALUE_FUNCTION, DERIVATIVE_FUNCTION, SENSITIVITY_FUNCTION);
		coverImmutableBean(test1);

		DoubleArray @params = DoubleArray.of(1.2);
		CurveMetadata metadata = DefaultCurveMetadata.builder().curveName("test").yValueType(ValueType.DISCOUNT_FACTOR).xValueType(ValueType.YEAR_FRACTION).build();
		System.Func<DoubleArray, double, double> value = (DoubleArray t, double? u) =>
		{
		return t.get(0) * u;
		};
		System.Func<DoubleArray, double, double> deriv = (DoubleArray t, double? u) =>
		{
		return t.get(0);
		};
		System.Func<DoubleArray, double, DoubleArray> sensi = (DoubleArray t, double? u) =>
		{
		return DoubleArray.of(u);
		};
		ParameterizedFunctionalCurve test2 = ParameterizedFunctionalCurve.of(metadata, @params, value, deriv, sensi);
		coverBeanEquals(test1, test2);
	  }

	}

}