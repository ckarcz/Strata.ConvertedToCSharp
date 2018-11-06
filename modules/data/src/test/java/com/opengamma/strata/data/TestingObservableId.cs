using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data
{

	using StandardId = com.opengamma.strata.basics.StandardId;

	/// <summary>
	/// MarketDataId implementation used in tests.
	/// </summary>
	[Serializable]
	public class TestingObservableId : ObservableId
	{

	  private const long serialVersionUID = 1L;

	  private readonly string id;

	  public TestingObservableId(string id)
	  {
		this.id = id;
	  }

	  public virtual string Id
	  {
		  get
		  {
			return id;
		  }
	  }

	  public override Type<double> MarketDataType
	  {
		  get
		  {
			return typeof(Double);
		  }
	  }

	  public virtual StandardId StandardId
	  {
		  get
		  {
			return StandardId.of("Test", id);
		  }
	  }

	  public virtual FieldName FieldName
	  {
		  get
		  {
			return FieldName.MARKET_VALUE;
		  }
	  }

	  public virtual ObservableSource ObservableSource
	  {
		  get
		  {
			return ObservableSource.NONE;
		  }
	  }

	  public virtual ObservableId withObservableSource(ObservableSource obsSource)
	  {
		return this;
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (obj == null || this.GetType() != obj.GetType())
		{
		  return false;
		}
		TestingObservableId that = (TestingObservableId) obj;
		return Objects.Equals(id, that.id);
	  }

	  public override int GetHashCode()
	  {
		return Objects.hash(id);
	  }

	  public override string ToString()
	  {
		return "TestingMarketDataId [id=" + id + "]";
	  }

	}

}