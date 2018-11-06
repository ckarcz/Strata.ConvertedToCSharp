using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.rate
{

	using Bean = org.joda.beans.Bean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using BasicBeanBuilder = org.joda.beans.impl.BasicBeanBuilder;
	using MinimalMetaBean = org.joda.beans.impl.direct.MinimalMetaBean;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Index = com.opengamma.strata.basics.index.Index;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using FxForwardRates = com.opengamma.strata.pricer.fx.FxForwardRates;
	using FxIndexRates = com.opengamma.strata.pricer.fx.FxIndexRates;

	/// <summary>
	/// A simple rates provider for overnight rates.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "minimal") public final class SimpleRatesProvider implements RatesProvider, org.joda.beans.Bean
	public sealed class SimpleRatesProvider : RatesProvider, Bean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private java.time.LocalDate valuationDate;
		private LocalDate valuationDate;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private com.opengamma.strata.basics.date.DayCount dayCount;
	  private DayCount dayCount;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private com.opengamma.strata.pricer.DiscountFactors discountFactors;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private DiscountFactors discountFactors_Renamed;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private com.opengamma.strata.pricer.fx.FxIndexRates fxIndexRates;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private FxIndexRates fxIndexRates_Renamed;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private com.opengamma.strata.pricer.fx.FxForwardRates fxForwardRates;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private FxForwardRates fxForwardRates_Renamed;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private IborIndexRates iborRates;
	  private IborIndexRates iborRates;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private OvernightIndexRates overnightRates;
	  private OvernightIndexRates overnightRates;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private PriceIndexValues priceIndexValues;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private PriceIndexValues priceIndexValues_Renamed;

	  public SimpleRatesProvider()
	  {
	  }

	  public SimpleRatesProvider(LocalDate valuationDate)
	  {
		this.valuationDate = valuationDate;
	  }

	  public SimpleRatesProvider(OvernightIndexRates overnightRates)
	  {
		this.overnightRates = overnightRates;
	  }

	  public SimpleRatesProvider(LocalDate valuationDate, OvernightIndexRates overnightRates)
	  {
		this.valuationDate = valuationDate;
		this.overnightRates = overnightRates;
	  }

	  public SimpleRatesProvider(DiscountFactors discountFactors)
	  {
		this.discountFactors_Renamed = discountFactors;
	  }

	  public SimpleRatesProvider(LocalDate valuationDate, DiscountFactors discountFactors)
	  {
		this.valuationDate = valuationDate;
		this.discountFactors_Renamed = discountFactors;
	  }

	  public SimpleRatesProvider(LocalDate valuationDate, DiscountFactors discountFactors, IborIndexRates iborRates)
	  {
		this.valuationDate = valuationDate;
		this.discountFactors_Renamed = discountFactors;
		this.iborRates = iborRates;
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableSet<Currency> DiscountCurrencies
	  {
		  get
		  {
			if (discountFactors_Renamed != null)
			{
			  return ImmutableSet.of(discountFactors_Renamed.Currency);
			}
			return ImmutableSet.of();
		  }
	  }

	  public ImmutableSet<IborIndex> IborIndices
	  {
		  get
		  {
			if (iborRates != null)
			{
			  return ImmutableSet.of(iborRates.Index);
			}
			return ImmutableSet.of();
		  }
	  }

	  public ImmutableSet<OvernightIndex> OvernightIndices
	  {
		  get
		  {
			if (overnightRates != null)
			{
			  return ImmutableSet.of(overnightRates.Index);
			}
			return ImmutableSet.of();
		  }
	  }

	  public ImmutableSet<PriceIndex> PriceIndices
	  {
		  get
		  {
			if (priceIndexValues_Renamed != null)
			{
			  return ImmutableSet.of(priceIndexValues_Renamed.Index);
			}
			return ImmutableSet.of();
		  }
	  }

	  public ImmutableSet<Index> TimeSeriesIndices
	  {
		  get
		  {
			return ImmutableSet.of();
		  }
	  }

	  //-------------------------------------------------------------------------
	  public T data<T>(MarketDataId<T> key)
	  {
		throw new System.NotSupportedException();
	  }

	  public double fxRate(Currency baseCurrency, Currency counterCurrency)
	  {
		if (baseCurrency.Equals(counterCurrency))
		{
		  return 1d;
		}
		throw new System.NotSupportedException("FxRate not found: " + baseCurrency + "/" + counterCurrency);
	  }

	  public DiscountFactors discountFactors(Currency currency)
	  {
		return discountFactors_Renamed;
	  }

	  public FxIndexRates fxIndexRates(FxIndex index)
	  {
		return fxIndexRates_Renamed;
	  }

	  public FxForwardRates fxForwardRates(CurrencyPair currencyPair)
	  {
		return fxForwardRates_Renamed;
	  }

	  public IborIndexRates iborIndexRates(IborIndex index)
	  {
		return iborRates;
	  }

	  public OvernightIndexRates overnightIndexRates(OvernightIndex index)
	  {
		return overnightRates;
	  }

	  public PriceIndexValues priceIndexValues(PriceIndex index)
	  {
		return priceIndexValues_Renamed;
	  }

	  public Optional<T> findData<T>(MarketDataName<T> name)
	  {
		return null;
	  }

	  public LocalDateDoubleTimeSeries timeSeries(Index index)
	  {
		throw new System.NotSupportedException();
	  }

	  public ImmutableRatesProvider toImmutableRatesProvider()
	  {
		throw new System.NotSupportedException();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SimpleRatesProvider}.
	  /// </summary>
	  private static readonly TypedMetaBean<SimpleRatesProvider> META_BEAN = MinimalMetaBean.of(typeof(SimpleRatesProvider), new string[] {"valuationDate", "dayCount", "discountFactors", "fxIndexRates", "fxForwardRates", "iborRates", "overnightRates", "priceIndexValues"}, () => new BasicBeanBuilder<SimpleRatesProvider>(new SimpleRatesProvider()), Arrays.asList<System.Func<SimpleRatesProvider, object>>(b => b.ValuationDate, b => b.DayCount, b => b.DiscountFactors, b => b.FxIndexRates, b => b.FxForwardRates, b => b.IborRates, b => b.OvernightRates, b => b.PriceIndexValues), Arrays.asList<System.Action<SimpleRatesProvider, object>>((b, v) => b.setValuationDate((LocalDate) v), (b, v) => b.setDayCount((DayCount) v), (b, v) => b.setDiscountFactors((DiscountFactors) v), (b, v) => b.setFxIndexRates((FxIndexRates) v), (b, v) => b.setFxForwardRates((FxForwardRates) v), (b, v) => b.setIborRates((IborIndexRates) v), (b, v) => b.setOvernightRates((OvernightIndexRates) v), (b, v) => b.setPriceIndexValues((PriceIndexValues) v)));

	  /// <summary>
	  /// The meta-bean for {@code SimpleRatesProvider}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<SimpleRatesProvider> meta()
	  {
		return META_BEAN;
	  }

	  static SimpleRatesProvider()
	  {
		MetaBean.register(META_BEAN);
	  }

	  public override TypedMetaBean<SimpleRatesProvider> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the valuationDate. </summary>
	  /// <returns> the value of the property </returns>
	  public LocalDate ValuationDate
	  {
		  get
		  {
			return valuationDate;
		  }
		  set
		  {
			this.valuationDate = value;
		  }
	  }


	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the dayCount. </summary>
	  /// <returns> the value of the property </returns>
	  public DayCount DayCount
	  {
		  get
		  {
			return dayCount;
		  }
		  set
		  {
			this.dayCount = value;
		  }
	  }


	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the discountFactors. </summary>
	  /// <returns> the value of the property </returns>
	  public DiscountFactors DiscountFactors
	  {
		  get
		  {
			return discountFactors_Renamed;
		  }
		  set
		  {
			this.discountFactors_Renamed = value;
		  }
	  }


	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fxIndexRates. </summary>
	  /// <returns> the value of the property </returns>
	  public FxIndexRates FxIndexRates
	  {
		  get
		  {
			return fxIndexRates_Renamed;
		  }
		  set
		  {
			this.fxIndexRates_Renamed = value;
		  }
	  }


	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fxForwardRates. </summary>
	  /// <returns> the value of the property </returns>
	  public FxForwardRates FxForwardRates
	  {
		  get
		  {
			return fxForwardRates_Renamed;
		  }
		  set
		  {
			this.fxForwardRates_Renamed = value;
		  }
	  }


	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the iborRates. </summary>
	  /// <returns> the value of the property </returns>
	  public IborIndexRates IborRates
	  {
		  get
		  {
			return iborRates;
		  }
		  set
		  {
			this.iborRates = value;
		  }
	  }


	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the overnightRates. </summary>
	  /// <returns> the value of the property </returns>
	  public OvernightIndexRates OvernightRates
	  {
		  get
		  {
			return overnightRates;
		  }
		  set
		  {
			this.overnightRates = value;
		  }
	  }


	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the priceIndexValues. </summary>
	  /// <returns> the value of the property </returns>
	  public PriceIndexValues PriceIndexValues
	  {
		  get
		  {
			return priceIndexValues_Renamed;
		  }
		  set
		  {
			this.priceIndexValues_Renamed = value;
		  }
	  }


	  //-----------------------------------------------------------------------
	  public override SimpleRatesProvider clone()
	  {
		return JodaBeanUtils.cloneAlways(this);
	  }

	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  SimpleRatesProvider other = (SimpleRatesProvider) obj;
		  return JodaBeanUtils.equal(ValuationDate, other.ValuationDate) && JodaBeanUtils.equal(DayCount, other.DayCount) && JodaBeanUtils.equal(DiscountFactors, other.DiscountFactors) && JodaBeanUtils.equal(FxIndexRates, other.FxIndexRates) && JodaBeanUtils.equal(FxForwardRates, other.FxForwardRates) && JodaBeanUtils.equal(IborRates, other.IborRates) && JodaBeanUtils.equal(OvernightRates, other.OvernightRates) && JodaBeanUtils.equal(PriceIndexValues, other.PriceIndexValues);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(ValuationDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(DayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(DiscountFactors);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(FxIndexRates);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(FxForwardRates);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(IborRates);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(OvernightRates);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(PriceIndexValues);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(288);
		buf.Append("SimpleRatesProvider{");
		buf.Append("valuationDate").Append('=').Append(ValuationDate).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(DayCount).Append(',').Append(' ');
		buf.Append("discountFactors").Append('=').Append(DiscountFactors).Append(',').Append(' ');
		buf.Append("fxIndexRates").Append('=').Append(FxIndexRates).Append(',').Append(' ');
		buf.Append("fxForwardRates").Append('=').Append(FxForwardRates).Append(',').Append(' ');
		buf.Append("iborRates").Append('=').Append(IborRates).Append(',').Append(' ');
		buf.Append("overnightRates").Append('=').Append(OvernightRates).Append(',').Append(' ');
		buf.Append("priceIndexValues").Append('=').Append(JodaBeanUtils.ToString(PriceIndexValues));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}