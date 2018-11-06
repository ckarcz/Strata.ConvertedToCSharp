using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.result
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

	using ImmutableSet = com.google.common.collect.ImmutableSet;

	/// <summary>
	/// Description of a failed result.
	/// <para>
	/// If calculation of a result fails this class provides details of the failure.
	/// There is a single reason and message and a set of detailed failure items.
	/// Each <seealso cref="FailureItem"/> has details of the actual cause.
	/// </para>
	/// <para>
	/// In most cases, instances of {@code Failure} should be created using one of the
	/// {@code failure} methods on <seealso cref="Result"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class Failure implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class Failure : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final FailureReason reason;
		private readonly FailureReason reason;
	  /// <summary>
	  /// The error message associated with the failure.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final String message;
	  private readonly string message;
	  /// <summary>
	  /// The set of failure items.
	  /// There will be at least one failure item.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final com.google.common.collect.ImmutableSet<FailureItem> items;
	  private readonly ImmutableSet<FailureItem> items;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a failure from a reason and message.
	  /// <para>
	  /// The message is produced using a template that contains zero to many "{}" placeholders.
	  /// Each placeholder is replaced by the next available argument.
	  /// If there are too few arguments, then the message will be left with placeholders.
	  /// If there are too many arguments, then the excess arguments are appended to the
	  /// end of the message. No attempt is made to format the arguments.
	  /// See <seealso cref="Messages#format(String, Object...)"/> for more details.
	  /// </para>
	  /// <para>
	  /// An exception will be created internally to obtain a stack trace.
	  /// The cause type will not be present in the resulting failure.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="reason">  the reason </param>
	  /// <param name="message">  a message explaining the failure, not empty, uses "{}" for inserting {@code messageArgs} </param>
	  /// <param name="messageArgs">  the arguments for the message </param>
	  /// <returns> the failure </returns>
	  public static Failure of(FailureReason reason, string message, params object[] messageArgs)
	  {
		string msg = Messages.format(message, messageArgs);
		return Failure.of(FailureItem.of(reason, msg, 1));
	  }

	  /// <summary>
	  /// Obtains a failure from a reason, message and exception.
	  /// <para>
	  /// The message is produced using a template that contains zero to many "{}" placeholders.
	  /// Each placeholder is replaced by the next available argument.
	  /// If there are too few arguments, then the message will be left with placeholders.
	  /// If there are too many arguments, then the excess arguments are appended to the
	  /// end of the message. No attempt is made to format the arguments.
	  /// See <seealso cref="Messages#format(String, Object...)"/> for more details.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="reason">  the reason </param>
	  /// <param name="cause">  the cause </param>
	  /// <param name="message">  the failure message, possibly containing placeholders, formatted using <seealso cref="Messages#format"/> </param>
	  /// <param name="messageArgs">  arguments used to create the failure message </param>
	  /// <returns> the failure </returns>
	  public static Failure of(FailureReason reason, Exception cause, string message, params object[] messageArgs)
	  {
		return Failure.of(FailureItem.of(reason, cause, message, messageArgs));
	  }

	  /// <summary>
	  /// Obtains a failure from a reason and exception.
	  /// </summary>
	  /// <param name="reason">  the reason </param>
	  /// <param name="cause">  the cause </param>
	  /// <returns> the failure </returns>
	  public static Failure of(FailureReason reason, Exception cause)
	  {
		return Failure.of(FailureItem.of(reason, cause));
	  }

	  /// <summary>
	  /// Obtains a failure for a single failure item.
	  /// </summary>
	  /// <param name="item">  the failure item </param>
	  /// <returns> the failure </returns>
	  public static Failure of(FailureItem item)
	  {
		return new Failure(item.Reason, item.Message, ImmutableSet.of(item));
	  }

	  /// <summary>
	  /// Obtains a failure for multiple failure items.
	  /// </summary>
	  /// <param name="item">  the first failure item </param>
	  /// <param name="additionalItems">  additional failure items </param>
	  /// <returns> the failure </returns>
	  public static Failure of(FailureItem item, params FailureItem[] additionalItems)
	  {
		return of(ImmutableSet.builder<FailureItem>().add(item).add(additionalItems).build());
	  }

	  /// <summary>
	  /// Obtains a failure for a non-empty collection of failure items.
	  /// </summary>
	  /// <param name="items">  the failures, not empty </param>
	  /// <returns> the failure </returns>
	  public static Failure of(ICollection<FailureItem> items)
	  {
		ArgChecker.notEmpty(items, "items");
		ISet<FailureItem> itemSet = ImmutableSet.copyOf(items);
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		string message = itemSet.Select(FailureItem::getMessage).collect(Collectors.joining(", "));
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		FailureReason reason = itemSet.Select(FailureItem::getReason).Aggregate((s1, s2) => s1 == s2 ? s1 : FailureReason.MULTIPLE).get();
		return new Failure(reason, message, itemSet);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code Failure}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static Failure.Meta meta()
	  {
		return Failure.Meta.INSTANCE;
	  }

	  static Failure()
	  {
		MetaBean.register(Failure.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private Failure(FailureReason reason, string message, ISet<FailureItem> items)
	  {
		JodaBeanUtils.notNull(reason, "reason");
		JodaBeanUtils.notEmpty(message, "message");
		JodaBeanUtils.notEmpty(items, "items");
		this.reason = reason;
		this.message = message;
		this.items = ImmutableSet.copyOf(items);
	  }

	  public override Failure.Meta metaBean()
	  {
		return Failure.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the reason associated with the failure. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FailureReason Reason
	  {
		  get
		  {
			return reason;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the error message associated with the failure. </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public string Message
	  {
		  get
		  {
			return message;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the set of failure items.
	  /// There will be at least one failure item. </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public ImmutableSet<FailureItem> Items
	  {
		  get
		  {
			return items;
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
		  Failure other = (Failure) obj;
		  return JodaBeanUtils.equal(reason, other.reason) && JodaBeanUtils.equal(message, other.message) && JodaBeanUtils.equal(items, other.items);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(reason);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(message);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(items);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("Failure{");
		buf.Append("reason").Append('=').Append(reason).Append(',').Append(' ');
		buf.Append("message").Append('=').Append(message).Append(',').Append(' ');
		buf.Append("items").Append('=').Append(JodaBeanUtils.ToString(items));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code Failure}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  reason_Renamed = DirectMetaProperty.ofImmutable(this, "reason", typeof(Failure), typeof(FailureReason));
			  message_Renamed = DirectMetaProperty.ofImmutable(this, "message", typeof(Failure), typeof(string));
			  items_Renamed = DirectMetaProperty.ofImmutable(this, "items", typeof(Failure), (Type) typeof(ImmutableSet));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "reason", "message", "items");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code reason} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FailureReason> reason_Renamed;
		/// <summary>
		/// The meta-property for the {@code message} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> message_Renamed;
		/// <summary>
		/// The meta-property for the {@code items} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableSet<FailureItem>> items = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "items", Failure.class, (Class) com.google.common.collect.ImmutableSet.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableSet<FailureItem>> items_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "reason", "message", "items");
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
			case -934964668: // reason
			  return reason_Renamed;
			case 954925063: // message
			  return message_Renamed;
			case 100526016: // items
			  return items_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends Failure> builder()
		public override BeanBuilder<Failure> builder()
		{
		  return new Failure.Builder();
		}

		public override Type beanType()
		{
		  return typeof(Failure);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code reason} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FailureReason> reason()
		{
		  return reason_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code message} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> message()
		{
		  return message_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code items} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableSet<FailureItem>> items()
		{
		  return items_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -934964668: // reason
			  return ((Failure) bean).Reason;
			case 954925063: // message
			  return ((Failure) bean).Message;
			case 100526016: // items
			  return ((Failure) bean).Items;
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
	  /// The bean-builder for {@code Failure}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<Failure>
	  {

		internal FailureReason reason;
		internal string message;
		internal ISet<FailureItem> items = ImmutableSet.of();

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
			case -934964668: // reason
			  return reason;
			case 954925063: // message
			  return message;
			case 100526016: // items
			  return items;
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
			case -934964668: // reason
			  this.reason = (FailureReason) newValue;
			  break;
			case 954925063: // message
			  this.message = (string) newValue;
			  break;
			case 100526016: // items
			  this.items = (ISet<FailureItem>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override Failure build()
		{
		  return new Failure(reason, message, items);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("Failure.Builder{");
		  buf.Append("reason").Append('=').Append(JodaBeanUtils.ToString(reason)).Append(',').Append(' ');
		  buf.Append("message").Append('=').Append(JodaBeanUtils.ToString(message)).Append(',').Append(' ');
		  buf.Append("items").Append('=').Append(JodaBeanUtils.ToString(items));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}