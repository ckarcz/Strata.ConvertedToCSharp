using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.rate
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using Index = com.opengamma.strata.basics.index.Index;

	/// <summary>
	/// Defines the computation of a rate of interest from a single Ibor index.
	/// <para>
	/// An interest rate determined directly from an Ibor index.
	/// For example, a rate determined from 'GBP-LIBOR-3M' on a single fixing date.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class IborRateComputation implements RateComputation, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IborRateComputation : RateComputation, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.IborIndexObservation observation;
		private readonly IborIndexObservation observation;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance from an index and fixing date.
	  /// <para>
	  /// The reference data is used to find the maturity date from the fixing date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="fixingDate">  the fixing date </param>
	  /// <param name="refData">  the reference data to use when resolving holiday calendars </param>
	  /// <returns> the rate computation </returns>
	  public static IborRateComputation of(IborIndex index, LocalDate fixingDate, ReferenceData refData)
	  {
		return new IborRateComputation(IborIndexObservation.of(index, fixingDate, refData));
	  }

	  /// <summary>
	  /// Creates an instance from the underlying index observation.
	  /// </summary>
	  /// <param name="underlyingObservation">  the underlying index observation </param>
	  /// <returns> the rate computation </returns>
	  public static IborRateComputation of(IborIndexObservation underlyingObservation)
	  {
		return new IborRateComputation(underlyingObservation);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Ibor index.
	  /// </summary>
	  /// <returns> the index </returns>
	  public IborIndex Index
	  {
		  get
		  {
			return observation.Index;
		  }
	  }

	  /// <summary>
	  /// Gets the currency of the Ibor index.
	  /// </summary>
	  /// <returns> the currency of the index </returns>
	  public Currency Currency
	  {
		  get
		  {
			return Index.Currency;
		  }
	  }

	  /// <summary>
	  /// Gets the fixing date.
	  /// </summary>
	  /// <returns> the fixing date </returns>
	  public LocalDate FixingDate
	  {
		  get
		  {
			return observation.FixingDate;
		  }
	  }

	  /// <summary>
	  /// Gets the effective date.
	  /// </summary>
	  /// <returns> the effective date </returns>
	  public LocalDate EffectiveDate
	  {
		  get
		  {
			return observation.EffectiveDate;
		  }
	  }

	  /// <summary>
	  /// Gets the maturity date.
	  /// </summary>
	  /// <returns> the maturity date </returns>
	  public LocalDate MaturityDate
	  {
		  get
		  {
			return observation.MaturityDate;
		  }
	  }

	  /// <summary>
	  /// Gets the year fraction.
	  /// </summary>
	  /// <returns> the year fraction </returns>
	  public double YearFraction
	  {
		  get
		  {
			return observation.YearFraction;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public void collectIndices(ImmutableSet.Builder<Index> builder)
	  {
		builder.add(Index);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborRateComputation}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static IborRateComputation.Meta meta()
	  {
		return IborRateComputation.Meta.INSTANCE;
	  }

	  static IborRateComputation()
	  {
		MetaBean.register(IborRateComputation.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private IborRateComputation(IborIndexObservation observation)
	  {
		JodaBeanUtils.notNull(observation, "observation");
		this.observation = observation;
	  }

	  public override IborRateComputation.Meta metaBean()
	  {
		return IborRateComputation.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying index observation. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborIndexObservation Observation
	  {
		  get
		  {
			return observation;
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
		  IborRateComputation other = (IborRateComputation) obj;
		  return JodaBeanUtils.equal(observation, other.observation);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(observation);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("IborRateComputation{");
		buf.Append("observation").Append('=').Append(JodaBeanUtils.ToString(observation));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborRateComputation}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  observation_Renamed = DirectMetaProperty.ofImmutable(this, "observation", typeof(IborRateComputation), typeof(IborIndexObservation));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "observation");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code observation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborIndexObservation> observation_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "observation");
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
			case 122345516: // observation
			  return observation_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends IborRateComputation> builder()
		public override BeanBuilder<IborRateComputation> builder()
		{
		  return new IborRateComputation.Builder();
		}

		public override Type beanType()
		{
		  return typeof(IborRateComputation);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code observation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborIndexObservation> observation()
		{
		  return observation_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 122345516: // observation
			  return ((IborRateComputation) bean).Observation;
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
	  /// The bean-builder for {@code IborRateComputation}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<IborRateComputation>
	  {

		internal IborIndexObservation observation;

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
			case 122345516: // observation
			  return observation;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 122345516: // observation
			  this.observation = (IborIndexObservation) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override IborRateComputation build()
		{
		  return new IborRateComputation(observation);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("IborRateComputation.Builder{");
		  buf.Append("observation").Append('=').Append(JodaBeanUtils.ToString(observation));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}