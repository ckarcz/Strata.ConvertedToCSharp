using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using DerivedProperty = org.joda.beans.gen.DerivedProperty;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Index = com.opengamma.strata.basics.index.Index;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// A rate swap, resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="Swap"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedSwap} from a {@code Swap}
	/// using <seealso cref="Swap#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// A rate swap is a financial instrument that represents the exchange of streams of payments.
	/// The swap is formed of legs, where each leg typically represents the obligations
	/// of the seller or buyer of the swap. In the simplest vanilla interest rate swap,
	/// there are two legs, one with a fixed rate and the other a floating rate.
	/// Many other more complex swaps can also be represented.
	/// </para>
	/// <para>
	/// This class defines a swap as a set of legs, each of which contains a list of payment periods.
	/// Each payment period typically consists of one or more accrual periods.
	/// Additional payment events may also be specified.
	/// </para>
	/// <para>
	/// Any combination of legs, payments and accrual periods is supported in the data model,
	/// however there is no guarantee that exotic combinations will price sensibly.
	/// </para>
	/// <para>
	/// A {@code ResolvedSwap} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ResolvedSwap implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedSwap : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final com.google.common.collect.ImmutableList<ResolvedSwapLeg> legs;
		private readonly ImmutableList<ResolvedSwapLeg> legs;
	  /// <summary>
	  /// The set of currencies.
	  /// </summary>
	  [NonSerialized]
	  private readonly ImmutableSet<Currency> currencies; // not a property, derived and cached from input data
	  /// <summary>
	  /// The set of indices.
	  /// </summary>
	  [NonSerialized]
	  private readonly ImmutableSet<Index> indices; // not a property, derived and cached from input data

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a swap from one or more swap legs.
	  /// <para>
	  /// While most swaps have two legs, other combinations are possible.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="legs">  the array of legs </param>
	  /// <returns> the swap </returns>
	  public static ResolvedSwap of(params ResolvedSwapLeg[] legs)
	  {
		ArgChecker.notEmpty(legs, "legs");
		return new ResolvedSwap(ImmutableList.copyOf(legs));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private ResolvedSwap(java.util.List<ResolvedSwapLeg> legs)
	  private ResolvedSwap(IList<ResolvedSwapLeg> legs)
	  {
		JodaBeanUtils.notEmpty(legs, "legs");
		this.legs = ImmutableList.copyOf(legs);
		this.currencies = buildCurrencies(legs);
		this.indices = buildIndices(legs);
	  }

	  // trusted constructor
	  internal ResolvedSwap(ImmutableList<ResolvedSwapLeg> legs, ImmutableSet<Currency> currencies, ImmutableSet<Index> indices)
	  {
		this.legs = legs;
		this.currencies = currencies;
		this.indices = indices;
	  }

	  // collect the set of currencies
	  private static ImmutableSet<Currency> buildCurrencies(IList<ResolvedSwapLeg> legs)
	  {
		// avoid streams as profiling showed a hotspot
		ImmutableSet.Builder<Currency> builder = ImmutableSet.builder();
		foreach (ResolvedSwapLeg leg in legs)
		{
		  builder.add(leg.Currency);
		}
		return builder.build();
	  }

	  // collect the set of indices
	  private static ImmutableSet<Index> buildIndices(IList<ResolvedSwapLeg> legs)
	  {
		// avoid streams as profiling showed a hotspot
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		foreach (ResolvedSwapLeg leg in legs)
		{
		  leg.collectIndices(builder);
		}
		return builder.build();
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new ResolvedSwap(legs);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the legs of the swap with the specified type.
	  /// <para>
	  /// This returns all the legs with the given type.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="type">  the type to find </param>
	  /// <returns> the matching legs of the swap </returns>
	  public ImmutableList<ResolvedSwapLeg> getLegs(SwapLegType type)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return legs.Where(leg => leg.Type == type).collect(toImmutableList());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the first pay or receive leg of the swap.
	  /// <para>
	  /// This returns the first pay or receive leg of the swap, empty if no matching leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="payReceive">  the pay or receive flag </param>
	  /// <returns> the first matching leg of the swap </returns>
	  public Optional<ResolvedSwapLeg> getLeg(PayReceive payReceive)
	  {
		return legs.Where(leg => leg.PayReceive == payReceive).First();
	  }

	  /// <summary>
	  /// Gets the first pay leg of the swap.
	  /// <para>
	  /// This returns the first pay leg of the swap, empty if no pay leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the first pay leg of the swap </returns>
	  public Optional<ResolvedSwapLeg> PayLeg
	  {
		  get
		  {
			return getLeg(PayReceive.PAY);
		  }
	  }

	  /// <summary>
	  /// Gets the first receive leg of the swap.
	  /// <para>
	  /// This returns the first receive leg of the swap, empty if no receive leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the first receive leg of the swap </returns>
	  public Optional<ResolvedSwapLeg> ReceiveLeg
	  {
		  get
		  {
			return getLeg(PayReceive.RECEIVE);
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the accrual start date of the swap.
	  /// <para>
	  /// This is the earliest accrual date of the legs, often known as the effective date.
	  /// This date has typically been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the start date of the swap </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DerivedProperty public java.time.LocalDate getStartDate()
	  public LocalDate StartDate
	  {
		  get
		  {
	//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
			return legs.Select(ResolvedSwapLeg::getStartDate).Min(System.Collections.IComparer.naturalOrder()).get(); // always at least one leg, so get() is safe
		  }
	  }

	  /// <summary>
	  /// Gets the accrual end date of the swap.
	  /// <para>
	  /// This is the latest accrual date of the legs, often known as the termination date.
	  /// This date has typically been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the end date of the swap </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DerivedProperty public java.time.LocalDate getEndDate()
	  public LocalDate EndDate
	  {
		  get
		  {
	//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
			return legs.Select(ResolvedSwapLeg::getEndDate).Max(System.Collections.IComparer.naturalOrder()).get(); // always at least one leg, so get() is safe
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this trade is cross-currency.
	  /// <para>
	  /// A cross currency swap is defined as one with legs in two different currencies.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if cross currency </returns>
	  public bool CrossCurrency
	  {
		  get
		  {
			return currencies.size() > 1;
		  }
	  }

	  /// <summary>
	  /// Returns the set of payment currencies referred to by the swap.
	  /// <para>
	  /// This returns the complete set of payment currencies for the swap.
	  /// This will typically return one or two currencies.
	  /// </para>
	  /// <para>
	  /// If there is an FX reset, then this set contains the currency of the payment,
	  /// not the currency of the notional. Note that in many cases, the currency of
	  /// the FX reset notional will be the currency of the other leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currencies </returns>
	  public ImmutableSet<Currency> allPaymentCurrencies()
	  {
		return currencies;
	  }

	  /// <summary>
	  /// Returns the set of indices referred to by the swap.
	  /// <para>
	  /// A swap will typically refer to at least one index, such as 'GBP-LIBOR-3M'.
	  /// Calling this method will return the complete list of indices, including
	  /// any associated with FX reset.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of indices referred to by this swap </returns>
	  public ImmutableSet<Index> allIndices()
	  {
		return indices;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedSwap}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedSwap.Meta meta()
	  {
		return ResolvedSwap.Meta.INSTANCE;
	  }

	  static ResolvedSwap()
	  {
		MetaBean.register(ResolvedSwap.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedSwap.Builder builder()
	  {
		return new ResolvedSwap.Builder();
	  }

	  public override ResolvedSwap.Meta metaBean()
	  {
		return ResolvedSwap.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the legs of the swap.
	  /// <para>
	  /// A swap consists of one or more legs.
	  /// The legs of a swap are essentially unordered, however it is more efficient
	  /// and closer to user expectation to treat them as being ordered.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public ImmutableList<ResolvedSwapLeg> Legs
	  {
		  get
		  {
			return legs;
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
		  ResolvedSwap other = (ResolvedSwap) obj;
		  return JodaBeanUtils.equal(legs, other.legs);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(legs);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("ResolvedSwap{");
		buf.Append("legs").Append('=').Append(JodaBeanUtils.ToString(legs));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedSwap}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  legs_Renamed = DirectMetaProperty.ofImmutable(this, "legs", typeof(ResolvedSwap), (Type) typeof(ImmutableList));
			  startDate_Renamed = DirectMetaProperty.ofDerived(this, "startDate", typeof(ResolvedSwap), typeof(LocalDate));
			  endDate_Renamed = DirectMetaProperty.ofDerived(this, "endDate", typeof(ResolvedSwap), typeof(LocalDate));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "legs", "startDate", "endDate");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code legs} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<ResolvedSwapLeg>> legs = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "legs", ResolvedSwap.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<ResolvedSwapLeg>> legs_Renamed;
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
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "legs", "startDate", "endDate");
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
			case 3317797: // legs
			  return legs_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResolvedSwap.Builder builder()
		{
		  return new ResolvedSwap.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedSwap);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code legs} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<ResolvedSwapLeg>> legs()
		{
		  return legs_Renamed;
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

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3317797: // legs
			  return ((ResolvedSwap) bean).Legs;
			case -2129778896: // startDate
			  return ((ResolvedSwap) bean).StartDate;
			case -1607727319: // endDate
			  return ((ResolvedSwap) bean).EndDate;
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
	  /// The bean-builder for {@code ResolvedSwap}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResolvedSwap>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<ResolvedSwapLeg> legs_Renamed = ImmutableList.of();

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ResolvedSwap beanToCopy)
		{
		  this.legs_Renamed = beanToCopy.Legs;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3317797: // legs
			  return legs_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder set(String propertyName, Object newValue)
		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3317797: // legs
			  this.legs_Renamed = (IList<ResolvedSwapLeg>) newValue;
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

		public override ResolvedSwap build()
		{
		  return new ResolvedSwap(legs_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the legs of the swap.
		/// <para>
		/// A swap consists of one or more legs.
		/// The legs of a swap are essentially unordered, however it is more efficient
		/// and closer to user expectation to treat them as being ordered.
		/// </para>
		/// </summary>
		/// <param name="legs">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder legs(IList<ResolvedSwapLeg> legs)
		{
		  JodaBeanUtils.notEmpty(legs, "legs");
		  this.legs_Renamed = legs;
		  return this;
		}

		/// <summary>
		/// Sets the {@code legs} property in the builder
		/// from an array of objects. </summary>
		/// <param name="legs">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder legs(params ResolvedSwapLeg[] legs)
		{
		  return this.legs(ImmutableList.copyOf(legs));
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("ResolvedSwap.Builder{");
		  buf.Append("legs").Append('=').Append(JodaBeanUtils.ToString(legs_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}