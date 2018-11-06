using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{

	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;

	/// <summary>
	/// MarketDataId implementation used in tests.
	/// </summary>
	public class TestId : MarketDataId<string>
	{

	  private readonly string id;
	  private readonly ObservableSource observableSource;

	  public static TestId of(string id)
	  {
		return new TestId(id);
	  }

	  public TestId(string id, ObservableSource obsSource)
	  {
		this.id = id;
		this.observableSource = obsSource;
	  }

	  public TestId(string id) : this(id, ObservableSource.NONE)
	  {
	  }

	  public virtual Type<string> MarketDataType
	  {
		  get
		  {
			return typeof(string);
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
		TestId testId = (TestId) o;
		return Objects.Equals(id, testId.id) && Objects.Equals(observableSource, testId.observableSource);
	  }

	  public override int GetHashCode()
	  {
		return Objects.hash(id, observableSource);
	  }

	  public override string ToString()
	  {
		return "TestId [id='" + id + "', observableSource=" + observableSource + "]";
	  }
	}

}