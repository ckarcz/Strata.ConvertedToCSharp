using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{

	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;

	/// <summary>
	/// A test market data ID.
	/// </summary>
	public class TestSimpleId : MarketDataId<string>
	{

	  private readonly string id;
	  private readonly ObservableSource observableSource;

	  public TestSimpleId(string id, ObservableSource obsSource)
	  {
		this.id = id;
		this.observableSource = obsSource;
	  }

	  public virtual Type<string> MarketDataType
	  {
		  get
		  {
			return typeof(string);
		  }
	  }

	  public virtual ObservableSource ObservableSource
	  {
		  get
		  {
			return observableSource;
		  }
	  }

	  public override bool Equals(object o)
	  {
		if (this == o)
		{
		  return true;
		}
		if (o == null || this.GetType() != o.GetType())
		{
		  return false;
		}
		TestSimpleId that = (TestSimpleId) o;
		return Objects.Equals(id, that.id) && Objects.Equals(observableSource, that.observableSource);
	  }

	  public override int GetHashCode()
	  {
		return Objects.hash(id, observableSource);
	  }
	}

}