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

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ComparisonChain = com.google.common.collect.ComparisonChain;
	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using Surface = com.opengamma.strata.market.surface.Surface;

	/// <summary>
	/// Unit parameter sensitivity for parameterized market data, such as a curve.
	/// <para>
	/// Parameter sensitivity is the sensitivity of a value to the parameters of a
	/// <seealso cref="ParameterizedData parameterized market data"/> object that is used to determine the value.
	/// Common {@code ParameterizedData} implementations include <seealso cref="Curve"/> and <seealso cref="Surface"/>.
	/// </para>
	/// <para>
	/// The sensitivity is expressed as an array, with one entry for each parameter in the {@code ParameterizedData}.
	/// The sensitivity has no associated currency.
	/// </para>
	/// <para>
	/// A single {@code UnitParameterSensitivity} represents the sensitivity to a single {@code ParameterizedData} instance.
	/// However, a {@code ParameterizedData} instance can itself be backed by more than one underlying instance.
	/// For example, a curve formed from two underlying curves.
	/// Information about the split between these underlying instances can optionally be stored.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class UnitParameterSensitivity implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class UnitParameterSensitivity : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.MarketDataName<?> marketDataName;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		private readonly MarketDataName<object> marketDataName;
	  /// <summary>
	  /// The list of parameter metadata.
	  /// <para>
	  /// There is one entry for each parameter.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "List<? extends ParameterMetadata>") private final com.google.common.collect.ImmutableList<ParameterMetadata> parameterMetadata;
	  private readonly ImmutableList<ParameterMetadata> parameterMetadata;
	  /// <summary>
	  /// The parameter sensitivity values.
	  /// <para>
	  /// There is one sensitivity value for each parameter.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleArray sensitivity;
	  private readonly DoubleArray sensitivity;
	  /// <summary>
	  /// The split of parameters between the underlying parameterized data.
	  /// <para>
	  /// A single {@code UnitParameterSensitivity} represents the sensitivity to a single <seealso cref="ParameterizedData"/> instance.
	  /// However, a {@code ParameterizedData} instance can itself be backed by more than one underlying instance.
	  /// For example, a curve formed from two underlying curves.
	  /// This list is present, it represents how to split this sensitivity between the underlying instances.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional", type = "List<>") private final com.google.common.collect.ImmutableList<ParameterSize> parameterSplit;
	  private readonly ImmutableList<ParameterSize> parameterSplit;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the market data name, metadata and sensitivity.
	  /// <para>
	  /// The market data name identifies the <seealso cref="ParameterizedData"/> instance that was queried.
	  /// The parameter metadata provides information on each parameter.
	  /// The size of the parameter metadata list must match the size of the sensitivity array.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataName">  the name of the market data that the sensitivity refers to </param>
	  /// <param name="parameterMetadata">  the parameter metadata </param>
	  /// <param name="sensitivity">  the sensitivity values, one for each parameter </param>
	  /// <returns> the sensitivity object </returns>
	  public static UnitParameterSensitivity of<T1, T2>(MarketDataName<T1> marketDataName, IList<T2> parameterMetadata, DoubleArray sensitivity) where T2 : ParameterMetadata
	  {

		return new UnitParameterSensitivity(marketDataName, parameterMetadata, sensitivity, null);
	  }

	  /// <summary>
	  /// Obtains an instance from the market data name and sensitivity.
	  /// <para>
	  /// The market data name identifies the <seealso cref="ParameterizedData"/> instance that was queried.
	  /// The parameter metadata will be empty.
	  /// The size of the parameter metadata list must match the size of the sensitivity array.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataName">  the name of the market data that the sensitivity refers to </param>
	  /// <param name="sensitivity">  the sensitivity values, one for each parameter </param>
	  /// <returns> the sensitivity object </returns>
	  public static UnitParameterSensitivity of<T1>(MarketDataName<T1> marketDataName, DoubleArray sensitivity)
	  {
		return of(marketDataName, ParameterMetadata.listOfEmpty(sensitivity.size()), sensitivity);
	  }

	  /// <summary>
	  /// Obtains an instance from the market data name, metadata, sensitivity and parameter split.
	  /// <para>
	  /// The market data name identifies the <seealso cref="ParameterizedData"/> instance that was queried.
	  /// The parameter metadata provides information on each parameter.
	  /// The size of the parameter metadata list must match the size of the sensitivity array.
	  /// </para>
	  /// <para>
	  /// The parameter split allows the sensitivity to represent the split between two or more
	  /// underlying <seealso cref="ParameterizedData"/> instances. The sum of the parameters in the split
	  /// must equal the size of the sensitivity array, and each name must be unique.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataName">  the name of the market data that the sensitivity refers to </param>
	  /// <param name="parameterMetadata">  the parameter metadata </param>
	  /// <param name="sensitivity">  the sensitivity values, one for each parameter </param>
	  /// <param name="parameterSplit">  the split between the underlying {@code ParameterizedData} instances </param>
	  /// <returns> the sensitivity object </returns>
	  public static UnitParameterSensitivity of<T1, T2>(MarketDataName<T1> marketDataName, IList<T2> parameterMetadata, DoubleArray sensitivity, IList<ParameterSize> parameterSplit) where T2 : ParameterMetadata
	  {

		return new UnitParameterSensitivity(marketDataName, parameterMetadata, sensitivity, parameterSplit);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Combines two or more instances to form a single sensitivity instance.
	  /// <para>
	  /// The result will store information about the separate instances allowing it to be <seealso cref="#split()"/> later.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataName">  the combined name of the market data that the sensitivity refers to </param>
	  /// <param name="sensitivities">  the sensitivity instances to combine, two or more </param>
	  /// <returns> the combined sensitivity object </returns>
	  public static UnitParameterSensitivity combine<T1>(MarketDataName<T1> marketDataName, params UnitParameterSensitivity[] sensitivities)
	  {

		ArgChecker.notEmpty(sensitivities, "sensitivities");
		if (sensitivities.Length < 2)
		{
		  throw new System.ArgumentException("At least two sensitivity instances must be specified");
		}
		int size = Stream.of(sensitivities).mapToInt(s => s.ParameterCount).sum();
		double[] combinedSensitivities = new double[size];
		ImmutableList.Builder<ParameterMetadata> combinedMeta = ImmutableList.builder();
		ImmutableList.Builder<ParameterSize> split = ImmutableList.builder();
		int count = 0;
		for (int i = 0; i < sensitivities.Length; i++)
		{
		  UnitParameterSensitivity sens = sensitivities[i];
		  Array.Copy(sens.Sensitivity.toArrayUnsafe(), 0, combinedSensitivities, count, sens.ParameterCount);
		  combinedMeta.addAll(sens.ParameterMetadata);
		  split.add(ParameterSize.of(sens.MarketDataName, sens.ParameterCount));
		  count += sens.ParameterCount;
		}

		return new UnitParameterSensitivity(marketDataName, combinedMeta.build(), DoubleArray.ofUnsafe(combinedSensitivities), split.build());
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (sensitivity.size() != parameterMetadata.size())
		{
		  throw new System.ArgumentException("Length of sensitivity and parameter metadata must match");
		}
		if (parameterSplit != null)
		{
		  long total = parameterSplit.Select(p => p.ParameterCount).Sum();
		  if (sensitivity.size() != total)
		  {
			throw new System.ArgumentException("Length of sensitivity and parameter split must match");
		  }
		  if (parameterSplit.Select(p => p.Name).Distinct().Count() != parameterSplit.size())
		  {
			throw new System.ArgumentException("Parameter split must not contain duplicate market data names");
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of parameters.
	  /// <para>
	  /// This returns the number of parameters in the <seealso cref="ParameterizedData"/> instance
	  /// which is the same size as the sensitivity array.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the number of parameters </returns>
	  public int ParameterCount
	  {
		  get
		  {
			return sensitivity.size();
		  }
	  }

	  /// <summary>
	  /// Gets the parameter metadata at the specified index.
	  /// <para>
	  /// If there is no specific parameter metadata, an empty instance will be returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameterIndex">  the zero-based index of the parameter to get </param>
	  /// <returns> the metadata of the parameter </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return parameterMetadata.get(parameterIndex);
	  }

	  /// <summary>
	  /// Compares the key of two sensitivity objects, excluding the parameter sensitivity values.
	  /// </summary>
	  /// <param name="other">  the other sensitivity object </param>
	  /// <returns> positive if greater, zero if equal, negative if less </returns>
	  public int compareKey(UnitParameterSensitivity other)
	  {
		return ComparisonChain.start().compare(marketDataName, other.marketDataName).result();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance converted this sensitivity to a monetary value, multiplying by the specified factor.
	  /// <para>
	  /// Each value in the sensitivity array will be multiplied by the specified factor.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency of the amount </param>
	  /// <param name="amount">  the amount to multiply by </param>
	  /// <returns> the resulting sensitivity object </returns>
	  public CurrencyParameterSensitivity multipliedBy(Currency currency, double amount)
	  {
		return CurrencyParameterSensitivity.of(marketDataName, parameterMetadata, currency, sensitivity.multipliedBy(amount), parameterSplit);
	  }

	  /// <summary>
	  /// Returns an instance with the sensitivity values multiplied by the specified factor.
	  /// <para>
	  /// Each value in the sensitivity array will be multiplied by the factor.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="factor">  the multiplicative factor </param>
	  /// <returns> an instance based on this one, with each sensitivity multiplied by the factor </returns>
	  public UnitParameterSensitivity multipliedBy(double factor)
	  {
		return mapSensitivity(s => s * factor);
	  }

	  /// <summary>
	  /// Returns an instance with the specified operation applied to the sensitivity values.
	  /// <para>
	  /// Each value in the sensitivity array will be operated on.
	  /// For example, the operator could multiply the sensitivities by a constant, or take the inverse.
	  /// <pre>
	  ///   inverse = base.mapSensitivity(value -> 1 / value);
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="operator">  the operator to be applied to the sensitivities </param>
	  /// <returns> an instance based on this one, with the operator applied to the sensitivity values </returns>
	  public UnitParameterSensitivity mapSensitivity(System.Func<double, double> @operator)
	  {
		return new UnitParameterSensitivity(marketDataName, parameterMetadata, sensitivity.map(@operator), parameterSplit);
	  }

	  /// <summary>
	  /// Returns an instance with new parameter sensitivity values.
	  /// </summary>
	  /// <param name="sensitivity">  the new sensitivity values </param>
	  /// <returns> an instance based on this one, with the specified sensitivity values </returns>
	  public UnitParameterSensitivity withSensitivity(DoubleArray sensitivity)
	  {
		return new UnitParameterSensitivity(marketDataName, parameterMetadata, sensitivity, parameterSplit);
	  }

	  /// <summary>
	  /// Returns an instance with the specified sensitivity array added to the array in this instance.
	  /// <para>
	  /// The specified array must match the size of the array in this instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="otherSensitivty">  the other parameter sensitivity </param>
	  /// <returns> an instance based on this one, with the other instance added </returns>
	  /// <exception cref="IllegalArgumentException"> if the market data name, metadata or parameter split differs </exception>
	  public UnitParameterSensitivity plus(DoubleArray otherSensitivty)
	  {
		if (otherSensitivty.size() != sensitivity.size())
		{
		  throw new System.ArgumentException(Messages.format("Sensitivity array size {} must match size {}", otherSensitivty.size(), sensitivity.size()));
		}
		return withSensitivity(sensitivity.plus(otherSensitivty));
	  }

	  /// <summary>
	  /// Returns an instance with the specified sensitivity array added to the array in this instance.
	  /// <para>
	  /// The specified instance must have the same name, metadata and parameter split as this instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="otherSensitivty">  the other parameter sensitivity </param>
	  /// <returns> an instance based on this one, with the other instance added </returns>
	  /// <exception cref="IllegalArgumentException"> if the market data name, metadata or parameter split differs </exception>
	  public UnitParameterSensitivity plus(UnitParameterSensitivity otherSensitivty)
	  {
		if (!marketDataName.Equals(otherSensitivty.marketDataName) || !parameterMetadata.Equals(otherSensitivty.parameterMetadata) || (parameterSplit != null && !parameterSplit.Equals(otherSensitivty.parameterSplit)))
		{
		  throw new System.ArgumentException("Two sensitivity instances can only be added if name, metadata and split are equal");
		}
		return plus(otherSensitivty.Sensitivity);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Splits this sensitivity instance.
	  /// <para>
	  /// A single sensitivity instance may be based on more than one underlying <seealso cref="ParameterizedData"/>,
	  /// as represented by <seealso cref="#getParameterSplit()"/>. Calling this method returns a list
	  /// where the sensitivity of this instance has been split into multiple instances as per
	  /// the parameter split definition. In the common case where there is a single underlying
	  /// {@code ParameterizedData}, the list will be of size one containing this instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> this sensitivity split as per the defined parameter split, ordered as per this instance </returns>
	  public ImmutableList<UnitParameterSensitivity> split()
	  {
		if (parameterSplit == null)
		{
		  return ImmutableList.of(this);
		}
		ImmutableList.Builder<UnitParameterSensitivity> builder = ImmutableList.builder();
		int count = 0;
		foreach (ParameterSize size in parameterSplit)
		{
		  IList<ParameterMetadata> splitMetadata = parameterMetadata.subList(count, count + size.ParameterCount);
		  DoubleArray splitSensitivity = sensitivity.subArray(count, count + size.ParameterCount);
		  builder.add(UnitParameterSensitivity.of(size.Name, splitMetadata, splitSensitivity));
		  count += size.ParameterCount;
		}
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the total of the sensitivity values.
	  /// </summary>
	  /// <returns> the total sensitivity values </returns>
	  public double total()
	  {
		return sensitivity.sum();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code UnitParameterSensitivity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static UnitParameterSensitivity.Meta meta()
	  {
		return UnitParameterSensitivity.Meta.INSTANCE;
	  }

	  static UnitParameterSensitivity()
	  {
		MetaBean.register(UnitParameterSensitivity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private UnitParameterSensitivity<T1, T2>(MarketDataName<T1> marketDataName, IList<T2> parameterMetadata, DoubleArray sensitivity, IList<ParameterSize> parameterSplit) where T2 : ParameterMetadata
	  {
		JodaBeanUtils.notNull(marketDataName, "marketDataName");
		JodaBeanUtils.notNull(parameterMetadata, "parameterMetadata");
		JodaBeanUtils.notNull(sensitivity, "sensitivity");
		this.marketDataName = marketDataName;
		this.parameterMetadata = ImmutableList.copyOf(parameterMetadata);
		this.sensitivity = sensitivity;
		this.parameterSplit = (parameterSplit != null ? ImmutableList.copyOf(parameterSplit) : null);
		validate();
	  }

	  public override UnitParameterSensitivity.Meta metaBean()
	  {
		return UnitParameterSensitivity.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market data name.
	  /// <para>
	  /// This name is used in the market data system to identify the data that the sensitivities refer to.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.opengamma.strata.data.MarketDataName<?> getMarketDataName()
	  public MarketDataName<object> MarketDataName
	  {
		  get
		  {
			return marketDataName;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the list of parameter metadata.
	  /// <para>
	  /// There is one entry for each parameter.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<ParameterMetadata> ParameterMetadata
	  {
		  get
		  {
			return parameterMetadata;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the parameter sensitivity values.
	  /// <para>
	  /// There is one sensitivity value for each parameter.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleArray Sensitivity
	  {
		  get
		  {
			return sensitivity;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the split of parameters between the underlying parameterized data.
	  /// <para>
	  /// A single {@code UnitParameterSensitivity} represents the sensitivity to a single <seealso cref="ParameterizedData"/> instance.
	  /// However, a {@code ParameterizedData} instance can itself be backed by more than one underlying instance.
	  /// For example, a curve formed from two underlying curves.
	  /// This list is present, it represents how to split this sensitivity between the underlying instances.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<IList<ParameterSize>> ParameterSplit
	  {
		  get
		  {
			return Optional.ofNullable(parameterSplit);
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
		  UnitParameterSensitivity other = (UnitParameterSensitivity) obj;
		  return JodaBeanUtils.equal(marketDataName, other.marketDataName) && JodaBeanUtils.equal(parameterMetadata, other.parameterMetadata) && JodaBeanUtils.equal(sensitivity, other.sensitivity) && JodaBeanUtils.equal(parameterSplit, other.parameterSplit);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(marketDataName);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(parameterMetadata);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sensitivity);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(parameterSplit);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("UnitParameterSensitivity{");
		buf.Append("marketDataName").Append('=').Append(marketDataName).Append(',').Append(' ');
		buf.Append("parameterMetadata").Append('=').Append(parameterMetadata).Append(',').Append(' ');
		buf.Append("sensitivity").Append('=').Append(sensitivity).Append(',').Append(' ');
		buf.Append("parameterSplit").Append('=').Append(JodaBeanUtils.ToString(parameterSplit));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code UnitParameterSensitivity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  marketDataName_Renamed = DirectMetaProperty.ofImmutable(this, "marketDataName", typeof(UnitParameterSensitivity), (Type) typeof(MarketDataName));
			  parameterMetadata_Renamed = DirectMetaProperty.ofImmutable(this, "parameterMetadata", typeof(UnitParameterSensitivity), (Type) typeof(ImmutableList));
			  sensitivity_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivity", typeof(UnitParameterSensitivity), typeof(DoubleArray));
			  parameterSplit_Renamed = DirectMetaProperty.ofImmutable(this, "parameterSplit", typeof(UnitParameterSensitivity), (Type) typeof(System.Collections.IList));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "marketDataName", "parameterMetadata", "sensitivity", "parameterSplit");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code marketDataName} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.opengamma.strata.data.MarketDataName<?>> marketDataName = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "marketDataName", UnitParameterSensitivity.class, (Class) com.opengamma.strata.data.MarketDataName.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<MarketDataName<object>> marketDataName_Renamed;
		/// <summary>
		/// The meta-property for the {@code parameterMetadata} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<ParameterMetadata>> parameterMetadata = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "parameterMetadata", UnitParameterSensitivity.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<ParameterMetadata>> parameterMetadata_Renamed;
		/// <summary>
		/// The meta-property for the {@code sensitivity} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleArray> sensitivity_Renamed;
		/// <summary>
		/// The meta-property for the {@code parameterSplit} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<java.util.List<ParameterSize>> parameterSplit = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "parameterSplit", UnitParameterSensitivity.class, (Class) java.util.List.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IList<ParameterSize>> parameterSplit_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "marketDataName", "parameterMetadata", "sensitivity", "parameterSplit");
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
			case 842855857: // marketDataName
			  return marketDataName_Renamed;
			case -1169106440: // parameterMetadata
			  return parameterMetadata_Renamed;
			case 564403871: // sensitivity
			  return sensitivity_Renamed;
			case 1122130161: // parameterSplit
			  return parameterSplit_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends UnitParameterSensitivity> builder()
		public override BeanBuilder<UnitParameterSensitivity> builder()
		{
		  return new UnitParameterSensitivity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(UnitParameterSensitivity);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code marketDataName} property. </summary>
		/// <returns> the meta-property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public org.joda.beans.MetaProperty<com.opengamma.strata.data.MarketDataName<?>> marketDataName()
		public MetaProperty<MarketDataName<object>> marketDataName()
		{
		  return marketDataName_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code parameterMetadata} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<ParameterMetadata>> parameterMetadata()
		{
		  return parameterMetadata_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code sensitivity} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleArray> sensitivity()
		{
		  return sensitivity_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code parameterSplit} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IList<ParameterSize>> parameterSplit()
		{
		  return parameterSplit_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 842855857: // marketDataName
			  return ((UnitParameterSensitivity) bean).MarketDataName;
			case -1169106440: // parameterMetadata
			  return ((UnitParameterSensitivity) bean).ParameterMetadata;
			case 564403871: // sensitivity
			  return ((UnitParameterSensitivity) bean).Sensitivity;
			case 1122130161: // parameterSplit
			  return ((UnitParameterSensitivity) bean).parameterSplit;
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
	  /// The bean-builder for {@code UnitParameterSensitivity}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<UnitParameterSensitivity>
	  {

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.data.MarketDataName<?> marketDataName;
		internal MarketDataName<object> marketDataName;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.List<? extends ParameterMetadata> parameterMetadata = com.google.common.collect.ImmutableList.of();
		internal IList<ParameterMetadata> parameterMetadata = ImmutableList.of();
		internal DoubleArray sensitivity;
		internal IList<ParameterSize> parameterSplit;

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
			case 842855857: // marketDataName
			  return marketDataName;
			case -1169106440: // parameterMetadata
			  return parameterMetadata;
			case 564403871: // sensitivity
			  return sensitivity;
			case 1122130161: // parameterSplit
			  return parameterSplit;
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
			case 842855857: // marketDataName
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.marketDataName = (com.opengamma.strata.data.MarketDataName<?>) newValue;
			  this.marketDataName = (MarketDataName<object>) newValue;
			  break;
			case -1169106440: // parameterMetadata
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.parameterMetadata = (java.util.List<? extends ParameterMetadata>) newValue;
			  this.parameterMetadata = (IList<ParameterMetadata>) newValue;
			  break;
			case 564403871: // sensitivity
			  this.sensitivity = (DoubleArray) newValue;
			  break;
			case 1122130161: // parameterSplit
			  this.parameterSplit = (IList<ParameterSize>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override UnitParameterSensitivity build()
		{
		  return new UnitParameterSensitivity(marketDataName, parameterMetadata, sensitivity, parameterSplit);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("UnitParameterSensitivity.Builder{");
		  buf.Append("marketDataName").Append('=').Append(JodaBeanUtils.ToString(marketDataName)).Append(',').Append(' ');
		  buf.Append("parameterMetadata").Append('=').Append(JodaBeanUtils.ToString(parameterMetadata)).Append(',').Append(' ');
		  buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity)).Append(',').Append(' ');
		  buf.Append("parameterSplit").Append('=').Append(JodaBeanUtils.ToString(parameterSplit));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}