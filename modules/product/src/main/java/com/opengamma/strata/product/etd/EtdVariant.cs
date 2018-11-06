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
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// The variant of an exchange traded derivative (ETD).
	/// <para>
	/// Most ETDs are monthly, where there is one expiry date per month, but a few are issued weekly or daily.
	/// </para>
	/// <para>
	/// A special category of ETD are <i>flex</i> futures and options.
	/// These have additional contract flexibility, with a settlement type and option type.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", metaScope = "private") public final class EtdVariant implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class EtdVariant : ImmutableBean
	{

	  /// <summary>
	  /// The standard Monthly type.
	  /// </summary>
	  public static readonly EtdVariant MONTHLY = new EtdVariant(EtdExpiryType.MONTHLY, null, null, null);

	  /// <summary>
	  /// The type of ETD - Monthly, Weekly or Daily.
	  /// <para>
	  /// Flex Futures and Options are always Daily.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final EtdExpiryType type;
	  private readonly EtdExpiryType type;
	  /// <summary>
	  /// The optional date code, populated for Weekly and Daily.
	  /// <para>
	  /// This will be the week number for Weekly and the day-of-week for Daily.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final System.Nullable<int> dateCode;
	  private readonly int? dateCode;
	  /// <summary>
	  /// The optional settlement type, such as 'Cash' or 'Physical', populated for Flex Futures and Flex Options.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final EtdSettlementType settlementType;
	  private readonly EtdSettlementType settlementType;
	  /// <summary>
	  /// The optional option type, 'American' or 'European', populated for Flex Options.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final EtdOptionType optionType;
	  private readonly EtdOptionType optionType;
	  /// <summary>
	  /// The short code.
	  /// </summary>
	  private readonly string code; // derived, not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The standard monthly ETD.
	  /// </summary>
	  /// <returns> the variant </returns>
	  public static EtdVariant ofMonthly()
	  {
		return MONTHLY;
	  }

	  /// <summary>
	  /// The standard weekly ETD.
	  /// </summary>
	  /// <param name="week">  the week number </param>
	  /// <returns> the variant </returns>
	  public static EtdVariant ofWeekly(int week)
	  {
		return new EtdVariant(EtdExpiryType.WEEKLY, week, null, null);
	  }

	  /// <summary>
	  /// The standard daily ETD.
	  /// </summary>
	  /// <param name="dayOfMonth">  the day-of-month </param>
	  /// <returns> the variant </returns>
	  public static EtdVariant ofDaily(int dayOfMonth)
	  {
		return new EtdVariant(EtdExpiryType.DAILY, dayOfMonth, null, null);
	  }

	  /// <summary>
	  /// The standard monthly ETD.
	  /// </summary>
	  /// <param name="dayOfMonth">  the day-of-month </param>
	  /// <param name="settlementType">  the settlement type </param>
	  /// <returns> the variant </returns>
	  public static EtdVariant ofFlexFuture(int dayOfMonth, EtdSettlementType settlementType)
	  {
		return new EtdVariant(EtdExpiryType.DAILY, dayOfMonth, settlementType, null);
	  }

	  /// <summary>
	  /// The standard monthly ETD.
	  /// </summary>
	  /// <param name="dayOfMonth">  the day-of-month </param>
	  /// <param name="settlementType">  the settlement type </param>
	  /// <param name="optionType">  the option type </param>
	  /// <returns> the variant </returns>
	  public static EtdVariant ofFlexOption(int dayOfMonth, EtdSettlementType settlementType, EtdOptionType optionType)
	  {
		return new EtdVariant(EtdExpiryType.DAILY, dayOfMonth, settlementType, optionType);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private EtdVariant(EtdExpiryType type, System.Nullable<int> dateCode, EtdSettlementType settlementType, EtdOptionType optionType)
	  private EtdVariant(EtdExpiryType type, int? dateCode, EtdSettlementType settlementType, EtdOptionType optionType)
	  {

		this.type = ArgChecker.notNull(type, "type");
		this.dateCode = dateCode;
		this.settlementType = settlementType;
		this.optionType = optionType;
		if (type == EtdExpiryType.MONTHLY)
		{
		  ArgChecker.isTrue(dateCode == null, "Monthly variant must have no dateCode");
		  ArgChecker.isTrue(settlementType == null, "Monthly variant must have no settlementType");
		  ArgChecker.isTrue(optionType == null, "Monthly variant must have no optionType");
		  this.code = "";
		}
		else if (type == EtdExpiryType.WEEKLY)
		{
		  ArgChecker.notNull(dateCode, "dateCode");
		  ArgChecker.isTrue(dateCode >= 1 && dateCode <= 5, "Week must be from 1 to 5");
		  ArgChecker.isTrue(settlementType == null, "Weekly variant must have no settlementType");
		  ArgChecker.isTrue(optionType == null, "Weekly variant must have no optionType");
		  this.code = "W" + dateCode;
		}
		else
		{ // DAILY and Flex
		  ArgChecker.notNull(dateCode, "dateCode");
		  ArgChecker.isTrue(dateCode >= 1 && dateCode <= 31, "Day-of-week must be from 1 to 31");
		  ArgChecker.isFalse(settlementType == null && optionType != null, "Flex Option must have both settlementType and optionType");
		  string dateCodeStr = dateCode < 10 ? "0" + dateCode : Convert.ToString(dateCode);
		  string settlementCode = settlementType != null ? settlementType.Code : "";
		  string optionCode = optionType != null ? optionType.Code : "";
		  this.code = dateCodeStr + settlementCode + optionCode;
		}
	  }

	  // resolve after deserialization
	  private object readResolve()
	  {
		return new EtdVariant(type, dateCode, settlementType, optionType);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the variant is a Flex Future or Flex Option.
	  /// </summary>
	  /// <returns> true if this is a Flex Future or Flex Option </returns>
	  public bool Flex
	  {
		  get
		  {
			return settlementType != null;
		  }
	  }

	  /// <summary>
	  /// Gets the short code that describes the variant.
	  /// <para>
	  /// This is an empty string for Monthly, the week number prefixed by 'W' for Weekly,
	  /// the day number for daily, with a suffix of the settlement type and option type codes.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the short code </returns>
	  public string Code
	  {
		  get
		  {
			return code;
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code EtdVariant}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static MetaBean meta()
	  {
		return EtdVariant.Meta.INSTANCE;
	  }

	  static EtdVariant()
	  {
		MetaBean.register(EtdVariant.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override MetaBean metaBean()
	  {
		return EtdVariant.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type of ETD - Monthly, Weekly or Daily.
	  /// <para>
	  /// Flex Futures and Options are always Daily.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public EtdExpiryType Type
	  {
		  get
		  {
			return type;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the optional date code, populated for Weekly and Daily.
	  /// <para>
	  /// This will be the week number for Weekly and the day-of-week for Daily.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public int? DateCode
	  {
		  get
		  {
			return dateCode != null ? int?.of(dateCode) : int?.empty();
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the optional settlement type, such as 'Cash' or 'Physical', populated for Flex Futures and Flex Options. </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<EtdSettlementType> SettlementType
	  {
		  get
		  {
			return Optional.ofNullable(settlementType);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the optional option type, 'American' or 'European', populated for Flex Options. </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<EtdOptionType> OptionType
	  {
		  get
		  {
			return Optional.ofNullable(optionType);
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
		  EtdVariant other = (EtdVariant) obj;
		  return JodaBeanUtils.equal(type, other.type) && JodaBeanUtils.equal(dateCode, other.dateCode) && JodaBeanUtils.equal(settlementType, other.settlementType) && JodaBeanUtils.equal(optionType, other.optionType);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(type);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dateCode);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(settlementType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(optionType);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("EtdVariant{");
		buf.Append("type").Append('=').Append(type).Append(',').Append(' ');
		buf.Append("dateCode").Append('=').Append(dateCode).Append(',').Append(' ');
		buf.Append("settlementType").Append('=').Append(settlementType).Append(',').Append(' ');
		buf.Append("optionType").Append('=').Append(JodaBeanUtils.ToString(optionType));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code EtdVariant}.
	  /// </summary>
	  private sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  type = DirectMetaProperty.ofImmutable(this, "type", typeof(EtdVariant), typeof(EtdExpiryType));
			  dateCode = DirectMetaProperty.ofImmutable(this, "dateCode", typeof(EtdVariant), typeof(Integer));
			  settlementType = DirectMetaProperty.ofImmutable(this, "settlementType", typeof(EtdVariant), typeof(EtdSettlementType));
			  optionType = DirectMetaProperty.ofImmutable(this, "optionType", typeof(EtdVariant), typeof(EtdOptionType));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "type", "dateCode", "settlementType", "optionType");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code type} property.
		/// </summary>
		internal MetaProperty<EtdExpiryType> type;
		/// <summary>
		/// The meta-property for the {@code dateCode} property.
		/// </summary>
		internal MetaProperty<int> dateCode;
		/// <summary>
		/// The meta-property for the {@code settlementType} property.
		/// </summary>
		internal MetaProperty<EtdSettlementType> settlementType;
		/// <summary>
		/// The meta-property for the {@code optionType} property.
		/// </summary>
		internal MetaProperty<EtdOptionType> optionType;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "type", "dateCode", "settlementType", "optionType");
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
			case 3575610: // type
			  return type;
			case 1792248507: // dateCode
			  return dateCode;
			case -295448573: // settlementType
			  return settlementType;
			case 1373587791: // optionType
			  return optionType;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends EtdVariant> builder()
		public override BeanBuilder<EtdVariant> builder()
		{
		  return new EtdVariant.Builder();
		}

		public override Type beanType()
		{
		  return typeof(EtdVariant);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3575610: // type
			  return ((EtdVariant) bean).Type;
			case 1792248507: // dateCode
			  return ((EtdVariant) bean).dateCode;
			case -295448573: // settlementType
			  return ((EtdVariant) bean).settlementType;
			case 1373587791: // optionType
			  return ((EtdVariant) bean).optionType;
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
	  /// The bean-builder for {@code EtdVariant}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<EtdVariant>
	  {

		internal EtdExpiryType type;
		internal int? dateCode;
		internal EtdSettlementType settlementType;
		internal EtdOptionType optionType;

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
			case 3575610: // type
			  return type;
			case 1792248507: // dateCode
			  return dateCode;
			case -295448573: // settlementType
			  return settlementType;
			case 1373587791: // optionType
			  return optionType;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3575610: // type
			  this.type = (EtdExpiryType) newValue;
			  break;
			case 1792248507: // dateCode
			  this.dateCode = (int?) newValue;
			  break;
			case -295448573: // settlementType
			  this.settlementType = (EtdSettlementType) newValue;
			  break;
			case 1373587791: // optionType
			  this.optionType = (EtdOptionType) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override EtdVariant build()
		{
		  return new EtdVariant(type, dateCode, settlementType, optionType);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("EtdVariant.Builder{");
		  buf.Append("type").Append('=').Append(JodaBeanUtils.ToString(type)).Append(',').Append(' ');
		  buf.Append("dateCode").Append('=').Append(JodaBeanUtils.ToString(dateCode)).Append(',').Append(' ');
		  buf.Append("settlementType").Append('=').Append(JodaBeanUtils.ToString(settlementType)).Append(',').Append(' ');
		  buf.Append("optionType").Append('=').Append(JodaBeanUtils.ToString(optionType));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}