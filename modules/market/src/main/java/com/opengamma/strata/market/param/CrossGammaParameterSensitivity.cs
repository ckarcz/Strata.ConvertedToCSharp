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
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxConvertible = com.opengamma.strata.basics.currency.FxConvertible;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using Curve = com.opengamma.strata.market.curve.Curve;

	/// <summary>
	/// The second order parameter sensitivity for parameterized market data.
	/// <para>
	/// Parameter sensitivity is the sensitivity of a value to the parameters of a
	/// <seealso cref="ParameterizedData parameterized market data"/> object that is used to determine the value.
	/// The main application of this class is the parameter sensitivities for curves.
	/// Thus {@code ParameterizedData} is typically <seealso cref="Curve"/>.
	/// </para>
	/// <para>
	/// The sensitivity is expressed as a matrix.
	/// The {@code (i,j)} component is the sensitivity of the {@code i}-th component of the {@code parameterMetadata} delta to
	/// the {@code j}-th parameter in {@code order}.
	/// </para>
	/// <para>
	/// The sensitivity represents a monetary value in the specified currency.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class CrossGammaParameterSensitivity implements com.opengamma.strata.basics.currency.FxConvertible<CrossGammaParameterSensitivity>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CrossGammaParameterSensitivity : FxConvertible<CrossGammaParameterSensitivity>, ImmutableBean
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
	  /// The sensitivity order.
	  /// <para>
	  /// This defines the order of sensitivity values, which can be used as a key to interpret {@code sensitivity}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "List<Pair<MarketDataName<?>, List<? extends ParameterMetadata>>>") private final com.google.common.collect.ImmutableList<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.data.MarketDataName<?>, java.util.List<? extends ParameterMetadata>>> order;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly ImmutableList<Pair<MarketDataName<object>, IList<ParameterMetadata>>> order;

	  /// <summary>
	  /// The currency of the sensitivity.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
	  /// <summary>
	  /// The parameter sensitivity values.
	  /// <para>
	  /// The curve delta sensitivities to parameterized market data.
	  /// This is a {@code n x m} matrix, where {@code n} must agree with the size of {@code parameterMetadata} and 
	  /// {@code m} must be the sum of parameter count in {@code order}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleMatrix sensitivity;
	  private readonly DoubleMatrix sensitivity;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the market data name, metadata, currency and sensitivity.
	  /// <para>
	  /// This creates a sensitivity instance which stores the second order sensitivity values to a single market data, i.e., 
	  /// the block diagonal part of the full second order sensitivity matrix.
	  /// </para>
	  /// <para>
	  /// The market data name identifies the <seealso cref="ParameterizedData"/> instance that was queried.
	  /// The parameter metadata provides information on each parameter.
	  /// The size of the parameter metadata list must match the size of the sensitivity array.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataName">  the name of the market data that the sensitivity refers to </param>
	  /// <param name="parameterMetadata">  the parameter metadata </param>
	  /// <param name="currency">  the currency of the sensitivity </param>
	  /// <param name="sensitivity">  the sensitivity values, one for each parameter </param>
	  /// <returns> the sensitivity object </returns>
	  public static CrossGammaParameterSensitivity of<T1, T2>(MarketDataName<T1> marketDataName, IList<T2> parameterMetadata, Currency currency, DoubleMatrix sensitivity) where T2 : ParameterMetadata
	  {

		return of(marketDataName, parameterMetadata, marketDataName, parameterMetadata, currency, sensitivity);
	  }

	  /// <summary>
	  /// Obtains an instance from the market data names, metadatas, currency and sensitivity.
	  /// <para>
	  /// This creates a sensitivity instance which stores the second order sensitivity values: the delta of a market data 
	  /// to another market data. The first market data and the second market data can be the same.
	  /// </para>
	  /// <para>
	  /// The market data name identifies the <seealso cref="ParameterizedData"/> instance that was queried.
	  /// The parameter metadata provides information on each parameter.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataName">  the name of the first market data that the sensitivity refers to </param>
	  /// <param name="parameterMetadata">  the first parameter metadata </param>
	  /// <param name="marketDataNameOther">  the name of the second market data that the sensitivity refers to </param>
	  /// <param name="parameterMetadataOther">  the second parameter metadata </param>
	  /// <param name="currency">  the currency of the sensitivity </param>
	  /// <param name="sensitivity">  the sensitivity values, one for each parameter </param>
	  /// <returns> the sensitivity object </returns>
	  public static CrossGammaParameterSensitivity of<T1, T2, T3, T4>(MarketDataName<T1> marketDataName, IList<T2> parameterMetadata, MarketDataName<T3> marketDataNameOther, IList<T4> parameterMetadataOther, Currency currency, DoubleMatrix sensitivity) where T2 : ParameterMetadata where T4 : ParameterMetadata
	  {

		return new CrossGammaParameterSensitivity(marketDataName, parameterMetadata, ImmutableList.of(Pair.of(marketDataNameOther, parameterMetadataOther)), currency, sensitivity);
	  }

	  /// <summary>
	  /// Obtains an instance from the market data names, metadatas, currency and sensitivity.
	  /// <para>
	  /// This creates a sensitivity instance which stores the second order sensitivity values: the delta of a market data 
	  /// to a set of other market data. 
	  /// The market data set is represented in terms of {@code List<Pair<MarketDataName<?>, List<? extends ParameterMetadata>>>}. 
	  /// which defines the order of the sensitivity values.
	  /// </para>
	  /// <para>
	  /// The market data name identifies the <seealso cref="ParameterizedData"/> instance that was queried.
	  /// The parameter metadata provides information on each parameter.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataName">  the name of the market data that the sensitivity refers to </param>
	  /// <param name="parameterMetadata">  the parameter metadata </param>
	  /// <param name="order">  the order </param>
	  /// <param name="currency">  the currency of the sensitivity </param>
	  /// <param name="sensitivity">  the sensitivity values, one for each parameter </param>
	  /// <returns> the sensitivity object </returns>
	  public static CrossGammaParameterSensitivity of<T1, T2, T3>(MarketDataName<T1> marketDataName, IList<T2> parameterMetadata, IList<T3> order, Currency currency, DoubleMatrix sensitivity) where T2 : ParameterMetadata where T3 : ParameterMetadata
	  {

		return new CrossGammaParameterSensitivity(marketDataName, parameterMetadata, order, currency, sensitivity);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		int col = sensitivity.columnCount();
		int row = sensitivity.rowCount();
		if (row != parameterMetadata.size())
		{
		  throw new System.ArgumentException("row count of sensitivity and parameter metadata size must match");
		}
		int nParamsTotal = 0;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.data.MarketDataName<?>, java.util.List<? extends ParameterMetadata>> entry : order)
		foreach (Pair<MarketDataName<object>, IList<ParameterMetadata>> entry in order)
		{
		  nParamsTotal += entry.Second.Count;
		}
		if (col != nParamsTotal)
		{
		  throw new System.ArgumentException("column count of sensitivity and total parameter metadata size of order must match");
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
			return sensitivity.rowCount();
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
	  public int compareKey(CrossGammaParameterSensitivity other)
	  {
		return ComparisonChain.start().compare(marketDataName, other.marketDataName).compare(order.ToString(), other.order.ToString()).compare(currency, other.currency).result();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this sensitivity to an equivalent in the specified currency.
	  /// <para>
	  /// Any FX conversion that is required will use rates from the provider.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resultCurrency">  the currency of the result </param>
	  /// <param name="rateProvider">  the provider of FX rates </param>
	  /// <returns> the sensitivity object expressed in terms of the result currency </returns>
	  /// <exception cref="RuntimeException"> if no FX rate could be found </exception>
	  public CrossGammaParameterSensitivity convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		if (currency.Equals(resultCurrency))
		{
		  return this;
		}
		double fxRate = rateProvider.fxRate(currency, resultCurrency);
		return mapSensitivity(s => s * fxRate, resultCurrency);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with the sensitivity values multiplied by the specified factor.
	  /// <para>
	  /// Each value in the sensitivity array will be multiplied by the factor.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="factor">  the multiplicative factor </param>
	  /// <returns> an instance based on this one, with each sensitivity multiplied by the factor </returns>
	  public CrossGammaParameterSensitivity multipliedBy(double factor)
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
	  public CrossGammaParameterSensitivity mapSensitivity(System.Func<double, double> @operator)
	  {
		return mapSensitivity(@operator, currency);
	  }

	  // maps the sensitivities and potentially changes the currency
	  private CrossGammaParameterSensitivity mapSensitivity(System.Func<double, double> @operator, Currency currency)
	  {
		return new CrossGammaParameterSensitivity(marketDataName, parameterMetadata, order, currency, sensitivity.map(@operator));
	  }

	  /// <summary>
	  /// Returns an instance with new parameter sensitivity values.
	  /// </summary>
	  /// <param name="sensitivity">  the new sensitivity values </param>
	  /// <returns> an instance based on this one, with the specified sensitivity values </returns>
	  public CrossGammaParameterSensitivity withSensitivity(DoubleMatrix sensitivity)
	  {
		return new CrossGammaParameterSensitivity(marketDataName, parameterMetadata, order, currency, sensitivity);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the total of the sensitivity values.
	  /// </summary>
	  /// <returns> the total sensitivity values </returns>
	  public CurrencyAmount total()
	  {
		return CurrencyAmount.of(currency, sensitivity.total());
	  }

	  /// <summary>
	  /// Returns the diagonal part of the sensitivity as {@code CurrencyParameterSensitivity}.
	  /// </summary>
	  /// <returns> the diagonal part </returns>
	  public CurrencyParameterSensitivity diagonal()
	  {
		CrossGammaParameterSensitivity blockDiagonal = getSensitivity(MarketDataName);
		int size = ParameterCount;
		return CurrencyParameterSensitivity.of(MarketDataName, ParameterMetadata, Currency, DoubleArray.of(size, i => blockDiagonal.Sensitivity.get(i, i)));
	  }

	  /// <summary>
	  /// Returns the sensitivity to the market data specified by {@code name}.
	  /// <para>
	  /// This returns a sensitivity instance which stores the sensitivity of the {@code marketDataName} delta to another 
	  /// market data of {@code name}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name </param>
	  /// <returns> the sensitivity </returns>
	  /// <exception cref="IllegalArgumentException"> if the name does not match an entry </exception>
	  public CrossGammaParameterSensitivity getSensitivity<T1>(MarketDataName<T1> name)
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.tuple.Pair<int, java.util.List<? extends ParameterMetadata>> indexAndMetadata = findStartIndexAndMetadata(name);
		Pair<int, IList<ParameterMetadata>> indexAndMetadata = findStartIndexAndMetadata(name);
		int startIndex = indexAndMetadata.First;
		int rowCt = ParameterCount;
		int colCt = indexAndMetadata.Second.Count;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] sensi = new double[rowCt][colCt];
		double[][] sensi = RectangularArrays.ReturnRectangularDoubleArray(rowCt, colCt);
		for (int i = 0; i < rowCt; ++i)
		{
		  Array.Copy(Sensitivity.rowArray(i), startIndex, sensi[i], 0, colCt);
		}
		return CrossGammaParameterSensitivity.of(MarketDataName, ParameterMetadata, name, indexAndMetadata.Second, Currency, DoubleMatrix.ofUnsafe(sensi));
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.tuple.Pair<int, java.util.List<? extends ParameterMetadata>> findStartIndexAndMetadata(com.opengamma.strata.data.MarketDataName<?> name)
	  private Pair<int, IList<ParameterMetadata>> findStartIndexAndMetadata<T1>(MarketDataName<T1> name)
	  {
		int startIndex = 0;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.data.MarketDataName<?>, java.util.List<? extends ParameterMetadata>> entry : order)
		foreach (Pair<MarketDataName<object>, IList<ParameterMetadata>> entry in order)
		{
		  if (entry.First.Equals(name))
		  {
			return Pair.of(startIndex, entry.Second);
		  }
		  startIndex += entry.Second.Count;
		}
		throw new System.ArgumentException(Messages.format("Unable to find sensitivity: {} and {}", marketDataName, name));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CrossGammaParameterSensitivity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CrossGammaParameterSensitivity.Meta meta()
	  {
		return CrossGammaParameterSensitivity.Meta.INSTANCE;
	  }

	  static CrossGammaParameterSensitivity()
	  {
		MetaBean.register(CrossGammaParameterSensitivity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private CrossGammaParameterSensitivity<T1, T2, T3>(MarketDataName<T1> marketDataName, IList<T2> parameterMetadata, IList<T3> order, Currency currency, DoubleMatrix sensitivity) where T2 : ParameterMetadata where T3 : ParameterMetadata
	  {
		JodaBeanUtils.notNull(marketDataName, "marketDataName");
		JodaBeanUtils.notNull(parameterMetadata, "parameterMetadata");
		JodaBeanUtils.notNull(order, "order");
		JodaBeanUtils.notNull(currency, "currency");
		JodaBeanUtils.notNull(sensitivity, "sensitivity");
		this.marketDataName = marketDataName;
		this.parameterMetadata = ImmutableList.copyOf(parameterMetadata);
		this.order = ImmutableList.copyOf(order);
		this.currency = currency;
		this.sensitivity = sensitivity;
		validate();
	  }

	  public override CrossGammaParameterSensitivity.Meta metaBean()
	  {
		return CrossGammaParameterSensitivity.Meta.INSTANCE;
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
	  /// Gets the sensitivity order.
	  /// <para>
	  /// This defines the order of sensitivity values, which can be used as a key to interpret {@code sensitivity}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableList<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.data.MarketDataName<?>, java.util.List<? extends ParameterMetadata>>> getOrder()
	  public ImmutableList<Pair<MarketDataName<object>, IList<ParameterMetadata>>> Order
	  {
		  get
		  {
			return order;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the sensitivity. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Currency Currency
	  {
		  get
		  {
			return currency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the parameter sensitivity values.
	  /// <para>
	  /// The curve delta sensitivities to parameterized market data.
	  /// This is a {@code n x m} matrix, where {@code n} must agree with the size of {@code parameterMetadata} and
	  /// {@code m} must be the sum of parameter count in {@code order}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleMatrix Sensitivity
	  {
		  get
		  {
			return sensitivity;
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
		  CrossGammaParameterSensitivity other = (CrossGammaParameterSensitivity) obj;
		  return JodaBeanUtils.equal(marketDataName, other.marketDataName) && JodaBeanUtils.equal(parameterMetadata, other.parameterMetadata) && JodaBeanUtils.equal(order, other.order) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(sensitivity, other.sensitivity);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(marketDataName);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(parameterMetadata);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(order);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sensitivity);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("CrossGammaParameterSensitivity{");
		buf.Append("marketDataName").Append('=').Append(marketDataName).Append(',').Append(' ');
		buf.Append("parameterMetadata").Append('=').Append(parameterMetadata).Append(',').Append(' ');
		buf.Append("order").Append('=').Append(order).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CrossGammaParameterSensitivity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  marketDataName_Renamed = DirectMetaProperty.ofImmutable(this, "marketDataName", typeof(CrossGammaParameterSensitivity), (Type) typeof(MarketDataName));
			  parameterMetadata_Renamed = DirectMetaProperty.ofImmutable(this, "parameterMetadata", typeof(CrossGammaParameterSensitivity), (Type) typeof(ImmutableList));
			  order_Renamed = DirectMetaProperty.ofImmutable(this, "order", typeof(CrossGammaParameterSensitivity), (Type) typeof(ImmutableList));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(CrossGammaParameterSensitivity), typeof(Currency));
			  sensitivity_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivity", typeof(CrossGammaParameterSensitivity), typeof(DoubleMatrix));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "marketDataName", "parameterMetadata", "order", "currency", "sensitivity");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code marketDataName} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.opengamma.strata.data.MarketDataName<?>> marketDataName = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "marketDataName", CrossGammaParameterSensitivity.class, (Class) com.opengamma.strata.data.MarketDataName.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<MarketDataName<object>> marketDataName_Renamed;
		/// <summary>
		/// The meta-property for the {@code parameterMetadata} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<ParameterMetadata>> parameterMetadata = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "parameterMetadata", CrossGammaParameterSensitivity.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<ParameterMetadata>> parameterMetadata_Renamed;
		/// <summary>
		/// The meta-property for the {@code order} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.data.MarketDataName<?>, java.util.List<? extends ParameterMetadata>>>> order = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "order", CrossGammaParameterSensitivity.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<ImmutableList<Pair<MarketDataName<object>, IList<ParameterMetadata>>>> order_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code sensitivity} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleMatrix> sensitivity_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "marketDataName", "parameterMetadata", "order", "currency", "sensitivity");
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
			case 106006350: // order
			  return order_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 564403871: // sensitivity
			  return sensitivity_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends CrossGammaParameterSensitivity> builder()
		public override BeanBuilder<CrossGammaParameterSensitivity> builder()
		{
		  return new CrossGammaParameterSensitivity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CrossGammaParameterSensitivity);
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
		/// The meta-property for the {@code order} property. </summary>
		/// <returns> the meta-property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.data.MarketDataName<?>, java.util.List<? extends ParameterMetadata>>>> order()
		public MetaProperty<ImmutableList<Pair<MarketDataName<object>, IList<ParameterMetadata>>>> order()
		{
		  return order_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code sensitivity} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleMatrix> sensitivity()
		{
		  return sensitivity_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 842855857: // marketDataName
			  return ((CrossGammaParameterSensitivity) bean).MarketDataName;
			case -1169106440: // parameterMetadata
			  return ((CrossGammaParameterSensitivity) bean).ParameterMetadata;
			case 106006350: // order
			  return ((CrossGammaParameterSensitivity) bean).Order;
			case 575402001: // currency
			  return ((CrossGammaParameterSensitivity) bean).Currency;
			case 564403871: // sensitivity
			  return ((CrossGammaParameterSensitivity) bean).Sensitivity;
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
	  /// The bean-builder for {@code CrossGammaParameterSensitivity}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<CrossGammaParameterSensitivity>
	  {

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.data.MarketDataName<?> marketDataName;
		internal MarketDataName<object> marketDataName;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.List<? extends ParameterMetadata> parameterMetadata = com.google.common.collect.ImmutableList.of();
		internal IList<ParameterMetadata> parameterMetadata = ImmutableList.of();
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.List<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.data.MarketDataName<?>, java.util.List<? extends ParameterMetadata>>> order = com.google.common.collect.ImmutableList.of();
		internal IList<Pair<MarketDataName<object>, IList<ParameterMetadata>>> order = ImmutableList.of();
		internal Currency currency;
		internal DoubleMatrix sensitivity;

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
			case 106006350: // order
			  return order;
			case 575402001: // currency
			  return currency;
			case 564403871: // sensitivity
			  return sensitivity;
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
			case 106006350: // order
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.order = (java.util.List<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.data.MarketDataName<?>, java.util.List<? extends ParameterMetadata>>>) newValue;
			  this.order = (IList<Pair<MarketDataName<object>, IList<ParameterMetadata>>>) newValue;
			  break;
			case 575402001: // currency
			  this.currency = (Currency) newValue;
			  break;
			case 564403871: // sensitivity
			  this.sensitivity = (DoubleMatrix) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override CrossGammaParameterSensitivity build()
		{
		  return new CrossGammaParameterSensitivity(marketDataName, parameterMetadata, order, currency, sensitivity);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("CrossGammaParameterSensitivity.Builder{");
		  buf.Append("marketDataName").Append('=').Append(JodaBeanUtils.ToString(marketDataName)).Append(',').Append(' ');
		  buf.Append("parameterMetadata").Append('=').Append(JodaBeanUtils.ToString(parameterMetadata)).Append(',').Append(' ');
		  buf.Append("order").Append('=').Append(JodaBeanUtils.ToString(order)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency)).Append(',').Append(' ');
		  buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}