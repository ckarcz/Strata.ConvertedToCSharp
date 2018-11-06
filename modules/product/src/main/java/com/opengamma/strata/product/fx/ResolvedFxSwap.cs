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
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// An FX Swap, resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="FxSwap"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedFxSwap} from a {@code FxSwap}
	/// using <seealso cref="FxSwap#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// A {@code ResolvedFxSwap} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ResolvedFxSwap implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedFxSwap : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ResolvedFxSingle nearLeg;
		private readonly ResolvedFxSingle nearLeg;
	  /// <summary>
	  /// The foreign exchange transaction at the later date.
	  /// <para>
	  /// This provides details of a single foreign exchange at a specific date.
	  /// The payment date of this transaction must be after that of the near leg.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ResolvedFxSingle farLeg;
	  private readonly ResolvedFxSingle farLeg;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a {@code ResolvedFxSwap} from two legs.
	  /// <para>
	  /// The transactions must be passed in with payment dates in the correct order.
	  /// The currency pair of each leg must match and have amounts flowing in opposite directions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="nearLeg">  the earlier leg </param>
	  /// <param name="farLeg">  the later leg </param>
	  /// <returns> the resolved FX swap </returns>
	  public static ResolvedFxSwap of(ResolvedFxSingle nearLeg, ResolvedFxSingle farLeg)
	  {
		return new ResolvedFxSwap(nearLeg, farLeg);
	  }

	  /// <summary>
	  /// Creates a {@code ResolvedFxSwap} using forward points.
	  /// <para>
	  /// The FX rate at the near date is specified as {@code fxRate}.
	  /// The FX rate at the far date is equal to {@code fxRate + forwardPoints}
	  /// </para>
	  /// <para>
	  /// The two currencies must not be equal.
	  /// The near date must be before the far date.
	  /// Conventions will be used to determine the base and counter currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amountCurrency1">  the amount of the near leg in the first currency </param>
	  /// <param name="currency2">  the second currency </param>
	  /// <param name="nearFxRate">  the near FX rate, where {@code (1.0 * amountCurrency1 = fxRate * amountCurrency2)} </param>
	  /// <param name="forwardPoints">  the forward points, where the far FX rate is {@code (fxRate + forwardPoints)} </param>
	  /// <param name="nearDate">  the near value date </param>
	  /// <param name="farDate">  the far value date </param>
	  /// <returns> the resolved FX swap </returns>
	  public static ResolvedFxSwap ofForwardPoints(CurrencyAmount amountCurrency1, Currency currency2, double nearFxRate, double forwardPoints, LocalDate nearDate, LocalDate farDate)
	  {

		Currency currency1 = amountCurrency1.Currency;
		ArgChecker.isFalse(currency1.Equals(currency2), "Currencies must not be equal");
		ArgChecker.notNegativeOrZero(nearFxRate, "fxRate");
		double farFxRate = nearFxRate + forwardPoints;
		ResolvedFxSingle nearLeg = ResolvedFxSingle.of(amountCurrency1, FxRate.of(currency1, currency2, nearFxRate), nearDate);
		ResolvedFxSingle farLeg = ResolvedFxSingle.of(amountCurrency1.negated(), FxRate.of(currency1, currency2, farFxRate), farDate);
		return of(nearLeg, farLeg);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.inOrderNotEqual(nearLeg.PaymentDate, farLeg.PaymentDate, "nearLeg.paymentDate", "farLeg.paymentDate");
		if (!nearLeg.BaseCurrencyPayment.Currency.Equals(farLeg.BaseCurrencyPayment.Currency) || !nearLeg.CounterCurrencyPayment.Currency.Equals(farLeg.CounterCurrencyPayment.Currency))
		{
		  throw new System.ArgumentException("Legs must have the same currency pair");
		}
		if (signum(nearLeg.BaseCurrencyPayment.Amount) == signum(farLeg.BaseCurrencyPayment.Amount))
		{
		  throw new System.ArgumentException("Legs must have payments flowing in opposite directions");
		}
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedFxSwap}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedFxSwap.Meta meta()
	  {
		return ResolvedFxSwap.Meta.INSTANCE;
	  }

	  static ResolvedFxSwap()
	  {
		MetaBean.register(ResolvedFxSwap.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ResolvedFxSwap(ResolvedFxSingle nearLeg, ResolvedFxSingle farLeg)
	  {
		JodaBeanUtils.notNull(nearLeg, "nearLeg");
		JodaBeanUtils.notNull(farLeg, "farLeg");
		this.nearLeg = nearLeg;
		this.farLeg = farLeg;
		validate();
	  }

	  public override ResolvedFxSwap.Meta metaBean()
	  {
		return ResolvedFxSwap.Meta.INSTANCE;
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
	  public ResolvedFxSingle NearLeg
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
	  public ResolvedFxSingle FarLeg
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
		  ResolvedFxSwap other = (ResolvedFxSwap) obj;
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
		buf.Append("ResolvedFxSwap{");
		buf.Append("nearLeg").Append('=').Append(nearLeg).Append(',').Append(' ');
		buf.Append("farLeg").Append('=').Append(JodaBeanUtils.ToString(farLeg));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedFxSwap}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  nearLeg_Renamed = DirectMetaProperty.ofImmutable(this, "nearLeg", typeof(ResolvedFxSwap), typeof(ResolvedFxSingle));
			  farLeg_Renamed = DirectMetaProperty.ofImmutable(this, "farLeg", typeof(ResolvedFxSwap), typeof(ResolvedFxSingle));
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
		internal MetaProperty<ResolvedFxSingle> nearLeg_Renamed;
		/// <summary>
		/// The meta-property for the {@code farLeg} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ResolvedFxSingle> farLeg_Renamed;
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
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ResolvedFxSwap> builder()
		public override BeanBuilder<ResolvedFxSwap> builder()
		{
		  return new ResolvedFxSwap.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedFxSwap);
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
		public MetaProperty<ResolvedFxSingle> nearLeg()
		{
		  return nearLeg_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code farLeg} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ResolvedFxSingle> farLeg()
		{
		  return farLeg_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1825755334: // nearLeg
			  return ((ResolvedFxSwap) bean).NearLeg;
			case -1281739913: // farLeg
			  return ((ResolvedFxSwap) bean).FarLeg;
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
	  /// The bean-builder for {@code ResolvedFxSwap}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ResolvedFxSwap>
	  {

		internal ResolvedFxSingle nearLeg;
		internal ResolvedFxSingle farLeg;

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
			  this.nearLeg = (ResolvedFxSingle) newValue;
			  break;
			case -1281739913: // farLeg
			  this.farLeg = (ResolvedFxSingle) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ResolvedFxSwap build()
		{
		  return new ResolvedFxSwap(nearLeg, farLeg);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("ResolvedFxSwap.Builder{");
		  buf.Append("nearLeg").Append('=').Append(JodaBeanUtils.ToString(nearLeg)).Append(',').Append(' ');
		  buf.Append("farLeg").Append('=').Append(JodaBeanUtils.ToString(farLeg));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}