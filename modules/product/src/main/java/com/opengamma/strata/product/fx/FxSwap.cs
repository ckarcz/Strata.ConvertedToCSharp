using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fx
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static Math.signum;


	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// An FX swap.
	/// <para>
	/// An FX swap is a financial instrument that represents the exchange of an equivalent amount
	/// in two different currencies between counterparties on two different dates.
	/// </para>
	/// <para>
	/// The two exchanges are based on the same currency pair, with the two payment flows in the opposite directions.
	/// </para>
	/// <para>
	/// For example, an FX swap might represent the payment of USD 1,000 and the receipt of EUR 932
	/// on one date, and the payment of EUR 941 and the receipt of USD 1,000 at a later date.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class FxSwap implements FxProduct, com.opengamma.strata.basics.Resolvable<ResolvedFxSwap>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxSwap : FxProduct, Resolvable<ResolvedFxSwap>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final FxSingle nearLeg;
		private readonly FxSingle nearLeg;
	  /// <summary>
	  /// The foreign exchange transaction at the later date.
	  /// <para>
	  /// This provides details of a single foreign exchange at a specific date.
	  /// The payment date of this transaction must be after that of the near leg.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final FxSingle farLeg;
	  private readonly FxSingle farLeg;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an {@code FxSwap} from two transactions.
	  /// <para>
	  /// The transactions must be passed in with value dates in the correct order.
	  /// The currency pair of each leg must match and have amounts flowing in opposite directions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="nearLeg">  the earlier leg </param>
	  /// <param name="farLeg">  the later leg </param>
	  /// <returns> the FX swap </returns>
	  public static FxSwap of(FxSingle nearLeg, FxSingle farLeg)
	  {
		return new FxSwap(nearLeg, farLeg);
	  }

	  /// <summary>
	  /// Creates an {@code FxSwap} using two FX rates, near and far, specifying a date adjustment.
	  /// <para>
	  /// The FX rate at the near date is specified as {@code nearRate}.
	  /// The FX rate at the far date is specified as {@code farRate}.
	  /// The FX rates must have the same currency pair.
	  /// </para>
	  /// <para>
	  /// The two currencies are specified by the FX rates.
	  /// The amount must be specified using one of the currencies of the near FX rate.
	  /// The near date must be before the far date.
	  /// Conventions will be used to determine the base and counter currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount being exchanged, positive if being received in the near leg, negative if being paid </param>
	  /// <param name="nearRate">  the near FX rate </param>
	  /// <param name="farRate">  the far FX rate </param>
	  /// <param name="nearDate">  the near value date </param>
	  /// <param name="farDate">  the far value date </param>
	  /// <returns> the FX swap </returns>
	  /// <exception cref="IllegalArgumentException"> if the FX rate and amount do not have a currency in common,
	  ///   or if the FX rate currencies differ </exception>
	  public static FxSwap of(CurrencyAmount amount, FxRate nearRate, LocalDate nearDate, FxRate farRate, LocalDate farDate)
	  {

		Currency currency1 = amount.Currency;
		if (!nearRate.Pair.contains(currency1))
		{
		  throw new System.ArgumentException(Messages.format("FxRate '{}' and CurrencyAmount '{}' must have a currency in common", nearRate, amount));
		}
		if (!nearRate.Pair.toConventional().Equals(farRate.Pair.toConventional()))
		{
		  throw new System.ArgumentException(Messages.format("FxRate '{}' and FxRate '{}' must contain the same currencies", nearRate, farRate));
		}
		FxSingle nearLeg = FxSingle.of(amount, nearRate, nearDate);
		FxSingle farLeg = FxSingle.of(amount.negated(), farRate, farDate);
		return of(nearLeg, farLeg);
	  }

	  /// <summary>
	  /// Creates an {@code FxSwap} using two FX rates, near and far, specifying a date adjustment.
	  /// <para>
	  /// The FX rate at the near date is specified as {@code nearRate}.
	  /// The FX rate at the far date is specified as {@code farRate}.
	  /// The FX rates must have the same currency pair.
	  /// </para>
	  /// <para>
	  /// The two currencies are specified by the FX rates.
	  /// The amount must be specified using one of the currencies of the near FX rate.
	  /// The near date must be before the far date.
	  /// Conventions will be used to determine the base and counter currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount being exchanged, positive if being received in the near leg, negative if being paid </param>
	  /// <param name="nearRate">  the near FX rate </param>
	  /// <param name="farRate">  the far FX rate </param>
	  /// <param name="nearDate">  the near value date </param>
	  /// <param name="farDate">  the far value date </param>
	  /// <param name="paymentDateAdjustment">  the adjustment to apply to the payment dates </param>
	  /// <returns> the FX swap </returns>
	  /// <exception cref="IllegalArgumentException"> if the FX rate and amount do not have a currency in common,
	  ///   or if the FX rate currencies differ </exception>
	  public static FxSwap of(CurrencyAmount amount, FxRate nearRate, LocalDate nearDate, FxRate farRate, LocalDate farDate, BusinessDayAdjustment paymentDateAdjustment)
	  {

		ArgChecker.notNull(paymentDateAdjustment, "paymentDateAdjustment");
		Currency currency1 = amount.Currency;
		if (!nearRate.Pair.contains(currency1))
		{
		  throw new System.ArgumentException(Messages.format("FxRate '{}' and CurrencyAmount '{}' must have a currency in common", nearRate, amount));
		}
		if (!nearRate.Pair.toConventional().Equals(farRate.Pair.toConventional()))
		{
		  throw new System.ArgumentException(Messages.format("FxRate '{}' and FxRate '{}' must contain the same currencies", nearRate, farRate));
		}
		FxSingle nearLeg = FxSingle.of(amount, nearRate, nearDate, paymentDateAdjustment);
		FxSingle farLeg = FxSingle.of(amount.negated(), farRate, farDate, paymentDateAdjustment);
		return of(nearLeg, farLeg);
	  }

	  /// <summary>
	  /// Creates an {@code FxSwap} using decimal forward points.
	  /// <para>
	  /// The FX rate at the near date is specified as {@code nearRate}.
	  /// The FX rate at the far date is equal to {@code nearRate + forwardPoints}.
	  /// Thus "FX forward spread" might be a better name for the concept.
	  /// </para>
	  /// <para>
	  /// The two currencies are specified by the near FX rate.
	  /// The amount must be specified using one of the currencies of the near FX rate.
	  /// The near date must be before the far date.
	  /// Conventions will be used to determine the base and counter currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount being exchanged, positive if being received in the near leg, negative if being paid </param>
	  /// <param name="nearRate">  the near FX rate </param>
	  /// <param name="decimalForwardPoints">  the decimal forward points, where the far FX rate is {@code (nearRate + forwardPoints)} </param>
	  /// <param name="nearDate">  the near value date </param>
	  /// <param name="farDate">  the far value date </param>
	  /// <returns> the FX swap </returns>
	  /// <exception cref="IllegalArgumentException"> if the FX rate and amount do not have a currency in common </exception>
	  public static FxSwap ofForwardPoints(CurrencyAmount amount, FxRate nearRate, double decimalForwardPoints, LocalDate nearDate, LocalDate farDate)
	  {

		FxRate farRate = FxRate.of(nearRate.Pair, nearRate.fxRate(nearRate.Pair) + decimalForwardPoints);
		return of(amount, nearRate, nearDate, farRate, farDate);
	  }

	  /// <summary>
	  /// Creates an {@code FxSwap} using decimal forward points, specifying a date adjustment.
	  /// <para>
	  /// The FX rate at the near date is specified as {@code nearRate}.
	  /// The FX rate at the far date is equal to {@code nearRate + forwardPoints}
	  /// Thus "FX forward spread" might be a better name for the concept.
	  /// </para>
	  /// <para>
	  /// The two currencies are specified by the near FX rate.
	  /// The amount must be specified using one of the currencies of the near FX rate.
	  /// The near date must be before the far date.
	  /// Conventions will be used to determine the base and counter currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount being exchanged, positive if being received in the near leg, negative if being paid </param>
	  /// <param name="nearRate">  the near FX rate </param>
	  /// <param name="decimalForwardPoints">  the decimal forward points, where the far FX rate is {@code (nearRate + forwardPoints)} </param>
	  /// <param name="nearDate">  the near value date </param>
	  /// <param name="farDate">  the far value date </param>
	  /// <param name="paymentDateAdjustment">  the adjustment to apply to the payment dates </param>
	  /// <returns> the FX swap </returns>
	  /// <exception cref="IllegalArgumentException"> if the FX rate and amount do not have a currency in common </exception>
	  public static FxSwap ofForwardPoints(CurrencyAmount amount, FxRate nearRate, double decimalForwardPoints, LocalDate nearDate, LocalDate farDate, BusinessDayAdjustment paymentDateAdjustment)
	  {

		FxRate farRate = FxRate.of(nearRate.Pair, nearRate.fxRate(nearRate.Pair) + decimalForwardPoints);
		return of(amount, nearRate, nearDate, farRate, farDate, paymentDateAdjustment);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.inOrderNotEqual(nearLeg.PaymentDate, farLeg.PaymentDate, "nearLeg.paymentDate", "farLeg.paymentDate");
		if (!nearLeg.BaseCurrencyAmount.Currency.Equals(farLeg.BaseCurrencyAmount.Currency) || !nearLeg.CounterCurrencyAmount.Currency.Equals(farLeg.CounterCurrencyAmount.Currency))
		{
		  throw new System.ArgumentException("Legs must have the same currency pair");
		}
		if (signum(nearLeg.BaseCurrencyAmount.Amount) == signum(farLeg.BaseCurrencyAmount.Amount))
		{
		  throw new System.ArgumentException("Legs must have payments flowing in opposite directions");
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency pair in conventional order.
	  /// </summary>
	  /// <returns> the currency pair </returns>
	  public CurrencyPair CurrencyPair
	  {
		  get
		  {
			return NearLeg.CurrencyPair.toConventional();
		  }
	  }

	  public ResolvedFxSwap resolve(ReferenceData refData)
	  {
		return ResolvedFxSwap.of(nearLeg.resolve(refData), farLeg.resolve(refData));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxSwap}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxSwap.Meta meta()
	  {
		return FxSwap.Meta.INSTANCE;
	  }

	  static FxSwap()
	  {
		MetaBean.register(FxSwap.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private FxSwap(FxSingle nearLeg, FxSingle farLeg)
	  {
		JodaBeanUtils.notNull(nearLeg, "nearLeg");
		JodaBeanUtils.notNull(farLeg, "farLeg");
		this.nearLeg = nearLeg;
		this.farLeg = farLeg;
		validate();
	  }

	  public override FxSwap.Meta metaBean()
	  {
		return FxSwap.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the foreign exchange transaction at the earlier date.
	  /// <para>
	  /// This provides details of a single foreign exchange at a specific date.
	  /// The payment date of this transaction must be before that of the far leg.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxSingle NearLeg
	  {
		  get
		  {
			return nearLeg;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the foreign exchange transaction at the later date.
	  /// <para>
	  /// This provides details of a single foreign exchange at a specific date.
	  /// The payment date of this transaction must be after that of the near leg.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxSingle FarLeg
	  {
		  get
		  {
			return farLeg;
		  }
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  FxSwap other = (FxSwap) obj;
		  return JodaBeanUtils.equal(nearLeg, other.nearLeg) && JodaBeanUtils.equal(farLeg, other.farLeg);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(nearLeg);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(farLeg);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("FxSwap{");
		buf.Append("nearLeg").Append('=').Append(nearLeg).Append(',').Append(' ');
		buf.Append("farLeg").Append('=').Append(JodaBeanUtils.ToString(farLeg));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxSwap}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  nearLeg_Renamed = DirectMetaProperty.ofImmutable(this, "nearLeg", typeof(FxSwap), typeof(FxSingle));
			  farLeg_Renamed = DirectMetaProperty.ofImmutable(this, "farLeg", typeof(FxSwap), typeof(FxSingle));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "nearLeg", "farLeg");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code nearLeg} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxSingle> nearLeg_Renamed;
		/// <summary>
		/// The meta-property for the {@code farLeg} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxSingle> farLeg_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "nearLeg", "farLeg");
		internal IDictionary<string, MetaProperty<object>> metaPropertyMap$;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Meta()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override protected org.joda.beans.MetaProperty<?> metaPropertyGet(String propertyName)
		protected internal override MetaProperty<object> metaPropertyGet(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1825755334: // nearLeg
			  return nearLeg_Renamed;
			case -1281739913: // farLeg
			  return farLeg_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends FxSwap> builder()
		public override BeanBuilder<FxSwap> builder()
		{
		  return new FxSwap.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxSwap);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code nearLeg} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxSingle> nearLeg()
		{
		  return nearLeg_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code farLeg} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxSingle> farLeg()
		{
		  return farLeg_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1825755334: // nearLeg
			  return ((FxSwap) bean).NearLeg;
			case -1281739913: // farLeg
			  return ((FxSwap) bean).FarLeg;
		  }
		  return base.propertyGet(bean, propertyName, quiet);
		}

		protected internal override void propertySet(Bean bean, string propertyName, object newValue, bool quiet)
		{
		  metaProperty(propertyName);
		  if (quiet)
		  {
			return;
		  }
		  throw new System.NotSupportedException("Property cannot be written: " + propertyName);
		}

	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The bean-builder for {@code FxSwap}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<FxSwap>
	  {

		internal FxSingle nearLeg;
		internal FxSingle farLeg;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1825755334: // nearLeg
			  return nearLeg;
			case -1281739913: // farLeg
			  return farLeg;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1825755334: // nearLeg
			  this.nearLeg = (FxSingle) newValue;
			  break;
			case -1281739913: // farLeg
			  this.farLeg = (FxSingle) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override FxSwap build()
		{
		  return new FxSwap(nearLeg, farLeg);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("FxSwap.Builder{");
		  buf.Append("nearLeg").Append('=').Append(JodaBeanUtils.ToString(nearLeg)).Append(',').Append(' ');
		  buf.Append("farLeg").Append('=').Append(JodaBeanUtils.ToString(farLeg));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}