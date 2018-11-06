using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


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

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Guavate = com.opengamma.strata.collect.Guavate;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using Surface = com.opengamma.strata.market.surface.Surface;

	/// <summary>
	/// Unit parameter sensitivity for parameterized market data, such as curves.
	/// <para>
	/// Parameter sensitivity is the sensitivity of a value to the parameters of
	/// <seealso cref="ParameterizedData parameterized market data"/> objects that are used to determine the value.
	/// Common {@code ParameterizedData} implementations include <seealso cref="Curve"/> and <seealso cref="Surface"/>.
	/// </para>
	/// <para>
	/// For example, par rate sensitivity to an underlying curve would be expressed using this
	/// class as there is no associated currency.
	/// </para>
	/// <para>
	/// The sensitivity is expressed as a single entry for piece of parameterized market data.
	/// The sensitivity has no associated currency.
	/// The order of the list has no specific meaning.
	/// </para>
	/// <para>
	/// One way of viewing this class is as a {@code Map} from a specific sensitivity key to
	/// {@code DoubleArray} sensitivity values. However, instead of being structured as a {@code Map},
	/// the data is structured as a {@code List}, with the key and value in each entry.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class UnitParameterSensitivities implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class UnitParameterSensitivities : ImmutableBean
	{

	  /// <summary>
	  /// An empty instance.
	  /// </summary>
	  private static readonly UnitParameterSensitivities EMPTY = new UnitParameterSensitivities(ImmutableList.of());

	  /// <summary>
	  /// The parameter sensitivities.
	  /// <para>
	  /// Each entry includes details of the <seealso cref="ParameterizedData"/> it relates to.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<UnitParameterSensitivity> sensitivities;
	  private readonly ImmutableList<UnitParameterSensitivity> sensitivities;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// An empty sensitivity instance.
	  /// </summary>
	  /// <returns> the empty instance </returns>
	  public static UnitParameterSensitivities empty()
	  {
		return EMPTY;
	  }

	  /// <summary>
	  /// Obtains an instance from a single sensitivity entry.
	  /// </summary>
	  /// <param name="sensitivity">  the sensitivity entry </param>
	  /// <returns> the sensitivities instance </returns>
	  public static UnitParameterSensitivities of(UnitParameterSensitivity sensitivity)
	  {
		return new UnitParameterSensitivities(ImmutableList.of(sensitivity));
	  }

	  /// <summary>
	  /// Obtains an instance from an array of sensitivity entries.
	  /// <para>
	  /// The order of sensitivities is typically unimportant, however it is retained
	  /// and exposed in <seealso cref="#equals(Object)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="sensitivities">  the sensitivities </param>
	  /// <returns> the sensitivities instance </returns>
	  public static UnitParameterSensitivities of(params UnitParameterSensitivity[] sensitivities)
	  {
		return of(Arrays.asList(sensitivities));
	  }

	  /// <summary>
	  /// Obtains an instance from a list of sensitivity entries.
	  /// <para>
	  /// The order of sensitivities is typically unimportant, however it is retained
	  /// and exposed in <seealso cref="#equals(Object)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="sensitivities">  the list of sensitivity entries </param>
	  /// <returns> the sensitivities instance </returns>
	  public static UnitParameterSensitivities of<T1>(IList<T1> sensitivities) where T1 : UnitParameterSensitivity
	  {
		IList<UnitParameterSensitivity> mutable = new List<UnitParameterSensitivity>();
		foreach (UnitParameterSensitivity otherSens in sensitivities)
		{
		  insert(mutable, otherSens);
		}
		return new UnitParameterSensitivities(ImmutableList.copyOf(mutable));
	  }

	  // used when not pre-sorted
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private UnitParameterSensitivities(java.util.List<? extends UnitParameterSensitivity> sensitivities)
	  private UnitParameterSensitivities<T1>(IList<T1> sensitivities) where T1 : UnitParameterSensitivity
	  {
		if (sensitivities.Count < 2)
		{
		  this.sensitivities = ImmutableList.copyOf(sensitivities);
		}
		else
		{
		  IList<UnitParameterSensitivity> mutable = new List<UnitParameterSensitivity>(sensitivities);
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		  mutable.sort(UnitParameterSensitivity::compareKey);
		  this.sensitivities = ImmutableList.copyOf(mutable);
		}
	  }

	  // used when pre-sorted
	  private UnitParameterSensitivities(ImmutableList<UnitParameterSensitivity> sensitivities)
	  {
		this.sensitivities = sensitivities;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of sensitivity entries.
	  /// </summary>
	  /// <returns> the size of the internal list of point sensitivities </returns>
	  public int size()
	  {
		return sensitivities.size();
	  }

	  /// <summary>
	  /// Gets a single sensitivity instance by name.
	  /// </summary>
	  /// <param name="name">  the curve name to find </param>
	  /// <returns> the matching sensitivity </returns>
	  /// <exception cref="IllegalArgumentException"> if the name and currency do not match an entry </exception>
	  public UnitParameterSensitivity getSensitivity<T1>(MarketDataName<T1> name)
	  {
		return findSensitivity(name).orElseThrow(() => new System.ArgumentException(Messages.format("Unable to find sensitivity: {}", name)));
	  }

	  /// <summary>
	  /// Finds a single sensitivity instance by name.
	  /// <para>
	  /// If the sensitivity is not found, optional empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name to find </param>
	  /// <returns> the matching sensitivity </returns>
	  public Optional<UnitParameterSensitivity> findSensitivity<T1>(MarketDataName<T1> name)
	  {
		return sensitivities.Where(sens => sens.MarketDataName.Equals(name)).First();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Combines this parameter sensitivities with another instance.
	  /// <para>
	  /// This returns a new sensitivity instance with the specified sensitivity added.
	  /// This instance is immutable and unaffected by this method.
	  /// The result may contain duplicate parameter sensitivities.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other parameter sensitivity </param>
	  /// <returns> an instance based on this one, with the other instance added </returns>
	  public UnitParameterSensitivities combinedWith(UnitParameterSensitivity other)
	  {
		IList<UnitParameterSensitivity> mutable = new List<UnitParameterSensitivity>(sensitivities);
		insert(mutable, other);
		return new UnitParameterSensitivities(ImmutableList.copyOf(mutable));
	  }

	  /// <summary>
	  /// Combines this parameter sensitivities with another instance.
	  /// <para>
	  /// This returns a new sensitivity instance with a combined list of parameter sensitivities.
	  /// This instance is immutable and unaffected by this method.
	  /// The result may contain duplicate parameter sensitivities.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other parameter sensitivities </param>
	  /// <returns> an instance based on this one, with the other instance added </returns>
	  public UnitParameterSensitivities combinedWith(UnitParameterSensitivities other)
	  {
		IList<UnitParameterSensitivity> mutable = new List<UnitParameterSensitivity>(sensitivities);
		foreach (UnitParameterSensitivity otherSens in other.sensitivities)
		{
		  insert(mutable, otherSens);
		}
		return new UnitParameterSensitivities(ImmutableList.copyOf(mutable));
	  }

	  // inserts a sensitivity into the mutable list in the right location
	  // merges the entry with an existing entry if the key matches
	  private static void insert(IList<UnitParameterSensitivity> mutable, UnitParameterSensitivity addition)
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		int index = Collections.binarySearch(mutable, addition, UnitParameterSensitivity::compareKey);
		if (index >= 0)
		{
		  UnitParameterSensitivity @base = mutable[index];
		  DoubleArray combined = @base.Sensitivity.plus(addition.Sensitivity);
		  mutable[index] = @base.withSensitivity(combined);
		}
		else
		{
		  int insertionPoint = -(index + 1);
		  mutable.Insert(insertionPoint, addition);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this sensitivity to a monetary value, multiplying by the specified factor.
	  /// <para>
	  /// The result will consist of the entries based on the entries of this instance.
	  /// Each entry in the result will be in the specified currency and multiplied by the specified amount.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency of the amount </param>
	  /// <param name="amount">  the amount to multiply by </param>
	  /// <returns> the resulting sensitivity object </returns>
	  public CurrencyParameterSensitivities multipliedBy(Currency currency, double amount)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return sensitivities.Select(s => s.multipliedBy(currency, amount)).collect(Collectors.collectingAndThen(Guavate.toImmutableList(), CurrencyParameterSensitivities.of));
	  }

	  /// <summary>
	  /// Returns an instance with the sensitivity values multiplied by the specified factor.
	  /// <para>
	  /// The result will consist of the same entries, but with each sensitivity value multiplied.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="factor">  the multiplicative factor </param>
	  /// <returns> an instance based on this one, with each sensitivity multiplied by the factor </returns>
	  public UnitParameterSensitivities multipliedBy(double factor)
	  {
		return mapSensitivities(s => s * factor);
	  }

	  /// <summary>
	  /// Returns an instance with the specified operation applied to the sensitivity values.
	  /// <para>
	  /// The result will consist of the same entries, but with the operator applied to each sensitivity value.
	  /// This instance is immutable and unaffected by this method.
	  /// </para>
	  /// <para>
	  /// This is used to apply a mathematical operation to the sensitivity values.
	  /// For example, the operator could multiply the sensitivities by a constant, or take the inverse.
	  /// <pre>
	  ///   inverse = base.mapSensitivities(value -> 1 / value);
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="operator">  the operator to be applied to the sensitivities </param>
	  /// <returns> an instance based on this one, with the operator applied to the sensitivity values </returns>
	  public UnitParameterSensitivities mapSensitivities(System.Func<double, double> @operator)
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return sensitivities.Select(s => s.mapSensitivity(@operator)).collect(Collectors.collectingAndThen(Guavate.toImmutableList(), UnitParameterSensitivities::new));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Splits this sensitivity instance.
	  /// <para>
	  /// This examines each individual sensitivity to see if it can be <seealso cref="CurrencyParameterSensitivity#split() split"/>.
	  /// If any can be split, the result will contain the combination of the split sensitivities.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> this sensitivity, with any combined sensitivities split </returns>
	  public UnitParameterSensitivities split()
	  {
		if (!sensitivities.Any(s => s.ParameterSplit.Present))
		{
		  return this;
		}
		return of(sensitivities.stream().flatMap(s => s.Split().stream()).collect(toImmutableList()));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this sensitivity equals another within the specified tolerance.
	  /// <para>
	  /// This returns true if the two instances have the same keys, with arrays of the
	  /// same length, where the {@code double} values are equal within the specified tolerance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other sensitivity </param>
	  /// <param name="tolerance">  the tolerance </param>
	  /// <returns> true if equal up to the tolerance </returns>
	  public bool equalWithTolerance(UnitParameterSensitivities other, double tolerance)
	  {
		IList<UnitParameterSensitivity> mutable = new List<UnitParameterSensitivity>(other.sensitivities);
		// for each sensitivity in this instance, find matching in other instance
		foreach (UnitParameterSensitivity sens1 in sensitivities)
		{
		  // list is already sorted so binary search is safe
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		  int index = Collections.binarySearch(mutable, sens1, UnitParameterSensitivity::compareKey);
		  if (index >= 0)
		  {
			// matched, so must be equal
			UnitParameterSensitivity sens2 = mutable[index];
			if (!sens1.Sensitivity.equalWithTolerance(sens2.Sensitivity, tolerance))
			{
			  return false;
			}
			mutable.RemoveAt(index);
		  }
		  else
		  {
			// did not match, so must be zero
			if (!sens1.Sensitivity.equalZeroWithTolerance(tolerance))
			{
			  return false;
			}
		  }
		}
		// all that remain from other instance must be zero
		foreach (UnitParameterSensitivity sens2 in mutable)
		{
		  if (!sens2.Sensitivity.equalZeroWithTolerance(tolerance))
		  {
			return false;
		  }
		}
		return true;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code UnitParameterSensitivities}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static UnitParameterSensitivities.Meta meta()
	  {
		return UnitParameterSensitivities.Meta.INSTANCE;
	  }

	  static UnitParameterSensitivities()
	  {
		MetaBean.register(UnitParameterSensitivities.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override UnitParameterSensitivities.Meta metaBean()
	  {
		return UnitParameterSensitivities.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the parameter sensitivities.
	  /// <para>
	  /// Each entry includes details of the <seealso cref="ParameterizedData"/> it relates to.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<UnitParameterSensitivity> Sensitivities
	  {
		  get
		  {
			return sensitivities;
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
		  UnitParameterSensitivities other = (UnitParameterSensitivities) obj;
		  return JodaBeanUtils.equal(sensitivities, other.sensitivities);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sensitivities);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("UnitParameterSensitivities{");
		buf.Append("sensitivities").Append('=').Append(JodaBeanUtils.ToString(sensitivities));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code UnitParameterSensitivities}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  sensitivities_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivities", typeof(UnitParameterSensitivities), (Type) typeof(ImmutableList));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "sensitivities");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code sensitivities} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<UnitParameterSensitivity>> sensitivities = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "sensitivities", UnitParameterSensitivities.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<UnitParameterSensitivity>> sensitivities_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "sensitivities");
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
			case 1226228605: // sensitivities
			  return sensitivities_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends UnitParameterSensitivities> builder()
		public override BeanBuilder<UnitParameterSensitivities> builder()
		{
		  return new UnitParameterSensitivities.Builder();
		}

		public override Type beanType()
		{
		  return typeof(UnitParameterSensitivities);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code sensitivities} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<UnitParameterSensitivity>> sensitivities()
		{
		  return sensitivities_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1226228605: // sensitivities
			  return ((UnitParameterSensitivities) bean).Sensitivities;
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
	  /// The bean-builder for {@code UnitParameterSensitivities}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<UnitParameterSensitivities>
	  {

		internal IList<UnitParameterSensitivity> sensitivities = ImmutableList.of();

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
			case 1226228605: // sensitivities
			  return sensitivities;
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
			case 1226228605: // sensitivities
			  this.sensitivities = (IList<UnitParameterSensitivity>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override UnitParameterSensitivities build()
		{
		  return new UnitParameterSensitivities(sensitivities);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("UnitParameterSensitivities.Builder{");
		  buf.Append("sensitivities").Append('=').Append(JodaBeanUtils.ToString(sensitivities));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}