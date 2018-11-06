using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
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
	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Index = com.opengamma.strata.basics.index.Index;

	/// <summary>
	/// A group of curves.
	/// <para>
	/// This is used to hold a group of related curves, typically forming a logical set.
	/// It is often used to hold the results of a curve calibration.
	/// </para>
	/// <para>
	/// Curve groups can also be created from a set of existing curves.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class RatesCurveGroup implements CurveGroup, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class RatesCurveGroup : CurveGroup, ImmutableBean
	{

	  private static readonly Logger log = LoggerFactory.getLogger(typeof(RatesCurveGroup));

	  /// <summary>
	  /// The name of the curve group.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final CurveGroupName name;
	  private readonly CurveGroupName name;
	  /// <summary>
	  /// The discount curves in the group, keyed by currency.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.currency.Currency, Curve> discountCurves;
	  private readonly ImmutableMap<Currency, Curve> discountCurves;
	  /// <summary>
	  /// The forward curves in the group, keyed by index.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "Map<? extends Index, ? extends Curve>") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.index.Index, Curve> forwardCurves;
	  private readonly ImmutableMap<Index, Curve> forwardCurves;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a curve group containing the specified curves.
	  /// </summary>
	  /// <param name="name">  the name of the curve group </param>
	  /// <param name="discountCurves">  the discount curves, keyed by currency </param>
	  /// <param name="forwardCurves">  the forward curves, keyed by index </param>
	  /// <returns> a curve group containing the specified curves </returns>
	  public static RatesCurveGroup of(CurveGroupName name, IDictionary<Currency, Curve> discountCurves, IDictionary<Index, Curve> forwardCurves)
	  {
		return new RatesCurveGroup(name, discountCurves, forwardCurves);
	  }

	  /// <summary>
	  /// Creates a curve group using a curve group definition and some existing curves.
	  /// <para>
	  /// If there are curves named in the definition which are not present in the curves the group is built using
	  /// whatever curves are available.
	  /// </para>
	  /// <para>
	  /// If there are multiple curves with the same name in the curves one of them is arbitrarily chosen.
	  /// </para>
	  /// <para>
	  /// Multiple curves with the same name are allowed to support the use case where the list contains the same
	  /// curve multiple times. This means the caller doesn't have to filter the input curves to remove duplicates.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveGroupDefinition">  the definition of a curve group </param>
	  /// <param name="curves">  some curves </param>
	  /// <returns> a curve group built from the definition and the list of curves </returns>
	  public static RatesCurveGroup ofCurves(RatesCurveGroupDefinition curveGroupDefinition, params Curve[] curves)
	  {
		return ofCurves(curveGroupDefinition, Arrays.asList(curves));
	  }

	  /// <summary>
	  /// Creates a curve group using a curve group definition and a list of existing curves.
	  /// <para>
	  /// If there are curves named in the definition which are not present in the curves the group is built using
	  /// whatever curves are available.
	  /// </para>
	  /// <para>
	  /// If there are multiple curves with the same name in the curves one of them is arbitrarily chosen.
	  /// </para>
	  /// <para>
	  /// Multiple curves with the same name are allowed to support the use case where the list contains the same
	  /// curve multiple times. This means the caller doesn't have to filter the input curves to remove duplicates.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveGroupDefinition">  the definition of a curve group </param>
	  /// <param name="curves">  some curves </param>
	  /// <returns> a curve group built from the definition and the list of curves </returns>
	  public static RatesCurveGroup ofCurves<T1>(RatesCurveGroupDefinition curveGroupDefinition, ICollection<T1> curves) where T1 : Curve
	  {
		IDictionary<Currency, Curve> discountCurves = new Dictionary<Currency, Curve>();
		IDictionary<Index, Curve> forwardCurves = new Dictionary<Index, Curve>();
		IDictionary<CurveName, Curve> curveMap = curves.ToDictionary(curve => curve.Metadata.CurveName, curve => curve, (curve1, curve2) => curve1);

		foreach (RatesCurveGroupEntry entry in curveGroupDefinition.Entries)
		{
		  CurveName curveName = entry.CurveName;
		  Curve curve = curveMap[curveName];

		  if (curve == null)
		  {
			log.debug("No curve found named '{}' when building curve group '{}'", curveName, curveGroupDefinition.Name);
			continue;
		  }
		  foreach (Currency currency in entry.DiscountCurrencies)
		  {
			discountCurves[currency] = curve;
		  }
		  foreach (Index index in entry.Indices)
		  {
			forwardCurves[index] = curve;
		  }
		}
		return RatesCurveGroup.of(curveGroupDefinition.Name, discountCurves, forwardCurves);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the curve with the specified name.
	  /// <para>
	  /// If the curve cannot be found, empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <returns> the curve, empty if not found </returns>
	  public Optional<Curve> findCurve(CurveName name)
	  {
		return Stream.concat(discountCurves.values().stream(), forwardCurves.values().stream()).filter(c => c.Name.Equals(name)).findFirst();
	  }

	  /// <summary>
	  /// Finds the discount curve for the currency if there is one in the group.
	  /// <para>
	  /// If the curve is not found, optional empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency for which a discount curve is required </param>
	  /// <returns> the discount curve for the currency if there is one in the group </returns>
	  public Optional<Curve> findDiscountCurve(Currency currency)
	  {
		return Optional.ofNullable(discountCurves.get(currency));
	  }

	  /// <summary>
	  /// Finds the forward curve for the index if there is one in the group.
	  /// <para>
	  /// If the curve is not found, optional empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index for which a forward curve is required </param>
	  /// <returns> the forward curve for the index if there is one in the group </returns>
	  public Optional<Curve> findForwardCurve(Index index)
	  {
		return Optional.ofNullable(forwardCurves.get(index));
	  }

	  /// <summary>
	  /// Returns a stream of all curves in the group.
	  /// </summary>
	  /// <returns> Returns a stream of all curves in the group </returns>
	  public Stream<Curve> stream()
	  {
		return Stream.concat(discountCurves.values().stream(), forwardCurves.values().stream());
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code RatesCurveGroup}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static RatesCurveGroup.Meta meta()
	  {
		return RatesCurveGroup.Meta.INSTANCE;
	  }

	  static RatesCurveGroup()
	  {
		MetaBean.register(RatesCurveGroup.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static RatesCurveGroup.Builder builder()
	  {
		return new RatesCurveGroup.Builder();
	  }

	  private RatesCurveGroup<T1>(CurveGroupName name, IDictionary<Currency, Curve> discountCurves, IDictionary<T1> forwardCurves) where T1 : com.opengamma.strata.basics.index.Index
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(discountCurves, "discountCurves");
		JodaBeanUtils.notNull(forwardCurves, "forwardCurves");
		this.name = name;
		this.discountCurves = ImmutableMap.copyOf(discountCurves);
		this.forwardCurves = ImmutableMap.copyOf(forwardCurves);
	  }

	  public override RatesCurveGroup.Meta metaBean()
	  {
		return RatesCurveGroup.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name of the curve group. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveGroupName Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the discount curves in the group, keyed by currency. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<Currency, Curve> DiscountCurves
	  {
		  get
		  {
			return discountCurves;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the forward curves in the group, keyed by index. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<Index, Curve> ForwardCurves
	  {
		  get
		  {
			return forwardCurves;
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
		  RatesCurveGroup other = (RatesCurveGroup) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(discountCurves, other.discountCurves) && JodaBeanUtils.equal(forwardCurves, other.forwardCurves);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(discountCurves);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(forwardCurves);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("RatesCurveGroup{");
		buf.Append("name").Append('=').Append(name).Append(',').Append(' ');
		buf.Append("discountCurves").Append('=').Append(discountCurves).Append(',').Append(' ');
		buf.Append("forwardCurves").Append('=').Append(JodaBeanUtils.ToString(forwardCurves));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code RatesCurveGroup}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(RatesCurveGroup), typeof(CurveGroupName));
			  discountCurves_Renamed = DirectMetaProperty.ofImmutable(this, "discountCurves", typeof(RatesCurveGroup), (Type) typeof(ImmutableMap));
			  forwardCurves_Renamed = DirectMetaProperty.ofImmutable(this, "forwardCurves", typeof(RatesCurveGroup), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "discountCurves", "forwardCurves");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveGroupName> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code discountCurves} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.currency.Currency, Curve>> discountCurves = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "discountCurves", RatesCurveGroup.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<Currency, Curve>> discountCurves_Renamed;
		/// <summary>
		/// The meta-property for the {@code forwardCurves} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.index.Index, Curve>> forwardCurves = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "forwardCurves", RatesCurveGroup.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<Index, Curve>> forwardCurves_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "discountCurves", "forwardCurves");
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
			case -624113147: // discountCurves
			  return discountCurves_Renamed;
			case -850086775: // forwardCurves
			  return forwardCurves_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override RatesCurveGroup.Builder builder()
		{
		  return new RatesCurveGroup.Builder();
		}

		public override Type beanType()
		{
		  return typeof(RatesCurveGroup);
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
		public MetaProperty<CurveGroupName> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code discountCurves} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<Currency, Curve>> discountCurves()
		{
		  return discountCurves_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code forwardCurves} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<Index, Curve>> forwardCurves()
		{
		  return forwardCurves_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((RatesCurveGroup) bean).Name;
			case -624113147: // discountCurves
			  return ((RatesCurveGroup) bean).DiscountCurves;
			case -850086775: // forwardCurves
			  return ((RatesCurveGroup) bean).ForwardCurves;
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
	  /// The bean-builder for {@code RatesCurveGroup}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<RatesCurveGroup>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveGroupName name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IDictionary<Currency, Curve> discountCurves_Renamed = ImmutableMap.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Map<? extends com.opengamma.strata.basics.index.Index, ? extends Curve> forwardCurves = com.google.common.collect.ImmutableMap.of();
		internal IDictionary<Index, ? extends Curve> forwardCurves_Renamed = ImmutableMap.of();

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(RatesCurveGroup beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.discountCurves_Renamed = beanToCopy.DiscountCurves;
		  this.forwardCurves_Renamed = beanToCopy.ForwardCurves;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case -624113147: // discountCurves
			  return discountCurves_Renamed;
			case -850086775: // forwardCurves
			  return forwardCurves_Renamed;
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
			  this.name_Renamed = (CurveGroupName) newValue;
			  break;
			case -624113147: // discountCurves
			  this.discountCurves_Renamed = (IDictionary<Currency, Curve>) newValue;
			  break;
			case -850086775: // forwardCurves
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.forwardCurves = (java.util.Map<? extends com.opengamma.strata.basics.index.Index, ? extends Curve>) newValue;
			  this.forwardCurves_Renamed = (IDictionary<Index, ? extends Curve>) newValue;
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

		public override RatesCurveGroup build()
		{
		  return new RatesCurveGroup(name_Renamed, discountCurves_Renamed, forwardCurves_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the name of the curve group. </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(CurveGroupName name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the discount curves in the group, keyed by currency. </summary>
		/// <param name="discountCurves">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder discountCurves(IDictionary<Currency, Curve> discountCurves)
		{
		  JodaBeanUtils.notNull(discountCurves, "discountCurves");
		  this.discountCurves_Renamed = discountCurves;
		  return this;
		}

		/// <summary>
		/// Sets the forward curves in the group, keyed by index. </summary>
		/// <param name="forwardCurves">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder forwardCurves<T1>(IDictionary<T1> forwardCurves) where T1 : com.opengamma.strata.basics.index.Index
		{
		  JodaBeanUtils.notNull(forwardCurves, "forwardCurves");
		  this.forwardCurves_Renamed = forwardCurves;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("RatesCurveGroup.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("discountCurves").Append('=').Append(JodaBeanUtils.ToString(discountCurves_Renamed)).Append(',').Append(' ');
		  buf.Append("forwardCurves").Append('=').Append(JodaBeanUtils.ToString(forwardCurves_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}