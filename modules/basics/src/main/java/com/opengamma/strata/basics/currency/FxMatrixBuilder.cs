using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// A mutable builder class for <seealso cref="FxMatrix"/>.
	/// </summary>
	public class FxMatrixBuilder
	{

	  /// <summary>
	  /// The minimum size of the FX rate matrix. This is intended such
	  /// that a number rates of can be added without needing to resize.
	  /// </summary>
	  private const int MINIMAL_MATRIX_SIZE = 8;

	  /// <summary>
	  /// The currencies held by the builder pointing to their position
	  /// in the rates array. An ordered map is used so that it retains
	  /// order which means the {@code toString} method of {@code FxMatrix}
	  /// is clearer.
	  /// </summary>
	  private readonly LinkedHashMap<Currency, int> currencies;
	  /// <summary>
	  /// A 2 dimensional array holding the rates. Each row of the array holds the
	  /// value of 1 unit of Currency (that the row represents) in each of the
	  /// alternate currencies.
	  /// 
	  /// The array is square with its order being a power of 2. This means that there
	  /// may be empty rows/cols at the bottom/right of the matrix. Leaving this space
	  /// means that adding currencies can be done more efficiently as the array only
	  /// needs to be resized (via copying) relatively infrequently..
	  /// </summary>
	  private double[][] rates;
	  /// <summary>
	  /// Rates that have been requested to be added, but which do not
	  /// have a currency in common with the currencies already present.
	  /// As additional currencies are added, this map will be checked to
	  /// see if rates can be handled.
	  /// <para>
	  /// If this map is not empty by the point that build is called,
	  /// an <seealso cref="IllegalStateException"/> will be thrown.
	  /// </para>
	  /// </summary>
	  private readonly IDictionary<CurrencyPair, double> disjointRates = new Dictionary<CurrencyPair, double>();

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Build a new {@code FxMatrix} from the data in the builder.
	  /// </summary>
	  /// <returns> a new {@code FxMatrix} </returns>
	  /// <exception cref="IllegalStateException"> if an attempt was made to add currencies
	  ///   which have no currency in common with other rates </exception>
	  public virtual FxMatrix build()
	  {
		if (disjointRates.Count > 0)
		{
		  throw new System.InvalidOperationException("Received rates with no currencies in common with other: " + disjointRates);
		}
		// Trim array down to the correct size - we have to copy the array
		// anyway to ensure immutability, so we may as well remove any
		// unused rows
		return new FxMatrix(ImmutableMap.copyOf(currencies), DoubleMatrix.ofUnsafe(copyArray(rates, currencies.size())));
	  }

	  /// <summary>
	  /// Adds a new rate for a currency pair to the builder. See
	  /// <seealso cref="#addRate(Currency, Currency, double)"/> for full
	  /// explanation.
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair to be added </param>
	  /// <param name="rate">  the FX rate between the base currency of the pair and the
	  ///   counter currency. The rate indicates the value of one unit of the base
	  ///   currency in terms of the counter currency. </param>
	  /// <returns> the builder updated with the new rate </returns>
	  public virtual FxMatrixBuilder addRate(CurrencyPair currencyPair, double rate)
	  {
		ArgChecker.notNull(currencyPair, "currencyPair");
		return addRate(currencyPair.Base, currencyPair.Counter, rate);
	  }

	  /// <summary>
	  /// Add a new pair of currencies to the builder.
	  /// <para>
	  /// An invocation of the method with {@code builder.addRate(GBP, USD, 1.6)}
	  /// indicates that 1 pound sterling is worth 1.6 US dollars. It is
	  /// equivalent to: {@code builder.addRate(USD, GBP, 1 / 1.6)} (1 US dollar
	  /// is worth 0.625 pounds sterling) for all cases except where the USD/GBP
	  /// rates is already in the matrix and so will be updated.
	  /// </para>
	  /// There are a number of possible outcomes when this  method is called:
	  /// <ul>
	  ///   <li>
	  ///     The builder is currently empty. In this case these currencies
	  ///     and rates will be added as the initial pair.</li>
	  ///   <li>
	  ///     The builder is non-empty and neither of the currencies are
	  ///     currently in the matrix. In this case the currencies cannot be
	  ///     immediately added as there is no common currency to allow the
	  ///     cross rates to be calculated. The currencies and rates are put
	  ///     into a pending set for later processing, for example after another
	  ///     pair containing one of the currencies and a currency already in
	  ///     the matrix is added. If no such event occurs, then an exception
	  ///     will be thrown when <seealso cref="#build()"/> is called.</li>
	  ///   <li>
	  ///     The builder is non-empty and one of the currencies in the pair
	  ///     is already in the matrix, whilst the other is not. In this case
	  ///     the pair and rate is added to the matrix and all the cross rates
	  ///     to the other currencies are calculated.
	  ///   </li>
	  ///   <li>
	  ///     The builder is non-empty and contains both of the currencies in
	  ///     the pair. In this case the pair is treated as an update to the
	  ///     rate already in the matrix. The first currency (ccy1) is treated
	  ///     as the reference currency and the second currency (ccy2) is the
	  ///     updated currency. All rates involving the updated currency will
	  ///     be recalculated using the new rate.
	  ///     <para>
	  ///     Note that due to one of the rates being treated as a reference, this
	  ///     operation is not symmetric. That is, the result of
	  ///     {@code matrix.addRate(USD, EUR, 1.23)} will be different to the
	  ///     result of {@code matrix.addRate(EUR, USD, 1 / 1.23)} when there
	  ///     are other currencies present in the builder.
	  ///   </li>
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="ccy1">  the first currency of the pair </param>
	  /// <param name="ccy2">  the second currency of the pair </param>
	  /// <param name="rate">  the FX rate between the first currency and the second currency.
	  ///   The rate indicates the value of one unit of the first currency in terms
	  ///   of the second currency. </param>
	  /// <returns> the builder updated with the new rate </returns>
	  public virtual FxMatrixBuilder addRate(Currency ccy1, Currency ccy2, double rate)
	  {
		ArgChecker.notNull(ccy1, "ccy1");
		ArgChecker.notNull(ccy2, "ccy2");

		if (currencies.Empty)
		{
		  addInitialCurrencyPair(ccy1, ccy2, rate);
		}
		else
		{
		  addCurrencyPair(ccy1, ccy2, rate);
		}
		return this;
	  }

	  /// <summary>
	  /// Adds a collection of new rates for currency pairs to the builder.
	  /// Pairs that are already in the builder are treated as updates to the
	  /// existing rates -> !e.getKey().equals(commonCurrency) && !currencies.containsKey(e.getKey())
	  /// </summary>
	  /// <param name="rates">  the currency pairs and rates to be added </param>
	  /// <returns> the builder updated with the new rates </returns>
	  public virtual FxMatrixBuilder addRates(IDictionary<CurrencyPair, double> rates)
	  {
		ArgChecker.notNull(rates, "rates");

		if (rates.Count > 0)
		{
		  ensureCapacity(rates.Keys.stream().flatMap(cp => Stream.of(cp.Base, cp.Counter)));

		  MapStream.of(rates).forEach((pair, rate) => addRate(pair, rate));
		}
		return this;
	  }

	  internal FxMatrixBuilder()
	  {
		this.currencies = new LinkedHashMap<Currency, int>();
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: this.rates = new double[MINIMAL_MATRIX_SIZE][MINIMAL_MATRIX_SIZE];
		this.rates = RectangularArrays.ReturnRectangularDoubleArray(MINIMAL_MATRIX_SIZE, MINIMAL_MATRIX_SIZE);
	  }

	  internal FxMatrixBuilder(ImmutableMap<Currency, int> currencies, double[][] rates)
	  {
		this.currencies = new LinkedHashMap<Currency, int>(currencies);
		// Ensure there is space to add at least one new currency
		this.rates = copyArray(rates, size(currencies.size() + 1));
	  }

	  internal virtual FxMatrixBuilder merge(FxMatrixBuilder other)
	  {

		// Find the common currencies
		Optional<Currency> common = currencies.Keys.Where(other.currencies.containsKey).First();

		Currency commonCurrency = common.orElseThrow(() => new System.ArgumentException("There are no currencies in common between " + currencies.Keys + " and " + other.currencies.Keys));

		// Add in all currencies that we don't already have
		MapStream.of(other.currencies).filterKeys(ccy => !ccy.Equals(commonCurrency) && !currencies.containsKey(ccy)).forEach((ccy, idx) => addCurrencyPair(commonCurrency, ccy, other.getRate(commonCurrency, ccy)));

		return this;
	  }

	  private double getRate(Currency ccy1, Currency ccy2)
	  {
		int i = currencies.get(ccy1);
		int j = currencies.get(ccy2);
		return rates[i][j];
	  }

	  private void addCurrencyPair(Currency ccy1, Currency ccy2, double rate)
	  {

		// Only resize if there's a danger we can't fit a new currency in
		if (rates.Length < currencies.size() + 1)
		{
		  ensureCapacity(Stream.of(ccy1, ccy2));
		}

		if (!currencies.containsKey(ccy1) && !currencies.containsKey(ccy2))
		{
		  // Neither currency present - add to disjoint set
		  disjointRates[CurrencyPair.of(ccy1, ccy2)] = rate;
		}
		else if (currencies.containsKey(ccy1) && currencies.containsKey(ccy2))
		{
		  // We already have a rate for this currency pair
		  updateRate(ccy1, ccy2, rate);
		}
		else
		{
		  // We have exactly one of the currencies already
		  addNewRate(ccy1, ccy2, rate);

		  // With a new rate added we may be able to handle the disjoint
		  retryDisjoints();
		}
	  }

	  private void retryDisjoints()
	  {

		ensureCapacity(disjointRates.Keys.stream().flatMap(cp => Stream.of(cp.Base, cp.Counter)));

		while (true)
		{
		  int initialSize = disjointRates.Count;
		  ImmutableMap<CurrencyPair, double> addable = MapStream.of(disjointRates).filterKeys(pair => currencies.containsKey(pair.Base) || currencies.containsKey(pair.Counter)).toMap();

		  MapStream.of(addable).forEach((pair, rate) => addNewRate(pair.Base, pair.Counter, rate));
		  addable.Keys.ForEach(disjointRates.remove);

		  if (disjointRates.Count == initialSize)
		  {
			// No effect so break out
			break;
		  }
		}
	  }

	  private void addNewRate(Currency ccy1, Currency ccy2, double rate)
	  {

		Currency existing = currencies.containsKey(ccy1) ? ccy1 : ccy2;
		Currency other = existing == ccy1 ? ccy2 : ccy1;

		double updatedRate = existing == ccy2 ? 1.0 / rate : rate;
		int indexRef = currencies.get(existing);
		int indexOther = currencies.size();

		currencies.put(other, indexOther);
		rates[indexOther][indexOther] = 1.0;

		for (int i = 0; i < indexOther; i++)
		{
		  double convertedRate = updatedRate * rates[i][indexRef];
		  rates[i][indexOther] = convertedRate;
		  rates[indexOther][i] = 1.0 / convertedRate;
		}
	  }

	  // We take the first currency as the reference and the second as
	  // the currency to be updated
	  private void updateRate(Currency ccy1, Currency ccy2, double rate)
	  {

		int index1 = currencies.get(ccy1);
		int index2 = currencies.get(ccy2);

		for (int i = 0; i < currencies.size(); i++)
		{
		  // Nothing to do - we know and want rates[index2][index2] = 1
		  if (i != index2)
		  {
			double convertedRate = rate * rates[i][index1];
			rates[i][index2] = convertedRate;
			rates[index2][i] = 1.0 / convertedRate;
		  }
		}
	  }

	  private void addInitialCurrencyPair(Currency ccy1, Currency ccy2, double rate)
	  {
		// No need for capacity check, as initial size is always enough
		currencies.put(ccy1, 0);
		currencies.put(ccy2, 1);
		rates[0][0] = 1.0;
		rates[0][1] = rate;
		rates[1][1] = 1.0;
		rates[1][0] = 1.0 / rate;
	  }

	  private void ensureCapacity(Stream<Currency> potentialCurrencies)
	  {
		// If adding the currencies would mean we have more
		// currencies than matrix size, create an expanded array
		int requiredOrder = (int) Stream.concat(currencies.Keys.stream(), potentialCurrencies).distinct().count();

		ensureCapacity(requiredOrder);
	  }

	  private void ensureCapacity(int requiredOrder)
	  {
		if (requiredOrder > rates.Length)
		{
		  rates = copyArray(rates, size(requiredOrder));
		}
	  }

	  // size the matrix to either the minimal matrix size, or a power of 2
	  // sufficient to hold the required currencies
	  private int size(int requiredCapacity)
	  {
		int lowerPower = Integer.highestOneBit(requiredCapacity);
		return Math.Max(requiredCapacity == lowerPower ? requiredCapacity : lowerPower << 2, MINIMAL_MATRIX_SIZE);
	  }

	  //-------------------------------------------------------------------------
	  // copies the array trimming to the specified size
	  private static double[][] copyArray(double[][] rates, int requestedSize)
	  {
		int order = Math.Min(rates.Length, requestedSize);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] copy = new double[requestedSize][requestedSize];
		double[][] copy = RectangularArrays.ReturnRectangularDoubleArray(requestedSize, requestedSize);
		for (int i = 0; i < order; i++)
		{
		  Array.Copy(rates[i], 0, copy[i], 0, order);
		}
		return copy;
	  }

	}

}