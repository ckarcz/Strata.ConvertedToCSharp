/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.dateUtc;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Test <seealso cref="IborCapletFloorletVolatilities"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborCapletFloorletVolatilitiesTest
	public class IborCapletFloorletVolatilitiesTest
	{

	  private static readonly ZonedDateTime DATE_TIME = dateUtc(2015, 8, 27);

	  //-------------------------------------------------------------------------
	  public virtual void test_defaultMethods()
	  {
		IborCapletFloorletVolatilities test = new TestingIborCapletFloorletVolatilities();
		assertEquals(test.ValuationDate, DATE_TIME.toLocalDate());
		assertEquals(test.volatility(DATE_TIME, 1, 2), 6d);
	  }

	  internal class TestingIborCapletFloorletVolatilities : IborCapletFloorletVolatilities
	  {

		public virtual IborIndex Index
		{
			get
			{
			  return GBP_LIBOR_3M;
			}
		}

		public virtual ZonedDateTime ValuationDateTime
		{
			get
			{
			  return DATE_TIME;
			}
		}

		public virtual Optional<T> findData<T>(MarketDataName<T> name)
		{
		  return null;
		}

		public virtual double volatility(double expiry, double strike, double forward)
		{
		  return expiry * 2d;
		}

		public virtual double price(double expiry, PutCall putCall, double strike, double forward, double volatility)
		{
		  throw new System.NotSupportedException();
		}

		public virtual double priceDelta(double expiry, PutCall putCall, double strike, double forward, double volatility)
		{
		  throw new System.NotSupportedException();
		}

		public virtual double priceGamma(double expiry, PutCall putCall, double strike, double forward, double volatility)
		{
		  throw new System.NotSupportedException();
		}

		public virtual double priceTheta(double expiry, PutCall putCall, double strike, double forward, double volatility)
		{
		  throw new System.NotSupportedException();
		}

		public virtual double priceVega(double expiry, PutCall putCall, double strike, double forward, double volatility)
		{
		  throw new System.NotSupportedException();
		}

		public virtual double relativeTime(ZonedDateTime date)
		{
		  return 3d;
		}

		public virtual int ParameterCount
		{
			get
			{
			  return 0;
			}
		}

		public virtual double getParameter(int parameterIndex)
		{
		  return 0;
		}

		public virtual ParameterMetadata getParameterMetadata(int parameterIndex)
		{
		  return null;
		}

		public virtual IborCapletFloorletVolatilitiesName Name
		{
			get
			{
			  return null;
			}
		}

		public virtual ValueType VolatilityType
		{
			get
			{
			  return null;
			}
		}

		public virtual IborCapletFloorletVolatilities withParameter(int parameterIndex, double newValue)
		{
		  return null;
		}

		public virtual IborCapletFloorletVolatilities withPerturbation(ParameterPerturbation perturbation)
		{
		  return null;
		}

		public virtual CurrencyParameterSensitivities parameterSensitivity(PointSensitivities pointSensitivities)
		{
		  return null;
		}

	  }

	}

}