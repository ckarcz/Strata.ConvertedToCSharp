using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{

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
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Index = com.opengamma.strata.basics.index.Index;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using IborInterpolatedRateComputation = com.opengamma.strata.product.rate.IborInterpolatedRateComputation;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// Defines the rates applicable in the initial or final stub of an Ibor swap leg.
	/// <para>
	/// A standard swap leg consists of a regular periodic schedule and one or two stub periods at each end.
	/// This class defines what floating rate to use during a stub.
	/// </para>
	/// <para>
	/// The rate may be specified in three ways.
	/// <ul>
	/// <li>A fixed rate, applicable for the whole stub
	/// <li>A floating rate based on a single Ibor index
	/// <li>A floating rate based on linear interpolation between two Ibor indices
	/// </ul>
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class IborRateStubCalculation implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IborRateStubCalculation : ImmutableBean
	{

	  /// <summary>
	  /// An instance that has no special rate handling.
	  /// </summary>
	  public static readonly IborRateStubCalculation NONE = new IborRateStubCalculation(null, null, null, null);

	  /// <summary>
	  /// The fixed rate to use in the stub.
	  /// A 5% rate will be expressed as 0.05.
	  /// <para>
	  /// In certain circumstances two counterparties agree a fixed rate for the stub.
	  /// It is used in place of an observed fixing.
	  /// Other calculation elements, such as gearing or spread, still apply.
	  /// </para>
	  /// <para>
	  /// If the fixed rate is present, then {@code knownAmount}, {@code index} and
	  /// {@code indexInterpolated} must not be present.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final System.Nullable<double> fixedRate;
	  private readonly double? fixedRate;
	  /// <summary>
	  /// The known amount to pay/receive for the stub.
	  /// <para>
	  /// If the known amount is present, then {@code fixedRate}, {@code index} and
	  /// {@code indexInterpolated} must not be present.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.currency.CurrencyAmount knownAmount;
	  private readonly CurrencyAmount knownAmount;
	  /// <summary>
	  /// The Ibor index to be used for the stub.
	  /// <para>
	  /// This will be used throughout the stub unless {@code indexInterpolated} is present.
	  /// </para>
	  /// <para>
	  /// If the index is present, then {@code fixedRate} and {@code knownAmount} must not be present.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.index.IborIndex index;
	  private readonly IborIndex index;
	  /// <summary>
	  /// The second Ibor index to be used for the stub, linearly interpolated.
	  /// <para>
	  /// This will be used with {@code index} to linearly interpolate the rate.
	  /// This index may be shorter or longer than {@code index}, but not the same.
	  /// </para>
	  /// <para>
	  /// If the interpolated index is present, then {@code index} must also be present,
	  /// and {@code fixedRate} and {@code knownAmount} must not be present.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.index.IborIndex indexInterpolated;
	  private readonly IborIndex indexInterpolated;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance with a single fixed rate.
	  /// </summary>
	  /// <param name="fixedRate">  the fixed rate for the stub </param>
	  /// <returns> the stub </returns>
	  public static IborRateStubCalculation ofFixedRate(double fixedRate)
	  {
		return new IborRateStubCalculation(fixedRate, null, null, null);
	  }

	  /// <summary>
	  /// Obtains an instance with a known amount of interest.
	  /// </summary>
	  /// <param name="knownAmount">  the known amount of interest </param>
	  /// <returns> the stub </returns>
	  public static IborRateStubCalculation ofKnownAmount(CurrencyAmount knownAmount)
	  {
		ArgChecker.notNull(knownAmount, "knownAmount");
		return new IborRateStubCalculation(null, knownAmount, null, null);
	  }

	  /// <summary>
	  /// Obtains an instance with a single floating rate.
	  /// </summary>
	  /// <param name="index">  the index that applies to the stub </param>
	  /// <returns> the stub </returns>
	  /// <exception cref="IllegalArgumentException"> if the index is null </exception>
	  public static IborRateStubCalculation ofIborRate(IborIndex index)
	  {
		ArgChecker.notNull(index, "index");
		return new IborRateStubCalculation(null, null, index, null);
	  }

	  /// <summary>
	  /// Obtains an instance with linear interpolation of two floating rates.
	  /// <para>
	  /// The two indices must be different, typically with one longer than another.
	  /// The order of input of the indices does not matter.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index1">  the first index </param>
	  /// <param name="index2">  the second index </param>
	  /// <returns> the stub </returns>
	  /// <exception cref="IllegalArgumentException"> if the two indices are the same or either is null </exception>
	  public static IborRateStubCalculation ofIborInterpolatedRate(IborIndex index1, IborIndex index2)
	  {
		ArgChecker.notNull(index1, "index1");
		ArgChecker.notNull(index2, "index2");
		return new IborRateStubCalculation(null, null, index1, index2);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (fixedRate != null && knownAmount != null)
		{
		  throw new System.ArgumentException("Either rate or amount may be specified, not both");
		}
		if (fixedRate != null && index != null)
		{
		  throw new System.ArgumentException("Either rate or index may be specified, not both");
		}
		if (index != null && knownAmount != null)
		{
		  throw new System.ArgumentException("Either index or amount may be specified, not both");
		}
		if (indexInterpolated != null)
		{
		  if (index == null)
		  {
			throw new System.ArgumentException("When indexInterpolated is present, index must also be present");
		  }
		  if (indexInterpolated.Equals(index))
		  {
			throw new System.ArgumentException("Interpolation requires two different indices");
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates the {@code RateComputation} for the stub.
	  /// </summary>
	  /// <param name="fixingDate">  the fixing date </param>
	  /// <param name="defaultIndex">  the default index to use if the stub has no rules </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the rate observation </returns>
	  internal RateComputation createRateComputation(LocalDate fixingDate, IborIndex defaultIndex, ReferenceData refData)
	  {
		if (Interpolated)
		{
		  return IborInterpolatedRateComputation.of(index, indexInterpolated, fixingDate, refData);
		}
		else if (FloatingRate)
		{
		  return IborRateComputation.of(index, fixingDate, refData);
		}
		else if (FixedRate)
		{
		  return FixedRateComputation.of(fixedRate.Value);
		}
		else if (KnownAmount)
		{
		  return KnownAmountRateComputation.of(knownAmount);
		}
		else
		{
		  return IborRateComputation.of(defaultIndex, fixingDate, refData);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the stub has a fixed rate.
	  /// </summary>
	  /// <returns> true if a fixed stub rate applies </returns>
	  public bool FixedRate
	  {
		  get
		  {
			return fixedRate != null;
		  }
	  }

	  /// <summary>
	  /// Checks if the stub has a known amount.
	  /// </summary>
	  /// <returns> true if the stub has a known amount </returns>
	  public bool KnownAmount
	  {
		  get
		  {
			return knownAmount != null;
		  }
	  }

	  /// <summary>
	  /// Checks if the stub has a floating rate.
	  /// </summary>
	  /// <returns> true if a floating stub rate applies </returns>
	  public bool FloatingRate
	  {
		  get
		  {
			return index != null;
		  }
	  }

	  /// <summary>
	  /// Checks if the stub has an interpolated rate.
	  /// <para>
	  /// An interpolated rate exists when there are two different rates that need linear interpolation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if linear interpolation applies </returns>
	  public bool Interpolated
	  {
		  get
		  {
			return index != null && indexInterpolated != null;
		  }
	  }

	  // collect the currencies
	  internal void collectCurrencies(ImmutableSet.Builder<Currency> builder)
	  {
		KnownAmount.ifPresent(amt => builder.add(amt.Currency));
		Index.ifPresent(idx => builder.add(idx.Currency));
		IndexInterpolated.ifPresent(idx => builder.add(idx.Currency));
	  }

	  // collect the indices
	  internal void collectIndices(ImmutableSet.Builder<Index> builder)
	  {
		Index.ifPresent(idx => builder.add(idx));
		IndexInterpolated.ifPresent(idx => builder.add(idx));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborRateStubCalculation}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static IborRateStubCalculation.Meta meta()
	  {
		return IborRateStubCalculation.Meta.INSTANCE;
	  }

	  static IborRateStubCalculation()
	  {
		MetaBean.register(IborRateStubCalculation.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static IborRateStubCalculation.Builder builder()
	  {
		return new IborRateStubCalculation.Builder();
	  }

	  private IborRateStubCalculation(double? fixedRate, CurrencyAmount knownAmount, IborIndex index, IborIndex indexInterpolated)
	  {
		this.fixedRate = fixedRate;
		this.knownAmount = knownAmount;
		this.index = index;
		this.indexInterpolated = indexInterpolated;
		validate();
	  }

	  public override IborRateStubCalculation.Meta metaBean()
	  {
		return IborRateStubCalculation.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fixed rate to use in the stub.
	  /// A 5% rate will be expressed as 0.05.
	  /// <para>
	  /// In certain circumstances two counterparties agree a fixed rate for the stub.
	  /// It is used in place of an observed fixing.
	  /// Other calculation elements, such as gearing or spread, still apply.
	  /// </para>
	  /// <para>
	  /// If the fixed rate is present, then {@code knownAmount}, {@code index} and
	  /// {@code indexInterpolated} must not be present.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public double? FixedRate
	  {
		  get
		  {
			return fixedRate != null ? double?.of(fixedRate) : double?.empty();
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the known amount to pay/receive for the stub.
	  /// <para>
	  /// If the known amount is present, then {@code fixedRate}, {@code index} and
	  /// {@code indexInterpolated} must not be present.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<CurrencyAmount> KnownAmount
	  {
		  get
		  {
			return Optional.ofNullable(knownAmount);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Ibor index to be used for the stub.
	  /// <para>
	  /// This will be used throughout the stub unless {@code indexInterpolated} is present.
	  /// </para>
	  /// <para>
	  /// If the index is present, then {@code fixedRate} and {@code knownAmount} must not be present.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<IborIndex> Index
	  {
		  get
		  {
			return Optional.ofNullable(index);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the second Ibor index to be used for the stub, linearly interpolated.
	  /// <para>
	  /// This will be used with {@code index} to linearly interpolate the rate.
	  /// This index may be shorter or longer than {@code index}, but not the same.
	  /// </para>
	  /// <para>
	  /// If the interpolated index is present, then {@code index} must also be present,
	  /// and {@code fixedRate} and {@code knownAmount} must not be present.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<IborIndex> IndexInterpolated
	  {
		  get
		  {
			return Optional.ofNullable(indexInterpolated);
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
		  IborRateStubCalculation other = (IborRateStubCalculation) obj;
		  return JodaBeanUtils.equal(fixedRate, other.fixedRate) && JodaBeanUtils.equal(knownAmount, other.knownAmount) && JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(indexInterpolated, other.indexInterpolated);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixedRate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(knownAmount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(indexInterpolated);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("IborRateStubCalculation{");
		buf.Append("fixedRate").Append('=').Append(fixedRate).Append(',').Append(' ');
		buf.Append("knownAmount").Append('=').Append(knownAmount).Append(',').Append(' ');
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("indexInterpolated").Append('=').Append(JodaBeanUtils.ToString(indexInterpolated));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborRateStubCalculation}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  fixedRate_Renamed = DirectMetaProperty.ofImmutable(this, "fixedRate", typeof(IborRateStubCalculation), typeof(Double));
			  knownAmount_Renamed = DirectMetaProperty.ofImmutable(this, "knownAmount", typeof(IborRateStubCalculation), typeof(CurrencyAmount));
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(IborRateStubCalculation), typeof(IborIndex));
			  indexInterpolated_Renamed = DirectMetaProperty.ofImmutable(this, "indexInterpolated", typeof(IborRateStubCalculation), typeof(IborIndex));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "fixedRate", "knownAmount", "index", "indexInterpolated");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code fixedRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> fixedRate_Renamed;
		/// <summary>
		/// The meta-property for the {@code knownAmount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyAmount> knownAmount_Renamed;
		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code indexInterpolated} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborIndex> indexInterpolated_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "fixedRate", "knownAmount", "index", "indexInterpolated");
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
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
			case -158727813: // knownAmount
			  return knownAmount_Renamed;
			case 100346066: // index
			  return index_Renamed;
			case -1934091915: // indexInterpolated
			  return indexInterpolated_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override IborRateStubCalculation.Builder builder()
		{
		  return new IborRateStubCalculation.Builder();
		}

		public override Type beanType()
		{
		  return typeof(IborRateStubCalculation);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code fixedRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> fixedRate()
		{
		  return fixedRate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code knownAmount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyAmount> knownAmount()
		{
		  return knownAmount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code index} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code indexInterpolated} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborIndex> indexInterpolated()
		{
		  return indexInterpolated_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 747425396: // fixedRate
			  return ((IborRateStubCalculation) bean).fixedRate;
			case -158727813: // knownAmount
			  return ((IborRateStubCalculation) bean).knownAmount;
			case 100346066: // index
			  return ((IborRateStubCalculation) bean).index;
			case -1934091915: // indexInterpolated
			  return ((IborRateStubCalculation) bean).indexInterpolated;
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
	  /// The bean-builder for {@code IborRateStubCalculation}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<IborRateStubCalculation>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double? fixedRate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurrencyAmount knownAmount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborIndex indexInterpolated_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(IborRateStubCalculation beanToCopy)
		{
		  this.fixedRate_Renamed = beanToCopy.fixedRate;
		  this.knownAmount_Renamed = beanToCopy.knownAmount;
		  this.index_Renamed = beanToCopy.index;
		  this.indexInterpolated_Renamed = beanToCopy.indexInterpolated;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
			case -158727813: // knownAmount
			  return knownAmount_Renamed;
			case 100346066: // index
			  return index_Renamed;
			case -1934091915: // indexInterpolated
			  return indexInterpolated_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 747425396: // fixedRate
			  this.fixedRate_Renamed = (double?) newValue;
			  break;
			case -158727813: // knownAmount
			  this.knownAmount_Renamed = (CurrencyAmount) newValue;
			  break;
			case 100346066: // index
			  this.index_Renamed = (IborIndex) newValue;
			  break;
			case -1934091915: // indexInterpolated
			  this.indexInterpolated_Renamed = (IborIndex) newValue;
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

		public override IborRateStubCalculation build()
		{
		  return new IborRateStubCalculation(fixedRate_Renamed, knownAmount_Renamed, index_Renamed, indexInterpolated_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the fixed rate to use in the stub.
		/// A 5% rate will be expressed as 0.05.
		/// <para>
		/// In certain circumstances two counterparties agree a fixed rate for the stub.
		/// It is used in place of an observed fixing.
		/// Other calculation elements, such as gearing or spread, still apply.
		/// </para>
		/// <para>
		/// If the fixed rate is present, then {@code knownAmount}, {@code index} and
		/// {@code indexInterpolated} must not be present.
		/// </para>
		/// </summary>
		/// <param name="fixedRate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixedRate(double? fixedRate)
		{
		  this.fixedRate_Renamed = fixedRate;
		  return this;
		}

		/// <summary>
		/// Sets the known amount to pay/receive for the stub.
		/// <para>
		/// If the known amount is present, then {@code fixedRate}, {@code index} and
		/// {@code indexInterpolated} must not be present.
		/// </para>
		/// </summary>
		/// <param name="knownAmount">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder knownAmount(CurrencyAmount knownAmount)
		{
		  this.knownAmount_Renamed = knownAmount;
		  return this;
		}

		/// <summary>
		/// Sets the Ibor index to be used for the stub.
		/// <para>
		/// This will be used throughout the stub unless {@code indexInterpolated} is present.
		/// </para>
		/// <para>
		/// If the index is present, then {@code fixedRate} and {@code knownAmount} must not be present.
		/// </para>
		/// </summary>
		/// <param name="index">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder index(IborIndex index)
		{
		  this.index_Renamed = index;
		  return this;
		}

		/// <summary>
		/// Sets the second Ibor index to be used for the stub, linearly interpolated.
		/// <para>
		/// This will be used with {@code index} to linearly interpolate the rate.
		/// This index may be shorter or longer than {@code index}, but not the same.
		/// </para>
		/// <para>
		/// If the interpolated index is present, then {@code index} must also be present,
		/// and {@code fixedRate} and {@code knownAmount} must not be present.
		/// </para>
		/// </summary>
		/// <param name="indexInterpolated">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder indexInterpolated(IborIndex indexInterpolated)
		{
		  this.indexInterpolated_Renamed = indexInterpolated;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("IborRateStubCalculation.Builder{");
		  buf.Append("fixedRate").Append('=').Append(JodaBeanUtils.ToString(fixedRate_Renamed)).Append(',').Append(' ');
		  buf.Append("knownAmount").Append('=').Append(JodaBeanUtils.ToString(knownAmount_Renamed)).Append(',').Append(' ');
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("indexInterpolated").Append('=').Append(JodaBeanUtils.ToString(indexInterpolated_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}