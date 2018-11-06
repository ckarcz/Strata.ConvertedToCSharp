using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableMap;


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
	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Guavate = com.opengamma.strata.collect.Guavate;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using ObjIntPair = com.opengamma.strata.collect.tuple.ObjIntPair;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioPerturbation = com.opengamma.strata.data.scenario.ScenarioPerturbation;

	/// <summary>
	/// A perturbation that applies different shifts to specific points in a parameterized data.
	/// <para>
	/// Examples of parameterized data include curve, option volatilities and model parameters.
	/// </para>
	/// <para>
	/// This class contains a set of shifts, each one associated with a different parameter in the data.
	/// Each shift has an associated key that is matched against the parameterized data.
	/// In order for this to work the parameterized data must have matching and unique parameter metadata.
	/// </para>
	/// <para>
	/// When matching the shift to the parameterized data, either the identifier or label parameter may be used.
	/// A shift is not applied if there is no point on the parameterized data with a matching identifier.
	/// 
	/// </para>
	/// </summary>
	/// <seealso cref= ParameterMetadata#getIdentifier() </seealso>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") public final class PointShifts implements com.opengamma.strata.data.scenario.ScenarioPerturbation<ParameterizedData>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class PointShifts : ScenarioPerturbation<ParameterizedData>, ImmutableBean
	{

	  /// <summary>
	  /// Logger. </summary>
	  private static readonly Logger log = LoggerFactory.getLogger(typeof(PointShifts));

	  /// <summary>
	  /// The type of shift applied to the parameters.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.ShiftType shiftType;
	  private readonly ShiftType shiftType;

	  /// <summary>
	  /// The shift to apply to the rates.
	  /// <para>
	  /// There is one row in the matrix for each scenario and one column for each parameter in the data.
	  /// Node indices are found using {@code nodeIndices}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleMatrix shifts;
	  private readonly DoubleMatrix shifts;

	  /// <summary>
	  /// Indices of each parameter, keyed by an object identifying the node.
	  /// <para>
	  /// The key is typically the node <seealso cref="ParameterMetadata#getIdentifier() identifier"/>.
	  /// The key may also be the node <seealso cref="ParameterMetadata#getLabel() label"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<Object, int> nodeIndices;
	  private readonly ImmutableMap<object, int> nodeIndices;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a new mutable builder for building instances of {@code ParameterizedDataPointShifts}.
	  /// </summary>
	  /// <param name="shiftType">  the type of shift to apply to the rates </param>
	  /// <returns> a new mutable builder for building instances of {@code ParameterizedDataPointShifts} </returns>
	  public static PointShiftsBuilder builder(ShiftType shiftType)
	  {
		return new PointShiftsBuilder(shiftType);
	  }

	  //--------------------------------------------------------------------------------------------------

	  /// <summary>
	  /// Creates a new set of point shifts.
	  /// </summary>
	  /// <param name="shiftType">  the type of the shift, absolute or relative </param>
	  /// <param name="shifts">  the shifts, with one row per scenario and one column per parameter </param>
	  /// <param name="nodeIdentifiers">  the node identifiers corresponding to the columns in the matrix of shifts </param>
	  internal PointShifts(ShiftType shiftType, DoubleMatrix shifts, IList<object> nodeIdentifiers) : this(shiftType, shifts, buildNodeMap(nodeIdentifiers))
	  {

	  }

	  private static IDictionary<object, int> buildNodeMap(IList<object> nodeIdentifiers)
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return Guavate.zipWithIndex(nodeIdentifiers.stream()).collect(toImmutableMap(ObjIntPair::getFirst, ObjIntPair::getSecond));
	  }

	  //-------------------------------------------------------------------------

	  public MarketDataBox<ParameterizedData> applyTo(MarketDataBox<ParameterizedData> marketData, ReferenceData refData)
	  {

		log.debug("Applying {} point shift to ParameterizedData '{}'", shiftType, marketData.getValue(0).ToString());
		return marketData.mapWithIndex(shifts.rowCount(), (prams, scenarioIndex) => applyShifts(scenarioIndex, prams));
	  }

	  private ParameterizedData applyShifts(int scenarioIndex, ParameterizedData prams)
	  {
		return prams.withPerturbation((index, value, meta) =>
		{
		double shiftAmount = shiftForNode(scenarioIndex, meta);
		return shiftType.applyShift(value, shiftAmount);
		});
	  }

	  public int ScenarioCount
	  {
		  get
		  {
			return shifts.rowCount();
		  }
	  }

	  private double shiftForNode(int scenarioIndex, ParameterMetadata meta)
	  {
		int? nodeIndex = nodeIndices.get(meta.Identifier);

		if (nodeIndex != null)
		{
		  return shifts.get(scenarioIndex, nodeIndex.Value);
		}
		nodeIndex = nodeIndices.get(meta.Label);

		if (nodeIndex != null)
		{
		  return shifts.get(scenarioIndex, nodeIndex.Value);
		}
		return 0;
	  }

	  public Type<ParameterizedData> MarketDataType
	  {
		  get
		  {
			return typeof(ParameterizedData);
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code PointShifts}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static PointShifts.Meta meta()
	  {
		return PointShifts.Meta.INSTANCE;
	  }

	  static PointShifts()
	  {
		MetaBean.register(PointShifts.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="shiftType">  the value of the property, not null </param>
	  /// <param name="shifts">  the value of the property, not null </param>
	  /// <param name="nodeIndices">  the value of the property, not null </param>
	  internal PointShifts(ShiftType shiftType, DoubleMatrix shifts, IDictionary<object, int> nodeIndices)
	  {
		JodaBeanUtils.notNull(shiftType, "shiftType");
		JodaBeanUtils.notNull(shifts, "shifts");
		JodaBeanUtils.notNull(nodeIndices, "nodeIndices");
		this.shiftType = shiftType;
		this.shifts = shifts;
		this.nodeIndices = ImmutableMap.copyOf(nodeIndices);
	  }

	  public override PointShifts.Meta metaBean()
	  {
		return PointShifts.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type of shift applied to the parameters. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ShiftType ShiftType
	  {
		  get
		  {
			return shiftType;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the shift to apply to the rates.
	  /// <para>
	  /// There is one row in the matrix for each scenario and one column for each parameter in the data.
	  /// Node indices are found using {@code nodeIndices}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleMatrix Shifts
	  {
		  get
		  {
			return shifts;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets indices of each parameter, keyed by an object identifying the node.
	  /// <para>
	  /// The key is typically the node <seealso cref="ParameterMetadata#getIdentifier() identifier"/>.
	  /// The key may also be the node <seealso cref="ParameterMetadata#getLabel() label"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<object, int> NodeIndices
	  {
		  get
		  {
			return nodeIndices;
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
		  PointShifts other = (PointShifts) obj;
		  return JodaBeanUtils.equal(shiftType, other.shiftType) && JodaBeanUtils.equal(shifts, other.shifts) && JodaBeanUtils.equal(nodeIndices, other.nodeIndices);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(shiftType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(shifts);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(nodeIndices);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("PointShifts{");
		buf.Append("shiftType").Append('=').Append(shiftType).Append(',').Append(' ');
		buf.Append("shifts").Append('=').Append(shifts).Append(',').Append(' ');
		buf.Append("nodeIndices").Append('=').Append(JodaBeanUtils.ToString(nodeIndices));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code PointShifts}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  shiftType_Renamed = DirectMetaProperty.ofImmutable(this, "shiftType", typeof(PointShifts), typeof(ShiftType));
			  shifts_Renamed = DirectMetaProperty.ofImmutable(this, "shifts", typeof(PointShifts), typeof(DoubleMatrix));
			  nodeIndices_Renamed = DirectMetaProperty.ofImmutable(this, "nodeIndices", typeof(PointShifts), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "shiftType", "shifts", "nodeIndices");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code shiftType} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ShiftType> shiftType_Renamed;
		/// <summary>
		/// The meta-property for the {@code shifts} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleMatrix> shifts_Renamed;
		/// <summary>
		/// The meta-property for the {@code nodeIndices} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<Object, int>> nodeIndices = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "nodeIndices", PointShifts.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<object, int>> nodeIndices_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "shiftType", "shifts", "nodeIndices");
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
			case 893345500: // shiftType
			  return shiftType_Renamed;
			case -903338959: // shifts
			  return shifts_Renamed;
			case -1547874491: // nodeIndices
			  return nodeIndices_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends PointShifts> builder()
		public override BeanBuilder<PointShifts> builder()
		{
		  return new PointShifts.Builder();
		}

		public override Type beanType()
		{
		  return typeof(PointShifts);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code shiftType} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ShiftType> shiftType()
		{
		  return shiftType_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code shifts} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleMatrix> shifts()
		{
		  return shifts_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code nodeIndices} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<object, int>> nodeIndices()
		{
		  return nodeIndices_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 893345500: // shiftType
			  return ((PointShifts) bean).ShiftType;
			case -903338959: // shifts
			  return ((PointShifts) bean).Shifts;
			case -1547874491: // nodeIndices
			  return ((PointShifts) bean).NodeIndices;
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
	  /// The bean-builder for {@code PointShifts}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<PointShifts>
	  {

		internal ShiftType shiftType;
		internal DoubleMatrix shifts;
		internal IDictionary<object, int> nodeIndices = ImmutableMap.of();

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
			case 893345500: // shiftType
			  return shiftType;
			case -903338959: // shifts
			  return shifts;
			case -1547874491: // nodeIndices
			  return nodeIndices;
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
			case 893345500: // shiftType
			  this.shiftType = (ShiftType) newValue;
			  break;
			case -903338959: // shifts
			  this.shifts = (DoubleMatrix) newValue;
			  break;
			case -1547874491: // nodeIndices
			  this.nodeIndices = (IDictionary<object, int>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override PointShifts build()
		{
		  return new PointShifts(shiftType, shifts, nodeIndices);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("PointShifts.Builder{");
		  buf.Append("shiftType").Append('=').Append(JodaBeanUtils.ToString(shiftType)).Append(',').Append(' ');
		  buf.Append("shifts").Append('=').Append(JodaBeanUtils.ToString(shifts)).Append(',').Append(' ');
		  buf.Append("nodeIndices").Append('=').Append(JodaBeanUtils.ToString(nodeIndices));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}