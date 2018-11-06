/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface
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
	/// Test <seealso cref="Surface"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SurfaceTest
	public class SurfaceTest
	{

	  private static readonly SurfaceName SURFACE_NAME = SurfaceName.of("Surface");
	  private static readonly LabelParameterMetadata PARAM_META = LabelParameterMetadata.of("TestParam");

	  public virtual void test_withPerturbation()
	  {
		Surface test = new TestingSurface(2d);
		assertEquals(test.withPerturbation((i, v, m) => v + 1).getParameter(0), 3d);
	  }

	  public virtual void test_createParameterSensitivity_unit()
	  {
		Surface test = new TestingSurface(2d);
		assertEquals(test.createParameterSensitivity(DoubleArray.of(2d)).MarketDataName, SURFACE_NAME);
		assertEquals(test.createParameterSensitivity(DoubleArray.of(2d)).ParameterCount, 1);
		assertEquals(test.createParameterSensitivity(DoubleArray.of(2d)).ParameterMetadata, ImmutableList.of(PARAM_META));
		assertEquals(test.createParameterSensitivity(DoubleArray.of(2d)).Sensitivity, DoubleArray.of(2d));
	  }

	  public virtual void test_createParameterSensitivity_currency()
	  {
		Surface test = new TestingSurface(2d);
		assertEquals(test.createParameterSensitivity(USD, DoubleArray.of(2d)).MarketDataName, SURFACE_NAME);
		assertEquals(test.createParameterSensitivity(USD, DoubleArray.of(2d)).ParameterCount, 1);
		assertEquals(test.createParameterSensitivity(USD, DoubleArray.of(2d)).ParameterMetadata, ImmutableList.of(PARAM_META));
		assertEquals(test.createParameterSensitivity(USD, DoubleArray.of(2d)).Currency, USD);
		assertEquals(test.createParameterSensitivity(USD, DoubleArray.of(2d)).Sensitivity, DoubleArray.of(2d));
	  }

	  //-------------------------------------------------------------------------
	  internal class TestingSurface : Surface
	  {

		internal readonly double value;

		internal TestingSurface(double value)
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

		public virtual SurfaceMetadata Metadata
		{
			get
			{
			  return DefaultSurfaceMetadata.of(SURFACE_NAME);
			}
		}

		public virtual Surface withMetadata(SurfaceMetadata metadata)
		{
		  return this;
		}

		public virtual Surface withParameter(int parameterIndex, double newValue)
		{
		  return new TestingSurface(newValue);
		}

		public virtual double zValue(double x, double y)
		{
		  return value;
		}

		public virtual UnitParameterSensitivity zValueParameterSensitivity(double x, double y)
		{
		  return createParameterSensitivity(DoubleArray.filled(1));
		}
	  }

	}

}