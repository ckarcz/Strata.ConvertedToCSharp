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
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using Index = com.opengamma.strata.basics.index.Index;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// Defines the computation of a rate of interest interpolated from two Ibor indices.
	/// <para>
	/// An interest rate determined from two Ibor indices by linear interpolation.
	/// Both indices are observed on the same fixing date and they must have the same currency.
	/// For example, linear interpolation between 'GBP-LIBOR-1M' and 'GBP-LIBOR-3M'.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class IborInterpolatedRateComputation implements RateComputation, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IborInterpolatedRateComputation : RateComputation, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.IborIndexObservation shortObservation;
		private readonly IborIndexObservation shortObservation;
	  /// <summary>
	  /// The longer Ibor index observation.
	  /// <para>
	  /// The rate to be paid is based on this index
	  /// It will be a well known market index such as 'GBP-LIBOR-3M'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.IborIndexObservation longObservation;
	  private readonly IborIndexObservation longObservation;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance from two indices and fixing date.
	  /// <para>
	  /// The indices may be passed in any order.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index1">  the first index </param>
	  /// <param name="index2">  the second index </param>
	  /// <param name="fixingDate">  the fixing date </param>
	  /// <param name="refData">  the reference data to use when resolving holiday calendars </param>
	  /// <returns> the interpolated rate computation </returns>
	  public static IborInterpolatedRateComputation of(IborIndex index1, IborIndex index2, LocalDate fixingDate, ReferenceData refData)
	  {

		bool inOrder = indicesInOrder(index1, index2, fixingDate);
		IborIndexObservation obs1 = IborIndexObservation.of(index1, fixingDate, refData);
		IborIndexObservation obs2 = IborIndexObservation.of(index2, fixingDate, refData);
		return new IborInterpolatedRateComputation(inOrder ? obs1 : obs2, inOrder ? obs2 : obs1);
	  }

	  /// <summary>
	  /// Creates an instance from the two underlying index observations.
	  /// <para>
	  /// The two observations must be for two different indexes in the same currency on the same fixing date.
	  /// The index with the shorter tenor must be passed as the first argument.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="shortObservation">  the short underlying index observation </param>
	  /// <param name="longObservation">  the long underlying index observation </param>
	  /// <returns> the rate computation </returns>
	  /// <exception cref="IllegalArgumentException"> if the indices are not short, then long </exception>
	  public static IborInterpolatedRateComputation of(IborIndexObservation shortObservation, IborIndexObservation longObservation)
	  {

		return new IborInterpolatedRateComputation(shortObservation, longObservation);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		IborIndex shortIndex = shortObservation.Index;
		IborIndex longIndex = longObservation.Index;
		if (!shortIndex.Currency.Equals(longIndex.Currency))
		{
		  throw new System.ArgumentException("Interpolation requires two indices in the same currency");
		}
		if (shortIndex.Equals(longIndex))
		{
		  throw new System.ArgumentException("Interpolation requires two different indices");
		}
		if (!shortObservation.FixingDate.Equals(longObservation.FixingDate))
		{
		  throw new System.ArgumentException("Interpolation requires observations with same fixing date");
		}
		if (!indicesInOrder(shortIndex, longIndex, shortObservation.FixingDate))
		{
		  throw new System.ArgumentException(Messages.format("Interpolation indices passed in wrong order: {} {}", shortIndex, longIndex));
		}
	  }

	  // checks that the indices are in order
	  private static bool indicesInOrder(IborIndex index1, IborIndex index2, LocalDate fixingDate)
	  {
		return fixingDate.plus(index1.Tenor).isBefore(fixingDate.plus(index2.Tenor));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fixing date.
	  /// </summary>
	  /// <returns> the fixing date </returns>
	  public LocalDate FixingDate
	  {
		  get
		  {
			// fixing date is the same for both observations
			return shortObservation.FixingDate;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public void collectIndices(ImmutableSet.Builder<Index> builder)
	  {
		builder.add(shortObservation.Index);
		builder.add(longObservation.Index);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborInterpolatedRateComputation}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static IborInterpolatedRateComputation.Meta meta()
	  {
		return IborInterpolatedRateComputation.Meta.INSTANCE;
	  }

	  static IborInterpolatedRateComputation()
	  {
		MetaBean.register(IborInterpolatedRateComputation.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private IborInterpolatedRateComputation(IborIndexObservation shortObservation, IborIndexObservation longObservation)
	  {
		JodaBeanUtils.notNull(shortObservation, "shortObservation");
		JodaBeanUtils.notNull(longObservation, "longObservation");
		this.shortObservation = shortObservation;
		this.longObservation = longObservation;
		validate();
	  }

	  public override IborInterpolatedRateComputation.Meta metaBean()
	  {
		return IborInterpolatedRateComputation.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the shorter Ibor index observation.
	  /// <para>
	  /// The rate to be paid is based on this index
	  /// It will be a well known market index such as 'GBP-LIBOR-1M'.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborIndexObservation ShortObservation
	  {
		  get
		  {
			return shortObservation;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the longer Ibor index observation.
	  /// <para>
	  /// The rate to be paid is based on this index
	  /// It will be a well known market index such as 'GBP-LIBOR-3M'.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborIndexObservation LongObservation
	  {
		  get
		  {
			return longObservation;
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
		  IborInterpolatedRateComputation other = (IborInterpolatedRateComputation) obj;
		  return JodaBeanUtils.equal(shortObservation, other.shortObservation) && JodaBeanUtils.equal(longObservation, other.longObservation);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(shortObservation);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(longObservation);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("IborInterpolatedRateComputation{");
		buf.Append("shortObservation").Append('=').Append(shortObservation).Append(',').Append(' ');
		buf.Append("longObservation").Append('=').Append(JodaBeanUtils.ToString(longObservation));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborInterpolatedRateComputation}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  shortObservation_Renamed = DirectMetaProperty.ofImmutable(this, "shortObservation", typeof(IborInterpolatedRateComputation), typeof(IborIndexObservation));
			  longObservation_Renamed = DirectMetaProperty.ofImmutable(this, "longObservation", typeof(IborInterpolatedRateComputation), typeof(IborIndexObservation));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "shortObservation", "longObservation");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code shortObservation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborIndexObservation> shortObservation_Renamed;
		/// <summary>
		/// The meta-property for the {@code longObservation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborIndexObservation> longObservation_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "shortObservation", "longObservation");
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
			case -496986608: // shortObservation
			  return shortObservation_Renamed;
			case -684321776: // longObservation
			  return longObservation_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends IborInterpolatedRateComputation> builder()
		public override BeanBuilder<IborInterpolatedRateComputation> builder()
		{
		  return new IborInterpolatedRateComputation.Builder();
		}

		public override Type beanType()
		{
		  return typeof(IborInterpolatedRateComputation);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code shortObservation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborIndexObservation> shortObservation()
		{
		  return shortObservation_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code longObservation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborIndexObservation> longObservation()
		{
		  return longObservation_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -496986608: // shortObservation
			  return ((IborInterpolatedRateComputation) bean).ShortObservation;
			case -684321776: // longObservation
			  return ((IborInterpolatedRateComputation) bean).LongObservation;
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
	  /// The bean-builder for {@code IborInterpolatedRateComputation}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<IborInterpolatedRateComputation>
	  {

		internal IborIndexObservation shortObservation;
		internal IborIndexObservation longObservation;

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
			case -496986608: // shortObservation
			  return shortObservation;
			case -684321776: // longObservation
			  return longObservation;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -496986608: // shortObservation
			  this.shortObservation = (IborIndexObservation) newValue;
			  break;
			case -684321776: // longObservation
			  this.longObservation = (IborIndexObservation) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override IborInterpolatedRateComputation build()
		{
		  return new IborInterpolatedRateComputation(shortObservation, longObservation);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("IborInterpolatedRateComputation.Builder{");
		  buf.Append("shortObservation").Append('=').Append(JodaBeanUtils.ToString(shortObservation)).Append(',').Append(' ');
		  buf.Append("longObservation").Append('=').Append(JodaBeanUtils.ToString(longObservation));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}