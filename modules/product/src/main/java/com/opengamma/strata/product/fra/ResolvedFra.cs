using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fra
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Index = com.opengamma.strata.basics.index.Index;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// A forward rate agreement (FRA), resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="Fra"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedFra} from a {@code Fra}
	/// using <seealso cref="Fra#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// A {@code ResolvedFra} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ResolvedFra implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedFra : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency currency;
		private readonly Currency currency;
	  /// <summary>
	  /// The notional amount.
	  /// <para>
	  /// The notional, which is a positive signed amount if the FRA is 'buy',
	  /// and a negative signed amount if the FRA is 'sell'.
	  /// </para>
	  /// <para>
	  /// The currency of the notional is specified by {@code currency}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double notional;
	  private readonly double notional;
	  /// <summary>
	  /// The date that payment occurs.
	  /// <para>
	  /// This is an adjusted date, which should be a valid business day
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate paymentDate;
	  private readonly LocalDate paymentDate;
	  /// <summary>
	  /// The start date, which is the effective date of the FRA.
	  /// <para>
	  /// This is the first date that interest accrues.
	  /// </para>
	  /// <para>
	  /// This is an adjusted date, which should be a valid business day
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate startDate;
	  private readonly LocalDate startDate;
	  /// <summary>
	  /// The end date, which is the termination date of the FRA.
	  /// <para>
	  /// This is the last day that interest accrues.
	  /// This date must be after the start date.
	  /// </para>
	  /// <para>
	  /// This is an adjusted date, which should be a valid business day
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate endDate;
	  private readonly LocalDate endDate;
	  /// <summary>
	  /// The year fraction between the start and end date.
	  /// <para>
	  /// The value is usually calculated using a <seealso cref="DayCount"/>.
	  /// Typically the value will be close to 1 for one year and close to 0.5 for six months.
	  /// The fraction may be greater than 1, but not less than 0.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final double yearFraction;
	  private readonly double yearFraction;
	  /// <summary>
	  /// The fixed rate of interest.
	  /// A 5% rate will be expressed as 0.05.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double fixedRate;
	  private readonly double fixedRate;
	  /// <summary>
	  /// The floating rate of interest.
	  /// <para>
	  /// The floating rate to be paid is based on this index.
	  /// It will be a well known market index such as 'GBP-LIBOR-3M'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.rate.RateComputation floatingRate;
	  private readonly RateComputation floatingRate;
	  /// <summary>
	  /// The method to use for discounting.
	  /// <para>
	  /// There are different approaches to FRA pricing in the area of discounting.
	  /// This method specifies the approach for this FRA.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final FraDiscountingMethod discounting;
	  private readonly FraDiscountingMethod discounting;
	  /// <summary>
	  /// The set of indices.
	  /// </summary>
	  [NonSerialized]
	  private readonly ImmutableSet<IborIndex> indices; // not a property, derived and cached from input data

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private ResolvedFra(com.opengamma.strata.basics.currency.Currency currency, double notional, java.time.LocalDate paymentDate, java.time.LocalDate startDate, java.time.LocalDate endDate, double yearFraction, double fixedRate, com.opengamma.strata.product.rate.RateComputation floatingRate, FraDiscountingMethod discounting)
	  private ResolvedFra(Currency currency, double notional, LocalDate paymentDate, LocalDate startDate, LocalDate endDate, double yearFraction, double fixedRate, RateComputation floatingRate, FraDiscountingMethod discounting)
	  {

		this.currency = ArgChecker.notNull(currency, "currency");
		this.notional = notional;
		this.paymentDate = ArgChecker.notNull(paymentDate, "paymentDate");
		ArgChecker.inOrderNotEqual(startDate, endDate, "startDate", "endDate");
		this.startDate = startDate;
		this.endDate = endDate;
		this.yearFraction = ArgChecker.notNegative(yearFraction, "yearFraction");
		this.fixedRate = fixedRate;
		this.floatingRate = ArgChecker.notNull(floatingRate, "floatingRate");
		this.discounting = ArgChecker.notNull(discounting, "discounting");
		this.indices = buildIndices(floatingRate);
	  }

	  // collect the set of indices, validating they are IborIndex
	  private static ImmutableSet<IborIndex> buildIndices(RateComputation floatingRate)
	  {
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		floatingRate.collectIndices(builder);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return builder.build().Select(index => typeof(IborIndex).cast(index)).collect(toImmutableSet());
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new ResolvedFra(currency, notional, paymentDate, startDate, endDate, yearFraction, fixedRate, floatingRate, discounting);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the set of indices referred to by the FRA.
	  /// <para>
	  /// A swap will typically refer to one index, such as 'GBP-LIBOR-3M'.
	  /// Occasionally, it will refer to two indices.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of indices referred to by this FRA </returns>
	  public ISet<IborIndex> allIndices()
	  {
		return indices;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedFra}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedFra.Meta meta()
	  {
		return ResolvedFra.Meta.INSTANCE;
	  }

	  static ResolvedFra()
	  {
		MetaBean.register(ResolvedFra.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedFra.Builder builder()
	  {
		return new ResolvedFra.Builder();
	  }

	  public override ResolvedFra.Meta metaBean()
	  {
		return ResolvedFra.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the primary currency.
	  /// <para>
	  /// This is the currency of the FRA and the currency that payment is made in.
	  /// The data model permits this currency to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Currency Currency
	  {
		  get
		  {
			return currency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the notional amount.
	  /// <para>
	  /// The notional, which is a positive signed amount if the FRA is 'buy',
	  /// and a negative signed amount if the FRA is 'sell'.
	  /// </para>
	  /// <para>
	  /// The currency of the notional is specified by {@code currency}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double Notional
	  {
		  get
		  {
			return notional;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the date that payment occurs.
	  /// <para>
	  /// This is an adjusted date, which should be a valid business day
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate PaymentDate
	  {
		  get
		  {
			return paymentDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the start date, which is the effective date of the FRA.
	  /// <para>
	  /// This is the first date that interest accrues.
	  /// </para>
	  /// <para>
	  /// This is an adjusted date, which should be a valid business day
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate StartDate
	  {
		  get
		  {
			return startDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the end date, which is the termination date of the FRA.
	  /// <para>
	  /// This is the last day that interest accrues.
	  /// This date must be after the start date.
	  /// </para>
	  /// <para>
	  /// This is an adjusted date, which should be a valid business day
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate EndDate
	  {
		  get
		  {
			return endDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the year fraction between the start and end date.
	  /// <para>
	  /// The value is usually calculated using a <seealso cref="DayCount"/>.
	  /// Typically the value will be close to 1 for one year and close to 0.5 for six months.
	  /// The fraction may be greater than 1, but not less than 0.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double YearFraction
	  {
		  get
		  {
			return yearFraction;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fixed rate of interest.
	  /// A 5% rate will be expressed as 0.05. </summary>
	  /// <returns> the value of the property </returns>
	  public double FixedRate
	  {
		  get
		  {
			return fixedRate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the floating rate of interest.
	  /// <para>
	  /// The floating rate to be paid is based on this index.
	  /// It will be a well known market index such as 'GBP-LIBOR-3M'.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public RateComputation FloatingRate
	  {
		  get
		  {
			return floatingRate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the method to use for discounting.
	  /// <para>
	  /// There are different approaches to FRA pricing in the area of discounting.
	  /// This method specifies the approach for this FRA.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FraDiscountingMethod Discounting
	  {
		  get
		  {
			return discounting;
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
		  ResolvedFra other = (ResolvedFra) obj;
		  return JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(paymentDate, other.paymentDate) && JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(endDate, other.endDate) && JodaBeanUtils.equal(yearFraction, other.yearFraction) && JodaBeanUtils.equal(fixedRate, other.fixedRate) && JodaBeanUtils.equal(floatingRate, other.floatingRate) && JodaBeanUtils.equal(discounting, other.discounting);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notional);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yearFraction);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixedRate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(floatingRate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(discounting);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(320);
		buf.Append("ResolvedFra{");
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(notional).Append(',').Append(' ');
		buf.Append("paymentDate").Append('=').Append(paymentDate).Append(',').Append(' ');
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(endDate).Append(',').Append(' ');
		buf.Append("yearFraction").Append('=').Append(yearFraction).Append(',').Append(' ');
		buf.Append("fixedRate").Append('=').Append(fixedRate).Append(',').Append(' ');
		buf.Append("floatingRate").Append('=').Append(floatingRate).Append(',').Append(' ');
		buf.Append("discounting").Append('=').Append(JodaBeanUtils.ToString(discounting));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedFra}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(ResolvedFra), typeof(Currency));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(ResolvedFra), Double.TYPE);
			  paymentDate_Renamed = DirectMetaProperty.ofImmutable(this, "paymentDate", typeof(ResolvedFra), typeof(LocalDate));
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(ResolvedFra), typeof(LocalDate));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(ResolvedFra), typeof(LocalDate));
			  yearFraction_Renamed = DirectMetaProperty.ofImmutable(this, "yearFraction", typeof(ResolvedFra), Double.TYPE);
			  fixedRate_Renamed = DirectMetaProperty.ofImmutable(this, "fixedRate", typeof(ResolvedFra), Double.TYPE);
			  floatingRate_Renamed = DirectMetaProperty.ofImmutable(this, "floatingRate", typeof(ResolvedFra), typeof(RateComputation));
			  discounting_Renamed = DirectMetaProperty.ofImmutable(this, "discounting", typeof(ResolvedFra), typeof(FraDiscountingMethod));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "currency", "notional", "paymentDate", "startDate", "endDate", "yearFraction", "fixedRate", "floatingRate", "discounting");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code notional} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> notional_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> paymentDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code startDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> startDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code endDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> endDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code yearFraction} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> yearFraction_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixedRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> fixedRate_Renamed;
		/// <summary>
		/// The meta-property for the {@code floatingRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<RateComputation> floatingRate_Renamed;
		/// <summary>
		/// The meta-property for the {@code discounting} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FraDiscountingMethod> discounting_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "currency", "notional", "paymentDate", "startDate", "endDate", "yearFraction", "fixedRate", "floatingRate", "discounting");
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
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -1540873516: // paymentDate
			  return paymentDate_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case -1731780257: // yearFraction
			  return yearFraction_Renamed;
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
			case -2130225658: // floatingRate
			  return floatingRate_Renamed;
			case -536441087: // discounting
			  return discounting_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResolvedFra.Builder builder()
		{
		  return new ResolvedFra.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedFra);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code notional} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> notional()
		{
		  return notional_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> paymentDate()
		{
		  return paymentDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code startDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> startDate()
		{
		  return startDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code endDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> endDate()
		{
		  return endDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code yearFraction} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> yearFraction()
		{
		  return yearFraction_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixedRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> fixedRate()
		{
		  return fixedRate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code floatingRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<RateComputation> floatingRate()
		{
		  return floatingRate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code discounting} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FraDiscountingMethod> discounting()
		{
		  return discounting_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  return ((ResolvedFra) bean).Currency;
			case 1585636160: // notional
			  return ((ResolvedFra) bean).Notional;
			case -1540873516: // paymentDate
			  return ((ResolvedFra) bean).PaymentDate;
			case -2129778896: // startDate
			  return ((ResolvedFra) bean).StartDate;
			case -1607727319: // endDate
			  return ((ResolvedFra) bean).EndDate;
			case -1731780257: // yearFraction
			  return ((ResolvedFra) bean).YearFraction;
			case 747425396: // fixedRate
			  return ((ResolvedFra) bean).FixedRate;
			case -2130225658: // floatingRate
			  return ((ResolvedFra) bean).FloatingRate;
			case -536441087: // discounting
			  return ((ResolvedFra) bean).Discounting;
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
	  /// The bean-builder for {@code ResolvedFra}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResolvedFra>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double notional_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate paymentDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate startDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate endDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double yearFraction_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double fixedRate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal RateComputation floatingRate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FraDiscountingMethod discounting_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ResolvedFra beanToCopy)
		{
		  this.currency_Renamed = beanToCopy.Currency;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.paymentDate_Renamed = beanToCopy.PaymentDate;
		  this.startDate_Renamed = beanToCopy.StartDate;
		  this.endDate_Renamed = beanToCopy.EndDate;
		  this.yearFraction_Renamed = beanToCopy.YearFraction;
		  this.fixedRate_Renamed = beanToCopy.FixedRate;
		  this.floatingRate_Renamed = beanToCopy.FloatingRate;
		  this.discounting_Renamed = beanToCopy.Discounting;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -1540873516: // paymentDate
			  return paymentDate_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case -1731780257: // yearFraction
			  return yearFraction_Renamed;
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
			case -2130225658: // floatingRate
			  return floatingRate_Renamed;
			case -536441087: // discounting
			  return discounting_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
			  break;
			case 1585636160: // notional
			  this.notional_Renamed = (double?) newValue.Value;
			  break;
			case -1540873516: // paymentDate
			  this.paymentDate_Renamed = (LocalDate) newValue;
			  break;
			case -2129778896: // startDate
			  this.startDate_Renamed = (LocalDate) newValue;
			  break;
			case -1607727319: // endDate
			  this.endDate_Renamed = (LocalDate) newValue;
			  break;
			case -1731780257: // yearFraction
			  this.yearFraction_Renamed = (double?) newValue.Value;
			  break;
			case 747425396: // fixedRate
			  this.fixedRate_Renamed = (double?) newValue.Value;
			  break;
			case -2130225658: // floatingRate
			  this.floatingRate_Renamed = (RateComputation) newValue;
			  break;
			case -536441087: // discounting
			  this.discounting_Renamed = (FraDiscountingMethod) newValue;
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

		public override ResolvedFra build()
		{
		  return new ResolvedFra(currency_Renamed, notional_Renamed, paymentDate_Renamed, startDate_Renamed, endDate_Renamed, yearFraction_Renamed, fixedRate_Renamed, floatingRate_Renamed, discounting_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the primary currency.
		/// <para>
		/// This is the currency of the FRA and the currency that payment is made in.
		/// The data model permits this currency to differ from that of the index,
		/// however the two are typically the same.
		/// </para>
		/// </summary>
		/// <param name="currency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currency(Currency currency)
		{
		  JodaBeanUtils.notNull(currency, "currency");
		  this.currency_Renamed = currency;
		  return this;
		}

		/// <summary>
		/// Sets the notional amount.
		/// <para>
		/// The notional, which is a positive signed amount if the FRA is 'buy',
		/// and a negative signed amount if the FRA is 'sell'.
		/// </para>
		/// <para>
		/// The currency of the notional is specified by {@code currency}.
		/// </para>
		/// </summary>
		/// <param name="notional">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder notional(double notional)
		{
		  this.notional_Renamed = notional;
		  return this;
		}

		/// <summary>
		/// Sets the date that payment occurs.
		/// <para>
		/// This is an adjusted date, which should be a valid business day
		/// </para>
		/// </summary>
		/// <param name="paymentDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentDate(LocalDate paymentDate)
		{
		  JodaBeanUtils.notNull(paymentDate, "paymentDate");
		  this.paymentDate_Renamed = paymentDate;
		  return this;
		}

		/// <summary>
		/// Sets the start date, which is the effective date of the FRA.
		/// <para>
		/// This is the first date that interest accrues.
		/// </para>
		/// <para>
		/// This is an adjusted date, which should be a valid business day
		/// </para>
		/// </summary>
		/// <param name="startDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder startDate(LocalDate startDate)
		{
		  JodaBeanUtils.notNull(startDate, "startDate");
		  this.startDate_Renamed = startDate;
		  return this;
		}

		/// <summary>
		/// Sets the end date, which is the termination date of the FRA.
		/// <para>
		/// This is the last day that interest accrues.
		/// This date must be after the start date.
		/// </para>
		/// <para>
		/// This is an adjusted date, which should be a valid business day
		/// </para>
		/// </summary>
		/// <param name="endDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder endDate(LocalDate endDate)
		{
		  JodaBeanUtils.notNull(endDate, "endDate");
		  this.endDate_Renamed = endDate;
		  return this;
		}

		/// <summary>
		/// Sets the year fraction between the start and end date.
		/// <para>
		/// The value is usually calculated using a <seealso cref="DayCount"/>.
		/// Typically the value will be close to 1 for one year and close to 0.5 for six months.
		/// The fraction may be greater than 1, but not less than 0.
		/// </para>
		/// </summary>
		/// <param name="yearFraction">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder yearFraction(double yearFraction)
		{
		  ArgChecker.notNegative(yearFraction, "yearFraction");
		  this.yearFraction_Renamed = yearFraction;
		  return this;
		}

		/// <summary>
		/// Sets the fixed rate of interest.
		/// A 5% rate will be expressed as 0.05. </summary>
		/// <param name="fixedRate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixedRate(double fixedRate)
		{
		  this.fixedRate_Renamed = fixedRate;
		  return this;
		}

		/// <summary>
		/// Sets the floating rate of interest.
		/// <para>
		/// The floating rate to be paid is based on this index.
		/// It will be a well known market index such as 'GBP-LIBOR-3M'.
		/// </para>
		/// </summary>
		/// <param name="floatingRate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder floatingRate(RateComputation floatingRate)
		{
		  JodaBeanUtils.notNull(floatingRate, "floatingRate");
		  this.floatingRate_Renamed = floatingRate;
		  return this;
		}

		/// <summary>
		/// Sets the method to use for discounting.
		/// <para>
		/// There are different approaches to FRA pricing in the area of discounting.
		/// This method specifies the approach for this FRA.
		/// </para>
		/// </summary>
		/// <param name="discounting">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder discounting(FraDiscountingMethod discounting)
		{
		  JodaBeanUtils.notNull(discounting, "discounting");
		  this.discounting_Renamed = discounting;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(320);
		  buf.Append("ResolvedFra.Builder{");
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentDate").Append('=').Append(JodaBeanUtils.ToString(paymentDate_Renamed)).Append(',').Append(' ');
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate_Renamed)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate_Renamed)).Append(',').Append(' ');
		  buf.Append("yearFraction").Append('=').Append(JodaBeanUtils.ToString(yearFraction_Renamed)).Append(',').Append(' ');
		  buf.Append("fixedRate").Append('=').Append(JodaBeanUtils.ToString(fixedRate_Renamed)).Append(',').Append(' ');
		  buf.Append("floatingRate").Append('=').Append(JodaBeanUtils.ToString(floatingRate_Renamed)).Append(',').Append(' ');
		  buf.Append("discounting").Append('=').Append(JodaBeanUtils.ToString(discounting_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}