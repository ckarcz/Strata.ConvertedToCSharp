using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
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

	/// <summary>
	/// A position in a security, where the security is referenced by identifier.
	/// <para>
	/// This represents a position in a security, defined by long and short quantity.
	/// The security is referenced using <seealso cref="SecurityId"/>.
	/// The identifier may be looked up in <seealso cref="ReferenceData"/>.
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
//ORIGINAL LINE: @BeanDefinition(constructorScope = "package") public final class SecurityPosition implements ResolvableSecurityPosition, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class SecurityPosition : ResolvableSecurityPosition, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final PositionInfo info;
		private readonly PositionInfo info;
	  /// <summary>
	  /// The identifier of the underlying security.
	  /// <para>
	  /// This identifier uniquely identifies the security within the system.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final SecurityId securityId;
	  private readonly SecurityId securityId;
	  /// <summary>
	  /// The long quantity of the security.
	  /// <para>
	  /// This is the quantity of the underlying security that is held.
	  /// The quantity cannot be negative, as that would imply short selling.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final double longQuantity;
	  private readonly double longQuantity;
	  /// <summary>
	  /// The quantity that was traded.
	  /// <para>
	  /// This is the quantity of the underlying security that has been short sold.
	  /// The quantity cannot be negative, as that would imply the position is long.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final double shortQuantity;
	  private readonly double shortQuantity;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the security identifier and net quantity.
	  /// <para>
	  /// The net quantity is the long quantity minus the short quantity, which may be negative.
	  /// If the quantity is positive it is treated as a long quantity.
	  /// Otherwise it is treated as a short quantity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="securityId">  the identifier of the underlying security </param>
	  /// <param name="netQuantity">  the net quantity of the underlying security </param>
	  /// <returns> the position </returns>
	  public static SecurityPosition ofNet(SecurityId securityId, double netQuantity)
	  {
		return ofNet(PositionInfo.empty(), securityId, netQuantity);
	  }

	  /// <summary>
	  /// Obtains an instance from position information, security identifier and net quantity.
	  /// <para>
	  /// The net quantity is the long quantity minus the short quantity, which may be negative.
	  /// If the quantity is positive it is treated as a long quantity.
	  /// Otherwise it is treated as a short quantity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="positionInfo">  the position information </param>
	  /// <param name="securityId">  the identifier of the underlying security </param>
	  /// <param name="netQuantity">  the net quantity of the underlying security </param>
	  /// <returns> the position </returns>
	  public static SecurityPosition ofNet(PositionInfo positionInfo, SecurityId securityId, double netQuantity)
	  {
		double longQuantity = netQuantity >= 0 ? netQuantity : 0;
		double shortQuantity = netQuantity >= 0 ? 0 : -netQuantity;
		return new SecurityPosition(positionInfo, securityId, longQuantity, shortQuantity);
	  }

	  /// <summary>
	  /// Obtains an instance from the security identifier, long quantity and short quantity.
	  /// <para>
	  /// The long quantity and short quantity must be zero or positive, not negative.
	  /// In many cases, only a long quantity or short quantity will be present with the other set to zero.
	  /// However it is also possible for both to be non-zero, allowing long and short positions to be treated separately.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="securityId">  the identifier of the underlying security </param>
	  /// <param name="longQuantity">  the long quantity of the underlying security </param>
	  /// <param name="shortQuantity">  the short quantity of the underlying security </param>
	  /// <returns> the position </returns>
	  public static SecurityPosition ofLongShort(SecurityId securityId, double longQuantity, double shortQuantity)
	  {
		return ofLongShort(PositionInfo.empty(), securityId, longQuantity, shortQuantity);
	  }

	  /// <summary>
	  /// Obtains an instance from position information, security identifier, long quantity and short quantity.
	  /// <para>
	  /// The long quantity and short quantity must be zero or positive, not negative.
	  /// In many cases, only a long quantity or short quantity will be present with the other set to zero.
	  /// However it is also possible for both to be non-zero, allowing long and short positions to be treated separately.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="positionInfo">  the position information </param>
	  /// <param name="securityId">  the identifier of the underlying security </param>
	  /// <param name="longQuantity">  the long quantity of the underlying security </param>
	  /// <param name="shortQuantity">  the short quantity of the underlying security </param>
	  /// <returns> the position </returns>
	  public static SecurityPosition ofLongShort(PositionInfo positionInfo, SecurityId securityId, double longQuantity, double shortQuantity)
	  {

		return new SecurityPosition(positionInfo, securityId, longQuantity, shortQuantity);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.info_Renamed = PositionInfo.empty();
	  }

	  //-------------------------------------------------------------------------
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

	  //-------------------------------------------------------------------------
	  public SecurityPosition withInfo(PositionInfo info)
	  {
		return new SecurityPosition(info, securityId, longQuantity, shortQuantity);
	  }

	  public SecurityPosition withQuantity(double quantity)
	  {
		return SecurityPosition.ofNet(info, securityId, quantity);
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public SecuritizedProductPosition<?> resolveTarget(com.opengamma.strata.basics.ReferenceData refData)
	  public SecuritizedProductPosition<object> resolveTarget(ReferenceData refData)
	  {
		SecurityId securityId = SecurityId;
		Security security = refData.getValue(securityId);
		Position position = security.createPosition(Info, LongQuantity, ShortQuantity, refData);
		if (position is SecuritizedProductPosition)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return (SecuritizedProductPosition<?>) position;
		  return (SecuritizedProductPosition<object>) position;
		}
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		throw new System.InvalidCastException(Messages.format("Reference data for security '{}' did not implement SecuritizedProductPosition: ", securityId, position.GetType().FullName));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SecurityPosition}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static SecurityPosition.Meta meta()
	  {
		return SecurityPosition.Meta.INSTANCE;
	  }

	  static SecurityPosition()
	  {
		MetaBean.register(SecurityPosition.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static SecurityPosition.Builder builder()
	  {
		return new SecurityPosition.Builder();
	  }

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="info">  the value of the property, not null </param>
	  /// <param name="securityId">  the value of the property, not null </param>
	  /// <param name="longQuantity">  the value of the property </param>
	  /// <param name="shortQuantity">  the value of the property </param>
	  internal SecurityPosition(PositionInfo info, SecurityId securityId, double longQuantity, double shortQuantity)
	  {
		JodaBeanUtils.notNull(info, "info");
		JodaBeanUtils.notNull(securityId, "securityId");
		ArgChecker.notNegative(longQuantity, "longQuantity");
		ArgChecker.notNegative(shortQuantity, "shortQuantity");
		this.info = info;
		this.securityId = securityId;
		this.longQuantity = longQuantity;
		this.shortQuantity = shortQuantity;
	  }

	  public override SecurityPosition.Meta metaBean()
	  {
		return SecurityPosition.Meta.INSTANCE;
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
	  /// Gets the identifier of the underlying security.
	  /// <para>
	  /// This identifier uniquely identifies the security within the system.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SecurityId SecurityId
	  {
		  get
		  {
			return securityId;
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
	  /// Gets the quantity that was traded.
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
		  SecurityPosition other = (SecurityPosition) obj;
		  return JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(securityId, other.securityId) && JodaBeanUtils.equal(longQuantity, other.longQuantity) && JodaBeanUtils.equal(shortQuantity, other.shortQuantity);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(securityId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(longQuantity);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(shortQuantity);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("SecurityPosition{");
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("securityId").Append('=').Append(securityId).Append(',').Append(' ');
		buf.Append("longQuantity").Append('=').Append(longQuantity).Append(',').Append(' ');
		buf.Append("shortQuantity").Append('=').Append(JodaBeanUtils.ToString(shortQuantity));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code SecurityPosition}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(SecurityPosition), typeof(PositionInfo));
			  securityId_Renamed = DirectMetaProperty.ofImmutable(this, "securityId", typeof(SecurityPosition), typeof(SecurityId));
			  longQuantity_Renamed = DirectMetaProperty.ofImmutable(this, "longQuantity", typeof(SecurityPosition), Double.TYPE);
			  shortQuantity_Renamed = DirectMetaProperty.ofImmutable(this, "shortQuantity", typeof(SecurityPosition), Double.TYPE);
			  quantity_Renamed = DirectMetaProperty.ofDerived(this, "quantity", typeof(SecurityPosition), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "info", "securityId", "longQuantity", "shortQuantity", "quantity");
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
		/// The meta-property for the {@code securityId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SecurityId> securityId_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "info", "securityId", "longQuantity", "shortQuantity", "quantity");
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
			case 1574023291: // securityId
			  return securityId_Renamed;
			case 611668775: // longQuantity
			  return longQuantity_Renamed;
			case -2094395097: // shortQuantity
			  return shortQuantity_Renamed;
			case -1285004149: // quantity
			  return quantity_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override SecurityPosition.Builder builder()
		{
		  return new SecurityPosition.Builder();
		}

		public override Type beanType()
		{
		  return typeof(SecurityPosition);
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
		/// The meta-property for the {@code securityId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SecurityId> securityId()
		{
		  return securityId_Renamed;
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
			  return ((SecurityPosition) bean).Info;
			case 1574023291: // securityId
			  return ((SecurityPosition) bean).SecurityId;
			case 611668775: // longQuantity
			  return ((SecurityPosition) bean).LongQuantity;
			case -2094395097: // shortQuantity
			  return ((SecurityPosition) bean).ShortQuantity;
			case -1285004149: // quantity
			  return ((SecurityPosition) bean).Quantity;
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
	  /// The bean-builder for {@code SecurityPosition}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<SecurityPosition>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PositionInfo info_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SecurityId securityId_Renamed;
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
		internal Builder(SecurityPosition beanToCopy)
		{
		  this.info_Renamed = beanToCopy.Info;
		  this.securityId_Renamed = beanToCopy.SecurityId;
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
			case 1574023291: // securityId
			  return securityId_Renamed;
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
			case 1574023291: // securityId
			  this.securityId_Renamed = (SecurityId) newValue;
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

		public override SecurityPosition build()
		{
		  return new SecurityPosition(info_Renamed, securityId_Renamed, longQuantity_Renamed, shortQuantity_Renamed);
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
		/// Sets the identifier of the underlying security.
		/// <para>
		/// This identifier uniquely identifies the security within the system.
		/// </para>
		/// </summary>
		/// <param name="securityId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder securityId(SecurityId securityId)
		{
		  JodaBeanUtils.notNull(securityId, "securityId");
		  this.securityId_Renamed = securityId;
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
		/// Sets the quantity that was traded.
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
		  buf.Append("SecurityPosition.Builder{");
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info_Renamed)).Append(',').Append(' ');
		  buf.Append("securityId").Append('=').Append(JodaBeanUtils.ToString(securityId_Renamed)).Append(',').Append(' ');
		  buf.Append("longQuantity").Append('=').Append(JodaBeanUtils.ToString(longQuantity_Renamed)).Append(',').Append(' ');
		  buf.Append("shortQuantity").Append('=').Append(JodaBeanUtils.ToString(shortQuantity_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}