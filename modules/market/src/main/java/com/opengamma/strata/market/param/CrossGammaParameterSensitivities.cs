using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.MultiCurrencyAmount.toMultiCurrencyAmount;


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
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxConvertible = com.opengamma.strata.basics.currency.FxConvertible;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using Guavate = com.opengamma.strata.collect.Guavate;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using Curve = com.opengamma.strata.market.curve.Curve;

	/// <summary>
	/// The second order parameter sensitivity for parameterized market data.
	/// <para>
	/// Parameter sensitivity is the sensitivity of a value to the parameters of
	/// <seealso cref="ParameterizedData parameterized market data"/> objects that are used to determine the value.
	/// The main application is the parameter sensitivities for curves. Thus {@code ParameterizedData} is typically <seealso cref="Curve"/>.
	/// </para>
	/// <para>
	/// The sensitivity is expressed as a single entry of second order sensitivities for piece of parameterized market data.
	/// The cross-gamma between different {@code ParameterizedData} is not represented.
	/// The sensitivity represents a monetary value in the specified currency.
	/// The order of the list has no specific meaning.
	/// </para>
	/// <para>
	/// One way of viewing this class is as a {@code Map} from a specific sensitivity key to
	/// {@code DoubleMatrix} sensitivity values. However, instead of being structured as a {@code Map},
	/// the data is structured as a {@code List}, with the key and value in each entry.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class CrossGammaParameterSensitivities implements com.opengamma.strata.basics.currency.FxConvertible<CrossGammaParameterSensitivities>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CrossGammaParameterSensitivities : FxConvertible<CrossGammaParameterSensitivities>, ImmutableBean
	{

	  /// <summary>
	  /// An empty instance.
	  /// </summary>
	  private static readonly CrossGammaParameterSensitivities EMPTY = new CrossGammaParameterSensitivities(ImmutableList.of());

	  /// <summary>
	  /// The parameter sensitivities.
	  /// <para>
	  /// Each entry includes details of the <seealso cref="ParameterizedData"/> it relates to.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<CrossGammaParameterSensitivity> sensitivities;
	  private readonly ImmutableList<CrossGammaParameterSensitivity> sensitivities;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// An empty sensitivity instance.
	  /// </summary>
	  /// <returns> the empty instance </returns>
	  public static CrossGammaParameterSensitivities empty()
	  {
		return EMPTY;
	  }

	  /// <summary>
	  /// Obtains an instance from a single sensitivity entry.
	  /// </summary>
	  /// <param name="sensitivity">  the sensitivity entry </param>
	  /// <returns> the sensitivities instance </returns>
	  public static CrossGammaParameterSensitivities of(CrossGammaParameterSensitivity sensitivity)
	  {
		return new CrossGammaParameterSensitivities(ImmutableList.of(sensitivity));
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
	  public static CrossGammaParameterSensitivities of(params CrossGammaParameterSensitivity[] sensitivities)
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
	  public static CrossGammaParameterSensitivities of<T1>(IList<T1> sensitivities) where T1 : CrossGammaParameterSensitivity
	  {
		IList<CrossGammaParameterSensitivity> mutable = new List<CrossGammaParameterSensitivity>();
		foreach (CrossGammaParameterSensitivity otherSens in sensitivities)
		{
		  insert(mutable, otherSens);
		}
		return new CrossGammaParameterSensitivities(ImmutableList.copyOf(mutable));
	  }

	  // used when not pre-sorted
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private CrossGammaParameterSensitivities(java.util.List<? extends CrossGammaParameterSensitivity> sensitivities)
	  private CrossGammaParameterSensitivities<T1>(IList<T1> sensitivities) where T1 : CrossGammaParameterSensitivity
	  {
		if (sensitivities.Count < 2)
		{
		  this.sensitivities = ImmutableList.copyOf(sensitivities);
		}
		else
		{
		  IList<CrossGammaParameterSensitivity> mutable = new List<CrossGammaParameterSensitivity>(sensitivities);
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		  mutable.sort(CrossGammaParameterSensitivity::compareKey);
		  this.sensitivities = ImmutableList.copyOf(mutable);
		}
	  }

	  // used when pre-sorted
	  private CrossGammaParameterSensitivities(ImmutableList<CrossGammaParameterSensitivity> sensitivities)
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
	  /// Gets a single sensitivity instance by name and currency.
	  /// </summary>
	  /// <param name="name">  the curve name to find </param>
	  /// <param name="currency">  the currency to find </param>
	  /// <returns> the matching sensitivity </returns>
	  /// <exception cref="IllegalArgumentException"> if the name and currency do not match an entry </exception>
	  public CrossGammaParameterSensitivity getSensitivity<T1>(MarketDataName<T1> name, Currency currency)
	  {
		return findSensitivity(name, currency).orElseThrow(() => new System.ArgumentException(Messages.format("Unable to find sensitivity: {} for {}", name, currency)));
	  }

	  /// <summary>
	  /// Gets a single sensitivity instance by names and currency.
	  /// <para>
	  /// This returns the sensitivity of the market data ({@code nameFirst}) delta to another market data ({@code nameSecond}).
	  /// The result is sensitive to the order of {@code nameFirst} and {@code nameSecond}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="nameFirst">  the name </param>
	  /// <param name="nameSecond">  the name </param>
	  /// <param name="currency">  the currency </param>
	  /// <returns> the matching sensitivity </returns>
	  /// <exception cref="IllegalArgumentException"> if the name and currency do not match an entry </exception>
	  public CrossGammaParameterSensitivity getSensitivity<T1, T2>(MarketDataName<T1> nameFirst, MarketDataName<T2> nameSecond, Currency currency)
	  {

		CrossGammaParameterSensitivity sensi = findSensitivity(nameFirst, currency).orElseThrow(() => new System.ArgumentException(Messages.format("Unable to find sensitivity: {} for {}", nameFirst, currency)));
		return sensi.getSensitivity(nameSecond);
	  }

	  /// <summary>
	  /// Finds a single sensitivity instance by name and currency.
	  /// <para>
	  /// If the sensitivity is not found, optional empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name to find </param>
	  /// <param name="currency">  the currency to find </param>
	  /// <returns> the matching sensitivity </returns>
	  public Optional<CrossGammaParameterSensitivity> findSensitivity<T1>(MarketDataName<T1> name, Currency currency)
	  {
		return sensitivities.Where(sens => sens.MarketDataName.Equals(name) && sens.Currency.Equals(currency)).First();
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
	  public CrossGammaParameterSensitivities combinedWith(CrossGammaParameterSensitivity other)
	  {
		IList<CrossGammaParameterSensitivity> mutable = new List<CrossGammaParameterSensitivity>(sensitivities);
		insert(mutable, other);
		return new CrossGammaParameterSensitivities(ImmutableList.copyOf(mutable));
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
	  public CrossGammaParameterSensitivities combinedWith(CrossGammaParameterSensitivities other)
	  {
		IList<CrossGammaParameterSensitivity> mutable = new List<CrossGammaParameterSensitivity>(sensitivities);
		foreach (CrossGammaParameterSensitivity otherSens in other.sensitivities)
		{
		  insert(mutable, otherSens);
		}
		return new CrossGammaParameterSensitivities(ImmutableList.copyOf(mutable));
	  }

	  // inserts a sensitivity into the mutable list in the right location
	  // merges the entry with an existing entry if the key matches
	  private static void insert(IList<CrossGammaParameterSensitivity> mutable, CrossGammaParameterSensitivity addition)
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		int index = Collections.binarySearch(mutable, addition, CrossGammaParameterSensitivity::compareKey);
		if (index >= 0)
		{
		  CrossGammaParameterSensitivity @base = mutable[index];
		  DoubleMatrix combined = @base.Sensitivity.plus(addition.Sensitivity);
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
	  /// Converts the sensitivities in this instance to an equivalent in the specified currency.
	  /// <para>
	  /// Any FX conversion that is required will use rates from the provider.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resultCurrency">  the currency of the result </param>
	  /// <param name="rateProvider">  the provider of FX rates </param>
	  /// <returns> the sensitivity object expressed in terms of the result currency </returns>
	  /// <exception cref="RuntimeException"> if no FX rate could be found </exception>
	  public CrossGammaParameterSensitivities convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		IList<CrossGammaParameterSensitivity> mutable = new List<CrossGammaParameterSensitivity>();
		foreach (CrossGammaParameterSensitivity sens in sensitivities)
		{
		  insert(mutable, sens.convertedTo(resultCurrency, rateProvider));
		}
		return new CrossGammaParameterSensitivities(ImmutableList.copyOf(mutable));
	  }

	  /// <summary>
	  /// Returns the total of the sensitivity values.
	  /// <para>
	  /// The result is the total of all values, as converted to the specified currency.
	  /// Any FX conversion that is required will use rates from the provider.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resultCurrency">  the currency of the result </param>
	  /// <param name="rateProvider">  the provider of FX rates </param>
	  /// <returns> the total sensitivity </returns>
	  /// <exception cref="RuntimeException"> if no FX rate could be found </exception>
	  public CurrencyAmount total(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		CrossGammaParameterSensitivities converted = convertedTo(resultCurrency, rateProvider);
		double total = converted.sensitivities.Select(s => s.Sensitivity.total()).Sum();
		return CurrencyAmount.of(resultCurrency, total);
	  }

	  /// <summary>
	  /// Returns the total of the sensitivity values.
	  /// <para>
	  /// The result is the total of all values, in whatever currency they are defined.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the total sensitivity </returns>
	  public MultiCurrencyAmount total()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return sensitivities.Select(CrossGammaParameterSensitivity::total).collect(toMultiCurrencyAmount());
	  }

	  /// <summary>
	  /// Returns the diagonal part of the sensitivity values.
	  /// </summary>
	  /// <returns> the diagonal part </returns>
	  public CurrencyParameterSensitivities diagonal()
	  {
		return CurrencyParameterSensitivities.of(sensitivities.Select(s => s.diagonal()).ToList());
	  }

	  //-------------------------------------------------------------------------
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
	  public CrossGammaParameterSensitivities multipliedBy(double factor)
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
	  public CrossGammaParameterSensitivities mapSensitivities(System.Func<double, double> @operator)
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return sensitivities.Select(s => s.mapSensitivity(@operator)).collect(Collectors.collectingAndThen(Guavate.toImmutableList(), CrossGammaParameterSensitivities::new));
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
	  public bool equalWithTolerance(CrossGammaParameterSensitivities other, double tolerance)
	  {
		IList<CrossGammaParameterSensitivity> mutable = new List<CrossGammaParameterSensitivity>(other.sensitivities);
		// for each sensitivity in this instance, find matching in other instance
		foreach (CrossGammaParameterSensitivity sens1 in sensitivities)
		{
		  // list is already sorted so binary search is safe
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		  int index = Collections.binarySearch(mutable, sens1, CrossGammaParameterSensitivity::compareKey);
		  if (index >= 0)
		  {
			// matched, so must be equal
			CrossGammaParameterSensitivity sens2 = mutable[index];
			if (!equalWithTolerance(sens1.Sensitivity, sens2.Sensitivity, tolerance))
			{
			  return false;
			}
			mutable.RemoveAt(index);
		  }
		  else
		  {
			// did not match, so must be zero
			if (!equalZeroWithTolerance(sens1.Sensitivity, tolerance))
			{
			  return false;
			}
		  }
		}
		// all that remain from other instance must be zero
		foreach (CrossGammaParameterSensitivity sens2 in mutable)
		{
		  if (!equalZeroWithTolerance(sens2.Sensitivity, tolerance))
		  {
			return false;
		  }
		}
		return true;
	  }

	  private bool equalWithTolerance(DoubleMatrix sens1, DoubleMatrix sens2, double tolerance)
	  {
		int colCount = sens1.columnCount();
		if (colCount != sens2.columnCount())
		{
		  return false;
		}
		for (int i = 0; i < colCount; ++i)
		{
		  if (!sens1.column(i).equalWithTolerance(sens2.column(i), tolerance))
		  {
			return false;
		  }
		}
		return true;
	  }

	  private bool equalZeroWithTolerance(DoubleMatrix sens, double tolerance)
	  {
		int colCount = sens.columnCount();
		for (int i = 0; i < colCount; ++i)
		{
		  if (!DoubleArrayMath.fuzzyEqualsZero(sens.column(i).toArray(), tolerance))
		  {
			return false;
		  }
		}
		return true;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CrossGammaParameterSensitivities}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CrossGammaParameterSensitivities.Meta meta()
	  {
		return CrossGammaParameterSensitivities.Meta.INSTANCE;
	  }

	  static CrossGammaParameterSensitivities()
	  {
		MetaBean.register(CrossGammaParameterSensitivities.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override CrossGammaParameterSensitivities.Meta metaBean()
	  {
		return CrossGammaParameterSensitivities.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the parameter sensitivities.
	  /// <para>
	  /// Each entry includes details of the <seealso cref="ParameterizedData"/> it relates to.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<CrossGammaParameterSensitivity> Sensitivities
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
		  CrossGammaParameterSensitivities other = (CrossGammaParameterSensitivities) obj;
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
		buf.Append("CrossGammaParameterSensitivities{");
		buf.Append("sensitivities").Append('=').Append(JodaBeanUtils.ToString(sensitivities));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CrossGammaParameterSensitivities}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  sensitivities_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivities", typeof(CrossGammaParameterSensitivities), (Type) typeof(ImmutableList));
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
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<CrossGammaParameterSensitivity>> sensitivities = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "sensitivities", CrossGammaParameterSensitivities.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<CrossGammaParameterSensitivity>> sensitivities_Renamed;
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
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends CrossGammaParameterSensitivities> builder()
		public override BeanBuilder<CrossGammaParameterSensitivities> builder()
		{
		  return new CrossGammaParameterSensitivities.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CrossGammaParameterSensitivities);
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
		public MetaProperty<ImmutableList<CrossGammaParameterSensitivity>> sensitivities()
		{
		  return sensitivities_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1226228605: // sensitivities
			  return ((CrossGammaParameterSensitivities) bean).Sensitivities;
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
	  /// The bean-builder for {@code CrossGammaParameterSensitivities}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<CrossGammaParameterSensitivities>
	  {

		internal IList<CrossGammaParameterSensitivity> sensitivities = ImmutableList.of();

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
			  this.sensitivities = (IList<CrossGammaParameterSensitivity>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override CrossGammaParameterSensitivities build()
		{
		  return new CrossGammaParameterSensitivities(sensitivities);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("CrossGammaParameterSensitivities.Builder{");
		  buf.Append("sensitivities").Append('=').Append(JodaBeanUtils.ToString(sensitivities));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}