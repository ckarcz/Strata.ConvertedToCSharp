using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.capfloor
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Index = com.opengamma.strata.basics.index.Index;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;

	/// <summary>
	/// An Ibor cap/floor, resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="IborCapFloor"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedIborCapFloor} from a {@code IborCapFloor}
	/// using <seealso cref="IborCapFloor#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// A {@code ResolvedIborCapFloor} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ResolvedIborCapFloor implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedIborCapFloor : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ResolvedIborCapFloorLeg capFloorLeg;
		private readonly ResolvedIborCapFloorLeg capFloorLeg;
	  /// <summary>
	  /// The optional pay leg of the product.
	  /// <para>
	  /// These periodic payments are not made for typical cap/floor products. Instead the premium is paid upfront.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.product.swap.ResolvedSwapLeg payLeg;
	  private readonly ResolvedSwapLeg payLeg;
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
	  /// Obtains an instance from a cap/floor leg with no pay leg.
	  /// <para>
	  /// The pay leg is absent in the resulting cap/floor.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="capFloorLeg">  the cap/floor leg </param>
	  /// <returns> the cap/floor </returns>
	  public static ResolvedIborCapFloor of(ResolvedIborCapFloorLeg capFloorLeg)
	  {
		ArgChecker.notNull(capFloorLeg, "capFloorLeg");
		return new ResolvedIborCapFloor(capFloorLeg, null);
	  }

	  /// <summary>
	  /// Obtains an instance from a cap/floor leg and a pay leg.
	  /// </summary>
	  /// <param name="capFloorLeg">  the cap/floor leg </param>
	  /// <param name="payLeg">  the pay leg </param>
	  /// <returns> the cap/floor </returns>
	  public static ResolvedIborCapFloor of(ResolvedIborCapFloorLeg capFloorLeg, ResolvedSwapLeg payLeg)
	  {
		ArgChecker.notNull(capFloorLeg, "capFloorLeg");
		ArgChecker.notNull(payLeg, "payLeg");
		return new ResolvedIborCapFloor(capFloorLeg, payLeg);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private ResolvedIborCapFloor(ResolvedIborCapFloorLeg capFloorLeg, com.opengamma.strata.product.swap.ResolvedSwapLeg payLeg)
	  private ResolvedIborCapFloor(ResolvedIborCapFloorLeg capFloorLeg, ResolvedSwapLeg payLeg)
	  {
		JodaBeanUtils.notNull(capFloorLeg, "capFloorLeg");
		if (payLeg != null)
		{
		  ArgChecker.isFalse(payLeg.PayReceive.Equals(capFloorLeg.PayReceive), "Legs must have different Pay/Receive flag, but both were {}", payLeg.PayReceive);
		}
		this.capFloorLeg = capFloorLeg;
		this.payLeg = payLeg;
		this.currencies = buildCurrencies(capFloorLeg, payLeg);
		this.indices = buildIndices(capFloorLeg, payLeg);
	  }

	  // collect the set of currencies
	  private static ImmutableSet<Currency> buildCurrencies(ResolvedIborCapFloorLeg capFloorLeg, ResolvedSwapLeg payLeg)
	  {
		ImmutableSet.Builder<Currency> builder = ImmutableSet.builder();
		builder.add(capFloorLeg.Currency);
		if (payLeg != null)
		{
		  builder.add(payLeg.Currency);
		}
		return builder.build();
	  }

	  // collect the set of indices
	  private static ImmutableSet<Index> buildIndices(ResolvedIborCapFloorLeg capFloorLeg, ResolvedSwapLeg payLeg)
	  {
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		builder.add(capFloorLeg.Index);
		if (payLeg != null)
		{
		  payLeg.collectIndices(builder);
		}
		return builder.build();
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new ResolvedIborCapFloor(capFloorLeg, payLeg);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the set of payment currencies referred to by the cap/floor.
	  /// <para>
	  /// This returns the complete set of payment currencies for the cap/floor.
	  /// This will typically return one currency, but could return two.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of payment currencies referred to by this swap </returns>
	  public ImmutableSet<Currency> allPaymentCurrencies()
	  {
		return currencies;
	  }

	  /// <summary>
	  /// Returns the set of indices referred to by the cap/floor.
	  /// <para>
	  /// A cap/floor will typically refer to one index, such as 'GBP-LIBOR-3M'.
	  /// Calling this method will return the complete list of indices.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of indices referred to by this cap/floor </returns>
	  public ImmutableSet<Index> allIndices()
	  {
		return indices;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedIborCapFloor}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedIborCapFloor.Meta meta()
	  {
		return ResolvedIborCapFloor.Meta.INSTANCE;
	  }

	  static ResolvedIborCapFloor()
	  {
		MetaBean.register(ResolvedIborCapFloor.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override ResolvedIborCapFloor.Meta metaBean()
	  {
		return ResolvedIborCapFloor.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Ibor cap/floor leg of the product.
	  /// <para>
	  /// This is associated with periodic payments based on Ibor rate.
	  /// The payments are Ibor caplets or Ibor floorlets.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ResolvedIborCapFloorLeg CapFloorLeg
	  {
		  get
		  {
			return capFloorLeg;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the optional pay leg of the product.
	  /// <para>
	  /// These periodic payments are not made for typical cap/floor products. Instead the premium is paid upfront.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<ResolvedSwapLeg> PayLeg
	  {
		  get
		  {
			return Optional.ofNullable(payLeg);
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
		  ResolvedIborCapFloor other = (ResolvedIborCapFloor) obj;
		  return JodaBeanUtils.equal(capFloorLeg, other.capFloorLeg) && JodaBeanUtils.equal(payLeg, other.payLeg);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(capFloorLeg);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(payLeg);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("ResolvedIborCapFloor{");
		buf.Append("capFloorLeg").Append('=').Append(capFloorLeg).Append(',').Append(' ');
		buf.Append("payLeg").Append('=').Append(JodaBeanUtils.ToString(payLeg));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedIborCapFloor}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  capFloorLeg_Renamed = DirectMetaProperty.ofImmutable(this, "capFloorLeg", typeof(ResolvedIborCapFloor), typeof(ResolvedIborCapFloorLeg));
			  payLeg_Renamed = DirectMetaProperty.ofImmutable(this, "payLeg", typeof(ResolvedIborCapFloor), typeof(ResolvedSwapLeg));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "capFloorLeg", "payLeg");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code capFloorLeg} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ResolvedIborCapFloorLeg> capFloorLeg_Renamed;
		/// <summary>
		/// The meta-property for the {@code payLeg} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ResolvedSwapLeg> payLeg_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "capFloorLeg", "payLeg");
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
			case 2124672084: // capFloorLeg
			  return capFloorLeg_Renamed;
			case -995239866: // payLeg
			  return payLeg_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ResolvedIborCapFloor> builder()
		public override BeanBuilder<ResolvedIborCapFloor> builder()
		{
		  return new ResolvedIborCapFloor.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedIborCapFloor);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code capFloorLeg} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ResolvedIborCapFloorLeg> capFloorLeg()
		{
		  return capFloorLeg_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code payLeg} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ResolvedSwapLeg> payLeg()
		{
		  return payLeg_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 2124672084: // capFloorLeg
			  return ((ResolvedIborCapFloor) bean).CapFloorLeg;
			case -995239866: // payLeg
			  return ((ResolvedIborCapFloor) bean).payLeg;
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
	  /// The bean-builder for {@code ResolvedIborCapFloor}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ResolvedIborCapFloor>
	  {

		internal ResolvedIborCapFloorLeg capFloorLeg;
		internal ResolvedSwapLeg payLeg;

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
			case 2124672084: // capFloorLeg
			  return capFloorLeg;
			case -995239866: // payLeg
			  return payLeg;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 2124672084: // capFloorLeg
			  this.capFloorLeg = (ResolvedIborCapFloorLeg) newValue;
			  break;
			case -995239866: // payLeg
			  this.payLeg = (ResolvedSwapLeg) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ResolvedIborCapFloor build()
		{
		  return new ResolvedIborCapFloor(capFloorLeg, payLeg);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("ResolvedIborCapFloor.Builder{");
		  buf.Append("capFloorLeg").Append('=').Append(JodaBeanUtils.ToString(capFloorLeg)).Append(',').Append(' ');
		  buf.Append("payLeg").Append('=').Append(JodaBeanUtils.ToString(payLeg));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}