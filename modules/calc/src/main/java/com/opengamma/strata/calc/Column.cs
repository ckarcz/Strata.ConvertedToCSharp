using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;

	/// <summary>
	/// Defines a column in a set of calculation results.
	/// <para>
	/// <seealso cref="CalculationRunner"/> provides the ability to calculate a grid of results
	/// for a given set targets and columns. This class is used to define the columns.
	/// </para>
	/// <para>
	/// A column is defined in terms of a unique name, measure to be calculated and
	/// a set of parameters that control the calculation. The functions to invoke
	/// and the default set of parameters are defined on <seealso cref="CalculationRules"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class Column implements org.joda.beans.ImmutableBean
	public sealed class Column : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ColumnName name;
		private readonly ColumnName name;
	  /// <summary>
	  /// The measure to be calculated.
	  /// <para>
	  /// This defines the calculation being performed, such as 'PresentValue' or 'ParRate'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final Measure measure;
	  private readonly Measure measure;
	  /// <summary>
	  /// The reporting currency, used to control currency conversion, optional.
	  /// <para>
	  /// This is used to specify the currency that the result should be reporting in.
	  /// If the result is not associated with a currency, such as for "par rate", then the
	  /// reporting currency will effectively be ignored.
	  /// </para>
	  /// <para>
	  /// If empty, the reporting currency from <seealso cref="CalculationRules"/> will be used.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final ReportingCurrency reportingCurrency;
	  private readonly ReportingCurrency reportingCurrency;
	  /// <summary>
	  /// The calculation parameters that apply to this column, used to control the how the calculation is performed.
	  /// <para>
	  /// The parameters from <seealso cref="CalculationRules"/> and {@code Column} are combined.
	  /// If a parameter is defined here and in the rules with the same
	  /// <seealso cref="CalculationParameter#queryType() query type"/>, then the column parameter takes precedence.
	  /// </para>
	  /// <para>
	  /// When building, these will default to be empty.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final com.opengamma.strata.calc.runner.CalculationParameters parameters;
	  private readonly CalculationParameters parameters;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance that will calculate the specified measure.
	  /// <para>
	  /// The column name will be the same as the name of the measure.
	  /// No calculation parameters are provided, thus the parameters from <seealso cref="CalculationRules"/> will be used.
	  /// Currency conversion is controlled by the reporting currency in {@code CalculationRules}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="measure">  the measure to be calculated </param>
	  /// <returns> a column with the specified measure </returns>
	  public static Column of(Measure measure)
	  {
		ColumnName name = ColumnName.of(measure);
		return new Column(name, measure, null, CalculationParameters.empty());
	  }

	  /// <summary>
	  /// Obtains an instance that will calculate the specified measure, converting to the specified currency.
	  /// <para>
	  /// The column name will be the same as the name of the measure.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="measure">  the measure to be calculated </param>
	  /// <param name="currency">  the currency to convert to </param>
	  /// <returns> a column with the specified measure </returns>
	  public static Column of(Measure measure, Currency currency)
	  {
		ColumnName name = ColumnName.of(measure);
		return new Column(name, measure, ReportingCurrency.of(currency), CalculationParameters.empty());
	  }

	  /// <summary>
	  /// Obtains an instance that will calculate the specified measure, defining additional parameters.
	  /// <para>
	  /// The column name will be the same as the name of the measure.
	  /// The specified calculation parameters take precedence over those in <seealso cref="CalculationRules"/>,
	  /// with the combined set being used for the column.
	  /// Currency conversion is controlled by the reporting currency in {@code CalculationRules}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="measure">  the measure to be calculated </param>
	  /// <param name="parameters">  the parameters that control the calculation, may be empty </param>
	  /// <returns> a column with the specified measure and reporting currency </returns>
	  public static Column of(Measure measure, params CalculationParameter[] parameters)
	  {
		ColumnName name = ColumnName.of(measure);
		return new Column(name, measure, null, CalculationParameters.of(parameters));
	  }

	  /// <summary>
	  /// Obtains an instance that will calculate the specified measure, converting to the specified currency,
	  /// defining additional parameters.
	  /// <para>
	  /// The column name will be the same as the name of the measure.
	  /// The specified calculation parameters take precedence over those in <seealso cref="CalculationRules"/>,
	  /// with the combined set being used for the column.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="measure">  the measure to be calculated </param>
	  /// <param name="currency">  the currency to convert to </param>
	  /// <param name="parameters">  the parameters that control the calculation, may be empty </param>
	  /// <returns> a column with the specified measure and reporting currency </returns>
	  public static Column of(Measure measure, Currency currency, params CalculationParameter[] parameters)
	  {
		ColumnName name = ColumnName.of(measure);
		return new Column(name, measure, ReportingCurrency.of(currency), CalculationParameters.of(parameters));
	  }

	  /// <summary>
	  /// Obtains an instance that will calculate the specified measure, defining the column name.
	  /// <para>
	  /// No calculation parameters are provided, thus the parameters from <seealso cref="CalculationRules"/> will be used.
	  /// Currency conversion is controlled by the reporting currency in {@code CalculationRules}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="measure">  the measure to be calculated </param>
	  /// <param name="columnName">  the column name </param>
	  /// <returns> a column with the specified measure and column name </returns>
	  public static Column of(Measure measure, string columnName)
	  {
		ColumnName name = ColumnName.of(columnName);
		return new Column(name, measure, null, CalculationParameters.empty());
	  }

	  /// <summary>
	  /// Obtains an instance that will calculate the specified measure, converting to the specified currency.
	  /// <para>
	  /// The specified currency will be wrapped in <seealso cref="ReportingCurrency"/> and added to the calculation parameters.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="measure">  the measure to be calculated </param>
	  /// <param name="columnName">  the column name </param>
	  /// <param name="currency">  the currency to convert to </param>
	  /// <returns> a column with the specified measure </returns>
	  public static Column of(Measure measure, string columnName, Currency currency)
	  {
		ColumnName name = ColumnName.of(columnName);
		return new Column(name, measure, ReportingCurrency.of(currency), CalculationParameters.empty());
	  }

	  /// <summary>
	  /// Obtains an instance that will calculate the specified measure, defining the column name and parameters.
	  /// <para>
	  /// The specified calculation parameters take precedence over those in <seealso cref="CalculationRules"/>,
	  /// with the combined set being used for the column.
	  /// Currency conversion is controlled by the reporting currency in {@code CalculationRules}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="measure">  the measure to be calculated </param>
	  /// <param name="columnName">  the column name </param>
	  /// <param name="parameters">  the parameters that control the calculation, may be empty </param>
	  /// <returns> a column with the specified measure, column name and reporting currency </returns>
	  public static Column of(Measure measure, string columnName, params CalculationParameter[] parameters)
	  {

		ColumnName name = ColumnName.of(columnName);
		return new Column(name, measure, null, CalculationParameters.of(parameters));
	  }

	  /// <summary>
	  /// Obtains an instance that will calculate the specified measure, converting to the specified currency,
	  /// defining the column name and parameters.
	  /// <para>
	  /// The specified calculation parameters take precedence over those in <seealso cref="CalculationRules"/>,
	  /// with the combined set being used for the column.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="measure">  the measure to be calculated </param>
	  /// <param name="columnName">  the column name </param>
	  /// <param name="currency">  the currency to convert to </param>
	  /// <param name="parameters">  the parameters that control the calculation, may be empty </param>
	  /// <returns> a column with the specified measure, column name and reporting currency </returns>
	  public static Column of(Measure measure, string columnName, Currency currency, params CalculationParameter[] parameters)
	  {

		ColumnName name = ColumnName.of(columnName);
		return new Column(name, measure, ReportingCurrency.of(currency), CalculationParameters.of(parameters));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.parameters(CalculationParameters.empty());
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.name_Renamed == null && builder.measure_Renamed != null)
		{
		  builder.name(ColumnName.of(builder.measure_Renamed.Name));
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Combines the parameters with another reporting currency and set of parameters.
	  /// </summary>
	  /// <param name="reportingCurrency">  the default reporting currency </param>
	  /// <param name="defaultParameters">  the default parameters </param>
	  /// <returns> the combined column </returns>
	  public Column combineWithDefaults(ReportingCurrency reportingCurrency, CalculationParameters defaultParameters)
	  {
		CalculationParameters combinedParams = parameters.combinedWith(defaultParameters);
		return new Column(name, measure, ReportingCurrency.orElse(reportingCurrency), combinedParams);
	  }

	  /// <summary>
	  /// Converts this column to a column header.
	  /// <para>
	  /// The header is a reduced form of the column used in <seealso cref="Results"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the column header </returns>
	  public ColumnHeader toHeader()
	  {
		if (measure.CurrencyConvertible)
		{
		  ReportingCurrency reportingCurrency = ReportingCurrency.orElse(ReportingCurrency.NATURAL);
		  if (reportingCurrency.Specific)
		  {
			return ColumnHeader.of(name, measure, reportingCurrency.Currency);
		  }
		}
		return ColumnHeader.of(name, measure);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code Column}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static Column.Meta meta()
	  {
		return Column.Meta.INSTANCE;
	  }

	  static Column()
	  {
		MetaBean.register(Column.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static Column.Builder builder()
	  {
		return new Column.Builder();
	  }

	  private Column(ColumnName name, Measure measure, ReportingCurrency reportingCurrency, CalculationParameters parameters)
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(measure, "measure");
		this.name = name;
		this.measure = measure;
		this.reportingCurrency = reportingCurrency;
		this.parameters = parameters;
	  }

	  public override Column.Meta metaBean()
	  {
		return Column.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the column name.
	  /// <para>
	  /// This is the name of the column, and should be unique in a list of columns.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ColumnName Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the measure to be calculated.
	  /// <para>
	  /// This defines the calculation being performed, such as 'PresentValue' or 'ParRate'.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Measure Measure
	  {
		  get
		  {
			return measure;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the reporting currency, used to control currency conversion, optional.
	  /// <para>
	  /// This is used to specify the currency that the result should be reporting in.
	  /// If the result is not associated with a currency, such as for "par rate", then the
	  /// reporting currency will effectively be ignored.
	  /// </para>
	  /// <para>
	  /// If empty, the reporting currency from <seealso cref="CalculationRules"/> will be used.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<ReportingCurrency> ReportingCurrency
	  {
		  get
		  {
			return Optional.ofNullable(reportingCurrency);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the calculation parameters that apply to this column, used to control the how the calculation is performed.
	  /// <para>
	  /// The parameters from <seealso cref="CalculationRules"/> and {@code Column} are combined.
	  /// If a parameter is defined here and in the rules with the same
	  /// <seealso cref="CalculationParameter#queryType() query type"/>, then the column parameter takes precedence.
	  /// </para>
	  /// <para>
	  /// When building, these will default to be empty.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public CalculationParameters Parameters
	  {
		  get
		  {
			return parameters;
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
		  Column other = (Column) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(measure, other.measure) && JodaBeanUtils.equal(reportingCurrency, other.reportingCurrency) && JodaBeanUtils.equal(parameters, other.parameters);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(measure);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(reportingCurrency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(parameters);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("Column{");
		buf.Append("name").Append('=').Append(name).Append(',').Append(' ');
		buf.Append("measure").Append('=').Append(measure).Append(',').Append(' ');
		buf.Append("reportingCurrency").Append('=').Append(reportingCurrency).Append(',').Append(' ');
		buf.Append("parameters").Append('=').Append(JodaBeanUtils.ToString(parameters));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code Column}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(Column), typeof(ColumnName));
			  measure_Renamed = DirectMetaProperty.ofImmutable(this, "measure", typeof(Column), typeof(Measure));
			  reportingCurrency_Renamed = DirectMetaProperty.ofImmutable(this, "reportingCurrency", typeof(Column), typeof(ReportingCurrency));
			  parameters_Renamed = DirectMetaProperty.ofImmutable(this, "parameters", typeof(Column), typeof(CalculationParameters));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "measure", "reportingCurrency", "parameters");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ColumnName> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code measure} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Measure> measure_Renamed;
		/// <summary>
		/// The meta-property for the {@code reportingCurrency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ReportingCurrency> reportingCurrency_Renamed;
		/// <summary>
		/// The meta-property for the {@code parameters} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CalculationParameters> parameters_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "measure", "reportingCurrency", "parameters");
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
			case 3373707: // name
			  return name_Renamed;
			case 938321246: // measure
			  return measure_Renamed;
			case -1287844769: // reportingCurrency
			  return reportingCurrency_Renamed;
			case 458736106: // parameters
			  return parameters_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override Column.Builder builder()
		{
		  return new Column.Builder();
		}

		public override Type beanType()
		{
		  return typeof(Column);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code name} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ColumnName> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code measure} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Measure> measure()
		{
		  return measure_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code reportingCurrency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ReportingCurrency> reportingCurrency()
		{
		  return reportingCurrency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code parameters} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CalculationParameters> parameters()
		{
		  return parameters_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((Column) bean).Name;
			case 938321246: // measure
			  return ((Column) bean).Measure;
			case -1287844769: // reportingCurrency
			  return ((Column) bean).reportingCurrency;
			case 458736106: // parameters
			  return ((Column) bean).Parameters;
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
	  /// The bean-builder for {@code Column}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<Column>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ColumnName name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Measure measure_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ReportingCurrency reportingCurrency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CalculationParameters parameters_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		  applyDefaults(this);
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(Column beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.measure_Renamed = beanToCopy.Measure;
		  this.reportingCurrency_Renamed = beanToCopy.reportingCurrency;
		  this.parameters_Renamed = beanToCopy.Parameters;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case 938321246: // measure
			  return measure_Renamed;
			case -1287844769: // reportingCurrency
			  return reportingCurrency_Renamed;
			case 458736106: // parameters
			  return parameters_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  this.name_Renamed = (ColumnName) newValue;
			  break;
			case 938321246: // measure
			  this.measure_Renamed = (Measure) newValue;
			  break;
			case -1287844769: // reportingCurrency
			  this.reportingCurrency_Renamed = (ReportingCurrency) newValue;
			  break;
			case 458736106: // parameters
			  this.parameters_Renamed = (CalculationParameters) newValue;
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

		public override Column build()
		{
		  preBuild(this);
		  return new Column(name_Renamed, measure_Renamed, reportingCurrency_Renamed, parameters_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the column name.
		/// <para>
		/// This is the name of the column, and should be unique in a list of columns.
		/// </para>
		/// </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(ColumnName name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the measure to be calculated.
		/// <para>
		/// This defines the calculation being performed, such as 'PresentValue' or 'ParRate'.
		/// </para>
		/// </summary>
		/// <param name="measure">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder measure(Measure measure)
		{
		  JodaBeanUtils.notNull(measure, "measure");
		  this.measure_Renamed = measure;
		  return this;
		}

		/// <summary>
		/// Sets the reporting currency, used to control currency conversion, optional.
		/// <para>
		/// This is used to specify the currency that the result should be reporting in.
		/// If the result is not associated with a currency, such as for "par rate", then the
		/// reporting currency will effectively be ignored.
		/// </para>
		/// <para>
		/// If empty, the reporting currency from <seealso cref="CalculationRules"/> will be used.
		/// </para>
		/// </summary>
		/// <param name="reportingCurrency">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder reportingCurrency(ReportingCurrency reportingCurrency)
		{
		  this.reportingCurrency_Renamed = reportingCurrency;
		  return this;
		}

		/// <summary>
		/// Sets the calculation parameters that apply to this column, used to control the how the calculation is performed.
		/// <para>
		/// The parameters from <seealso cref="CalculationRules"/> and {@code Column} are combined.
		/// If a parameter is defined here and in the rules with the same
		/// <seealso cref="CalculationParameter#queryType() query type"/>, then the column parameter takes precedence.
		/// </para>
		/// <para>
		/// When building, these will default to be empty.
		/// </para>
		/// </summary>
		/// <param name="parameters">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder parameters(CalculationParameters parameters)
		{
		  this.parameters_Renamed = parameters;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("Column.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("measure").Append('=').Append(JodaBeanUtils.ToString(measure_Renamed)).Append(',').Append(' ');
		  buf.Append("reportingCurrency").Append('=').Append(JodaBeanUtils.ToString(reportingCurrency_Renamed)).Append(',').Append(' ');
		  buf.Append("parameters").Append('=').Append(JodaBeanUtils.ToString(parameters_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}