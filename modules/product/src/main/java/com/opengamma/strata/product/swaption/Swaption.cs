using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.ensureOnlyOne;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using LongShort = com.opengamma.strata.product.common.LongShort;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;

	/// <summary>
	/// An option on an underlying swap.
	/// <para>
	/// A swaption is a financial instrument that provides an option based on the future value of a swap.
	/// The option is European, exercised only on the exercise date.
	/// The underlying swap must be a single currency, Fixed-Ibor swap with a single Ibor index and no interpolated stubs.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class Swaption implements com.opengamma.strata.product.Product, com.opengamma.strata.basics.Resolvable<ResolvedSwaption>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class Swaption : Product, Resolvable<ResolvedSwaption>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.common.LongShort longShort;
		private readonly LongShort longShort;
	  /// <summary>
	  /// Settlement method.
	  /// <para>
	  /// The settlement of the option is specified by <seealso cref="SwaptionSettlement"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final SwaptionSettlement swaptionSettlement;
	  private readonly SwaptionSettlement swaptionSettlement;
	  /// <summary>
	  /// The expiry date of the option.
	  /// <para>
	  /// The option is European, and can only be exercised on the expiry date.
	  /// </para>
	  /// <para>
	  /// This date is typically set to be a valid business day.
	  /// However, the {@code businessDayAdjustment} property may be set to provide a rule for adjustment.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.AdjustableDate expiryDate;
	  private readonly AdjustableDate expiryDate;
	  /// <summary>
	  /// The expiry time of the option.
	  /// <para>
	  /// The expiry time is related to the expiry date and time-zone.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalTime expiryTime;
	  private readonly LocalTime expiryTime;
	  /// <summary>
	  /// The time-zone of the expiry time.
	  /// <para>
	  /// The expiry time-zone is related to the expiry date and time.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.ZoneId expiryZone;
	  private readonly ZoneId expiryZone;
	  /// <summary>
	  /// The underlying swap.
	  /// <para>
	  /// At expiry, if the option is exercised, this swap will be entered into.
	  /// The swap description is the swap as viewed by the party long the option.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.swap.Swap underlying;
	  private readonly Swap underlying;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.inOrderOrEqual(expiryDate.Unadjusted, underlying.StartDate.Unadjusted, "expiryDate", "underlying.startDate.unadjusted");
		ArgChecker.isTrue(!underlying.CrossCurrency, "Underlying swap must not be cross-currency");
		ArgChecker.isTrue(underlying.getLegs(SwapLegType.FIXED).size() == 1, "Underlying swap must have one fixed leg");
		ArgChecker.isTrue(underlying.getLegs(SwapLegType.IBOR).size() == 1, "Underlying swap must have one Ibor leg");
		ArgChecker.isTrue(underlying.allIndices().size() == 1, "Underlying swap must have one index");
		ArgChecker.isTrue(underlying.allIndices().GetEnumerator().next() is IborIndex, "Underlying swap must have one Ibor index");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the expiry date-time.
	  /// <para>
	  /// The option expires at this date and time.
	  /// </para>
	  /// <para>
	  /// The result is returned by combining the expiry date, time and time-zone.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the expiry date and time </returns>
	  public ZonedDateTime Expiry
	  {
		  get
		  {
			return expiryDate.Unadjusted.atTime(expiryTime).atZone(expiryZone);
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the swaption.
	  /// <para>
	  /// This is the currency of the underlying swap, which is not allowed to be cross-currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the expiry date and time </returns>
	  public Currency Currency
	  {
		  get
		  {
			return underlying.Legs.Select(leg => leg.Currency).Distinct().Aggregate(ensureOnlyOne()).get();
		  }
	  }

	  public ImmutableSet<Currency> allCurrencies()
	  {
		return ImmutableSet.of(Currency);
	  }

	  /// <summary>
	  /// Gets the index of the underlying swap.
	  /// </summary>
	  /// <returns> the Ibor index of the underlying swap </returns>
	  public IborIndex Index
	  {
		  get
		  {
			return (IborIndex) underlying.allIndices().GetEnumerator().next();
		  }
	  }

	  //-------------------------------------------------------------------------
	  public ResolvedSwaption resolve(ReferenceData refData)
	  {
		return ResolvedSwaption.builder().expiry(expiryDate.adjusted(refData).atTime(expiryTime).atZone(expiryZone)).longShort(longShort).swaptionSettlement(swaptionSettlement).underlying(underlying.resolve(refData)).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code Swaption}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static Swaption.Meta meta()
	  {
		return Swaption.Meta.INSTANCE;
	  }

	  static Swaption()
	  {
		MetaBean.register(Swaption.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static Swaption.Builder builder()
	  {
		return new Swaption.Builder();
	  }

	  private Swaption(LongShort longShort, SwaptionSettlement swaptionSettlement, AdjustableDate expiryDate, LocalTime expiryTime, ZoneId expiryZone, Swap underlying)
	  {
		JodaBeanUtils.notNull(longShort, "longShort");
		JodaBeanUtils.notNull(swaptionSettlement, "swaptionSettlement");
		JodaBeanUtils.notNull(expiryDate, "expiryDate");
		JodaBeanUtils.notNull(expiryTime, "expiryTime");
		JodaBeanUtils.notNull(expiryZone, "expiryZone");
		JodaBeanUtils.notNull(underlying, "underlying");
		this.longShort = longShort;
		this.swaptionSettlement = swaptionSettlement;
		this.expiryDate = expiryDate;
		this.expiryTime = expiryTime;
		this.expiryZone = expiryZone;
		this.underlying = underlying;
		validate();
	  }

	  public override Swaption.Meta metaBean()
	  {
		return Swaption.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the option is long or short.
	  /// <para>
	  /// Long indicates that the owner wants the option to be in the money at expiry.
	  /// Short indicates that the owner wants the option to be out of the money at expiry.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LongShort LongShort
	  {
		  get
		  {
			return longShort;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets settlement method.
	  /// <para>
	  /// The settlement of the option is specified by <seealso cref="SwaptionSettlement"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SwaptionSettlement SwaptionSettlement
	  {
		  get
		  {
			return swaptionSettlement;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the expiry date of the option.
	  /// <para>
	  /// The option is European, and can only be exercised on the expiry date.
	  /// </para>
	  /// <para>
	  /// This date is typically set to be a valid business day.
	  /// However, the {@code businessDayAdjustment} property may be set to provide a rule for adjustment.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public AdjustableDate ExpiryDate
	  {
		  get
		  {
			return expiryDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the expiry time of the option.
	  /// <para>
	  /// The expiry time is related to the expiry date and time-zone.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalTime ExpiryTime
	  {
		  get
		  {
			return expiryTime;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the time-zone of the expiry time.
	  /// <para>
	  /// The expiry time-zone is related to the expiry date and time.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ZoneId ExpiryZone
	  {
		  get
		  {
			return expiryZone;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying swap.
	  /// <para>
	  /// At expiry, if the option is exercised, this swap will be entered into.
	  /// The swap description is the swap as viewed by the party long the option.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Swap Underlying
	  {
		  get
		  {
			return underlying;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder that allows this bean to be mutated. </summary>
	  /// <returns> the mutable builder, not null </returns>
	  public Builder toBuilder()
	  {
		return new Builder(this);
	  }

	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  Swaption other = (Swaption) obj;
		  return JodaBeanUtils.equal(longShort, other.longShort) && JodaBeanUtils.equal(swaptionSettlement, other.swaptionSettlement) && JodaBeanUtils.equal(expiryDate, other.expiryDate) && JodaBeanUtils.equal(expiryTime, other.expiryTime) && JodaBeanUtils.equal(expiryZone, other.expiryZone) && JodaBeanUtils.equal(underlying, other.underlying);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(longShort);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(swaptionSettlement);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiryDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiryTime);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiryZone);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlying);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(224);
		buf.Append("Swaption{");
		buf.Append("longShort").Append('=').Append(longShort).Append(',').Append(' ');
		buf.Append("swaptionSettlement").Append('=').Append(swaptionSettlement).Append(',').Append(' ');
		buf.Append("expiryDate").Append('=').Append(expiryDate).Append(',').Append(' ');
		buf.Append("expiryTime").Append('=').Append(expiryTime).Append(',').Append(' ');
		buf.Append("expiryZone").Append('=').Append(expiryZone).Append(',').Append(' ');
		buf.Append("underlying").Append('=').Append(JodaBeanUtils.ToString(underlying));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code Swaption}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  longShort_Renamed = DirectMetaProperty.ofImmutable(this, "longShort", typeof(Swaption), typeof(LongShort));
			  swaptionSettlement_Renamed = DirectMetaProperty.ofImmutable(this, "swaptionSettlement", typeof(Swaption), typeof(SwaptionSettlement));
			  expiryDate_Renamed = DirectMetaProperty.ofImmutable(this, "expiryDate", typeof(Swaption), typeof(AdjustableDate));
			  expiryTime_Renamed = DirectMetaProperty.ofImmutable(this, "expiryTime", typeof(Swaption), typeof(LocalTime));
			  expiryZone_Renamed = DirectMetaProperty.ofImmutable(this, "expiryZone", typeof(Swaption), typeof(ZoneId));
			  underlying_Renamed = DirectMetaProperty.ofImmutable(this, "underlying", typeof(Swaption), typeof(Swap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "longShort", "swaptionSettlement", "expiryDate", "expiryTime", "expiryZone", "underlying");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code longShort} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LongShort> longShort_Renamed;
		/// <summary>
		/// The meta-property for the {@code swaptionSettlement} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SwaptionSettlement> swaptionSettlement_Renamed;
		/// <summary>
		/// The meta-property for the {@code expiryDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<AdjustableDate> expiryDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code expiryTime} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalTime> expiryTime_Renamed;
		/// <summary>
		/// The meta-property for the {@code expiryZone} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ZoneId> expiryZone_Renamed;
		/// <summary>
		/// The meta-property for the {@code underlying} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Swap> underlying_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "longShort", "swaptionSettlement", "expiryDate", "expiryTime", "expiryZone", "underlying");
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
			case 116685664: // longShort
			  return longShort_Renamed;
			case -1937554512: // swaptionSettlement
			  return swaptionSettlement_Renamed;
			case -816738431: // expiryDate
			  return expiryDate_Renamed;
			case -816254304: // expiryTime
			  return expiryTime_Renamed;
			case -816069761: // expiryZone
			  return expiryZone_Renamed;
			case -1770633379: // underlying
			  return underlying_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override Swaption.Builder builder()
		{
		  return new Swaption.Builder();
		}

		public override Type beanType()
		{
		  return typeof(Swaption);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code longShort} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LongShort> longShort()
		{
		  return longShort_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code swaptionSettlement} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SwaptionSettlement> swaptionSettlement()
		{
		  return swaptionSettlement_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code expiryDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<AdjustableDate> expiryDate()
		{
		  return expiryDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code expiryTime} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalTime> expiryTime()
		{
		  return expiryTime_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code expiryZone} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ZoneId> expiryZone()
		{
		  return expiryZone_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code underlying} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Swap> underlying()
		{
		  return underlying_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 116685664: // longShort
			  return ((Swaption) bean).LongShort;
			case -1937554512: // swaptionSettlement
			  return ((Swaption) bean).SwaptionSettlement;
			case -816738431: // expiryDate
			  return ((Swaption) bean).ExpiryDate;
			case -816254304: // expiryTime
			  return ((Swaption) bean).ExpiryTime;
			case -816069761: // expiryZone
			  return ((Swaption) bean).ExpiryZone;
			case -1770633379: // underlying
			  return ((Swaption) bean).Underlying;
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
	  /// The bean-builder for {@code Swaption}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<Swaption>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LongShort longShort_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SwaptionSettlement swaptionSettlement_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal AdjustableDate expiryDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalTime expiryTime_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ZoneId expiryZone_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Swap underlying_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(Swaption beanToCopy)
		{
		  this.longShort_Renamed = beanToCopy.LongShort;
		  this.swaptionSettlement_Renamed = beanToCopy.SwaptionSettlement;
		  this.expiryDate_Renamed = beanToCopy.ExpiryDate;
		  this.expiryTime_Renamed = beanToCopy.ExpiryTime;
		  this.expiryZone_Renamed = beanToCopy.ExpiryZone;
		  this.underlying_Renamed = beanToCopy.Underlying;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 116685664: // longShort
			  return longShort_Renamed;
			case -1937554512: // swaptionSettlement
			  return swaptionSettlement_Renamed;
			case -816738431: // expiryDate
			  return expiryDate_Renamed;
			case -816254304: // expiryTime
			  return expiryTime_Renamed;
			case -816069761: // expiryZone
			  return expiryZone_Renamed;
			case -1770633379: // underlying
			  return underlying_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 116685664: // longShort
			  this.longShort_Renamed = (LongShort) newValue;
			  break;
			case -1937554512: // swaptionSettlement
			  this.swaptionSettlement_Renamed = (SwaptionSettlement) newValue;
			  break;
			case -816738431: // expiryDate
			  this.expiryDate_Renamed = (AdjustableDate) newValue;
			  break;
			case -816254304: // expiryTime
			  this.expiryTime_Renamed = (LocalTime) newValue;
			  break;
			case -816069761: // expiryZone
			  this.expiryZone_Renamed = (ZoneId) newValue;
			  break;
			case -1770633379: // underlying
			  this.underlying_Renamed = (Swap) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override Builder set<T1>(MetaProperty<T1> property, object value)
		{
		  base.set(property, value);
		  return this;
		}

		public override Swaption build()
		{
		  return new Swaption(longShort_Renamed, swaptionSettlement_Renamed, expiryDate_Renamed, expiryTime_Renamed, expiryZone_Renamed, underlying_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets whether the option is long or short.
		/// <para>
		/// Long indicates that the owner wants the option to be in the money at expiry.
		/// Short indicates that the owner wants the option to be out of the money at expiry.
		/// </para>
		/// </summary>
		/// <param name="longShort">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder longShort(LongShort longShort)
		{
		  JodaBeanUtils.notNull(longShort, "longShort");
		  this.longShort_Renamed = longShort;
		  return this;
		}

		/// <summary>
		/// Sets settlement method.
		/// <para>
		/// The settlement of the option is specified by <seealso cref="SwaptionSettlement"/>.
		/// </para>
		/// </summary>
		/// <param name="swaptionSettlement">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder swaptionSettlement(SwaptionSettlement swaptionSettlement)
		{
		  JodaBeanUtils.notNull(swaptionSettlement, "swaptionSettlement");
		  this.swaptionSettlement_Renamed = swaptionSettlement;
		  return this;
		}

		/// <summary>
		/// Sets the expiry date of the option.
		/// <para>
		/// The option is European, and can only be exercised on the expiry date.
		/// </para>
		/// <para>
		/// This date is typically set to be a valid business day.
		/// However, the {@code businessDayAdjustment} property may be set to provide a rule for adjustment.
		/// </para>
		/// </summary>
		/// <param name="expiryDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder expiryDate(AdjustableDate expiryDate)
		{
		  JodaBeanUtils.notNull(expiryDate, "expiryDate");
		  this.expiryDate_Renamed = expiryDate;
		  return this;
		}

		/// <summary>
		/// Sets the expiry time of the option.
		/// <para>
		/// The expiry time is related to the expiry date and time-zone.
		/// </para>
		/// </summary>
		/// <param name="expiryTime">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder expiryTime(LocalTime expiryTime)
		{
		  JodaBeanUtils.notNull(expiryTime, "expiryTime");
		  this.expiryTime_Renamed = expiryTime;
		  return this;
		}

		/// <summary>
		/// Sets the time-zone of the expiry time.
		/// <para>
		/// The expiry time-zone is related to the expiry date and time.
		/// </para>
		/// </summary>
		/// <param name="expiryZone">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder expiryZone(ZoneId expiryZone)
		{
		  JodaBeanUtils.notNull(expiryZone, "expiryZone");
		  this.expiryZone_Renamed = expiryZone;
		  return this;
		}

		/// <summary>
		/// Sets the underlying swap.
		/// <para>
		/// At expiry, if the option is exercised, this swap will be entered into.
		/// The swap description is the swap as viewed by the party long the option.
		/// </para>
		/// </summary>
		/// <param name="underlying">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder underlying(Swap underlying)
		{
		  JodaBeanUtils.notNull(underlying, "underlying");
		  this.underlying_Renamed = underlying;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(224);
		  buf.Append("Swaption.Builder{");
		  buf.Append("longShort").Append('=').Append(JodaBeanUtils.ToString(longShort_Renamed)).Append(',').Append(' ');
		  buf.Append("swaptionSettlement").Append('=').Append(JodaBeanUtils.ToString(swaptionSettlement_Renamed)).Append(',').Append(' ');
		  buf.Append("expiryDate").Append('=').Append(JodaBeanUtils.ToString(expiryDate_Renamed)).Append(',').Append(' ');
		  buf.Append("expiryTime").Append('=').Append(JodaBeanUtils.ToString(expiryTime_Renamed)).Append(',').Append(' ');
		  buf.Append("expiryZone").Append('=').Append(JodaBeanUtils.ToString(expiryZone_Renamed)).Append(',').Append(' ');
		  buf.Append("underlying").Append('=').Append(JodaBeanUtils.ToString(underlying_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}