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
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// Information about a single observation of an Overnight index.
	/// <para>
	/// Observing an Overnight index requires knowledge of the index, fixing date,
	/// publication date, effective date and maturity date.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class OvernightIndexObservation implements IndexObservation, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class OvernightIndexObservation : IndexObservation, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final OvernightIndex index;
		private readonly OvernightIndex index;
	  /// <summary>
	  /// The date of the index fixing.
	  /// <para>
	  /// This is an adjusted date with any business day rule applied.
	  /// Valid business days are defined by <seealso cref="OvernightIndex#getFixingCalendar()"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate fixingDate;
	  private readonly LocalDate fixingDate;
	  /// <summary>
	  /// The date that the rate implied by the fixing date is published.
	  /// <para>
	  /// This is an adjusted date with any business day rule applied.
	  /// This must be equal to <seealso cref="OvernightIndex#calculatePublicationFromFixing(LocalDate, ReferenceData)"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate publicationDate;
	  private readonly LocalDate publicationDate;
	  /// <summary>
	  /// The effective date of the investment implied by the fixing date.
	  /// <para>
	  /// This is an adjusted date with any business day rule applied.
	  /// This must be equal to <seealso cref="OvernightIndex#calculateEffectiveFromFixing(LocalDate, ReferenceData)"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate effectiveDate;
	  private readonly LocalDate effectiveDate;
	  /// <summary>
	  /// The maturity date of the investment implied by the fixing date.
	  /// <para>
	  /// This is an adjusted date with any business day rule applied.
	  /// This must be equal to <seealso cref="OvernightIndex#calculateMaturityFromEffective(LocalDate, ReferenceData)"/>.
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
	  /// Creates an {@code IborRateObservation} from an index and fixing date.
	  /// <para>
	  /// The reference data is used to find the maturity date from the fixing date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="fixingDate">  the fixing date </param>
	  /// <param name="refData">  the reference data to use when resolving holiday calendars </param>
	  /// <returns> the rate observation </returns>
	  public static OvernightIndexObservation of(OvernightIndex index, LocalDate fixingDate, ReferenceData refData)
	  {
		LocalDate publicationDate = index.calculatePublicationFromFixing(fixingDate, refData);
		LocalDate effectiveDate = index.calculateEffectiveFromFixing(fixingDate, refData);
		LocalDate maturityDate = index.calculateMaturityFromEffective(effectiveDate, refData);
		return OvernightIndexObservation.builder().index(index).fixingDate(fixingDate).publicationDate(publicationDate).effectiveDate(effectiveDate).maturityDate(maturityDate).yearFraction(index.DayCount.yearFraction(effectiveDate, maturityDate)).build();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the Overnight index.
	  /// </summary>
	  /// <returns> the currency of the index </returns>
	  public Currency Currency
	  {
		  get
		  {
			return index.Currency;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compares this observation to another based on the index and fixing date.
	  /// <para>
	  /// The publication, effective and maturity dates are ignored.
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
		  OvernightIndexObservation other = (OvernightIndexObservation) obj;
		  return JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(fixingDate, other.fixingDate);
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
		return (new StringBuilder(64)).Append("OvernightIndexObservation[").Append(index).Append(" on ").Append(fixingDate).Append(']').ToString();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code OvernightIndexObservation}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static OvernightIndexObservation.Meta meta()
	  {
		return OvernightIndexObservation.Meta.INSTANCE;
	  }

	  static OvernightIndexObservation()
	  {
		MetaBean.register(OvernightIndexObservation.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static OvernightIndexObservation.Builder builder()
	  {
		return new OvernightIndexObservation.Builder();
	  }

	  private OvernightIndexObservation(OvernightIndex index, LocalDate fixingDate, LocalDate publicationDate, LocalDate effectiveDate, LocalDate maturityDate, double yearFraction)
	  {
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(fixingDate, "fixingDate");
		JodaBeanUtils.notNull(publicationDate, "publicationDate");
		JodaBeanUtils.notNull(effectiveDate, "effectiveDate");
		JodaBeanUtils.notNull(maturityDate, "maturityDate");
		JodaBeanUtils.notNull(yearFraction, "yearFraction");
		this.index = index;
		this.fixingDate = fixingDate;
		this.publicationDate = publicationDate;
		this.effectiveDate = effectiveDate;
		this.maturityDate = maturityDate;
		this.yearFraction = yearFraction;
	  }

	  public override OvernightIndexObservation.Meta metaBean()
	  {
		return OvernightIndexObservation.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Overnight index.
	  /// <para>
	  /// The rate will be queried from this index.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public OvernightIndex Index
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
	  /// Valid business days are defined by <seealso cref="OvernightIndex#getFixingCalendar()"/>.
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
	  /// Gets the date that the rate implied by the fixing date is published.
	  /// <para>
	  /// This is an adjusted date with any business day rule applied.
	  /// This must be equal to <seealso cref="OvernightIndex#calculatePublicationFromFixing(LocalDate, ReferenceData)"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate PublicationDate
	  {
		  get
		  {
			return publicationDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the effective date of the investment implied by the fixing date.
	  /// <para>
	  /// This is an adjusted date with any business day rule applied.
	  /// This must be equal to <seealso cref="OvernightIndex#calculateEffectiveFromFixing(LocalDate, ReferenceData)"/>.
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
	  /// This must be equal to <seealso cref="OvernightIndex#calculateMaturityFromEffective(LocalDate, ReferenceData)"/>.
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
	  /// <summary>
	  /// Returns a builder that allows this bean to be mutated. </summary>
	  /// <returns> the mutable builder, not null </returns>
	  public Builder toBuilder()
	  {
		return new Builder(this);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code OvernightIndexObservation}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(OvernightIndexObservation), typeof(OvernightIndex));
			  fixingDate_Renamed = DirectMetaProperty.ofImmutable(this, "fixingDate", typeof(OvernightIndexObservation), typeof(LocalDate));
			  publicationDate_Renamed = DirectMetaProperty.ofImmutable(this, "publicationDate", typeof(OvernightIndexObservation), typeof(LocalDate));
			  effectiveDate_Renamed = DirectMetaProperty.ofImmutable(this, "effectiveDate", typeof(OvernightIndexObservation), typeof(LocalDate));
			  maturityDate_Renamed = DirectMetaProperty.ofImmutable(this, "maturityDate", typeof(OvernightIndexObservation), typeof(LocalDate));
			  yearFraction_Renamed = DirectMetaProperty.ofImmutable(this, "yearFraction", typeof(OvernightIndexObservation), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "index", "fixingDate", "publicationDate", "effectiveDate", "maturityDate", "yearFraction");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<OvernightIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> fixingDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code publicationDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> publicationDate_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "index", "fixingDate", "publicationDate", "effectiveDate", "maturityDate", "yearFraction");
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
			case 1470566394: // publicationDate
			  return publicationDate_Renamed;
			case -930389515: // effectiveDate
			  return effectiveDate_Renamed;
			case -414641441: // maturityDate
			  return maturityDate_Renamed;
			case -1731780257: // yearFraction
			  return yearFraction_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override OvernightIndexObservation.Builder builder()
		{
		  return new OvernightIndexObservation.Builder();
		}

		public override Type beanType()
		{
		  return typeof(OvernightIndexObservation);
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
		public MetaProperty<OvernightIndex> index()
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
		/// The meta-property for the {@code publicationDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> publicationDate()
		{
		  return publicationDate_Renamed;
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
			  return ((OvernightIndexObservation) bean).Index;
			case 1255202043: // fixingDate
			  return ((OvernightIndexObservation) bean).FixingDate;
			case 1470566394: // publicationDate
			  return ((OvernightIndexObservation) bean).PublicationDate;
			case -930389515: // effectiveDate
			  return ((OvernightIndexObservation) bean).EffectiveDate;
			case -414641441: // maturityDate
			  return ((OvernightIndexObservation) bean).MaturityDate;
			case -1731780257: // yearFraction
			  return ((OvernightIndexObservation) bean).YearFraction;
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
	  /// The bean-builder for {@code OvernightIndexObservation}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<OvernightIndexObservation>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal OvernightIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate fixingDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate publicationDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate effectiveDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate maturityDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double yearFraction_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(OvernightIndexObservation beanToCopy)
		{
		  this.index_Renamed = beanToCopy.Index;
		  this.fixingDate_Renamed = beanToCopy.FixingDate;
		  this.publicationDate_Renamed = beanToCopy.PublicationDate;
		  this.effectiveDate_Renamed = beanToCopy.EffectiveDate;
		  this.maturityDate_Renamed = beanToCopy.MaturityDate;
		  this.yearFraction_Renamed = beanToCopy.YearFraction;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return index_Renamed;
			case 1255202043: // fixingDate
			  return fixingDate_Renamed;
			case 1470566394: // publicationDate
			  return publicationDate_Renamed;
			case -930389515: // effectiveDate
			  return effectiveDate_Renamed;
			case -414641441: // maturityDate
			  return maturityDate_Renamed;
			case -1731780257: // yearFraction
			  return yearFraction_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  this.index_Renamed = (OvernightIndex) newValue;
			  break;
			case 1255202043: // fixingDate
			  this.fixingDate_Renamed = (LocalDate) newValue;
			  break;
			case 1470566394: // publicationDate
			  this.publicationDate_Renamed = (LocalDate) newValue;
			  break;
			case -930389515: // effectiveDate
			  this.effectiveDate_Renamed = (LocalDate) newValue;
			  break;
			case -414641441: // maturityDate
			  this.maturityDate_Renamed = (LocalDate) newValue;
			  break;
			case -1731780257: // yearFraction
			  this.yearFraction_Renamed = (double?) newValue.Value;
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

		public override OvernightIndexObservation build()
		{
		  return new OvernightIndexObservation(index_Renamed, fixingDate_Renamed, publicationDate_Renamed, effectiveDate_Renamed, maturityDate_Renamed, yearFraction_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the Overnight index.
		/// <para>
		/// The rate will be queried from this index.
		/// </para>
		/// </summary>
		/// <param name="index">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder index(OvernightIndex index)
		{
		  JodaBeanUtils.notNull(index, "index");
		  this.index_Renamed = index;
		  return this;
		}

		/// <summary>
		/// Sets the date of the index fixing.
		/// <para>
		/// This is an adjusted date with any business day rule applied.
		/// Valid business days are defined by <seealso cref="OvernightIndex#getFixingCalendar()"/>.
		/// </para>
		/// </summary>
		/// <param name="fixingDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingDate(LocalDate fixingDate)
		{
		  JodaBeanUtils.notNull(fixingDate, "fixingDate");
		  this.fixingDate_Renamed = fixingDate;
		  return this;
		}

		/// <summary>
		/// Sets the date that the rate implied by the fixing date is published.
		/// <para>
		/// This is an adjusted date with any business day rule applied.
		/// This must be equal to <seealso cref="OvernightIndex#calculatePublicationFromFixing(LocalDate, ReferenceData)"/>.
		/// </para>
		/// </summary>
		/// <param name="publicationDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder publicationDate(LocalDate publicationDate)
		{
		  JodaBeanUtils.notNull(publicationDate, "publicationDate");
		  this.publicationDate_Renamed = publicationDate;
		  return this;
		}

		/// <summary>
		/// Sets the effective date of the investment implied by the fixing date.
		/// <para>
		/// This is an adjusted date with any business day rule applied.
		/// This must be equal to <seealso cref="OvernightIndex#calculateEffectiveFromFixing(LocalDate, ReferenceData)"/>.
		/// </para>
		/// </summary>
		/// <param name="effectiveDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder effectiveDate(LocalDate effectiveDate)
		{
		  JodaBeanUtils.notNull(effectiveDate, "effectiveDate");
		  this.effectiveDate_Renamed = effectiveDate;
		  return this;
		}

		/// <summary>
		/// Sets the maturity date of the investment implied by the fixing date.
		/// <para>
		/// This is an adjusted date with any business day rule applied.
		/// This must be equal to <seealso cref="OvernightIndex#calculateMaturityFromEffective(LocalDate, ReferenceData)"/>.
		/// </para>
		/// </summary>
		/// <param name="maturityDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder maturityDate(LocalDate maturityDate)
		{
		  JodaBeanUtils.notNull(maturityDate, "maturityDate");
		  this.maturityDate_Renamed = maturityDate;
		  return this;
		}

		/// <summary>
		/// Sets the year fraction of the investment implied by the fixing date.
		/// <para>
		/// This is calculated using the day count of the index.
		/// It represents the fraction of the year between the effective date and the maturity date.
		/// Typically the value will be close to 1 for one year and close to 0.5 for six months.
		/// </para>
		/// </summary>
		/// <param name="yearFraction">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder yearFraction(double yearFraction)
		{
		  JodaBeanUtils.notNull(yearFraction, "yearFraction");
		  this.yearFraction_Renamed = yearFraction;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(224);
		  buf.Append("OvernightIndexObservation.Builder{");
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingDate").Append('=').Append(JodaBeanUtils.ToString(fixingDate_Renamed)).Append(',').Append(' ');
		  buf.Append("publicationDate").Append('=').Append(JodaBeanUtils.ToString(publicationDate_Renamed)).Append(',').Append(' ');
		  buf.Append("effectiveDate").Append('=').Append(JodaBeanUtils.ToString(effectiveDate_Renamed)).Append(',').Append(' ');
		  buf.Append("maturityDate").Append('=').Append(JodaBeanUtils.ToString(maturityDate_Renamed)).Append(',').Append(' ');
		  buf.Append("yearFraction").Append('=').Append(JodaBeanUtils.ToString(yearFraction_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}