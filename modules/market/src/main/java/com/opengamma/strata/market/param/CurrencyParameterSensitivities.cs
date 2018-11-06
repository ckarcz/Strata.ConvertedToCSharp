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
//	import static com.opengamma.strata.basics.currency.MultiCurrencyAmount.toMultiCurrencyAmount;
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
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxConvertible = com.opengamma.strata.basics.currency.FxConvertible;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Guavate = com.opengamma.strata.collect.Guavate;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using Surface = com.opengamma.strata.market.surface.Surface;

	/// <summary>
	/// Currency-based parameter sensitivity for parameterized market data, such as curves.
	/// <para>
	/// Parameter sensitivity is the sensitivity of a value to the parameters of
	/// <seealso cref="ParameterizedData parameterized market data"/> objects that are used to determine the value.
	/// Common {@code ParameterizedData} implementations include <seealso cref="Curve"/> and <seealso cref="Surface"/>.
	/// </para>
	/// <para>
	/// For example, the parameter sensitivity for present value on a FRA might contain
	/// two entries, one for the Ibor forward curve and one for the discount curve.
	/// Each entry identifies the curve that was queried and the resulting sensitivity values,
	/// one for each node on the curve.
	/// </para>
	/// <para>
	/// The sensitivity is expressed as a single entry for piece of parameterized market data.
	/// The sensitivity represents a monetary value in the specified currency.
	/// The order of the list has no specific meaning.
	/// </para>
	/// <para>
	/// One way of viewing this class is as a {@code Map} from a specific sensitivity key to
	/// {@code DoubleArray} sensitivity values. However, instead of being structured as a {@code Map},
	/// the data is structured as a {@code List}, with the key and value in each entry.
	/// As such, the sensitivities are always in a "normalized" form.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class CurrencyParameterSensitivities implements com.opengamma.strata.basics.currency.FxConvertible<CurrencyParameterSensitivities>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CurrencyParameterSensitivities : FxConvertible<CurrencyParameterSensitivities>, ImmutableBean
	{

	  /// <summary>
	  /// An empty instance.
	  /// </summary>
	  private static readonly CurrencyParameterSensitivities EMPTY = new CurrencyParameterSensitivities(ImmutableList.of());

	  /// <summary>
	  /// The parameter sensitivities.
	  /// <para>
	  /// Each entry includes details of the <seealso cref="ParameterizedData"/> it relates to.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<CurrencyParameterSensitivity> sensitivities;
	  private readonly ImmutableList<CurrencyParameterSensitivity> sensitivities;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// An empty sensitivity instance.
	  /// </summary>
	  /// <returns> the empty instance </returns>
	  public static CurrencyParameterSensitivities empty()
	  {
		return EMPTY;
	  }

	  /// <summary>
	  /// Returns a builder that can be used to create an instance of {@code CurrencyParameterSensitivities}.
	  /// <para>
	  /// The builder takes into account the parameter metadata when creating the sensitivity map.
	  /// As such, the parameter metadata added to the builder must not be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the builder </returns>
	  public static CurrencyParameterSensitivitiesBuilder builder()
	  {
		return new CurrencyParameterSensitivitiesBuilder();
	  }

	  /// <summary>
	  /// Obtains an instance from a single sensitivity entry.
	  /// </summary>
	  /// <param name="sensitivity">  the sensitivity entry </param>
	  /// <returns> the sensitivities instance </returns>
	  public static CurrencyParameterSensitivities of(CurrencyParameterSensitivity sensitivity)
	  {
		return new CurrencyParameterSensitivities(ImmutableList.of(sensitivity));
	  }

	  /// <summary>
	  /// Obtains an instance from an array of sensitivity entries.
	  /// <para>
	  /// The sensitivities are sorted using <seealso cref="CurrencyParameterSensitivity#compareKey"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="sensitivities">  the sensitivities </param>
	  /// <returns> the sensitivities instance </returns>
	  public static CurrencyParameterSensitivities of(params CurrencyParameterSensitivity[] sensitivities)
	  {
		return of(Arrays.asList(sensitivities));
	  }

	  /// <summary>
	  /// Obtains an instance from a list of sensitivity entries.
	  /// <para>
	  /// The sensitivities are sorted using <seealso cref="CurrencyParameterSensitivity#compareKey"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="sensitivities">  the list of sensitivity entries </param>
	  /// <returns> the sensitivities instance </returns>
	  public static CurrencyParameterSensitivities of<T1>(IList<T1> sensitivities) where T1 : CurrencyParameterSensitivity
	  {
		IList<CurrencyParameterSensitivity> mutable = new List<CurrencyParameterSensitivity>();
		foreach (CurrencyParameterSensitivity otherSens in sensitivities)
		{
		  insert(mutable, otherSens);
		}
		return new CurrencyParameterSensitivities(ImmutableList.copyOf(mutable));
	  }

	  // used when not pre-sorted
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private CurrencyParameterSensitivities(java.util.List<? extends CurrencyParameterSensitivity> sensitivities)
	  private CurrencyParameterSensitivities<T1>(IList<T1> sensitivities) where T1 : CurrencyParameterSensitivity
	  {
		if (sensitivities.Count < 2)
		{
		  this.sensitivities = ImmutableList.copyOf(sensitivities);
		}
		else
		{
		  IList<CurrencyParameterSensitivity> mutable = new List<CurrencyParameterSensitivity>(sensitivities);
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		  mutable.sort(CurrencyParameterSensitivity::compareKey);
		  this.sensitivities = ImmutableList.copyOf(mutable);
		}
	  }

	  // used when pre-sorted
	  private CurrencyParameterSensitivities(ImmutableList<CurrencyParameterSensitivity> sensitivities)
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
	  public CurrencyParameterSensitivity getSensitivity<T1>(MarketDataName<T1> name, Currency currency)
	  {
		return findSensitivity(name, currency).orElseThrow(() => new System.ArgumentException(Messages.format("Unable to find sensitivity: {} for {}", name, currency)));
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
	  public Optional<CurrencyParameterSensitivity> findSensitivity<T1>(MarketDataName<T1> name, Currency currency)
	  {
		return sensitivities.Where(sens => sens.MarketDataName.Equals(name) && sens.Currency.Equals(currency)).First();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Combines this parameter sensitivities with another instance.
	  /// <para>
	  /// This returns a new sensitivity instance with the specified sensitivity added.
	  /// This instance is immutable and unaffected by this method.
	  /// </para>
	  /// <para>
	  /// The sensitivities are merged using market data name and currency as a key.
	  /// The parameter metadata is not checked, thus the caller must ensure the sensitivities
	  /// are compatible with the same metadata and parameter count.
	  /// To combine taking the metadata into account, use <seealso cref="#mergedWith(CurrencyParameterSensitivities)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other parameter sensitivity </param>
	  /// <returns> an instance based on this one, with the other instance added </returns>
	  public CurrencyParameterSensitivities combinedWith(CurrencyParameterSensitivity other)
	  {
		IList<CurrencyParameterSensitivity> mutable = new List<CurrencyParameterSensitivity>(sensitivities);
		insert(mutable, other);
		return new CurrencyParameterSensitivities(ImmutableList.copyOf(mutable));
	  }

	  /// <summary>
	  /// Combines this parameter sensitivities with another instance.
	  /// <para>
	  /// This returns a new sensitivity instance with a combined list of parameter sensitivities.
	  /// This instance is immutable and unaffected by this method.
	  /// </para>
	  /// <para>
	  /// The sensitivities are merged using market data name and currency as a key.
	  /// The parameter metadata is not checked, thus the caller must ensure the sensitivities
	  /// are compatible with the same metadata and parameter count.
	  /// To combine taking the metadata into account, use <seealso cref="#mergedWith(CurrencyParameterSensitivities)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other parameter sensitivities </param>
	  /// <returns> an instance based on this one, with the other instance added </returns>
	  public CurrencyParameterSensitivities combinedWith(CurrencyParameterSensitivities other)
	  {
		IList<CurrencyParameterSensitivity> mutable = new List<CurrencyParameterSensitivity>(sensitivities);
		foreach (CurrencyParameterSensitivity otherSens in other.sensitivities)
		{
		  insert(mutable, otherSens);
		}
		return new CurrencyParameterSensitivities(ImmutableList.copyOf(mutable));
	  }

	  // inserts a sensitivity into the mutable list in the right location
	  // merges the entry with an existing entry if the key matches
	  private static void insert(IList<CurrencyParameterSensitivity> mutable, CurrencyParameterSensitivity addition)
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		int index = Collections.binarySearch(mutable, addition, CurrencyParameterSensitivity::compareKey);
		if (index >= 0)
		{
		  CurrencyParameterSensitivity @base = mutable[index];
		  DoubleArray combined = @base.Sensitivity.plus(addition.Sensitivity);
		  mutable[index] = @base.withSensitivity(combined);
		}
		else
		{
		  int insertionPoint = -(index + 1);
		  mutable.Insert(insertionPoint, addition);
		}
	  }

	  /// <summary>
	  /// Merges this parameter sensitivities with another instance taking the metadata into account.
	  /// <para>
	  /// This returns a new sensitivity instance with a combined set of parameter sensitivities.
	  /// This instance is immutable and unaffected by this method.
	  /// </para>
	  /// <para>
	  /// The sensitivities are merged using market data name and currency as a key.
	  /// Each sensitivity is then merged taking into account the metadata, such as the tenor.
	  /// As such, this method can only be used if the parameter metadata instances are not be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other parameter sensitivities </param>
	  /// <returns> an instance based on this one, with the other instance added </returns>
	  /// <exception cref="IllegalArgumentException"> if any metadata instance is empty </exception>
	  public CurrencyParameterSensitivities mergedWith(CurrencyParameterSensitivities other)
	  {
		return toBuilder().add(other).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks and adjusts the market data names.
	  /// <para>
	  /// The supplied function is invoked for each market data name in this sensitivities.
	  /// If the function returns the same name for two different inputs, the sensitivity values will be summed.
	  /// A typical use case would be to convert index names to curve names valid for an underlying system.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="nameFn">  the function for checking and adjusting the name </param>
	  /// <returns> the adjusted sensitivity </returns>
	  /// <exception cref="RuntimeException"> if the function throws an exception </exception>
	  public CurrencyParameterSensitivities withMarketDataNames<T1>(System.Func<T1> nameFn)
	  {
		CurrencyParameterSensitivitiesBuilder builder = CurrencyParameterSensitivities.builder();
		foreach (CurrencyParameterSensitivity sensitivity in sensitivities)
		{
		  builder.add(sensitivity.toBuilder().marketDataName(nameFn(sensitivity.MarketDataName)).build());
		}
		return builder.build();
	  }

	  /// <summary>
	  /// Checks and adjusts the parameter metadata.
	  /// <para>
	  /// The supplied function is invoked for each parameter metadata in this sensitivities.
	  /// If the function returns the same metadata for two different inputs, the sensitivity values will be summed.
	  /// A typical use case would be to normalize parameter metadata tenors to be valid for an underlying system.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mdFn">  the function for checking and adjusting the metadata </param>
	  /// <returns> the adjusted sensitivity </returns>
	  /// <exception cref="IllegalArgumentException"> if any metadata instance is empty </exception>
	  /// <exception cref="RuntimeException"> if the function throws an exception </exception>
	  public CurrencyParameterSensitivities withParameterMetadatas(System.Func<ParameterMetadata, ParameterMetadata> mdFn)
	  {
		return toBuilder().mapMetadata(mdFn).build();
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
	  public CurrencyParameterSensitivities convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		IList<CurrencyParameterSensitivity> mutable = new List<CurrencyParameterSensitivity>();
		foreach (CurrencyParameterSensitivity sens in sensitivities)
		{
		  insert(mutable, sens.convertedTo(resultCurrency, rateProvider));
		}
		return new CurrencyParameterSensitivities(ImmutableList.copyOf(mutable));
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
	  public CurrencyParameterSensitivities split()
	  {
		if (!sensitivities.Any(s => s.ParameterSplit.Present))
		{
		  return this;
		}
		return of(sensitivities.stream().flatMap(s => s.Split().stream()).collect(toImmutableList()));
	  }

	  //-------------------------------------------------------------------------
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
		CurrencyParameterSensitivities converted = convertedTo(resultCurrency, rateProvider);
		double total = converted.sensitivities.Select(s => s.Sensitivity.sum()).Sum();
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
		return sensitivities.Select(CurrencyParameterSensitivity::total).collect(toMultiCurrencyAmount());
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
	  public CurrencyParameterSensitivities multipliedBy(double factor)
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
	  public CurrencyParameterSensitivities mapSensitivities(System.Func<double, double> @operator)
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return sensitivities.Select(s => s.mapSensitivity(@operator)).collect(Collectors.collectingAndThen(Guavate.toImmutableList(), CurrencyParameterSensitivities::new));
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
	  public bool equalWithTolerance(CurrencyParameterSensitivities other, double tolerance)
	  {
		IList<CurrencyParameterSensitivity> mutable = new List<CurrencyParameterSensitivity>(other.sensitivities);
		// for each sensitivity in this instance, find matching in other instance
		foreach (CurrencyParameterSensitivity sens1 in sensitivities)
		{
		  // list is already sorted so binary search is safe
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		  int index = Collections.binarySearch(mutable, sens1, CurrencyParameterSensitivity::compareKey);
		  if (index >= 0)
		  {
			// matched, so must be equal
			CurrencyParameterSensitivity sens2 = mutable[index];
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
		foreach (CurrencyParameterSensitivity sens2 in mutable)
		{
		  if (!sens2.Sensitivity.equalZeroWithTolerance(tolerance))
		  {
			return false;
		  }
		}
		return true;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder populated with the set of sensitivities from this instance.
	  /// <para>
	  /// The builder takes into account the parameter metadata when creating the sensitivity map.
	  /// As such, the parameter metadata added to the builder must not be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the builder </returns>
	  /// <exception cref="IllegalArgumentException"> if any metadata instance is empty </exception>
	  public CurrencyParameterSensitivitiesBuilder toBuilder()
	  {
		return new CurrencyParameterSensitivitiesBuilder(sensitivities);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CurrencyParameterSensitivities}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CurrencyParameterSensitivities.Meta meta()
	  {
		return CurrencyParameterSensitivities.Meta.INSTANCE;
	  }

	  static CurrencyParameterSensitivities()
	  {
		MetaBean.register(CurrencyParameterSensitivities.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override CurrencyParameterSensitivities.Meta metaBean()
	  {
		return CurrencyParameterSensitivities.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the parameter sensitivities.
	  /// <para>
	  /// Each entry includes details of the <seealso cref="ParameterizedData"/> it relates to.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<CurrencyParameterSensitivity> Sensitivities
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
		  CurrencyParameterSensitivities other = (CurrencyParameterSensitivities) obj;
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
		buf.Append("CurrencyParameterSensitivities{");
		buf.Append("sensitivities").Append('=').Append(JodaBeanUtils.ToString(sensitivities));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CurrencyParameterSensitivities}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  sensitivities_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivities", typeof(CurrencyParameterSensitivities), (Type) typeof(ImmutableList));
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
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<CurrencyParameterSensitivity>> sensitivities = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "sensitivities", CurrencyParameterSensitivities.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<CurrencyParameterSensitivity>> sensitivities_Renamed;
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
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends CurrencyParameterSensitivities> builder()
		public override BeanBuilder<CurrencyParameterSensitivities> builder()
		{
		  return new CurrencyParameterSensitivities.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CurrencyParameterSensitivities);
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
		public MetaProperty<ImmutableList<CurrencyParameterSensitivity>> sensitivities()
		{
		  return sensitivities_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1226228605: // sensitivities
			  return ((CurrencyParameterSensitivities) bean).Sensitivities;
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
	  /// The bean-builder for {@code CurrencyParameterSensitivities}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<CurrencyParameterSensitivities>
	  {

		internal IList<CurrencyParameterSensitivity> sensitivities = ImmutableList.of();

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
			  this.sensitivities = (IList<CurrencyParameterSensitivity>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override CurrencyParameterSensitivities build()
		{
		  return new CurrencyParameterSensitivities(sensitivities);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("CurrencyParameterSensitivities.Builder{");
		  buf.Append("sensitivities").Append('=').Append(JodaBeanUtils.ToString(sensitivities));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}