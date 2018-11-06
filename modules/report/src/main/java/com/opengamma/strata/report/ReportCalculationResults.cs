using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report
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

	using ImmutableList = com.google.common.collect.ImmutableList;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Column = com.opengamma.strata.calc.Column;
	using Results = com.opengamma.strata.calc.Results;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using StandardComponents = com.opengamma.strata.measure.StandardComponents;

	/// <summary>
	/// Stores a set of engine calculation results along with the context required to run reports.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ReportCalculationResults implements org.joda.beans.ImmutableBean
	public sealed class ReportCalculationResults : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate valuationDate;
		private readonly LocalDate valuationDate;
	  /// <summary>
	  /// The targets on which the results are calculated.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.util.List<com.opengamma.strata.basics.CalculationTarget> targets;
	  private readonly IList<CalculationTarget> targets;
	  /// <summary>
	  /// The columns contained in the results.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.util.List<com.opengamma.strata.calc.Column> columns;
	  private readonly IList<Column> columns;
	  /// <summary>
	  /// The calculation results.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.calc.Results calculationResults;
	  private readonly Results calculationResults;
	  /// <summary>
	  /// The calculation functions.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.calc.runner.CalculationFunctions calculationFunctions;
	  private readonly CalculationFunctions calculationFunctions;
	  /// <summary>
	  /// The reference data.
	  /// This is used to resolve trade or security information if necessary.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.ReferenceData referenceData;
	  private readonly ReferenceData referenceData;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the valuation date, trades, columns and results.
	  /// <para>
	  /// This uses standard reference data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date used in the calculations </param>
	  /// <param name="targets">  the targets for which the results were calculated </param>
	  /// <param name="columns">  the columns in the results </param>
	  /// <param name="calculationResults">  the results of the calculations </param>
	  /// <returns> the results </returns>
	  public static ReportCalculationResults of<T1>(LocalDate valuationDate, IList<T1> targets, IList<Column> columns, Results calculationResults) where T1 : com.opengamma.strata.basics.CalculationTarget
	  {

		return of(valuationDate, targets, columns, calculationResults, StandardComponents.calculationFunctions(), ReferenceData.standard());
	  }

	  /// <summary>
	  /// Obtains an instance from the valuation date, trades, columns, results and reference data.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date used in the calculations </param>
	  /// <param name="targets">  the targets for which the results were calculated </param>
	  /// <param name="columns">  the columns in the results </param>
	  /// <param name="calculationResults">  the results of the calculations </param>
	  /// <param name="calculationFunctions">  the calculation functions that were used </param>
	  /// <param name="refData">  the reference data used in the calculation </param>
	  /// <returns> the results </returns>
	  public static ReportCalculationResults of<T1>(LocalDate valuationDate, IList<T1> targets, IList<Column> columns, Results calculationResults, CalculationFunctions calculationFunctions, ReferenceData refData) where T1 : com.opengamma.strata.basics.CalculationTarget
	  {

		return new ReportCalculationResults(valuationDate, ImmutableList.copyOf(targets), columns, calculationResults, calculationFunctions, refData);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ReportCalculationResults}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ReportCalculationResults.Meta meta()
	  {
		return ReportCalculationResults.Meta.INSTANCE;
	  }

	  static ReportCalculationResults()
	  {
		MetaBean.register(ReportCalculationResults.Meta.INSTANCE);
	  }

	  private ReportCalculationResults(LocalDate valuationDate, IList<CalculationTarget> targets, IList<Column> columns, Results calculationResults, CalculationFunctions calculationFunctions, ReferenceData referenceData)
	  {
		JodaBeanUtils.notNull(valuationDate, "valuationDate");
		JodaBeanUtils.notNull(targets, "targets");
		JodaBeanUtils.notNull(columns, "columns");
		JodaBeanUtils.notNull(calculationResults, "calculationResults");
		JodaBeanUtils.notNull(calculationFunctions, "calculationFunctions");
		JodaBeanUtils.notNull(referenceData, "referenceData");
		this.valuationDate = valuationDate;
		this.targets = ImmutableList.copyOf(targets);
		this.columns = ImmutableList.copyOf(columns);
		this.calculationResults = calculationResults;
		this.calculationFunctions = calculationFunctions;
		this.referenceData = referenceData;
	  }

	  public override ReportCalculationResults.Meta metaBean()
	  {
		return ReportCalculationResults.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the valuation date. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate ValuationDate
	  {
		  get
		  {
			return valuationDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the targets on which the results are calculated. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IList<CalculationTarget> Targets
	  {
		  get
		  {
			return targets;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the columns contained in the results. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IList<Column> Columns
	  {
		  get
		  {
			return columns;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the calculation results. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Results CalculationResults
	  {
		  get
		  {
			return calculationResults;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the calculation functions. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CalculationFunctions CalculationFunctions
	  {
		  get
		  {
			return calculationFunctions;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the reference data.
	  /// This is used to resolve trade or security information if necessary. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ReferenceData ReferenceData
	  {
		  get
		  {
			return referenceData;
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
		  ReportCalculationResults other = (ReportCalculationResults) obj;
		  return JodaBeanUtils.equal(valuationDate, other.valuationDate) && JodaBeanUtils.equal(targets, other.targets) && JodaBeanUtils.equal(columns, other.columns) && JodaBeanUtils.equal(calculationResults, other.calculationResults) && JodaBeanUtils.equal(calculationFunctions, other.calculationFunctions) && JodaBeanUtils.equal(referenceData, other.referenceData);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valuationDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(targets);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(columns);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(calculationResults);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(calculationFunctions);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(referenceData);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(224);
		buf.Append("ReportCalculationResults{");
		buf.Append("valuationDate").Append('=').Append(valuationDate).Append(',').Append(' ');
		buf.Append("targets").Append('=').Append(targets).Append(',').Append(' ');
		buf.Append("columns").Append('=').Append(columns).Append(',').Append(' ');
		buf.Append("calculationResults").Append('=').Append(calculationResults).Append(',').Append(' ');
		buf.Append("calculationFunctions").Append('=').Append(calculationFunctions).Append(',').Append(' ');
		buf.Append("referenceData").Append('=').Append(JodaBeanUtils.ToString(referenceData));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ReportCalculationResults}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  valuationDate_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDate", typeof(ReportCalculationResults), typeof(LocalDate));
			  targets_Renamed = DirectMetaProperty.ofImmutable(this, "targets", typeof(ReportCalculationResults), (Type) typeof(System.Collections.IList));
			  columns_Renamed = DirectMetaProperty.ofImmutable(this, "columns", typeof(ReportCalculationResults), (Type) typeof(System.Collections.IList));
			  calculationResults_Renamed = DirectMetaProperty.ofImmutable(this, "calculationResults", typeof(ReportCalculationResults), typeof(Results));
			  calculationFunctions_Renamed = DirectMetaProperty.ofImmutable(this, "calculationFunctions", typeof(ReportCalculationResults), typeof(CalculationFunctions));
			  referenceData_Renamed = DirectMetaProperty.ofImmutable(this, "referenceData", typeof(ReportCalculationResults), typeof(ReferenceData));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "valuationDate", "targets", "columns", "calculationResults", "calculationFunctions", "referenceData");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code valuationDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> valuationDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code targets} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<java.util.List<com.opengamma.strata.basics.CalculationTarget>> targets = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "targets", ReportCalculationResults.class, (Class) java.util.List.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IList<CalculationTarget>> targets_Renamed;
		/// <summary>
		/// The meta-property for the {@code columns} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<java.util.List<com.opengamma.strata.calc.Column>> columns = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "columns", ReportCalculationResults.class, (Class) java.util.List.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IList<Column>> columns_Renamed;
		/// <summary>
		/// The meta-property for the {@code calculationResults} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Results> calculationResults_Renamed;
		/// <summary>
		/// The meta-property for the {@code calculationFunctions} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CalculationFunctions> calculationFunctions_Renamed;
		/// <summary>
		/// The meta-property for the {@code referenceData} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ReferenceData> referenceData_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "valuationDate", "targets", "columns", "calculationResults", "calculationFunctions", "referenceData");
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
			case 113107279: // valuationDate
			  return valuationDate_Renamed;
			case -1538277118: // targets
			  return targets_Renamed;
			case 949721053: // columns
			  return columns_Renamed;
			case 2096132333: // calculationResults
			  return calculationResults_Renamed;
			case 1722473170: // calculationFunctions
			  return calculationFunctions_Renamed;
			case 1600456085: // referenceData
			  return referenceData_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ReportCalculationResults> builder()
		public override BeanBuilder<ReportCalculationResults> builder()
		{
		  return new ReportCalculationResults.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ReportCalculationResults);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code valuationDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> valuationDate()
		{
		  return valuationDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code targets} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IList<CalculationTarget>> targets()
		{
		  return targets_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code columns} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IList<Column>> columns()
		{
		  return columns_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code calculationResults} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Results> calculationResults()
		{
		  return calculationResults_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code calculationFunctions} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CalculationFunctions> calculationFunctions()
		{
		  return calculationFunctions_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code referenceData} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ReferenceData> referenceData()
		{
		  return referenceData_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 113107279: // valuationDate
			  return ((ReportCalculationResults) bean).ValuationDate;
			case -1538277118: // targets
			  return ((ReportCalculationResults) bean).Targets;
			case 949721053: // columns
			  return ((ReportCalculationResults) bean).Columns;
			case 2096132333: // calculationResults
			  return ((ReportCalculationResults) bean).CalculationResults;
			case 1722473170: // calculationFunctions
			  return ((ReportCalculationResults) bean).CalculationFunctions;
			case 1600456085: // referenceData
			  return ((ReportCalculationResults) bean).ReferenceData;
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
	  /// The bean-builder for {@code ReportCalculationResults}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ReportCalculationResults>
	  {

		internal LocalDate valuationDate;
		internal IList<CalculationTarget> targets = ImmutableList.of();
		internal IList<Column> columns = ImmutableList.of();
		internal Results calculationResults;
		internal CalculationFunctions calculationFunctions;
		internal ReferenceData referenceData;

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
			case 113107279: // valuationDate
			  return valuationDate;
			case -1538277118: // targets
			  return targets;
			case 949721053: // columns
			  return columns;
			case 2096132333: // calculationResults
			  return calculationResults;
			case 1722473170: // calculationFunctions
			  return calculationFunctions;
			case 1600456085: // referenceData
			  return referenceData;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder set(String propertyName, Object newValue)
		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 113107279: // valuationDate
			  this.valuationDate = (LocalDate) newValue;
			  break;
			case -1538277118: // targets
			  this.targets = (IList<CalculationTarget>) newValue;
			  break;
			case 949721053: // columns
			  this.columns = (IList<Column>) newValue;
			  break;
			case 2096132333: // calculationResults
			  this.calculationResults = (Results) newValue;
			  break;
			case 1722473170: // calculationFunctions
			  this.calculationFunctions = (CalculationFunctions) newValue;
			  break;
			case 1600456085: // referenceData
			  this.referenceData = (ReferenceData) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ReportCalculationResults build()
		{
		  return new ReportCalculationResults(valuationDate, targets, columns, calculationResults, calculationFunctions, referenceData);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(224);
		  buf.Append("ReportCalculationResults.Builder{");
		  buf.Append("valuationDate").Append('=').Append(JodaBeanUtils.ToString(valuationDate)).Append(',').Append(' ');
		  buf.Append("targets").Append('=').Append(JodaBeanUtils.ToString(targets)).Append(',').Append(' ');
		  buf.Append("columns").Append('=').Append(JodaBeanUtils.ToString(columns)).Append(',').Append(' ');
		  buf.Append("calculationResults").Append('=').Append(JodaBeanUtils.ToString(calculationResults)).Append(',').Append(' ');
		  buf.Append("calculationFunctions").Append('=').Append(JodaBeanUtils.ToString(calculationFunctions)).Append(',').Append(' ');
		  buf.Append("referenceData").Append('=').Append(JodaBeanUtils.ToString(referenceData));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}