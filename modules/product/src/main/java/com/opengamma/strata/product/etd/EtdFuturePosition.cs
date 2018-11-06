using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.etd
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using DerivedProperty = org.joda.beans.gen.DerivedProperty;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using SummarizerUtils = com.opengamma.strata.product.common.SummarizerUtils;

	/// <summary>
	/// A position in an ETD future, where the security is embedded ready for mark-to-market pricing.
	/// <para>
	/// This represents a position in a future, defined by long and short quantity.
	/// The future security is embedded directly, however the underlying product model is not available.
	/// </para>
	/// <para>
	/// The net quantity of the position is stored using two fields - {@code longQuantity} and {@code shortQuantity}.
	/// These two fields must not be negative.
	/// In many cases, only a long quantity or short quantity will be present with the other set to zero.
	/// However it is also possible for both to be non-zero, allowing long and short positions to be treated separately.
	/// The net quantity is available via <seealso cref="#getQuantity()"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class EtdFuturePosition implements EtdPosition, com.opengamma.strata.product.ResolvableSecurityPosition, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class EtdFuturePosition : EtdPosition, ResolvableSecurityPosition, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.PositionInfo info;
		private readonly PositionInfo info;
	  /// <summary>
	  /// The underlying security.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final EtdFutureSecurity security;
	  private readonly EtdFutureSecurity security;
	  /// <summary>
	  /// The long quantity of the security.
	  /// <para>
	  /// This is the quantity of the underlying security that is held.
	  /// The quantity cannot be negative, as that would imply short selling.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative", overrideGet = true) private final double longQuantity;
	  private readonly double longQuantity;
	  /// <summary>
	  /// The short quantity of the security.
	  /// <para>
	  /// This is the quantity of the underlying security that has been short sold.
	  /// The quantity cannot be negative, as that would imply the position is long.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative", overrideGet = true) private final double shortQuantity;
	  private readonly double shortQuantity;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the security and net quantity.
	  /// <para>
	  /// The net quantity is the long quantity minus the short quantity, which may be negative.
	  /// If the quantity is positive it is treated as a long quantity.
	  /// Otherwise it is treated as a short quantity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="security">  the underlying security </param>
	  /// <param name="netQuantity">  the net quantity of the underlying security </param>
	  /// <returns> the position </returns>
	  public static EtdFuturePosition ofNet(EtdFutureSecurity security, double netQuantity)
	  {
		return ofNet(PositionInfo.empty(), security, netQuantity);
	  }

	  /// <summary>
	  /// Obtains an instance from position information, security and net quantity.
	  /// <para>
	  /// The net quantity is the long quantity minus the short quantity, which may be negative.
	  /// If the quantity is positive it is treated as a long quantity.
	  /// Otherwise it is treated as a short quantity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="positionInfo">  the position information </param>
	  /// <param name="security">  the underlying security </param>
	  /// <param name="netQuantity">  the net quantity of the underlying security </param>
	  /// <returns> the position </returns>
	  public static EtdFuturePosition ofNet(PositionInfo positionInfo, EtdFutureSecurity security, double netQuantity)
	  {
		double longQuantity = netQuantity >= 0 ? netQuantity : 0;
		double shortQuantity = netQuantity >= 0 ? 0 : -netQuantity;
		return new EtdFuturePosition(positionInfo, security, longQuantity, shortQuantity);
	  }

	  /// <summary>
	  /// Obtains an instance from the security, long quantity and short quantity.
	  /// <para>
	  /// The long quantity and short quantity must be zero or positive, not negative.
	  /// In many cases, only a long quantity or short quantity will be present with the other set to zero.
	  /// However it is also possible for both to be non-zero, allowing long and short positions to be treated separately.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="security">  the underlying security </param>
	  /// <param name="longQuantity">  the long quantity of the underlying security </param>
	  /// <param name="shortQuantity">  the short quantity of the underlying security </param>
	  /// <returns> the position </returns>
	  public static EtdFuturePosition ofLongShort(EtdFutureSecurity security, double longQuantity, double shortQuantity)
	  {
		return ofLongShort(PositionInfo.empty(), security, longQuantity, shortQuantity);
	  }

	  /// <summary>
	  /// Obtains an instance from position information, security, long quantity and short quantity.
	  /// <para>
	  /// The long quantity and short quantity must be zero or positive, not negative.
	  /// In many cases, only a long quantity or short quantity will be present with the other set to zero.
	  /// However it is also possible for both to be non-zero, allowing long and short positions to be treated separately.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="positionInfo">  the position information </param>
	  /// <param name="security">  the underlying security </param>
	  /// <param name="longQuantity">  the long quantity of the underlying security </param>
	  /// <param name="shortQuantity">  the short quantity of the underlying security </param>
	  /// <returns> the position </returns>
	  public static EtdFuturePosition ofLongShort(PositionInfo positionInfo, EtdFutureSecurity security, double longQuantity, double shortQuantity)
	  {

		return new EtdFuturePosition(positionInfo, security, longQuantity, shortQuantity);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.info_Renamed = PositionInfo.empty();
	  }

	  //-------------------------------------------------------------------------
	  public EtdFuturePosition withInfo(PositionInfo info)
	  {
		return new EtdFuturePosition(info, security, longQuantity, shortQuantity);
	  }

	  public EtdFuturePosition withQuantity(double quantity)
	  {
		return EtdFuturePosition.ofNet(info, security, quantity);
	  }

	  //-------------------------------------------------------------------------
	  public PortfolioItemSummary summarize()
	  {
		// F-ECAG-FGBS-201706 x 200, Jun17
		string future = security.summaryDescription();
		string description = SecurityId.StandardId.Value + " x " + SummarizerUtils.value(Quantity) + ", " + future;
		return SummarizerUtils.summary(this, ProductType.ETD_FUTURE, description, Currency);
	  }

	  /// <summary>
	  /// Gets the net quantity of the security.
	  /// <para>
	  /// This returns the <i>net</i> quantity of the underlying security.
	  /// The result is positive if the net position is <i>long</i> and negative
	  /// if the net position is <i>short</i>.
	  /// </para>
	  /// <para>
	  /// This is calculated by subtracting the short quantity from the long quantity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the net quantity of the underlying security </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @DerivedProperty public double getQuantity()
	  public double Quantity
	  {
		  get
		  {
			return longQuantity - shortQuantity;
		  }
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.product.SecuritizedProductPosition<?> resolveTarget(com.opengamma.strata.basics.ReferenceData refData)
	  public SecuritizedProductPosition<object> resolveTarget(ReferenceData refData)
	  {
		SecurityId securityId = SecurityId;
		Security security = refData.getValue(securityId);
		Position position = security.createPosition(Info, LongQuantity, ShortQuantity, refData);
		if (position is SecuritizedProductPosition)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return (com.opengamma.strata.product.SecuritizedProductPosition<?>) position;
		  return (SecuritizedProductPosition<object>) position;
		}
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		throw new System.InvalidCastException(Messages.format("Reference data for security '{}' did not implement SecuritizedProductPosition: ", securityId, position.GetType().FullName));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code EtdFuturePosition}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static EtdFuturePosition.Meta meta()
	  {
		return EtdFuturePosition.Meta.INSTANCE;
	  }

	  static EtdFuturePosition()
	  {
		MetaBean.register(EtdFuturePosition.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static EtdFuturePosition.Builder builder()
	  {
		return new EtdFuturePosition.Builder();
	  }

	  private EtdFuturePosition(PositionInfo info, EtdFutureSecurity security, double longQuantity, double shortQuantity)
	  {
		JodaBeanUtils.notNull(info, "info");
		JodaBeanUtils.notNull(security, "security");
		ArgChecker.notNegative(longQuantity, "longQuantity");
		ArgChecker.notNegative(shortQuantity, "shortQuantity");
		this.info = info;
		this.security = security;
		this.longQuantity = longQuantity;
		this.shortQuantity = shortQuantity;
	  }

	  public override EtdFuturePosition.Meta metaBean()
	  {
		return EtdFuturePosition.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the additional position information, defaulted to an empty instance.
	  /// <para>
	  /// This allows additional information to be attached to the position.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PositionInfo Info
	  {
		  get
		  {
			return info;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying security. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public EtdFutureSecurity Security
	  {
		  get
		  {
			return security;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the long quantity of the security.
	  /// <para>
	  /// This is the quantity of the underlying security that is held.
	  /// The quantity cannot be negative, as that would imply short selling.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double LongQuantity
	  {
		  get
		  {
			return longQuantity;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the short quantity of the security.
	  /// <para>
	  /// This is the quantity of the underlying security that has been short sold.
	  /// The quantity cannot be negative, as that would imply the position is long.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double ShortQuantity
	  {
		  get
		  {
			return shortQuantity;
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
		  EtdFuturePosition other = (EtdFuturePosition) obj;
		  return JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(security, other.security) && JodaBeanUtils.equal(longQuantity, other.longQuantity) && JodaBeanUtils.equal(shortQuantity, other.shortQuantity);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(security);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(longQuantity);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(shortQuantity);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("EtdFuturePosition{");
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("security").Append('=').Append(security).Append(',').Append(' ');
		buf.Append("longQuantity").Append('=').Append(longQuantity).Append(',').Append(' ');
		buf.Append("shortQuantity").Append('=').Append(JodaBeanUtils.ToString(shortQuantity));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code EtdFuturePosition}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(EtdFuturePosition), typeof(PositionInfo));
			  security_Renamed = DirectMetaProperty.ofImmutable(this, "security", typeof(EtdFuturePosition), typeof(EtdFutureSecurity));
			  longQuantity_Renamed = DirectMetaProperty.ofImmutable(this, "longQuantity", typeof(EtdFuturePosition), Double.TYPE);
			  shortQuantity_Renamed = DirectMetaProperty.ofImmutable(this, "shortQuantity", typeof(EtdFuturePosition), Double.TYPE);
			  quantity_Renamed = DirectMetaProperty.ofDerived(this, "quantity", typeof(EtdFuturePosition), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "info", "security", "longQuantity", "shortQuantity", "quantity");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code info} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PositionInfo> info_Renamed;
		/// <summary>
		/// The meta-property for the {@code security} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<EtdFutureSecurity> security_Renamed;
		/// <summary>
		/// The meta-property for the {@code longQuantity} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> longQuantity_Renamed;
		/// <summary>
		/// The meta-property for the {@code shortQuantity} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> shortQuantity_Renamed;
		/// <summary>
		/// The meta-property for the {@code quantity} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> quantity_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "info", "security", "longQuantity", "shortQuantity", "quantity");
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
			case 3237038: // info
			  return info_Renamed;
			case 949122880: // security
			  return security_Renamed;
			case 611668775: // longQuantity
			  return longQuantity_Renamed;
			case -2094395097: // shortQuantity
			  return shortQuantity_Renamed;
			case -1285004149: // quantity
			  return quantity_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override EtdFuturePosition.Builder builder()
		{
		  return new EtdFuturePosition.Builder();
		}

		public override Type beanType()
		{
		  return typeof(EtdFuturePosition);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code info} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PositionInfo> info()
		{
		  return info_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code security} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<EtdFutureSecurity> security()
		{
		  return security_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code longQuantity} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> longQuantity()
		{
		  return longQuantity_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code shortQuantity} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> shortQuantity()
		{
		  return shortQuantity_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code quantity} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> quantity()
		{
		  return quantity_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return ((EtdFuturePosition) bean).Info;
			case 949122880: // security
			  return ((EtdFuturePosition) bean).Security;
			case 611668775: // longQuantity
			  return ((EtdFuturePosition) bean).LongQuantity;
			case -2094395097: // shortQuantity
			  return ((EtdFuturePosition) bean).ShortQuantity;
			case -1285004149: // quantity
			  return ((EtdFuturePosition) bean).Quantity;
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
	  /// The bean-builder for {@code EtdFuturePosition}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<EtdFuturePosition>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PositionInfo info_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal EtdFutureSecurity security_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double longQuantity_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double shortQuantity_Renamed;

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
		internal Builder(EtdFuturePosition beanToCopy)
		{
		  this.info_Renamed = beanToCopy.Info;
		  this.security_Renamed = beanToCopy.Security;
		  this.longQuantity_Renamed = beanToCopy.LongQuantity;
		  this.shortQuantity_Renamed = beanToCopy.ShortQuantity;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return info_Renamed;
			case 949122880: // security
			  return security_Renamed;
			case 611668775: // longQuantity
			  return longQuantity_Renamed;
			case -2094395097: // shortQuantity
			  return shortQuantity_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  this.info_Renamed = (PositionInfo) newValue;
			  break;
			case 949122880: // security
			  this.security_Renamed = (EtdFutureSecurity) newValue;
			  break;
			case 611668775: // longQuantity
			  this.longQuantity_Renamed = (double?) newValue.Value;
			  break;
			case -2094395097: // shortQuantity
			  this.shortQuantity_Renamed = (double?) newValue.Value;
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

		public override EtdFuturePosition build()
		{
		  return new EtdFuturePosition(info_Renamed, security_Renamed, longQuantity_Renamed, shortQuantity_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the additional position information, defaulted to an empty instance.
		/// <para>
		/// This allows additional information to be attached to the position.
		/// </para>
		/// </summary>
		/// <param name="info">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder info(PositionInfo info)
		{
		  JodaBeanUtils.notNull(info, "info");
		  this.info_Renamed = info;
		  return this;
		}

		/// <summary>
		/// Sets the underlying security. </summary>
		/// <param name="security">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder security(EtdFutureSecurity security)
		{
		  JodaBeanUtils.notNull(security, "security");
		  this.security_Renamed = security;
		  return this;
		}

		/// <summary>
		/// Sets the long quantity of the security.
		/// <para>
		/// This is the quantity of the underlying security that is held.
		/// The quantity cannot be negative, as that would imply short selling.
		/// </para>
		/// </summary>
		/// <param name="longQuantity">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder longQuantity(double longQuantity)
		{
		  ArgChecker.notNegative(longQuantity, "longQuantity");
		  this.longQuantity_Renamed = longQuantity;
		  return this;
		}

		/// <summary>
		/// Sets the short quantity of the security.
		/// <para>
		/// This is the quantity of the underlying security that has been short sold.
		/// The quantity cannot be negative, as that would imply the position is long.
		/// </para>
		/// </summary>
		/// <param name="shortQuantity">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder shortQuantity(double shortQuantity)
		{
		  ArgChecker.notNegative(shortQuantity, "shortQuantity");
		  this.shortQuantity_Renamed = shortQuantity;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("EtdFuturePosition.Builder{");
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info_Renamed)).Append(',').Append(' ');
		  buf.Append("security").Append('=').Append(JodaBeanUtils.ToString(security_Renamed)).Append(',').Append(' ');
		  buf.Append("longQuantity").Append('=').Append(JodaBeanUtils.ToString(longQuantity_Renamed)).Append(',').Append(' ');
		  buf.Append("shortQuantity").Append('=').Append(JodaBeanUtils.ToString(shortQuantity_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}