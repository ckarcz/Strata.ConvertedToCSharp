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

	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// Defines the observation of a rate of interest from a single Ibor index.
	/// <para>
	/// An interest rate determined directly from an Ibor index.
	/// For example, a rate determined from 'GBP-LIBOR-3M' on a single fixing date.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") public final class IborIndexObservation implements IndexObservation, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IborIndexObservation : IndexObservation, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final IborIndex index;
		private readonly IborIndex index;
	  /// <summary>
	  /// The date of the index fixing.
	  /// <para>
	  /// This is an adjusted date with any business day rule applied.
	  /// Valid business days are defined by <seealso cref="IborIndex#getFixingCalendar()"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate fixingDate;
	  private readonly LocalDate fixingDate;
	  /// <summary>
	  /// The effective date of the investment implied by the fixing date.
	  /// <para>
	  /// This is an adjusted date with any business day rule applied.
	  /// This must be equal to <seealso cref="IborIndex#calculateEffectiveFromFixing(LocalDate, ReferenceData)"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate effectiveDate;
	  private readonly LocalDate effectiveDate;
	  /// <summary>
	  /// The maturity date of the investment implied by the fixing date.
	  /// <para>
	  /// This is an adjusted date with any business day rule applied.
	  /// This must be equal to <seealso cref="IborIndex#calculateMaturityFromEffective(LocalDate, ReferenceData)"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate maturityDate;
	  private readonly LocalDate maturityDate;
	  /// <summary>
	  /// The year fraction of the investment implied by the fixing date.
	  /// <para>
	  /// This is calculated using the day count of the index.
	  /// It represents the fraction of the year between the effective date and the maturity date.
	  /// Typically the value will be close to 1 for one year and close to 0.5 for six months.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final double yearFraction;
	  private readonly double yearFraction;

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
	  public static IborIndexObservation of(IborIndex index, LocalDate fixingDate, ReferenceData refData)
	  {

		LocalDate effectiveDate = index.calculateEffectiveFromFixing(fixingDate, refData);
		LocalDate maturityDate = index.calculateMaturityFromEffective(effectiveDate, refData);
		double yearFraction = index.DayCount.yearFraction(effectiveDate, maturityDate);
		return new IborIndexObservation(index, fixingDate, effectiveDate, maturityDate, yearFraction);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the Ibor index.
	  /// </summary>
	  /// <returns> the currency of the index </returns>
	  public Currency Currency
	  {
		  get
		  {
			return index.Currency;
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborIndexObservation}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static IborIndexObservation.Meta meta()
	  {
		return IborIndexObservation.Meta.INSTANCE;
	  }

	  static IborIndexObservation()
	  {
		MetaBean.register(IborIndexObservation.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="index">  the value of the property, not null </param>
	  /// <param name="fixingDate">  the value of the property, not null </param>
	  /// <param name="effectiveDate">  the value of the property, not null </param>
	  /// <param name="maturityDate">  the value of the property, not null </param>
	  /// <param name="yearFraction">  the value of the property, not null </param>
	  internal IborIndexObservation(IborIndex index, LocalDate fixingDate, LocalDate effectiveDate, LocalDate maturityDate, double yearFraction)
	  {
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(fixingDate, "fixingDate");
		JodaBeanUtils.notNull(effectiveDate, "effectiveDate");
		JodaBeanUtils.notNull(maturityDate, "maturityDate");
		JodaBeanUtils.notNull(yearFraction, "yearFraction");
		this.index = index;
		this.fixingDate = fixingDate;
		this.effectiveDate = effectiveDate;
		this.maturityDate = maturityDate;
		this.yearFraction = yearFraction;
	  }

	  public override IborIndexObservation.Meta metaBean()
	  {
		return IborIndexObservation.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Ibor index.
	  /// <para>
	  /// The rate to be paid is based on this index.
	  /// It will be a well known market index such as 'GBP-LIBOR-3M'.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborIndex Index
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
	  /// Valid business days are defined by <seealso cref="IborIndex#getFixingCalendar()"/>.
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
	  /// Gets the effective date of the investment implied by the fixing date.
	  /// <para>
	  /// This is an adjusted date with any business day rule applied.
	  /// This must be equal to <seealso cref="IborIndex#calculateEffectiveFromFixing(LocalDate, ReferenceData)"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate EffectiveDate
	  {
		  get
		  {
			return effectiveDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the maturity date of the investment implied by the fixing date.
	  /// <para>
	  /// This is an adjusted date with any business day rule applied.
	  /// This must be equal to <seealso cref="IborIndex#calculateMaturityFromEffective(LocalDate, ReferenceData)"/>.
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
	  /// Gets the year fraction of the investment implied by the fixing date.
	  /// <para>
	  /// This is calculated using the day count of the index.
	  /// It represents the fraction of the year between the effective date and the maturity date.
	  /// Typically the value will be close to 1 for one year and close to 0.5 for six months.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public double YearFraction
	  {
		  get
		  {
			return yearFraction;
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
		  IborIndexObservation other = (IborIndexObservation) obj;
		  return JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(fixingDate, other.fixingDate) && JodaBeanUtils.equal(effectiveDate, other.effectiveDate) && JodaBeanUtils.equal(maturityDate, other.maturityDate) && JodaBeanUtils.equal(yearFraction, other.yearFraction);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixingDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(effectiveDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(maturityDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yearFraction);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("IborIndexObservation{");
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("fixingDate").Append('=').Append(fixingDate).Append(',').Append(' ');
		buf.Append("effectiveDate").Append('=').Append(effectiveDate).Append(',').Append(' ');
		buf.Append("maturityDate").Append('=').Append(maturityDate).Append(',').Append(' ');
		buf.Append("yearFraction").Append('=').Append(JodaBeanUtils.ToString(yearFraction));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborIndexObservation}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(IborIndexObservation), typeof(IborIndex));
			  fixingDate_Renamed = DirectMetaProperty.ofImmutable(this, "fixingDate", typeof(IborIndexObservation), typeof(LocalDate));
			  effectiveDate_Renamed = DirectMetaProperty.ofImmutable(this, "effectiveDate", typeof(IborIndexObservation), typeof(LocalDate));
			  maturityDate_Renamed = DirectMetaProperty.ofImmutable(this, "maturityDate", typeof(IborIndexObservation), typeof(LocalDate));
			  yearFraction_Renamed = DirectMetaProperty.ofImmutable(this, "yearFraction", typeof(IborIndexObservation), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "index", "fixingDate", "effectiveDate", "maturityDate", "yearFraction");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> fixingDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code effectiveDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> effectiveDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code maturityDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> maturityDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code yearFraction} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> yearFraction_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "index", "fixingDate", "effectiveDate", "maturityDate", "yearFraction");
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
			case -930389515: // effectiveDate
			  return effectiveDate_Renamed;
			case -414641441: // maturityDate
			  return maturityDate_Renamed;
			case -1731780257: // yearFraction
			  return yearFraction_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends IborIndexObservation> builder()
		public override BeanBuilder<IborIndexObservation> builder()
		{
		  return new IborIndexObservation.Builder();
		}

		public override Type beanType()
		{
		  return typeof(IborIndexObservation);
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
		public MetaProperty<IborIndex> index()
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
		/// The meta-property for the {@code effectiveDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> effectiveDate()
		{
		  return effectiveDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code maturityDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> maturityDate()
		{
		  return maturityDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code yearFraction} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> yearFraction()
		{
		  return yearFraction_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return ((IborIndexObservation) bean).Index;
			case 1255202043: // fixingDate
			  return ((IborIndexObservation) bean).FixingDate;
			case -930389515: // effectiveDate
			  return ((IborIndexObservation) bean).EffectiveDate;
			case -414641441: // maturityDate
			  return ((IborIndexObservation) bean).MaturityDate;
			case -1731780257: // yearFraction
			  return ((IborIndexObservation) bean).YearFraction;
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
	  /// The bean-builder for {@code IborIndexObservation}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<IborIndexObservation>
	  {

		internal IborIndex index;
		internal LocalDate fixingDate;
		internal LocalDate effectiveDate;
		internal LocalDate maturityDate;
		internal double yearFraction;

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
			case -930389515: // effectiveDate
			  return effectiveDate;
			case -414641441: // maturityDate
			  return maturityDate;
			case -1731780257: // yearFraction
			  return yearFraction;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  this.index = (IborIndex) newValue;
			  break;
			case 1255202043: // fixingDate
			  this.fixingDate = (LocalDate) newValue;
			  break;
			case -930389515: // effectiveDate
			  this.effectiveDate = (LocalDate) newValue;
			  break;
			case -414641441: // maturityDate
			  this.maturityDate = (LocalDate) newValue;
			  break;
			case -1731780257: // yearFraction
			  this.yearFraction = (double?) newValue.Value;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override IborIndexObservation build()
		{
		  return new IborIndexObservation(index, fixingDate, effectiveDate, maturityDate, yearFraction);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("IborIndexObservation.Builder{");
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index)).Append(',').Append(' ');
		  buf.Append("fixingDate").Append('=').Append(JodaBeanUtils.ToString(fixingDate)).Append(',').Append(' ');
		  buf.Append("effectiveDate").Append('=').Append(JodaBeanUtils.ToString(effectiveDate)).Append(',').Append(' ');
		  buf.Append("maturityDate").Append('=').Append(JodaBeanUtils.ToString(maturityDate)).Append(',').Append(' ');
		  buf.Append("yearFraction").Append('=').Append(JodaBeanUtils.ToString(yearFraction));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}