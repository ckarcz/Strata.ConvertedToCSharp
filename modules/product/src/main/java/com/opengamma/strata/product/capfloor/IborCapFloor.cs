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
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Index = com.opengamma.strata.basics.index.Index;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;

	/// <summary>
	/// An Ibor cap/floor product.
	/// <para>
	/// The Ibor cap/floor product consists of two legs, a cap/floor leg and a pay leg.
	/// The cap/floor leg involves a set of call/put options on successive Ibor index rates,
	/// known as Ibor caplets/floorlets.
	/// The pay leg is any swap leg from a standard interest rate swap. The pay leg is absent for typical
	/// Ibor cap/floor products, with the premium paid upfront instead, as defined in <seealso cref="IborCapFloorTrade"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class IborCapFloor implements com.opengamma.strata.product.Product, com.opengamma.strata.basics.Resolvable<ResolvedIborCapFloor>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IborCapFloor : Product, Resolvable<ResolvedIborCapFloor>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final IborCapFloorLeg capFloorLeg;
		private readonly IborCapFloorLeg capFloorLeg;
	  /// <summary>
	  /// The optional pay leg of the product.
	  /// <para>
	  /// These periodic payments are not made for typical cap/floor products.
	  /// Instead the premium is paid upfront.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.product.swap.SwapLeg payLeg;
	  private readonly SwapLeg payLeg;

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
	  public static IborCapFloor of(IborCapFloorLeg capFloorLeg)
	  {
		return new IborCapFloor(capFloorLeg, null);
	  }

	  /// <summary>
	  /// Obtains an instance from a cap/floor leg and a pay leg.
	  /// </summary>
	  /// <param name="capFloorLeg">  the cap/floor leg </param>
	  /// <param name="payLeg">  the pay leg </param>
	  /// <returns> the cap/floor </returns>
	  public static IborCapFloor of(IborCapFloorLeg capFloorLeg, SwapLeg payLeg)
	  {
		return new IborCapFloor(capFloorLeg, payLeg);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (payLeg != null)
		{
		  ArgChecker.isFalse(payLeg.PayReceive.Equals(capFloorLeg.PayReceive), "Legs must have different Pay/Receive flag, but both were {}", payLeg.PayReceive);
		}
	  }

	  //-------------------------------------------------------------------------
	  public override ImmutableSet<Currency> allPaymentCurrencies()
	  {
		if (payLeg == null)
		{
		  return ImmutableSet.of(capFloorLeg.Currency);
		}
		else
		{
		  return ImmutableSet.of(capFloorLeg.Currency, payLeg.Currency);
		}
	  }

	  public ImmutableSet<Currency> allCurrencies()
	  {
		if (payLeg == null)
		{
		  return ImmutableSet.of(capFloorLeg.Currency);
		}
		else
		{
		  ImmutableSet.Builder<Currency> builder = ImmutableSet.builder();
		  builder.add(capFloorLeg.Currency);
		  builder.addAll(payLeg.allCurrencies());
		  return builder.build();
		}
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
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		builder.add(capFloorLeg.Calculation.Index);
		if (payLeg != null)
		{
		  payLeg.collectIndices(builder);
		}
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  public ResolvedIborCapFloor resolve(ReferenceData refData)
	  {
		if (payLeg == null)
		{
		  return ResolvedIborCapFloor.of(capFloorLeg.resolve(refData));
		}
		return ResolvedIborCapFloor.of(capFloorLeg.resolve(refData), payLeg.resolve(refData));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborCapFloor}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static IborCapFloor.Meta meta()
	  {
		return IborCapFloor.Meta.INSTANCE;
	  }

	  static IborCapFloor()
	  {
		MetaBean.register(IborCapFloor.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private IborCapFloor(IborCapFloorLeg capFloorLeg, SwapLeg payLeg)
	  {
		JodaBeanUtils.notNull(capFloorLeg, "capFloorLeg");
		this.capFloorLeg = capFloorLeg;
		this.payLeg = payLeg;
		validate();
	  }

	  public override IborCapFloor.Meta metaBean()
	  {
		return IborCapFloor.Meta.INSTANCE;
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
	  public IborCapFloorLeg CapFloorLeg
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
	  /// These periodic payments are not made for typical cap/floor products.
	  /// Instead the premium is paid upfront.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<SwapLeg> PayLeg
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
		  IborCapFloor other = (IborCapFloor) obj;
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
		buf.Append("IborCapFloor{");
		buf.Append("capFloorLeg").Append('=').Append(capFloorLeg).Append(',').Append(' ');
		buf.Append("payLeg").Append('=').Append(JodaBeanUtils.ToString(payLeg));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborCapFloor}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  capFloorLeg_Renamed = DirectMetaProperty.ofImmutable(this, "capFloorLeg", typeof(IborCapFloor), typeof(IborCapFloorLeg));
			  payLeg_Renamed = DirectMetaProperty.ofImmutable(this, "payLeg", typeof(IborCapFloor), typeof(SwapLeg));
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
		internal MetaProperty<IborCapFloorLeg> capFloorLeg_Renamed;
		/// <summary>
		/// The meta-property for the {@code payLeg} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SwapLeg> payLeg_Renamed;
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
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends IborCapFloor> builder()
		public override BeanBuilder<IborCapFloor> builder()
		{
		  return new IborCapFloor.Builder();
		}

		public override Type beanType()
		{
		  return typeof(IborCapFloor);
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
		public MetaProperty<IborCapFloorLeg> capFloorLeg()
		{
		  return capFloorLeg_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code payLeg} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SwapLeg> payLeg()
		{
		  return payLeg_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 2124672084: // capFloorLeg
			  return ((IborCapFloor) bean).CapFloorLeg;
			case -995239866: // payLeg
			  return ((IborCapFloor) bean).payLeg;
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
	  /// The bean-builder for {@code IborCapFloor}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<IborCapFloor>
	  {

		internal IborCapFloorLeg capFloorLeg;
		internal SwapLeg payLeg;

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
			  this.capFloorLeg = (IborCapFloorLeg) newValue;
			  break;
			case -995239866: // payLeg
			  this.payLeg = (SwapLeg) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override IborCapFloor build()
		{
		  return new IborCapFloor(capFloorLeg, payLeg);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("IborCapFloor.Builder{");
		  buf.Append("capFloorLeg").Append('=').Append(JodaBeanUtils.ToString(capFloorLeg)).Append(',').Append(' ');
		  buf.Append("payLeg").Append('=').Append(JodaBeanUtils.ToString(payLeg));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}