using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using Strike = com.opengamma.strata.market.option.Strike;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Surface node metadata for a surface node with a specific time to expiry and strike.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class FxVolatilitySurfaceYearFractionParameterMetadata implements com.opengamma.strata.market.param.ParameterMetadata, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxVolatilitySurfaceYearFractionParameterMetadata : ParameterMetadata, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double yearFraction;
		private readonly double yearFraction;
	  /// <summary>
	  /// The strike of the surface node.
	  /// <para>
	  /// This is the strike that the node on the surface is defined as.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.option.Strike strike;
	  private readonly Strike strike;
	  /// <summary>
	  /// The currency pair that describes the node.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.CurrencyPair currencyPair;
	  private readonly CurrencyPair currencyPair;
	  /// <summary>
	  /// The label that describes the node.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty", overrideGet = true) private final String label;
	  private readonly string label;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates node metadata using year fraction, strike and currency pair.
	  /// </summary>
	  /// <param name="yearFraction">  the year fraction </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <returns> node metadata  </returns>
	  public static FxVolatilitySurfaceYearFractionParameterMetadata of(double yearFraction, Strike strike, CurrencyPair currencyPair)
	  {

		string label = Pair.of(yearFraction, strike.Label).ToString();
		return new FxVolatilitySurfaceYearFractionParameterMetadata(yearFraction, strike, currencyPair, label);
	  }

	  /// <summary>
	  /// Creates node using year fraction, strike, label and currency pair.
	  /// </summary>
	  /// <param name="yearFraction">  the year fraction </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="label">  the label to use </param>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <returns> the metadata </returns>
	  public static FxVolatilitySurfaceYearFractionParameterMetadata of(double yearFraction, Strike strike, string label, CurrencyPair currencyPair)
	  {

		return new FxVolatilitySurfaceYearFractionParameterMetadata(yearFraction, strike, currencyPair, label);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (string.ReferenceEquals(builder.label, null) && builder.strike != null)
		{
		  builder.label = Pair.of(builder.yearFraction, builder.strike.Label).ToString();
		}
	  }

	  public Pair<double, Strike> Identifier
	  {
		  get
		  {
			return Pair.of(yearFraction, strike);
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxVolatilitySurfaceYearFractionParameterMetadata}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxVolatilitySurfaceYearFractionParameterMetadata.Meta meta()
	  {
		return FxVolatilitySurfaceYearFractionParameterMetadata.Meta.INSTANCE;
	  }

	  static FxVolatilitySurfaceYearFractionParameterMetadata()
	  {
		MetaBean.register(FxVolatilitySurfaceYearFractionParameterMetadata.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private FxVolatilitySurfaceYearFractionParameterMetadata(double yearFraction, Strike strike, CurrencyPair currencyPair, string label)
	  {
		JodaBeanUtils.notNull(strike, "strike");
		JodaBeanUtils.notNull(currencyPair, "currencyPair");
		JodaBeanUtils.notEmpty(label, "label");
		this.yearFraction = yearFraction;
		this.strike = strike;
		this.currencyPair = currencyPair;
		this.label = label;
	  }

	  public override FxVolatilitySurfaceYearFractionParameterMetadata.Meta metaBean()
	  {
		return FxVolatilitySurfaceYearFractionParameterMetadata.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the year fraction of the surface node.
	  /// <para>
	  /// This is the time to expiry that the node on the surface is defined as.
	  /// There is not necessarily a direct relationship with a date from an underlying instrument.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double YearFraction
	  {
		  get
		  {
			return yearFraction;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the strike of the surface node.
	  /// <para>
	  /// This is the strike that the node on the surface is defined as.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Strike Strike
	  {
		  get
		  {
			return strike;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency pair that describes the node. </summary>
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
	  /// Gets the label that describes the node. </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public string Label
	  {
		  get
		  {
			return label;
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
		  FxVolatilitySurfaceYearFractionParameterMetadata other = (FxVolatilitySurfaceYearFractionParameterMetadata) obj;
		  return JodaBeanUtils.equal(yearFraction, other.yearFraction) && JodaBeanUtils.equal(strike, other.strike) && JodaBeanUtils.equal(currencyPair, other.currencyPair) && JodaBeanUtils.equal(label, other.label);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yearFraction);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strike);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currencyPair);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(label);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("FxVolatilitySurfaceYearFractionParameterMetadata{");
		buf.Append("yearFraction").Append('=').Append(yearFraction).Append(',').Append(' ');
		buf.Append("strike").Append('=').Append(strike).Append(',').Append(' ');
		buf.Append("currencyPair").Append('=').Append(currencyPair).Append(',').Append(' ');
		buf.Append("label").Append('=').Append(JodaBeanUtils.ToString(label));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxVolatilitySurfaceYearFractionParameterMetadata}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  yearFraction_Renamed = DirectMetaProperty.ofImmutable(this, "yearFraction", typeof(FxVolatilitySurfaceYearFractionParameterMetadata), Double.TYPE);
			  strike_Renamed = DirectMetaProperty.ofImmutable(this, "strike", typeof(FxVolatilitySurfaceYearFractionParameterMetadata), typeof(Strike));
			  currencyPair_Renamed = DirectMetaProperty.ofImmutable(this, "currencyPair", typeof(FxVolatilitySurfaceYearFractionParameterMetadata), typeof(CurrencyPair));
			  label_Renamed = DirectMetaProperty.ofImmutable(this, "label", typeof(FxVolatilitySurfaceYearFractionParameterMetadata), typeof(string));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "yearFraction", "strike", "currencyPair", "label");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code yearFraction} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> yearFraction_Renamed;
		/// <summary>
		/// The meta-property for the {@code strike} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Strike> strike_Renamed;
		/// <summary>
		/// The meta-property for the {@code currencyPair} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyPair> currencyPair_Renamed;
		/// <summary>
		/// The meta-property for the {@code label} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> label_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "yearFraction", "strike", "currencyPair", "label");
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
			case -1731780257: // yearFraction
			  return yearFraction_Renamed;
			case -891985998: // strike
			  return strike_Renamed;
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case 102727412: // label
			  return label_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends FxVolatilitySurfaceYearFractionParameterMetadata> builder()
		public override BeanBuilder<FxVolatilitySurfaceYearFractionParameterMetadata> builder()
		{
		  return new FxVolatilitySurfaceYearFractionParameterMetadata.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxVolatilitySurfaceYearFractionParameterMetadata);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code yearFraction} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> yearFraction()
		{
		  return yearFraction_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code strike} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Strike> strike()
		{
		  return strike_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currencyPair} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyPair> currencyPair()
		{
		  return currencyPair_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code label} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> label()
		{
		  return label_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1731780257: // yearFraction
			  return ((FxVolatilitySurfaceYearFractionParameterMetadata) bean).YearFraction;
			case -891985998: // strike
			  return ((FxVolatilitySurfaceYearFractionParameterMetadata) bean).Strike;
			case 1005147787: // currencyPair
			  return ((FxVolatilitySurfaceYearFractionParameterMetadata) bean).CurrencyPair;
			case 102727412: // label
			  return ((FxVolatilitySurfaceYearFractionParameterMetadata) bean).Label;
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
	  /// The bean-builder for {@code FxVolatilitySurfaceYearFractionParameterMetadata}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<FxVolatilitySurfaceYearFractionParameterMetadata>
	  {

		internal double yearFraction;
		internal Strike strike;
		internal CurrencyPair currencyPair;
		internal string label;

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
			case -1731780257: // yearFraction
			  return yearFraction;
			case -891985998: // strike
			  return strike;
			case 1005147787: // currencyPair
			  return currencyPair;
			case 102727412: // label
			  return label;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1731780257: // yearFraction
			  this.yearFraction = (double?) newValue.Value;
			  break;
			case -891985998: // strike
			  this.strike = (Strike) newValue;
			  break;
			case 1005147787: // currencyPair
			  this.currencyPair = (CurrencyPair) newValue;
			  break;
			case 102727412: // label
			  this.label = (string) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override FxVolatilitySurfaceYearFractionParameterMetadata build()
		{
		  preBuild(this);
		  return new FxVolatilitySurfaceYearFractionParameterMetadata(yearFraction, strike, currencyPair, label);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("FxVolatilitySurfaceYearFractionParameterMetadata.Builder{");
		  buf.Append("yearFraction").Append('=').Append(JodaBeanUtils.ToString(yearFraction)).Append(',').Append(' ');
		  buf.Append("strike").Append('=').Append(JodaBeanUtils.ToString(strike)).Append(',').Append(' ');
		  buf.Append("currencyPair").Append('=').Append(JodaBeanUtils.ToString(currencyPair)).Append(',').Append(' ');
		  buf.Append("label").Append('=').Append(JodaBeanUtils.ToString(label));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}