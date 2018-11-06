using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
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

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;

	/// <summary>
	/// Information about a single observation of an FX index.
	/// <para>
	/// Observing an FX index requires knowledge of the index, fixing date and maturity date.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") public final class FxIndexObservation implements IndexObservation, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxIndexObservation : IndexObservation, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final FxIndex index;
		private readonly FxIndex index;
	  /// <summary>
	  /// The date of the index fixing.
	  /// <para>
	  /// This is an adjusted date with any business day rule applied.
	  /// Valid business days are defined by <seealso cref="FxIndex#getFixingCalendar()"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate fixingDate;
	  private readonly LocalDate fixingDate;
	  /// <summary>
	  /// The date of the transfer implied by the fixing date.
	  /// <para>
	  /// This is an adjusted date with any business day rule applied.
	  /// This must be equal to <seealso cref="FxIndex#calculateMaturityFromFixing(LocalDate, ReferenceData)"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate maturityDate;
	  private readonly LocalDate maturityDate;

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
	  /// <returns> the rate observation </returns>
	  public static FxIndexObservation of(FxIndex index, LocalDate fixingDate, ReferenceData refData)
	  {
		LocalDate maturityDate = index.calculateMaturityFromFixing(fixingDate, refData);
		return new FxIndexObservation(index, fixingDate, maturityDate);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency pair of the FX index.
	  /// </summary>
	  /// <returns> the currency pair of the index </returns>
	  public CurrencyPair CurrencyPair
	  {
		  get
		  {
			return index.CurrencyPair;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compares this observation to another based on the index and fixing date.
	  /// <para>
	  /// The maturity date is ignored.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="obj">  the other observation </param>
	  /// <returns> true if equal </returns>
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  FxIndexObservation other = (FxIndexObservation) obj;
		  return index.Equals(other.index) && fixingDate.Equals(other.fixingDate);
		}
		return false;
	  }

	  /// <summary>
	  /// Returns a hash code based on the index and fixing date.
	  /// <para>
	  /// The maturity date is ignored.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + index.GetHashCode();
		return hash * 31 + fixingDate.GetHashCode();
	  }

	  public override string ToString()
	  {
		return (new StringBuilder(64)).Append("FxIndexObservation[").Append(index).Append(" on ").Append(fixingDate).Append(']').ToString();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxIndexObservation}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxIndexObservation.Meta meta()
	  {
		return FxIndexObservation.Meta.INSTANCE;
	  }

	  static FxIndexObservation()
	  {
		MetaBean.register(FxIndexObservation.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="index">  the value of the property, not null </param>
	  /// <param name="fixingDate">  the value of the property, not null </param>
	  /// <param name="maturityDate">  the value of the property, not null </param>
	  internal FxIndexObservation(FxIndex index, LocalDate fixingDate, LocalDate maturityDate)
	  {
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(fixingDate, "fixingDate");
		JodaBeanUtils.notNull(maturityDate, "maturityDate");
		this.index = index;
		this.fixingDate = fixingDate;
		this.maturityDate = maturityDate;
	  }

	  public override FxIndexObservation.Meta metaBean()
	  {
		return FxIndexObservation.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the FX index.
	  /// <para>
	  /// The rate will be queried from this index.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxIndex Index
	  {
		  get
		  {
			return index;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the date of the index fixing.
	  /// <para>
	  /// This is an adjusted date with any business day rule applied.
	  /// Valid business days are defined by <seealso cref="FxIndex#getFixingCalendar()"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate FixingDate
	  {
		  get
		  {
			return fixingDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the date of the transfer implied by the fixing date.
	  /// <para>
	  /// This is an adjusted date with any business day rule applied.
	  /// This must be equal to <seealso cref="FxIndex#calculateMaturityFromFixing(LocalDate, ReferenceData)"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate MaturityDate
	  {
		  get
		  {
			return maturityDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxIndexObservation}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(FxIndexObservation), typeof(FxIndex));
			  fixingDate_Renamed = DirectMetaProperty.ofImmutable(this, "fixingDate", typeof(FxIndexObservation), typeof(LocalDate));
			  maturityDate_Renamed = DirectMetaProperty.ofImmutable(this, "maturityDate", typeof(FxIndexObservation), typeof(LocalDate));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "index", "fixingDate", "maturityDate");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> fixingDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code maturityDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> maturityDate_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "index", "fixingDate", "maturityDate");
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
			case 100346066: // index
			  return index_Renamed;
			case 1255202043: // fixingDate
			  return fixingDate_Renamed;
			case -414641441: // maturityDate
			  return maturityDate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends FxIndexObservation> builder()
		public override BeanBuilder<FxIndexObservation> builder()
		{
		  return new FxIndexObservation.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxIndexObservation);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code index} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> fixingDate()
		{
		  return fixingDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code maturityDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> maturityDate()
		{
		  return maturityDate_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return ((FxIndexObservation) bean).Index;
			case 1255202043: // fixingDate
			  return ((FxIndexObservation) bean).FixingDate;
			case -414641441: // maturityDate
			  return ((FxIndexObservation) bean).MaturityDate;
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
	  /// The bean-builder for {@code FxIndexObservation}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<FxIndexObservation>
	  {

		internal FxIndex index;
		internal LocalDate fixingDate;
		internal LocalDate maturityDate;

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
			case 100346066: // index
			  return index;
			case 1255202043: // fixingDate
			  return fixingDate;
			case -414641441: // maturityDate
			  return maturityDate;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  this.index = (FxIndex) newValue;
			  break;
			case 1255202043: // fixingDate
			  this.fixingDate = (LocalDate) newValue;
			  break;
			case -414641441: // maturityDate
			  this.maturityDate = (LocalDate) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override FxIndexObservation build()
		{
		  return new FxIndexObservation(index, fixingDate, maturityDate);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("FxIndexObservation.Builder{");
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index)).Append(',').Append(' ');
		  buf.Append("fixingDate").Append('=').Append(JodaBeanUtils.ToString(fixingDate)).Append(',').Append(' ');
		  buf.Append("maturityDate").Append('=').Append(JodaBeanUtils.ToString(maturityDate));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}