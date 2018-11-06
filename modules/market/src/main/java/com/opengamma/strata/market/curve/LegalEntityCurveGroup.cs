using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
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

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Pair = com.opengamma.strata.collect.tuple.Pair;

	/// <summary>
	/// A group of repo curves and issuer curves.
	/// <para>
	/// This is used to hold a group of related curves, typically forming a logical set.
	/// It is often used to hold the results of a curve calibration.
	/// </para>
	/// <para>
	/// Curve groups can also be created from a set of existing curves.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class LegalEntityCurveGroup implements CurveGroup, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class LegalEntityCurveGroup : CurveGroup, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final CurveGroupName name;
		private readonly CurveGroupName name;
	  /// <summary>
	  /// The repo curves in the curve group, keyed by repo group and currency.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.collect.tuple.Pair<RepoGroup, com.opengamma.strata.basics.currency.Currency>, Curve> repoCurves;
	  private readonly ImmutableMap<Pair<RepoGroup, Currency>, Curve> repoCurves;
	  /// <summary>
	  /// The issuer curves in the curve group, keyed by legal entity group and currency.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.collect.tuple.Pair<LegalEntityGroup, com.opengamma.strata.basics.currency.Currency>, Curve> issuerCurves;
	  private readonly ImmutableMap<Pair<LegalEntityGroup, Currency>, Curve> issuerCurves;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a curve group containing the specified curves.
	  /// </summary>
	  /// <param name="name">  the name of the curve group </param>
	  /// <param name="repoCurves">  the repo curves, keyed by pair of repo group and currency </param>
	  /// <param name="issuerCurves">  the issuer curves, keyed by pair of legal entity group and currency </param>
	  /// <returns> a curve group containing the specified curves </returns>
	  public static LegalEntityCurveGroup of(CurveGroupName name, IDictionary<Pair<RepoGroup, Currency>, Curve> repoCurves, IDictionary<Pair<LegalEntityGroup, Currency>, Curve> issuerCurves)
	  {

		return new LegalEntityCurveGroup(name, repoCurves, issuerCurves);
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
		return Stream.concat(repoCurves.values().stream(), issuerCurves.values().stream()).filter(c => c.Name.Equals(name)).findFirst();
	  }

	  /// <summary>
	  /// Finds the repo curve for the repo group and currency if there is one in the group.
	  /// <para>
	  /// If the curve is not found, optional empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="repoGroup">  the repo group </param>
	  /// <param name="currency">  the currency </param>
	  /// <returns> the repo curve for the repo group and currency if there is one in the group </returns>
	  public Optional<Curve> findRepoCurve(RepoGroup repoGroup, Currency currency)
	  {
		return Optional.ofNullable(repoCurves.get(Pair.of(repoGroup, currency)));
	  }

	  /// <summary>
	  /// Finds the issuer curve for the legal entity group and currency if there is one in the group.
	  /// <para>
	  /// If the curve is not found, optional empty is returned. 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="legalEntityGroup">  the legal entity group </param>
	  /// <param name="currency">  the currency </param>
	  /// <returns> the issuer curve for the legal entity group and currency if there is one in the group </returns>
	  public Optional<Curve> findIssuerCurve(LegalEntityGroup legalEntityGroup, Currency currency)
	  {
		return Optional.ofNullable(issuerCurves.get(Pair.of(legalEntityGroup, currency)));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a stream of all curves in the group.
	  /// </summary>
	  /// <returns> Returns a stream of all curves in the group </returns>
	  public Stream<Curve> stream()
	  {
		return Stream.concat(repoCurves.values().stream(), issuerCurves.values().stream());
	  }

	  /// <summary>
	  /// Returns a stream of all repo curves in the group.
	  /// </summary>
	  /// <returns> Returns a stream of all repo curves in the group </returns>
	  public Stream<Curve> repoCurveStream()
	  {
		return repoCurves.values().stream();
	  }

	  /// <summary>
	  /// Returns a stream of all issuer curves in the group.
	  /// </summary>
	  /// <returns> Returns a stream of all issuer curves in the group </returns>
	  public Stream<Curve> issuerCurveStream()
	  {
		return issuerCurves.values().stream();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code LegalEntityCurveGroup}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static LegalEntityCurveGroup.Meta meta()
	  {
		return LegalEntityCurveGroup.Meta.INSTANCE;
	  }

	  static LegalEntityCurveGroup()
	  {
		MetaBean.register(LegalEntityCurveGroup.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static LegalEntityCurveGroup.Builder builder()
	  {
		return new LegalEntityCurveGroup.Builder();
	  }

	  private LegalEntityCurveGroup(CurveGroupName name, IDictionary<Pair<RepoGroup, Currency>, Curve> repoCurves, IDictionary<Pair<LegalEntityGroup, Currency>, Curve> issuerCurves)
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(repoCurves, "repoCurves");
		JodaBeanUtils.notNull(issuerCurves, "issuerCurves");
		this.name = name;
		this.repoCurves = ImmutableMap.copyOf(repoCurves);
		this.issuerCurves = ImmutableMap.copyOf(issuerCurves);
	  }

	  public override LegalEntityCurveGroup.Meta metaBean()
	  {
		return LegalEntityCurveGroup.Meta.INSTANCE;
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
	  /// Gets the repo curves in the curve group, keyed by repo group and currency. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<Pair<RepoGroup, Currency>, Curve> RepoCurves
	  {
		  get
		  {
			return repoCurves;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the issuer curves in the curve group, keyed by legal entity group and currency. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<Pair<LegalEntityGroup, Currency>, Curve> IssuerCurves
	  {
		  get
		  {
			return issuerCurves;
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
		  LegalEntityCurveGroup other = (LegalEntityCurveGroup) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(repoCurves, other.repoCurves) && JodaBeanUtils.equal(issuerCurves, other.issuerCurves);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(repoCurves);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(issuerCurves);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("LegalEntityCurveGroup{");
		buf.Append("name").Append('=').Append(name).Append(',').Append(' ');
		buf.Append("repoCurves").Append('=').Append(repoCurves).Append(',').Append(' ');
		buf.Append("issuerCurves").Append('=').Append(JodaBeanUtils.ToString(issuerCurves));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code LegalEntityCurveGroup}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(LegalEntityCurveGroup), typeof(CurveGroupName));
			  repoCurves_Renamed = DirectMetaProperty.ofImmutable(this, "repoCurves", typeof(LegalEntityCurveGroup), (Type) typeof(ImmutableMap));
			  issuerCurves_Renamed = DirectMetaProperty.ofImmutable(this, "issuerCurves", typeof(LegalEntityCurveGroup), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "repoCurves", "issuerCurves");
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
		/// The meta-property for the {@code repoCurves} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.collect.tuple.Pair<RepoGroup, com.opengamma.strata.basics.currency.Currency>, Curve>> repoCurves = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "repoCurves", LegalEntityCurveGroup.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<Pair<RepoGroup, Currency>, Curve>> repoCurves_Renamed;
		/// <summary>
		/// The meta-property for the {@code issuerCurves} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.collect.tuple.Pair<LegalEntityGroup, com.opengamma.strata.basics.currency.Currency>, Curve>> issuerCurves = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "issuerCurves", LegalEntityCurveGroup.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<Pair<LegalEntityGroup, Currency>, Curve>> issuerCurves_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "repoCurves", "issuerCurves");
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
			case 587630454: // repoCurves
			  return repoCurves_Renamed;
			case -1909076611: // issuerCurves
			  return issuerCurves_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override LegalEntityCurveGroup.Builder builder()
		{
		  return new LegalEntityCurveGroup.Builder();
		}

		public override Type beanType()
		{
		  return typeof(LegalEntityCurveGroup);
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
		/// The meta-property for the {@code repoCurves} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<Pair<RepoGroup, Currency>, Curve>> repoCurves()
		{
		  return repoCurves_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code issuerCurves} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<Pair<LegalEntityGroup, Currency>, Curve>> issuerCurves()
		{
		  return issuerCurves_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((LegalEntityCurveGroup) bean).Name;
			case 587630454: // repoCurves
			  return ((LegalEntityCurveGroup) bean).RepoCurves;
			case -1909076611: // issuerCurves
			  return ((LegalEntityCurveGroup) bean).IssuerCurves;
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
	  /// The bean-builder for {@code LegalEntityCurveGroup}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<LegalEntityCurveGroup>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveGroupName name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IDictionary<Pair<RepoGroup, Currency>, Curve> repoCurves_Renamed = ImmutableMap.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IDictionary<Pair<LegalEntityGroup, Currency>, Curve> issuerCurves_Renamed = ImmutableMap.of();

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(LegalEntityCurveGroup beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.repoCurves_Renamed = beanToCopy.RepoCurves;
		  this.issuerCurves_Renamed = beanToCopy.IssuerCurves;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case 587630454: // repoCurves
			  return repoCurves_Renamed;
			case -1909076611: // issuerCurves
			  return issuerCurves_Renamed;
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
			case 587630454: // repoCurves
			  this.repoCurves_Renamed = (IDictionary<Pair<RepoGroup, Currency>, Curve>) newValue;
			  break;
			case -1909076611: // issuerCurves
			  this.issuerCurves_Renamed = (IDictionary<Pair<LegalEntityGroup, Currency>, Curve>) newValue;
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

		public override LegalEntityCurveGroup build()
		{
		  return new LegalEntityCurveGroup(name_Renamed, repoCurves_Renamed, issuerCurves_Renamed);
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
		/// Sets the repo curves in the curve group, keyed by repo group and currency. </summary>
		/// <param name="repoCurves">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder repoCurves(IDictionary<Pair<RepoGroup, Currency>, Curve> repoCurves)
		{
		  JodaBeanUtils.notNull(repoCurves, "repoCurves");
		  this.repoCurves_Renamed = repoCurves;
		  return this;
		}

		/// <summary>
		/// Sets the issuer curves in the curve group, keyed by legal entity group and currency. </summary>
		/// <param name="issuerCurves">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder issuerCurves(IDictionary<Pair<LegalEntityGroup, Currency>, Curve> issuerCurves)
		{
		  JodaBeanUtils.notNull(issuerCurves, "issuerCurves");
		  this.issuerCurves_Renamed = issuerCurves;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("LegalEntityCurveGroup.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("repoCurves").Append('=').Append(JodaBeanUtils.ToString(repoCurves_Renamed)).Append(',').Append(' ');
		  buf.Append("issuerCurves").Append('=').Append(JodaBeanUtils.ToString(issuerCurves_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}