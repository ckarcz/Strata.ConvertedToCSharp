using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using Strings = com.google.common.@base.Strings;
	using Throwables = com.google.common.@base.Throwables;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Interner = com.google.common.collect.Interner;
	using Interners = com.google.common.collect.Interners;
	using Pair = com.opengamma.strata.collect.tuple.Pair;

	/// <summary>
	/// Details of a single failed item.
	/// <para>
	/// This is used in <seealso cref="Failure"/> and <seealso cref="FailureItems"/> to capture details of a single failure.
	/// Details include the reason, message and stack trace.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class FailureItem implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FailureItem : ImmutableBean
	{

	  /// <summary>
	  /// Attribute used to store the exception message.
	  /// </summary>
	  public const string EXCEPTION_MESSAGE_ATTRIBUTE = "exceptionMessage";
	  /// <summary>
	  /// Header used when generating stack trace internally.
	  /// </summary>
	  private const string FAILURE_EXCEPTION = "com.opengamma.strata.collect.result.FailureItem: ";
	  /// <summary>
	  /// Stack traces can take up a lot of memory if a large number of failures are stored.
	  /// They are often duplicated many times so interning them can save a significant amount of memory.
	  /// </summary>
	  private static readonly Interner<string> INTERNER = Interners.newWeakInterner();

	  /// <summary>
	  /// The reason associated with the failure.
	  /// </summary>
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
	  /// The attributes associated with this failure.
	  /// Attributes can contain additional information about the failure. For example, a line number in a file or the ID of a trade.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<String, String> attributes;
	  private readonly ImmutableMap<string, string> attributes;
	  /// <summary>
	  /// Stack trace where the failure occurred.
	  /// If the failure was caused by an {@code Exception} its stack trace is used, otherwise it's the
	  /// location where the failure was created.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final String stackTrace;
	  private readonly string stackTrace;
	  /// <summary>
	  /// The type of the exception that caused the failure, not present if it wasn't caused by an exception.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final Class causeType;
	  private readonly Type causeType;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a failure from a reason and message.
	  /// <para>
	  /// The message is produced using a template that contains zero to many "{}" or "{abc}" placeholders.
	  /// Each placeholder is replaced by the next available argument.
	  /// If the placeholder has a name, its value is added to the attributes map with the name as a key.
	  /// If there are too few arguments, then the message will be left with placeholders.
	  /// If there are too many arguments, then the excess arguments are appended to the
	  /// end of the message. No attempt is made to format the arguments.
	  /// See <seealso cref="Messages#formatWithAttributes(String, Object...)"/> for more details.
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
	  public static FailureItem of(FailureReason reason, string message, params object[] messageArgs)
	  {
		Pair<string, IDictionary<string, string>> msg = Messages.formatWithAttributes(message, messageArgs);
		return of(reason, msg.First, msg.Second);
	  }

	  /// <summary>
	  /// Obtains a failure from a reason and message.
	  /// <para>
	  /// The failure will still have a stack trace, but the cause type will not be present.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="reason">  the reason </param>
	  /// <param name="message">  the failure message, not empty </param>
	  /// <param name="skipFrames">  the number of caller frames to skip, not including this one </param>
	  /// <returns> the failure </returns>
	  internal static FailureItem of(FailureReason reason, string message, int skipFrames)
	  {
		ArgChecker.notNull(reason, "reason");
		ArgChecker.notEmpty(message, "message");
		string stackTrace = localGetStackTraceAsString(message, skipFrames);
		return new FailureItem(reason, message, ImmutableMap.of(), stackTrace, null);
	  }

	  /// <summary>
	  /// Obtains a failure from a reason and message.
	  /// <para>
	  /// The failure will still have a stack trace, but the cause type will not be present.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="reason">  the reason </param>
	  /// <param name="message">  the failure message, not empty </param>
	  /// <param name="attributes"> the attributes associated with this failure </param>
	  /// <returns> the failure </returns>
	  private static FailureItem of(FailureReason reason, string message, IDictionary<string, string> attributes)
	  {
		ArgChecker.notNull(reason, "reason");
		ArgChecker.notEmpty(message, "message");
		string stackTrace = localGetStackTraceAsString(message, 1);
		return new FailureItem(reason, message, attributes, stackTrace, null);
	  }

	  private static string localGetStackTraceAsString(string message, int skipFrames)
	  {
		StringBuilder builder = new StringBuilder();
		StackTraceElement[] stackTrace = Thread.CurrentThread.StackTrace;
		// simulate full stack trace, pretending this class is a Throwable subclass
		builder.Append(FAILURE_EXCEPTION).Append(message).Append("\n");
		// drop the first few frames because they are part of the immediate calling code
		for (int i = skipFrames + 3; i < stackTrace.Length; i++)
		{
		  builder.Append("\tat ").Append(stackTrace[i]).Append("\n");
		}
		return builder.ToString();
	  }

	  /// <summary>
	  /// Obtains a failure from a reason and exception.
	  /// </summary>
	  /// <param name="reason">  the reason </param>
	  /// <param name="cause">  the cause </param>
	  /// <returns> the failure </returns>
	  public static FailureItem of(FailureReason reason, Exception cause)
	  {
		ArgChecker.notNull(reason, "reason");
		ArgChecker.notNull(cause, "cause");
		string causeMessage = cause.Message;
		string message = Strings.isNullOrEmpty(causeMessage) ? cause.GetType().Name : causeMessage;
		return FailureItem.of(reason, cause, message);
	  }

	  /// <summary>
	  /// Obtains a failure from a reason, exception and message.
	  /// <para>
	  /// The message is produced using a template that contains zero to many "{}" placeholders.
	  /// Each placeholder is replaced by the next available argument.
	  /// If there are too few arguments, then the message will be left with placeholders.
	  /// If there are too many arguments, then the excess arguments are appended to the
	  /// end of the message. No attempt is made to format the arguments.
	  /// See <seealso cref="Messages#formatWithAttributes(String, Object...)"/> for more details.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="reason">  the reason </param>
	  /// <param name="cause">  the cause </param>
	  /// <param name="message">  a message explaining the failure, not empty, uses "{}" for inserting {@code messageArgs} </param>
	  /// <param name="messageArgs">  the arguments for the message </param>
	  /// <returns> the failure </returns>
	  public static FailureItem of(FailureReason reason, Exception cause, string message, params object[] messageArgs)
	  {
		ArgChecker.notNull(reason, "reason");
		ArgChecker.notNull(cause, "cause");
		Pair<string, IDictionary<string, string>> msg = Messages.formatWithAttributes(message, messageArgs);
		string stackTrace = Throwables.getStackTraceAsString(cause).replace(Environment.NewLine, "\n");
		FailureItem @base = new FailureItem(reason, msg.First, msg.Second, stackTrace, cause.GetType());
		string causeMessage = cause.Message;
		if (!@base.Attributes.containsKey(EXCEPTION_MESSAGE_ATTRIBUTE) && !Strings.isNullOrEmpty(causeMessage))
		{
		  return @base.withAttribute(EXCEPTION_MESSAGE_ATTRIBUTE, causeMessage);
		}
		return @base;
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private FailureItem(FailureReason reason, String message, java.util.Map<String, String> attributes, String stackTrace, Class causeType)
	  private FailureItem(FailureReason reason, string message, IDictionary<string, string> attributes, string stackTrace, Type causeType)
	  {
		this.attributes = ImmutableMap.copyOf(attributes);
		JodaBeanUtils.notNull(reason, "reason");
		JodaBeanUtils.notEmpty(message, "message");
		JodaBeanUtils.notNull(stackTrace, "stackTrace");
		this.reason = reason;
		this.message = message;
		this.stackTrace = INTERNER.intern(stackTrace);
		this.causeType = causeType;
	  }

	  /// <summary>
	  /// Returns an instance with the specified attribute added.
	  /// <para>
	  /// If the attribute map of this instance has the specified key, the value is replaced.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="key">  the key to add </param>
	  /// <param name="value">  the value to add </param>
	  /// <returns> the new failure item </returns>
	  public FailureItem withAttribute(string key, string value)
	  {
		IDictionary<string, string> attributes = new Dictionary<string, string>(this.attributes);
		attributes[key] = value;
		return new FailureItem(reason, message, attributes, stackTrace, causeType);
	  }

	  /// <summary>
	  /// Returns an instance with the specified attributes added.
	  /// <para>
	  /// If the attribute map of this instance has any of the new attribute keys, the values are replaced.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="attributes">  the new attributes to add </param>
	  /// <returns> the new failure item </returns>
	  public FailureItem withAttributes(IDictionary<string, string> attributes)
	  {
		IDictionary<string, string> newAttributes = new Dictionary<string, string>(this.attributes);
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		newAttributes.putAll(attributes);
		return new FailureItem(reason, message, newAttributes, stackTrace, causeType);
	  }

	  /// <summary>
	  /// Returns a string summary of the failure, as a single line excluding the stack trace.
	  /// </summary>
	  /// <returns> the summary string </returns>
	  public override string ToString()
	  {
		if (stackTrace.StartsWith(FAILURE_EXCEPTION, StringComparison.Ordinal))
		{
		  return reason + ": " + message;
		}
		int endLine = stackTrace.IndexOf("\n", StringComparison.Ordinal);
		string firstLine = endLine < 0 ? stackTrace : stackTrace.Substring(0, endLine);
		if (firstLine.EndsWith(": " + message, StringComparison.Ordinal))
		{
		  return reason + ": " + message + ": " + firstLine.Substring(0, firstLine.Length - message.Length - 2);
		}
		return reason + ": " + message + ": " + firstLine;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FailureItem}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FailureItem.Meta meta()
	  {
		return FailureItem.Meta.INSTANCE;
	  }

	  static FailureItem()
	  {
		MetaBean.register(FailureItem.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override FailureItem.Meta metaBean()
	  {
		return FailureItem.Meta.INSTANCE;
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
	  /// Gets the attributes associated with this failure.
	  /// Attributes can contain additional information about the failure. For example, a line number in a file or the ID of a trade. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<string, string> Attributes
	  {
		  get
		  {
			return attributes;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets stack trace where the failure occurred.
	  /// If the failure was caused by an {@code Exception} its stack trace is used, otherwise it's the
	  /// location where the failure was created. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public string StackTrace
	  {
		  get
		  {
			return stackTrace;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type of the exception that caused the failure, not present if it wasn't caused by an exception. </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<Type> CauseType
	  {
		  get
		  {
			return Optional.ofNullable(causeType);
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
		  FailureItem other = (FailureItem) obj;
		  return JodaBeanUtils.equal(reason, other.reason) && JodaBeanUtils.equal(message, other.message) && JodaBeanUtils.equal(attributes, other.attributes) && JodaBeanUtils.equal(stackTrace, other.stackTrace) && JodaBeanUtils.equal(causeType, other.causeType);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(reason);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(message);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(attributes);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(stackTrace);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(causeType);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FailureItem}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  reason_Renamed = DirectMetaProperty.ofImmutable(this, "reason", typeof(FailureItem), typeof(FailureReason));
			  message_Renamed = DirectMetaProperty.ofImmutable(this, "message", typeof(FailureItem), typeof(string));
			  attributes_Renamed = DirectMetaProperty.ofImmutable(this, "attributes", typeof(FailureItem), (Type) typeof(ImmutableMap));
			  stackTrace_Renamed = DirectMetaProperty.ofImmutable(this, "stackTrace", typeof(FailureItem), typeof(string));
			  causeType_Renamed = DirectMetaProperty.ofImmutable(this, "causeType", typeof(FailureItem), (Type) typeof(Type));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "reason", "message", "attributes", "stackTrace", "causeType");
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
		/// The meta-property for the {@code attributes} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<String, String>> attributes = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "attributes", FailureItem.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<string, string>> attributes_Renamed;
		/// <summary>
		/// The meta-property for the {@code stackTrace} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> stackTrace_Renamed;
		/// <summary>
		/// The meta-property for the {@code causeType} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<Class> causeType = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "causeType", FailureItem.class, (Class) Class.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Type> causeType_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "reason", "message", "attributes", "stackTrace", "causeType");
		internal IDictionary<string, MetaProperty<object>> metaPropertyMap$;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Meta()
		{
			outerInstance.if (!InstanceFieldsInitialized)
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
			case 405645655: // attributes
			  return attributes_Renamed;
			case 2026279837: // stackTrace
			  return stackTrace_Renamed;
			case -1443456189: // causeType
			  return causeType_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends FailureItem> builder()
		public override BeanBuilder<FailureItem> builder()
		{
		  return new FailureItem.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FailureItem);
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
		/// The meta-property for the {@code attributes} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<string, string>> attributes()
		{
		  return attributes_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code stackTrace} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> stackTrace()
		{
		  return stackTrace_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code causeType} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Type> causeType()
		{
		  return causeType_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -934964668: // reason
			  return ((FailureItem) bean).Reason;
			case 954925063: // message
			  return ((FailureItem) bean).Message;
			case 405645655: // attributes
			  return ((FailureItem) bean).Attributes;
			case 2026279837: // stackTrace
			  return ((FailureItem) bean).StackTrace;
			case -1443456189: // causeType
			  return ((FailureItem) bean).causeType;
		  }
		  return base.propertyGet(bean, propertyName, quiet);
		}

		protected internal override void propertySet(Bean bean, string propertyName, object newValue, bool quiet)
		{
		  metaProperty(propertyName);
		  outerInstance.if (quiet)
		  {
			return;
		  }
		  throw new System.NotSupportedException("Property cannot be written: " + propertyName);
		}

	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The bean-builder for {@code FailureItem}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<FailureItem>
	  {

		internal FailureReason reason;
		internal string message;
		internal IDictionary<string, string> attributes = ImmutableMap.of();
		internal string stackTrace;
		internal Type causeType;

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
			case 405645655: // attributes
			  return attributes;
			case 2026279837: // stackTrace
			  return stackTrace;
			case -1443456189: // causeType
			  return causeType;
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
			case 405645655: // attributes
			  this.attributes = (IDictionary<string, string>) newValue;
			  break;
			case 2026279837: // stackTrace
			  this.stackTrace = (string) newValue;
			  break;
			case -1443456189: // causeType
			  this.causeType = (Type) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override FailureItem build()
		{
		  return new FailureItem(reason, message, attributes, stackTrace, causeType);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("FailureItem.Builder{");
		  buf.Append("reason").Append('=').Append(JodaBeanUtils.ToString(reason)).Append(',').Append(' ');
		  buf.Append("message").Append('=').Append(JodaBeanUtils.ToString(message)).Append(',').Append(' ');
		  buf.Append("attributes").Append('=').Append(JodaBeanUtils.ToString(attributes)).Append(',').Append(' ');
		  buf.Append("stackTrace").Append('=').Append(JodaBeanUtils.ToString(stackTrace)).Append(',').Append(' ');
		  buf.Append("causeType").Append('=').Append(JodaBeanUtils.ToString(causeType));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}