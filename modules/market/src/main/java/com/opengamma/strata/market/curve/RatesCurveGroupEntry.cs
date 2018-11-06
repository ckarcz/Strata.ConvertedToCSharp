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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Sets = com.google.common.collect.Sets;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Index = com.opengamma.strata.basics.index.Index;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// A single entry in the curve group definition.
	/// <para>
	/// Each entry stores the definition of a single curve and how it is to be used.
	/// This structure allows the curve itself to be used for multiple purposes.
	/// </para>
	/// <para>
	/// The currencies are used to specify that the curve is to be used as a discount curve.
	/// The indices are used to specify that the curve is to be used as a forward curve.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class RatesCurveGroupEntry implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class RatesCurveGroupEntry : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final CurveName curveName;
		private readonly CurveName curveName;
	  /// <summary>
	  /// The currencies for which the curve provides discount rates.
	  /// This is empty if the curve is not used for Ibor rates.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableSet<com.opengamma.strata.basics.currency.Currency> discountCurrencies;
	  private readonly ImmutableSet<Currency> discountCurrencies;
	  /// <summary>
	  /// The indices for which the curve provides forward rates.
	  /// This is empty if the curve is not used for forward rates.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableSet<com.opengamma.strata.basics.index.Index> indices;
	  private readonly ImmutableSet<Index> indices;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Merges the specified entry with this entry, returning a new entry.
	  /// <para>
	  /// The two entries must have the same curve name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="newEntry">  the new entry </param>
	  /// <returns> the merged entry </returns>
	  internal RatesCurveGroupEntry merge(RatesCurveGroupEntry newEntry)
	  {
		if (!curveName.Equals(newEntry.curveName))
		{
		  throw new System.ArgumentException(Messages.format("A CurveGroupEntry can only be merged with an entry with the same curve name. name: {}, other name: {}", curveName, newEntry.curveName));
		}
		return RatesCurveGroupEntry.builder().curveName(curveName).discountCurrencies(Sets.union(discountCurrencies, newEntry.discountCurrencies)).indices(Sets.union(indices, newEntry.indices)).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the subset of indices matching the specified type for which the curve provides forward rates.
	  /// <para>
	  /// The set of indices is filtered and cast using the specified type.
	  /// This is empty if the curve is not used for forward rates.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of index </param>
	  /// <param name="indexType">  the type of index required </param>
	  /// <returns> the subset of indices </returns>
	  public ImmutableSet<T> getIndices<T>(Type<T> indexType) where T : com.opengamma.strata.basics.index.Index
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return indices.Where(indexType.isInstance).Select(indexType.cast).collect(toImmutableSet());
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code RatesCurveGroupEntry}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static RatesCurveGroupEntry.Meta meta()
	  {
		return RatesCurveGroupEntry.Meta.INSTANCE;
	  }

	  static RatesCurveGroupEntry()
	  {
		MetaBean.register(RatesCurveGroupEntry.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static RatesCurveGroupEntry.Builder builder()
	  {
		return new RatesCurveGroupEntry.Builder();
	  }

	  private RatesCurveGroupEntry(CurveName curveName, ISet<Currency> discountCurrencies, ISet<Index> indices)
	  {
		JodaBeanUtils.notNull(curveName, "curveName");
		JodaBeanUtils.notNull(discountCurrencies, "discountCurrencies");
		JodaBeanUtils.notNull(indices, "indices");
		this.curveName = curveName;
		this.discountCurrencies = ImmutableSet.copyOf(discountCurrencies);
		this.indices = ImmutableSet.copyOf(indices);
	  }

	  public override RatesCurveGroupEntry.Meta metaBean()
	  {
		return RatesCurveGroupEntry.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the curve name. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveName CurveName
	  {
		  get
		  {
			return curveName;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currencies for which the curve provides discount rates.
	  /// This is empty if the curve is not used for Ibor rates. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableSet<Currency> DiscountCurrencies
	  {
		  get
		  {
			return discountCurrencies;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the indices for which the curve provides forward rates.
	  /// This is empty if the curve is not used for forward rates. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableSet<Index> Indices
	  {
		  get
		  {
			return indices;
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
		  RatesCurveGroupEntry other = (RatesCurveGroupEntry) obj;
		  return JodaBeanUtils.equal(curveName, other.curveName) && JodaBeanUtils.equal(discountCurrencies, other.discountCurrencies) && JodaBeanUtils.equal(indices, other.indices);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(curveName);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(discountCurrencies);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(indices);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("RatesCurveGroupEntry{");
		buf.Append("curveName").Append('=').Append(curveName).Append(',').Append(' ');
		buf.Append("discountCurrencies").Append('=').Append(discountCurrencies).Append(',').Append(' ');
		buf.Append("indices").Append('=').Append(JodaBeanUtils.ToString(indices));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code RatesCurveGroupEntry}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  curveName_Renamed = DirectMetaProperty.ofImmutable(this, "curveName", typeof(RatesCurveGroupEntry), typeof(CurveName));
			  discountCurrencies_Renamed = DirectMetaProperty.ofImmutable(this, "discountCurrencies", typeof(RatesCurveGroupEntry), (Type) typeof(ImmutableSet));
			  indices_Renamed = DirectMetaProperty.ofImmutable(this, "indices", typeof(RatesCurveGroupEntry), (Type) typeof(ImmutableSet));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "curveName", "discountCurrencies", "indices");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code curveName} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveName> curveName_Renamed;
		/// <summary>
		/// The meta-property for the {@code discountCurrencies} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableSet<com.opengamma.strata.basics.currency.Currency>> discountCurrencies = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "discountCurrencies", RatesCurveGroupEntry.class, (Class) com.google.common.collect.ImmutableSet.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableSet<Currency>> discountCurrencies_Renamed;
		/// <summary>
		/// The meta-property for the {@code indices} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableSet<com.opengamma.strata.basics.index.Index>> indices = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "indices", RatesCurveGroupEntry.class, (Class) com.google.common.collect.ImmutableSet.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableSet<Index>> indices_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "curveName", "discountCurrencies", "indices");
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
			case 771153946: // curveName
			  return curveName_Renamed;
			case -538086256: // discountCurrencies
			  return discountCurrencies_Renamed;
			case 1943391143: // indices
			  return indices_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override RatesCurveGroupEntry.Builder builder()
		{
		  return new RatesCurveGroupEntry.Builder();
		}

		public override Type beanType()
		{
		  return typeof(RatesCurveGroupEntry);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code curveName} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveName> curveName()
		{
		  return curveName_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code discountCurrencies} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableSet<Currency>> discountCurrencies()
		{
		  return discountCurrencies_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code indices} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableSet<Index>> indices()
		{
		  return indices_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 771153946: // curveName
			  return ((RatesCurveGroupEntry) bean).CurveName;
			case -538086256: // discountCurrencies
			  return ((RatesCurveGroupEntry) bean).DiscountCurrencies;
			case 1943391143: // indices
			  return ((RatesCurveGroupEntry) bean).Indices;
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
	  /// The bean-builder for {@code RatesCurveGroupEntry}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<RatesCurveGroupEntry>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveName curveName_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ISet<Currency> discountCurrencies_Renamed = ImmutableSet.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ISet<Index> indices_Renamed = ImmutableSet.of();

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(RatesCurveGroupEntry beanToCopy)
		{
		  this.curveName_Renamed = beanToCopy.CurveName;
		  this.discountCurrencies_Renamed = beanToCopy.DiscountCurrencies;
		  this.indices_Renamed = beanToCopy.Indices;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 771153946: // curveName
			  return curveName_Renamed;
			case -538086256: // discountCurrencies
			  return discountCurrencies_Renamed;
			case 1943391143: // indices
			  return indices_Renamed;
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
			case 771153946: // curveName
			  this.curveName_Renamed = (CurveName) newValue;
			  break;
			case -538086256: // discountCurrencies
			  this.discountCurrencies_Renamed = (ISet<Currency>) newValue;
			  break;
			case 1943391143: // indices
			  this.indices_Renamed = (ISet<Index>) newValue;
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

		public override RatesCurveGroupEntry build()
		{
		  return new RatesCurveGroupEntry(curveName_Renamed, discountCurrencies_Renamed, indices_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the curve name. </summary>
		/// <param name="curveName">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder curveName(CurveName curveName)
		{
		  JodaBeanUtils.notNull(curveName, "curveName");
		  this.curveName_Renamed = curveName;
		  return this;
		}

		/// <summary>
		/// Sets the currencies for which the curve provides discount rates.
		/// This is empty if the curve is not used for Ibor rates. </summary>
		/// <param name="discountCurrencies">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder discountCurrencies(ISet<Currency> discountCurrencies)
		{
		  JodaBeanUtils.notNull(discountCurrencies, "discountCurrencies");
		  this.discountCurrencies_Renamed = discountCurrencies;
		  return this;
		}

		/// <summary>
		/// Sets the {@code discountCurrencies} property in the builder
		/// from an array of objects. </summary>
		/// <param name="discountCurrencies">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder discountCurrencies(params Currency[] discountCurrencies)
		{
		  return this.discountCurrencies(ImmutableSet.copyOf(discountCurrencies));
		}

		/// <summary>
		/// Sets the indices for which the curve provides forward rates.
		/// This is empty if the curve is not used for forward rates. </summary>
		/// <param name="indices">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder indices(ISet<Index> indices)
		{
		  JodaBeanUtils.notNull(indices, "indices");
		  this.indices_Renamed = indices;
		  return this;
		}

		/// <summary>
		/// Sets the {@code indices} property in the builder
		/// from an array of objects. </summary>
		/// <param name="indices">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder indices(params Index[] indices)
		{
		  return this.indices(ImmutableSet.copyOf(indices));
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("RatesCurveGroupEntry.Builder{");
		  buf.Append("curveName").Append('=').Append(JodaBeanUtils.ToString(curveName_Renamed)).Append(',').Append(' ');
		  buf.Append("discountCurrencies").Append('=').Append(JodaBeanUtils.ToString(discountCurrencies_Renamed)).Append(',').Append(' ');
		  buf.Append("indices").Append('=').Append(JodaBeanUtils.ToString(indices_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}