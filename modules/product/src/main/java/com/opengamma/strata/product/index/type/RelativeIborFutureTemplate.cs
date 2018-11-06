using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index.type
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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// A template for creating an Ibor Future trade using a relative definition of time.
	/// <para>
	/// The future is selected based on a minimum period and a sequence number.
	/// Given a date, the minimum period is added, and then a futures contract is selected
	/// according the sequence number.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") final class RelativeIborFutureTemplate implements IborFutureTemplate, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class RelativeIborFutureTemplate : IborFutureTemplate, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.Period minimumPeriod;
		private readonly Period minimumPeriod;
	  /// <summary>
	  /// The sequence number of the futures.
	  /// <para>
	  /// This is used to select the nth future after the value date.
	  /// For example, the 2nd future of the series where the 1st future is at least 1 week after the value date
	  /// would be represented by a minimum period of 1 week and sequence number 2.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegativeOrZero") private final int sequenceNumber;
	  private readonly int sequenceNumber;
	  /// <summary>
	  /// The underlying futures convention.
	  /// <para>
	  /// This specifies the market convention of the Ibor Futures to be created.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final IborFutureConvention convention;
	  private readonly IborFutureConvention convention;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a template based on the specified convention.
	  /// <para>
	  /// The specific future is defined by two date-related inputs, the minimum period and the 1-based future number.
	  /// For example, the 2nd future of the series where the 1st future is at least 1 week after the value date
	  /// would be represented by a minimum period of 1 week and future number 2.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="minimumPeriod">  the minimum period between the base date and the first future </param>
	  /// <param name="sequenceNumber">  the 1-based index of the future after the minimum period, must be 1 or greater </param>
	  /// <param name="convention">  the future convention </param>
	  /// <returns> the template </returns>
	  public static RelativeIborFutureTemplate of(Period minimumPeriod, int sequenceNumber, IborFutureConvention convention)
	  {
		return new RelativeIborFutureTemplate(minimumPeriod, sequenceNumber, convention);
	  }

	  //-------------------------------------------------------------------------
	  public IborIndex Index
	  {
		  get
		  {
			return convention.Index;
		  }
	  }

	  public IborFutureTrade createTrade(LocalDate tradeDate, SecurityId securityId, double quantity, double notional, double price, ReferenceData refData)
	  {

		return convention.createTrade(tradeDate, securityId, minimumPeriod, sequenceNumber, quantity, notional, price, refData);
	  }

	  public LocalDate calculateReferenceDateFromTradeDate(LocalDate tradeDate, ReferenceData refData)
	  {
		return convention.calculateReferenceDateFromTradeDate(tradeDate, minimumPeriod, sequenceNumber, refData);
	  }

	  public double approximateMaturity(LocalDate valuationDate)
	  {
		return minimumPeriod.plus(convention.Index.Tenor).toTotalMonths() / 12d;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code RelativeIborFutureTemplate}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static RelativeIborFutureTemplate.Meta meta()
	  {
		return RelativeIborFutureTemplate.Meta.INSTANCE;
	  }

	  static RelativeIborFutureTemplate()
	  {
		MetaBean.register(RelativeIborFutureTemplate.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private RelativeIborFutureTemplate(Period minimumPeriod, int sequenceNumber, IborFutureConvention convention)
	  {
		JodaBeanUtils.notNull(minimumPeriod, "minimumPeriod");
		ArgChecker.notNegativeOrZero(sequenceNumber, "sequenceNumber");
		JodaBeanUtils.notNull(convention, "convention");
		this.minimumPeriod = minimumPeriod;
		this.sequenceNumber = sequenceNumber;
		this.convention = convention;
	  }

	  public override RelativeIborFutureTemplate.Meta metaBean()
	  {
		return RelativeIborFutureTemplate.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the minimum period between the value date and the first future.
	  /// <para>
	  /// This is used to select a future that is at least this period of time after the value date.
	  /// For example, the 2nd future of the series where the 1st future is at least 1 week after the value date
	  /// would be represented by a minimum period of 1 week and sequence number 2.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Period MinimumPeriod
	  {
		  get
		  {
			return minimumPeriod;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the sequence number of the futures.
	  /// <para>
	  /// This is used to select the nth future after the value date.
	  /// For example, the 2nd future of the series where the 1st future is at least 1 week after the value date
	  /// would be represented by a minimum period of 1 week and sequence number 2.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public int SequenceNumber
	  {
		  get
		  {
			return sequenceNumber;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying futures convention.
	  /// <para>
	  /// This specifies the market convention of the Ibor Futures to be created.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborFutureConvention Convention
	  {
		  get
		  {
			return convention;
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
		  RelativeIborFutureTemplate other = (RelativeIborFutureTemplate) obj;
		  return JodaBeanUtils.equal(minimumPeriod, other.minimumPeriod) && (sequenceNumber == other.sequenceNumber) && JodaBeanUtils.equal(convention, other.convention);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(minimumPeriod);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sequenceNumber);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(convention);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("RelativeIborFutureTemplate{");
		buf.Append("minimumPeriod").Append('=').Append(minimumPeriod).Append(',').Append(' ');
		buf.Append("sequenceNumber").Append('=').Append(sequenceNumber).Append(',').Append(' ');
		buf.Append("convention").Append('=').Append(JodaBeanUtils.ToString(convention));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code RelativeIborFutureTemplate}.
	  /// </summary>
	  internal sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  minimumPeriod_Renamed = DirectMetaProperty.ofImmutable(this, "minimumPeriod", typeof(RelativeIborFutureTemplate), typeof(Period));
			  sequenceNumber_Renamed = DirectMetaProperty.ofImmutable(this, "sequenceNumber", typeof(RelativeIborFutureTemplate), Integer.TYPE);
			  convention_Renamed = DirectMetaProperty.ofImmutable(this, "convention", typeof(RelativeIborFutureTemplate), typeof(IborFutureConvention));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "minimumPeriod", "sequenceNumber", "convention");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code minimumPeriod} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Period> minimumPeriod_Renamed;
		/// <summary>
		/// The meta-property for the {@code sequenceNumber} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> sequenceNumber_Renamed;
		/// <summary>
		/// The meta-property for the {@code convention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborFutureConvention> convention_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "minimumPeriod", "sequenceNumber", "convention");
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
			case -1855508625: // minimumPeriod
			  return minimumPeriod_Renamed;
			case -1353995670: // sequenceNumber
			  return sequenceNumber_Renamed;
			case 2039569265: // convention
			  return convention_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends RelativeIborFutureTemplate> builder()
		public override BeanBuilder<RelativeIborFutureTemplate> builder()
		{
		  return new RelativeIborFutureTemplate.Builder();
		}

		public override Type beanType()
		{
		  return typeof(RelativeIborFutureTemplate);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code minimumPeriod} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Period> minimumPeriod()
		{
		  return minimumPeriod_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code sequenceNumber} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> sequenceNumber()
		{
		  return sequenceNumber_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code convention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborFutureConvention> convention()
		{
		  return convention_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1855508625: // minimumPeriod
			  return ((RelativeIborFutureTemplate) bean).MinimumPeriod;
			case -1353995670: // sequenceNumber
			  return ((RelativeIborFutureTemplate) bean).SequenceNumber;
			case 2039569265: // convention
			  return ((RelativeIborFutureTemplate) bean).Convention;
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
	  /// The bean-builder for {@code RelativeIborFutureTemplate}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<RelativeIborFutureTemplate>
	  {

		internal Period minimumPeriod;
		internal int sequenceNumber;
		internal IborFutureConvention convention;

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
			case -1855508625: // minimumPeriod
			  return minimumPeriod;
			case -1353995670: // sequenceNumber
			  return sequenceNumber;
			case 2039569265: // convention
			  return convention;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1855508625: // minimumPeriod
			  this.minimumPeriod = (Period) newValue;
			  break;
			case -1353995670: // sequenceNumber
			  this.sequenceNumber = (int?) newValue.Value;
			  break;
			case 2039569265: // convention
			  this.convention = (IborFutureConvention) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override RelativeIborFutureTemplate build()
		{
		  return new RelativeIborFutureTemplate(minimumPeriod, sequenceNumber, convention);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("RelativeIborFutureTemplate.Builder{");
		  buf.Append("minimumPeriod").Append('=').Append(JodaBeanUtils.ToString(minimumPeriod)).Append(',').Append(' ');
		  buf.Append("sequenceNumber").Append('=').Append(JodaBeanUtils.ToString(sequenceNumber)).Append(',').Append(' ');
		  buf.Append("convention").Append('=').Append(JodaBeanUtils.ToString(convention));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}