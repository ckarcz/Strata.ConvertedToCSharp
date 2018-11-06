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
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CalculationFunction = com.opengamma.strata.calc.runner.CalculationFunction;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;

	/// <summary>
	/// A set of rules that define how the calculation runner should perform calculations.
	/// <para>
	/// <seealso cref="CalculationRunner"/> provides the ability to perform calculations on many targets,
	/// such as trades and positions. It returns a grid of results, with the targets as rows.
	/// Each individual calculation is controlled by three things:
	/// <ul>
	///   <li>The <seealso cref="CalculationFunction function"/>, selected by the target type</li>
	///   <li>The <seealso cref="Measure measure"/>, the high-level output to be calculated</li>
	///   <li>The <seealso cref="CalculationParameters parameters"/>, adjust how the measure is to be calculated</li>
	/// </ul>
	/// {@code CalculationRules} operates in association with <seealso cref="Column"/>.
	/// The column is used to define the measure. It can also be used to specify column-specific parameters.
	/// The rules contain the complete set of functions and the default set of parameters.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class CalculationRules implements org.joda.beans.ImmutableBean
	public sealed class CalculationRules : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.calc.runner.CalculationFunctions functions;
		private readonly CalculationFunctions functions;
	  /// <summary>
	  /// The reporting currency, used to control currency conversion.
	  /// <para>
	  /// This is used to specify the currency that the result should be reporting in.
	  /// If the result is not associated with a currency, such as for "par rate", then the
	  /// reporting currency will effectively be ignored.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ReportingCurrency reportingCurrency;
	  private readonly ReportingCurrency reportingCurrency;
	  /// <summary>
	  /// The calculation parameters, used to control the how the calculation is performed.
	  /// <para>
	  /// Parameters are used to parameterize the <seealso cref="Measure"/> to be calculated.
	  /// They may be specified in two places - here and in the <seealso cref="Column"/>.
	  /// The parameters specified here are the defaults that apply to all columns.
	  /// </para>
	  /// <para>
	  /// If a parameter is defined here and in the column with the same
	  /// <seealso cref="CalculationParameter#queryType() query type"/>, then the column parameter takes precedence.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.calc.runner.CalculationParameters parameters;
	  private readonly CalculationParameters parameters;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance specifying the functions to use and some additional parameters.
	  /// <para>
	  /// The output will uses the "natural" <seealso cref="ReportingCurrency reporting currency"/>.
	  /// Most functions require a parameter to control their behavior, such as {@code RatesMarketDataLookup}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="functions">  the calculation functions </param>
	  /// <param name="parameters">  the parameters that control the calculation, may be empty </param>
	  /// <returns> the rules </returns>
	  public static CalculationRules of(CalculationFunctions functions, params CalculationParameter[] parameters)
	  {
		CalculationParameters @params = CalculationParameters.of(parameters);
		return new CalculationRules(functions, ReportingCurrency.NATURAL, @params);
	  }

	  /// <summary>
	  /// Obtains an instance specifying the functions to use and some additional parameters.
	  /// <para>
	  /// The output will uses the "natural" <seealso cref="ReportingCurrency reporting currency"/>.
	  /// Most functions require a parameter to control their behavior, such as {@code RatesMarketDataLookup}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="functions">  the calculation functions </param>
	  /// <param name="parameters">  the parameters that control the calculation, may be empty </param>
	  /// <returns> the rules </returns>
	  public static CalculationRules of(CalculationFunctions functions, CalculationParameters parameters)
	  {
		return new CalculationRules(functions, ReportingCurrency.NATURAL, parameters);
	  }

	  /// <summary>
	  /// Obtains an instance specifying the functions, reporting currency and additional parameters.
	  /// <para>
	  /// Most functions require a parameter to control their behavior, such as {@code RatesMarketDataLookup}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="functions">  the calculation functions </param>
	  /// <param name="reportingCurrency">  the reporting currency </param>
	  /// <param name="parameters">  the parameters that control the calculation, may be empty </param>
	  /// <returns> the rules </returns>
	  public static CalculationRules of(CalculationFunctions functions, Currency reportingCurrency, params CalculationParameter[] parameters)
	  {

		CalculationParameters @params = CalculationParameters.of(parameters);
		return new CalculationRules(functions, ReportingCurrency.of(reportingCurrency), @params);
	  }

	  /// <summary>
	  /// Obtains an instance specifying the functions, reporting currency and additional parameters.
	  /// <para>
	  /// Most functions require a parameter to control their behavior, such as {@code RatesMarketDataLookup}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="functions">  the calculation functions </param>
	  /// <param name="reportingCurrency">  the reporting currency </param>
	  /// <param name="parameters">  the parameters that control the calculation, may be empty </param>
	  /// <returns> the rules </returns>
	  public static CalculationRules of(CalculationFunctions functions, ReportingCurrency reportingCurrency, CalculationParameters parameters)
	  {

		return new CalculationRules(functions, reportingCurrency, parameters);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.parameters = CalculationParameters.empty();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CalculationRules}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CalculationRules.Meta meta()
	  {
		return CalculationRules.Meta.INSTANCE;
	  }

	  static CalculationRules()
	  {
		MetaBean.register(CalculationRules.Meta.INSTANCE);
	  }

	  private CalculationRules(CalculationFunctions functions, ReportingCurrency reportingCurrency, CalculationParameters parameters)
	  {
		JodaBeanUtils.notNull(functions, "functions");
		JodaBeanUtils.notNull(reportingCurrency, "reportingCurrency");
		JodaBeanUtils.notNull(parameters, "parameters");
		this.functions = functions;
		this.reportingCurrency = reportingCurrency;
		this.parameters = parameters;
	  }

	  public override CalculationRules.Meta metaBean()
	  {
		return CalculationRules.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the calculation functions.
	  /// <para>
	  /// Functions provide the logic of the calculation.
	  /// Each type of target must have an associated function in order for calculations to be performed.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CalculationFunctions Functions
	  {
		  get
		  {
			return functions;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the reporting currency, used to control currency conversion.
	  /// <para>
	  /// This is used to specify the currency that the result should be reporting in.
	  /// If the result is not associated with a currency, such as for "par rate", then the
	  /// reporting currency will effectively be ignored.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ReportingCurrency ReportingCurrency
	  {
		  get
		  {
			return reportingCurrency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the calculation parameters, used to control the how the calculation is performed.
	  /// <para>
	  /// Parameters are used to parameterize the <seealso cref="Measure"/> to be calculated.
	  /// They may be specified in two places - here and in the <seealso cref="Column"/>.
	  /// The parameters specified here are the defaults that apply to all columns.
	  /// </para>
	  /// <para>
	  /// If a parameter is defined here and in the column with the same
	  /// <seealso cref="CalculationParameter#queryType() query type"/>, then the column parameter takes precedence.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CalculationParameters Parameters
	  {
		  get
		  {
			return parameters;
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
		  CalculationRules other = (CalculationRules) obj;
		  return JodaBeanUtils.equal(functions, other.functions) && JodaBeanUtils.equal(reportingCurrency, other.reportingCurrency) && JodaBeanUtils.equal(parameters, other.parameters);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(functions);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(reportingCurrency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(parameters);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("CalculationRules{");
		buf.Append("functions").Append('=').Append(functions).Append(',').Append(' ');
		buf.Append("reportingCurrency").Append('=').Append(reportingCurrency).Append(',').Append(' ');
		buf.Append("parameters").Append('=').Append(JodaBeanUtils.ToString(parameters));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CalculationRules}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  functions_Renamed = DirectMetaProperty.ofImmutable(this, "functions", typeof(CalculationRules), typeof(CalculationFunctions));
			  reportingCurrency_Renamed = DirectMetaProperty.ofImmutable(this, "reportingCurrency", typeof(CalculationRules), typeof(ReportingCurrency));
			  parameters_Renamed = DirectMetaProperty.ofImmutable(this, "parameters", typeof(CalculationRules), typeof(CalculationParameters));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "functions", "reportingCurrency", "parameters");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code functions} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CalculationFunctions> functions_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "functions", "reportingCurrency", "parameters");
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
			case -140572773: // functions
			  return functions_Renamed;
			case -1287844769: // reportingCurrency
			  return reportingCurrency_Renamed;
			case 458736106: // parameters
			  return parameters_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends CalculationRules> builder()
		public override BeanBuilder<CalculationRules> builder()
		{
		  return new CalculationRules.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CalculationRules);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code functions} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CalculationFunctions> functions()
		{
		  return functions_Renamed;
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
			case -140572773: // functions
			  return ((CalculationRules) bean).Functions;
			case -1287844769: // reportingCurrency
			  return ((CalculationRules) bean).ReportingCurrency;
			case 458736106: // parameters
			  return ((CalculationRules) bean).Parameters;
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
	  /// The bean-builder for {@code CalculationRules}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<CalculationRules>
	  {

		internal CalculationFunctions functions;
		internal ReportingCurrency reportingCurrency;
		internal CalculationParameters parameters;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		  applyDefaults(this);
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -140572773: // functions
			  return functions;
			case -1287844769: // reportingCurrency
			  return reportingCurrency;
			case 458736106: // parameters
			  return parameters;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -140572773: // functions
			  this.functions = (CalculationFunctions) newValue;
			  break;
			case -1287844769: // reportingCurrency
			  this.reportingCurrency = (ReportingCurrency) newValue;
			  break;
			case 458736106: // parameters
			  this.parameters = (CalculationParameters) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override CalculationRules build()
		{
		  return new CalculationRules(functions, reportingCurrency, parameters);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("CalculationRules.Builder{");
		  buf.Append("functions").Append('=').Append(JodaBeanUtils.ToString(functions)).Append(',').Append(' ');
		  buf.Append("reportingCurrency").Append('=').Append(JodaBeanUtils.ToString(reportingCurrency)).Append(',').Append(' ');
		  buf.Append("parameters").Append('=').Append(JodaBeanUtils.ToString(parameters));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}