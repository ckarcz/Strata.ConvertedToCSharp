using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{

	using ObjIntFunction = com.opengamma.strata.collect.function.ObjIntFunction;

	/// <summary>
	/// A market data box that contains no data.
	/// </summary>
	[Serializable]
	internal class EmptyMarketDataBox : MarketDataBox<Void>
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The single shared instance of this class. </summary>
	  private static readonly EmptyMarketDataBox INSTANCE = new EmptyMarketDataBox();

	  /// <summary>
	  /// Obtains an instance containing no data.
	  /// </summary>
	  /// @param <T>  the required type of the market data box </param>
	  /// <returns> a market data box containing no data </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") static <T> MarketDataBox<T> empty()
	  internal static MarketDataBox<T> empty<T>()
	  {
		return (MarketDataBox<T>) INSTANCE;
	  }

	  //-------------------------------------------------------------------------
	  public virtual Void SingleValue
	  {
		  get
		  {
			throw new System.NotSupportedException("Cannot get a value from an empty market data box");
		  }
	  }

	  public virtual ScenarioArray<Void> ScenarioValue
	  {
		  get
		  {
			throw new System.NotSupportedException("Cannot get a value from an empty market data box");
		  }
	  }

	  public virtual Void getValue(int scenarioIndex)
	  {
		throw new System.NotSupportedException("Cannot get a value from an empty market data box");
	  }

	  public virtual bool SingleValue
	  {
		  get
		  {
			return true;
		  }
	  }

	  public virtual MarketDataBox<R> map<R>(System.Func<Void, R> fn)
	  {
		return empty();
	  }

	  public virtual MarketDataBox<R> combineWith<U, R>(MarketDataBox<U> other, System.Func<Void, U, R> fn)
	  {
		return empty();
	  }

	  public virtual MarketDataBox<R> mapWithIndex<R>(int scenarioCount, ObjIntFunction<Void, R> fn)
	  {
		return empty();
	  }

	  public virtual int ScenarioCount
	  {
		  get
		  {
			return 0;
		  }
	  }

	  public virtual Type<Void> MarketDataType
	  {
		  get
		  {
			return typeof(Void);
		  }
	  }

	  public virtual Stream<Void> stream()
	  {
		return Stream.empty();
	  }
	}

}