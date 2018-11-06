using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.FLAT;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurveExtrapolator = com.opengamma.strata.market.curve.interpolator.CurveExtrapolator;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using SimpleStrike = com.opengamma.strata.market.option.SimpleStrike;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using SurfaceName = com.opengamma.strata.market.surface.SurfaceName;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using BlackFxOptionSurfaceVolatilities = com.opengamma.strata.pricer.fxopt.BlackFxOptionSurfaceVolatilities;
	using FxOptionVolatilitiesName = com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesName;
	using FxVolatilitySurfaceYearFractionParameterMetadata = com.opengamma.strata.pricer.fxopt.FxVolatilitySurfaceYearFractionParameterMetadata;

	/// <summary>
	/// The specification of how to build FX option volatilities. 
	/// <para>
	/// This is the specification for a single volatility object, <seealso cref="BlackFxOptionSurfaceVolatilities"/>. 
	/// The underlying surface in the volatilities is {@code InterpolatedNodalSurface}.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification implements FxOptionVolatilitiesSpecification, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification : FxOptionVolatilitiesSpecification, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesName name;
		private readonly FxOptionVolatilitiesName name;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.CurrencyPair currencyPair;
	  private readonly CurrencyPair currencyPair;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.google.common.collect.ImmutableList<FxOptionVolatilitiesNode> nodes;
	  private readonly ImmutableList<FxOptionVolatilitiesNode> nodes;
	  /// <summary>
	  /// The interpolator used in the time dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveInterpolator timeInterpolator;
	  private readonly CurveInterpolator timeInterpolator;
	  /// <summary>
	  /// The left extrapolator used in the time dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator timeExtrapolatorLeft;
	  private readonly CurveExtrapolator timeExtrapolatorLeft;
	  /// <summary>
	  /// The right extrapolator used in the time dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator timeExtrapolatorRight;
	  private readonly CurveExtrapolator timeExtrapolatorRight;
	  /// <summary>
	  /// The interpolator used in the strike dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveInterpolator strikeInterpolator;
	  private readonly CurveInterpolator strikeInterpolator;
	  /// <summary>
	  /// The left extrapolator used in the strike dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator strikeExtrapolatorLeft;
	  private readonly CurveExtrapolator strikeExtrapolatorLeft;
	  /// <summary>
	  /// The right extrapolator used in the strike dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator strikeExtrapolatorRight;
	  private readonly CurveExtrapolator strikeExtrapolatorRight;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		int nParams = nodes.size();
		for (int i = 0; i < nParams; ++i)
		{
		  ArgChecker.isTrue(nodes.get(i).CurrencyPair.Equals(currencyPair), "Currency pair must be the same");
		  ArgChecker.isTrue(nodes.get(i).Strike is SimpleStrike, "Strike must be SimpleStrike");
		  ArgChecker.isTrue(nodes.get(i).QuoteValueType.Equals(ValueType.BLACK_VOLATILITY), "Quote value type must be BLACK_VOLATILITY");
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.strikeExtrapolatorLeft_Renamed = FLAT;
		builder.strikeExtrapolatorRight_Renamed = FLAT;
		builder.timeExtrapolatorLeft_Renamed = FLAT;
		builder.timeExtrapolatorRight_Renamed = FLAT;
	  }

	  //-------------------------------------------------------------------------
	  public BlackFxOptionSurfaceVolatilities volatilities(ZonedDateTime valuationDateTime, DoubleArray parameters, ReferenceData refData)
	  {

		int nNodes = ParameterCount;
		ArgChecker.isTrue(parameters.size() == nNodes, Messages.format("size of parameters must be {}, but found {}", nNodes, parameters.size()));
		DoubleArray strikes = DoubleArray.of(nNodes, i => nodes.get(i).Strike.Value);
		DoubleArray expiries = DoubleArray.of(nNodes, i => nodes.get(i).timeToExpiry(valuationDateTime, dayCount, refData));
		SurfaceMetadata metadata = Surfaces.blackVolatilityByExpiryStrike(SurfaceName.of(name.Name), dayCount).withParameterMetadata(parameterMetadata(expiries));

		SurfaceInterpolator interp = GridSurfaceInterpolator.of(timeInterpolator, timeExtrapolatorLeft, timeExtrapolatorRight, strikeInterpolator, strikeExtrapolatorLeft, strikeExtrapolatorRight);
		InterpolatedNodalSurface surface = InterpolatedNodalSurface.ofUnsorted(metadata, expiries, strikes, parameters, interp);
		return BlackFxOptionSurfaceVolatilities.of(name, currencyPair, valuationDateTime, surface);
	  }

	  private ImmutableList<ParameterMetadata> parameterMetadata(DoubleArray expiries)
	  {
		int nParams = nodes.size();
		return IntStream.range(0, nParams).mapToObj(n => FxVolatilitySurfaceYearFractionParameterMetadata.of(expiries.get(n), nodes.get(n).Strike, currencyPair)).collect(toImmutableList());
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification.Meta meta()
	  {
		return BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification.Meta.INSTANCE;
	  }

	  static BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification()
	  {
		MetaBean.register(BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification.Builder builder()
	  {
		return new BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification.Builder();
	  }

	  private BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification(FxOptionVolatilitiesName name, CurrencyPair currencyPair, DayCount dayCount, IList<FxOptionVolatilitiesNode> nodes, CurveInterpolator timeInterpolator, CurveExtrapolator timeExtrapolatorLeft, CurveExtrapolator timeExtrapolatorRight, CurveInterpolator strikeInterpolator, CurveExtrapolator strikeExtrapolatorLeft, CurveExtrapolator strikeExtrapolatorRight)
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(currencyPair, "currencyPair");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(nodes, "nodes");
		JodaBeanUtils.notNull(timeInterpolator, "timeInterpolator");
		JodaBeanUtils.notNull(timeExtrapolatorLeft, "timeExtrapolatorLeft");
		JodaBeanUtils.notNull(timeExtrapolatorRight, "timeExtrapolatorRight");
		JodaBeanUtils.notNull(strikeInterpolator, "strikeInterpolator");
		JodaBeanUtils.notNull(strikeExtrapolatorLeft, "strikeExtrapolatorLeft");
		JodaBeanUtils.notNull(strikeExtrapolatorRight, "strikeExtrapolatorRight");
		this.name = name;
		this.currencyPair = currencyPair;
		this.dayCount = dayCount;
		this.nodes = ImmutableList.copyOf(nodes);
		this.timeInterpolator = timeInterpolator;
		this.timeExtrapolatorLeft = timeExtrapolatorLeft;
		this.timeExtrapolatorRight = timeExtrapolatorRight;
		this.strikeInterpolator = strikeInterpolator;
		this.strikeExtrapolatorLeft = strikeExtrapolatorLeft;
		this.strikeExtrapolatorRight = strikeExtrapolatorRight;
		validate();
	  }

	  public override BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification.Meta metaBean()
	  {
		return BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxOptionVolatilitiesName Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currencyPair. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurrencyPair CurrencyPair
	  {
		  get
		  {
			return currencyPair;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the dayCount. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DayCount DayCount
	  {
		  get
		  {
			return dayCount;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the nodes. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<FxOptionVolatilitiesNode> Nodes
	  {
		  get
		  {
			return nodes;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the interpolator used in the time dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveInterpolator TimeInterpolator
	  {
		  get
		  {
			return timeInterpolator;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the left extrapolator used in the time dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator TimeExtrapolatorLeft
	  {
		  get
		  {
			return timeExtrapolatorLeft;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the right extrapolator used in the time dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator TimeExtrapolatorRight
	  {
		  get
		  {
			return timeExtrapolatorRight;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the interpolator used in the strike dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveInterpolator StrikeInterpolator
	  {
		  get
		  {
			return strikeInterpolator;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the left extrapolator used in the strike dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator StrikeExtrapolatorLeft
	  {
		  get
		  {
			return strikeExtrapolatorLeft;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the right extrapolator used in the strike dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator StrikeExtrapolatorRight
	  {
		  get
		  {
			return strikeExtrapolatorRight;
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
		  BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification other = (BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(currencyPair, other.currencyPair) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(nodes, other.nodes) && JodaBeanUtils.equal(timeInterpolator, other.timeInterpolator) && JodaBeanUtils.equal(timeExtrapolatorLeft, other.timeExtrapolatorLeft) && JodaBeanUtils.equal(timeExtrapolatorRight, other.timeExtrapolatorRight) && JodaBeanUtils.equal(strikeInterpolator, other.strikeInterpolator) && JodaBeanUtils.equal(strikeExtrapolatorLeft, other.strikeExtrapolatorLeft) && JodaBeanUtils.equal(strikeExtrapolatorRight, other.strikeExtrapolatorRight);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currencyPair);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(nodes);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(timeInterpolator);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(timeExtrapolatorLeft);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(timeExtrapolatorRight);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strikeInterpolator);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strikeExtrapolatorLeft);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strikeExtrapolatorRight);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(352);
		buf.Append("BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification{");
		buf.Append("name").Append('=').Append(name).Append(',').Append(' ');
		buf.Append("currencyPair").Append('=').Append(currencyPair).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("nodes").Append('=').Append(nodes).Append(',').Append(' ');
		buf.Append("timeInterpolator").Append('=').Append(timeInterpolator).Append(',').Append(' ');
		buf.Append("timeExtrapolatorLeft").Append('=').Append(timeExtrapolatorLeft).Append(',').Append(' ');
		buf.Append("timeExtrapolatorRight").Append('=').Append(timeExtrapolatorRight).Append(',').Append(' ');
		buf.Append("strikeInterpolator").Append('=').Append(strikeInterpolator).Append(',').Append(' ');
		buf.Append("strikeExtrapolatorLeft").Append('=').Append(strikeExtrapolatorLeft).Append(',').Append(' ');
		buf.Append("strikeExtrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(strikeExtrapolatorRight));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification), typeof(FxOptionVolatilitiesName));
			  currencyPair_Renamed = DirectMetaProperty.ofImmutable(this, "currencyPair", typeof(BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification), typeof(CurrencyPair));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification), typeof(DayCount));
			  nodes_Renamed = DirectMetaProperty.ofImmutable(this, "nodes", typeof(BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification), (Type) typeof(ImmutableList));
			  timeInterpolator_Renamed = DirectMetaProperty.ofImmutable(this, "timeInterpolator", typeof(BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification), typeof(CurveInterpolator));
			  timeExtrapolatorLeft_Renamed = DirectMetaProperty.ofImmutable(this, "timeExtrapolatorLeft", typeof(BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification), typeof(CurveExtrapolator));
			  timeExtrapolatorRight_Renamed = DirectMetaProperty.ofImmutable(this, "timeExtrapolatorRight", typeof(BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification), typeof(CurveExtrapolator));
			  strikeInterpolator_Renamed = DirectMetaProperty.ofImmutable(this, "strikeInterpolator", typeof(BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification), typeof(CurveInterpolator));
			  strikeExtrapolatorLeft_Renamed = DirectMetaProperty.ofImmutable(this, "strikeExtrapolatorLeft", typeof(BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification), typeof(CurveExtrapolator));
			  strikeExtrapolatorRight_Renamed = DirectMetaProperty.ofImmutable(this, "strikeExtrapolatorRight", typeof(BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification), typeof(CurveExtrapolator));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "currencyPair", "dayCount", "nodes", "timeInterpolator", "timeExtrapolatorLeft", "timeExtrapolatorRight", "strikeInterpolator", "strikeExtrapolatorLeft", "strikeExtrapolatorRight");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxOptionVolatilitiesName> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code currencyPair} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyPair> currencyPair_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code nodes} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<FxOptionVolatilitiesNode>> nodes = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "nodes", BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<FxOptionVolatilitiesNode>> nodes_Renamed;
		/// <summary>
		/// The meta-property for the {@code timeInterpolator} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveInterpolator> timeInterpolator_Renamed;
		/// <summary>
		/// The meta-property for the {@code timeExtrapolatorLeft} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> timeExtrapolatorLeft_Renamed;
		/// <summary>
		/// The meta-property for the {@code timeExtrapolatorRight} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> timeExtrapolatorRight_Renamed;
		/// <summary>
		/// The meta-property for the {@code strikeInterpolator} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveInterpolator> strikeInterpolator_Renamed;
		/// <summary>
		/// The meta-property for the {@code strikeExtrapolatorLeft} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> strikeExtrapolatorLeft_Renamed;
		/// <summary>
		/// The meta-property for the {@code strikeExtrapolatorRight} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> strikeExtrapolatorRight_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "currencyPair", "dayCount", "nodes", "timeInterpolator", "timeExtrapolatorLeft", "timeExtrapolatorRight", "strikeInterpolator", "strikeExtrapolatorLeft", "strikeExtrapolatorRight");
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
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 104993457: // nodes
			  return nodes_Renamed;
			case -587914188: // timeInterpolator
			  return timeInterpolator_Renamed;
			case -286652761: // timeExtrapolatorLeft
			  return timeExtrapolatorLeft_Renamed;
			case -290640004: // timeExtrapolatorRight
			  return timeExtrapolatorRight_Renamed;
			case 815202713: // strikeInterpolator
			  return strikeInterpolator_Renamed;
			case -1176196724: // strikeExtrapolatorLeft
			  return strikeExtrapolatorLeft_Renamed;
			case -2096699081: // strikeExtrapolatorRight
			  return strikeExtrapolatorRight_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification.Builder builder()
		{
		  return new BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification.Builder();
		}

		public override Type beanType()
		{
		  return typeof(BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification);
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
		public MetaProperty<FxOptionVolatilitiesName> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currencyPair} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyPair> currencyPair()
		{
		  return currencyPair_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code nodes} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<FxOptionVolatilitiesNode>> nodes()
		{
		  return nodes_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code timeInterpolator} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveInterpolator> timeInterpolator()
		{
		  return timeInterpolator_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code timeExtrapolatorLeft} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> timeExtrapolatorLeft()
		{
		  return timeExtrapolatorLeft_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code timeExtrapolatorRight} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> timeExtrapolatorRight()
		{
		  return timeExtrapolatorRight_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code strikeInterpolator} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveInterpolator> strikeInterpolator()
		{
		  return strikeInterpolator_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code strikeExtrapolatorLeft} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> strikeExtrapolatorLeft()
		{
		  return strikeExtrapolatorLeft_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code strikeExtrapolatorRight} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> strikeExtrapolatorRight()
		{
		  return strikeExtrapolatorRight_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification) bean).Name;
			case 1005147787: // currencyPair
			  return ((BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification) bean).CurrencyPair;
			case 1905311443: // dayCount
			  return ((BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification) bean).DayCount;
			case 104993457: // nodes
			  return ((BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification) bean).Nodes;
			case -587914188: // timeInterpolator
			  return ((BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification) bean).TimeInterpolator;
			case -286652761: // timeExtrapolatorLeft
			  return ((BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification) bean).TimeExtrapolatorLeft;
			case -290640004: // timeExtrapolatorRight
			  return ((BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification) bean).TimeExtrapolatorRight;
			case 815202713: // strikeInterpolator
			  return ((BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification) bean).StrikeInterpolator;
			case -1176196724: // strikeExtrapolatorLeft
			  return ((BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification) bean).StrikeExtrapolatorLeft;
			case -2096699081: // strikeExtrapolatorRight
			  return ((BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification) bean).StrikeExtrapolatorRight;
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
	  /// The bean-builder for {@code BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxOptionVolatilitiesName name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurrencyPair currencyPair_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<FxOptionVolatilitiesNode> nodes_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveInterpolator timeInterpolator_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveExtrapolator timeExtrapolatorLeft_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveExtrapolator timeExtrapolatorRight_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveInterpolator strikeInterpolator_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveExtrapolator strikeExtrapolatorLeft_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveExtrapolator strikeExtrapolatorRight_Renamed;

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
		internal Builder(BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.currencyPair_Renamed = beanToCopy.CurrencyPair;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.nodes_Renamed = beanToCopy.Nodes;
		  this.timeInterpolator_Renamed = beanToCopy.TimeInterpolator;
		  this.timeExtrapolatorLeft_Renamed = beanToCopy.TimeExtrapolatorLeft;
		  this.timeExtrapolatorRight_Renamed = beanToCopy.TimeExtrapolatorRight;
		  this.strikeInterpolator_Renamed = beanToCopy.StrikeInterpolator;
		  this.strikeExtrapolatorLeft_Renamed = beanToCopy.StrikeExtrapolatorLeft;
		  this.strikeExtrapolatorRight_Renamed = beanToCopy.StrikeExtrapolatorRight;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 104993457: // nodes
			  return nodes_Renamed;
			case -587914188: // timeInterpolator
			  return timeInterpolator_Renamed;
			case -286652761: // timeExtrapolatorLeft
			  return timeExtrapolatorLeft_Renamed;
			case -290640004: // timeExtrapolatorRight
			  return timeExtrapolatorRight_Renamed;
			case 815202713: // strikeInterpolator
			  return strikeInterpolator_Renamed;
			case -1176196724: // strikeExtrapolatorLeft
			  return strikeExtrapolatorLeft_Renamed;
			case -2096699081: // strikeExtrapolatorRight
			  return strikeExtrapolatorRight_Renamed;
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
			case 3373707: // name
			  this.name_Renamed = (FxOptionVolatilitiesName) newValue;
			  break;
			case 1005147787: // currencyPair
			  this.currencyPair_Renamed = (CurrencyPair) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case 104993457: // nodes
			  this.nodes_Renamed = (IList<FxOptionVolatilitiesNode>) newValue;
			  break;
			case -587914188: // timeInterpolator
			  this.timeInterpolator_Renamed = (CurveInterpolator) newValue;
			  break;
			case -286652761: // timeExtrapolatorLeft
			  this.timeExtrapolatorLeft_Renamed = (CurveExtrapolator) newValue;
			  break;
			case -290640004: // timeExtrapolatorRight
			  this.timeExtrapolatorRight_Renamed = (CurveExtrapolator) newValue;
			  break;
			case 815202713: // strikeInterpolator
			  this.strikeInterpolator_Renamed = (CurveInterpolator) newValue;
			  break;
			case -1176196724: // strikeExtrapolatorLeft
			  this.strikeExtrapolatorLeft_Renamed = (CurveExtrapolator) newValue;
			  break;
			case -2096699081: // strikeExtrapolatorRight
			  this.strikeExtrapolatorRight_Renamed = (CurveExtrapolator) newValue;
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

		public override BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification build()
		{
		  return new BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification(name_Renamed, currencyPair_Renamed, dayCount_Renamed, nodes_Renamed, timeInterpolator_Renamed, timeExtrapolatorLeft_Renamed, timeExtrapolatorRight_Renamed, strikeInterpolator_Renamed, strikeExtrapolatorLeft_Renamed, strikeExtrapolatorRight_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the name. </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(FxOptionVolatilitiesName name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the currencyPair. </summary>
		/// <param name="currencyPair">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currencyPair(CurrencyPair currencyPair)
		{
		  JodaBeanUtils.notNull(currencyPair, "currencyPair");
		  this.currencyPair_Renamed = currencyPair;
		  return this;
		}

		/// <summary>
		/// Sets the dayCount. </summary>
		/// <param name="dayCount">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dayCount(DayCount dayCount)
		{
		  JodaBeanUtils.notNull(dayCount, "dayCount");
		  this.dayCount_Renamed = dayCount;
		  return this;
		}

		/// <summary>
		/// Sets the nodes. </summary>
		/// <param name="nodes">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder nodes(IList<FxOptionVolatilitiesNode> nodes)
		{
		  JodaBeanUtils.notNull(nodes, "nodes");
		  this.nodes_Renamed = nodes;
		  return this;
		}

		/// <summary>
		/// Sets the {@code nodes} property in the builder
		/// from an array of objects. </summary>
		/// <param name="nodes">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder nodes(params FxOptionVolatilitiesNode[] nodes)
		{
		  return this.nodes(ImmutableList.copyOf(nodes));
		}

		/// <summary>
		/// Sets the interpolator used in the time dimension. </summary>
		/// <param name="timeInterpolator">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder timeInterpolator(CurveInterpolator timeInterpolator)
		{
		  JodaBeanUtils.notNull(timeInterpolator, "timeInterpolator");
		  this.timeInterpolator_Renamed = timeInterpolator;
		  return this;
		}

		/// <summary>
		/// Sets the left extrapolator used in the time dimension. </summary>
		/// <param name="timeExtrapolatorLeft">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder timeExtrapolatorLeft(CurveExtrapolator timeExtrapolatorLeft)
		{
		  JodaBeanUtils.notNull(timeExtrapolatorLeft, "timeExtrapolatorLeft");
		  this.timeExtrapolatorLeft_Renamed = timeExtrapolatorLeft;
		  return this;
		}

		/// <summary>
		/// Sets the right extrapolator used in the time dimension. </summary>
		/// <param name="timeExtrapolatorRight">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder timeExtrapolatorRight(CurveExtrapolator timeExtrapolatorRight)
		{
		  JodaBeanUtils.notNull(timeExtrapolatorRight, "timeExtrapolatorRight");
		  this.timeExtrapolatorRight_Renamed = timeExtrapolatorRight;
		  return this;
		}

		/// <summary>
		/// Sets the interpolator used in the strike dimension. </summary>
		/// <param name="strikeInterpolator">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder strikeInterpolator(CurveInterpolator strikeInterpolator)
		{
		  JodaBeanUtils.notNull(strikeInterpolator, "strikeInterpolator");
		  this.strikeInterpolator_Renamed = strikeInterpolator;
		  return this;
		}

		/// <summary>
		/// Sets the left extrapolator used in the strike dimension. </summary>
		/// <param name="strikeExtrapolatorLeft">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder strikeExtrapolatorLeft(CurveExtrapolator strikeExtrapolatorLeft)
		{
		  JodaBeanUtils.notNull(strikeExtrapolatorLeft, "strikeExtrapolatorLeft");
		  this.strikeExtrapolatorLeft_Renamed = strikeExtrapolatorLeft;
		  return this;
		}

		/// <summary>
		/// Sets the right extrapolator used in the strike dimension. </summary>
		/// <param name="strikeExtrapolatorRight">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder strikeExtrapolatorRight(CurveExtrapolator strikeExtrapolatorRight)
		{
		  JodaBeanUtils.notNull(strikeExtrapolatorRight, "strikeExtrapolatorRight");
		  this.strikeExtrapolatorRight_Renamed = strikeExtrapolatorRight;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(352);
		  buf.Append("BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("currencyPair").Append('=').Append(JodaBeanUtils.ToString(currencyPair_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("nodes").Append('=').Append(JodaBeanUtils.ToString(nodes_Renamed)).Append(',').Append(' ');
		  buf.Append("timeInterpolator").Append('=').Append(JodaBeanUtils.ToString(timeInterpolator_Renamed)).Append(',').Append(' ');
		  buf.Append("timeExtrapolatorLeft").Append('=').Append(JodaBeanUtils.ToString(timeExtrapolatorLeft_Renamed)).Append(',').Append(' ');
		  buf.Append("timeExtrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(timeExtrapolatorRight_Renamed)).Append(',').Append(' ');
		  buf.Append("strikeInterpolator").Append('=').Append(JodaBeanUtils.ToString(strikeInterpolator_Renamed)).Append(',').Append(' ');
		  buf.Append("strikeExtrapolatorLeft").Append('=').Append(JodaBeanUtils.ToString(strikeExtrapolatorLeft_Renamed)).Append(',').Append(' ');
		  buf.Append("strikeExtrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(strikeExtrapolatorRight_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}