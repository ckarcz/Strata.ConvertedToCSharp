using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{

	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using NamedMarketDataId = com.opengamma.strata.data.NamedMarketDataId;

	/// <summary>
	/// NamedMarketDataId implementation used in tests.
	/// </summary>
	[Serializable]
	public class TestingNamedId : NamedMarketDataId<string>
	{

	  private const long serialVersionUID = 1L;

	  private readonly string name;

	  public TestingNamedId(string name)
	  {
		this.name = name;
	  }

	  public virtual MarketDataName<string> MarketDataName
	  {
		  get
		  {
			return new TestingName(name);
		  }
	  }

	  public override Type<string> MarketDataType
	  {
		  get
		  {
			return typeof(string);
		  }
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (obj is TestingNamedId)
		{
		  TestingNamedId other = (TestingNamedId) obj;
		  return Objects.Equals(name, other.name);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		return Objects.hash(name);
	  }

	  public override string ToString()
	  {
		return name;
	  }

	}

}