/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.GBP_FIXED_1Y_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;

	/// <summary>
	/// Test <seealso cref="SwaptionVolatilities"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwaptionVolatilitiesTest
	public class SwaptionVolatilitiesTest
	{

	  private static readonly ZonedDateTime DATE_TIME = ZonedDateTime.now();

	  //-------------------------------------------------------------------------
	  public virtual void test_defaultMethods()
	  {
		SwaptionVolatilities test = new TestingSwaptionVolatilities();
		assertEquals(test.ValuationDate, DATE_TIME.toLocalDate());
		assertEquals(test.volatility(DATE_TIME, 1, 2, 3), 6d);
		assertEquals(test.parameterSensitivity(), CurrencyParameterSensitivities.empty());
	  }

	  internal class TestingSwaptionVolatilities : SwaptionVolatilities
	  {

		public virtual SwaptionVolatilitiesName Name
		{
			get
			{
			  return SwaptionVolatilitiesName.of("Default");
			}
		}

		public virtual FixedIborSwapConvention Convention
		{
			get
			{
			  return GBP_FIXED_1Y_LIBOR_3M;
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

		public virtual int ParameterCount
		{
			get
			{
			  throw new System.NotSupportedException();
			}
		}

		public virtual double getParameter(int parameterIndex)
		{
		  throw new System.NotSupportedException();
		}

		public virtual ParameterMetadata getParameterMetadata(int parameterIndex)
		{
		  throw new System.NotSupportedException();
		}

		public virtual SwaptionVolatilities withParameter(int parameterIndex, double newValue)
		{
		  throw new System.NotSupportedException();
		}

		public virtual SwaptionVolatilities withPerturbation(ParameterPerturbation perturbation)
		{
		  throw new System.NotSupportedException();
		}

		public virtual double volatility(double expiry, double tenor, double strike, double forward)
		{
		  return expiry * 2d;
		}

		public virtual CurrencyParameterSensitivities parameterSensitivity(PointSensitivities pointSensitivities)
		{
		  return CurrencyParameterSensitivities.empty();
		}

		public virtual double price(double expiry, double tenor, PutCall putCall, double strike, double forward, double volatility)
		{
		  throw new System.NotSupportedException();
		}

		public virtual double priceDelta(double expiry, double tenor, PutCall putCall, double strike, double forward, double volatility)
		{
		  throw new System.NotSupportedException();
		}

		public virtual double priceGamma(double expiry, double tenor, PutCall putCall, double strike, double forward, double volatility)
		{
		  throw new System.NotSupportedException();
		}

		public virtual double priceTheta(double expiry, double tenor, PutCall putCall, double strike, double forward, double volatility)
		{
		  throw new System.NotSupportedException();
		}

		public virtual double priceVega(double expiry, double tenor, PutCall putCall, double strike, double forward, double volatility)
		{
		  throw new System.NotSupportedException();
		}

		public virtual double relativeTime(ZonedDateTime date)
		{
		  return 3d;
		}

		public virtual double tenor(LocalDate startDate, LocalDate endDate)
		{
		  throw new System.NotSupportedException();
		}

		public virtual LocalDate ValuationDate
		{
			get
			{
			  return ValuationDateTime.toLocalDate();
			}
		}

		public virtual ValueType VolatilityType
		{
			get
			{
			  return ValueType.BLACK_VOLATILITY;
			}
		}

	  }

	}

}