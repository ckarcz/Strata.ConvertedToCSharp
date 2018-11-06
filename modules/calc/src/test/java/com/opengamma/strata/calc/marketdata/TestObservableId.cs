using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{

	using StandardId = com.opengamma.strata.basics.StandardId;
	using FieldName = com.opengamma.strata.data.FieldName;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;

	/// <summary>
	/// An observable ID implementation used in tests.
	/// </summary>
	[Serializable]
	public class TestObservableId : ObservableId
	{

	  private const long serialVersionUID = 1L;

	  private readonly StandardId id;

	  private readonly ObservableSource observableSource;

	  public static TestObservableId of(string id)
	  {
		return new TestObservableId(id, ObservableSource.NONE);
	  }

	  public static TestObservableId of(string id, ObservableSource obsSource)
	  {
		return new TestObservableId(id, obsSource);
	  }

	  public static TestObservableId of(StandardId id)
	  {
		return new TestObservableId(id, ObservableSource.NONE);
	  }

	  public static TestObservableId of(StandardId id, ObservableSource obsSource)
	  {
		return new TestObservableId(id, obsSource);
	  }

	  internal TestObservableId(string id, ObservableSource obsSource) : this(StandardId.of("test", id), obsSource)
	  {
	  }

	  internal TestObservableId(StandardId id, ObservableSource obsSource)
	  {
		this.observableSource = obsSource;
		this.id = id;
	  }

	  public virtual StandardId StandardId
	  {
		  get
		  {
			return id;
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
			return observableSource;
		  }
	  }

	  public virtual ObservableId withObservableSource(ObservableSource obsSource)
	  {
		return new TestObservableId(id, obsSource);
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
		TestObservableId that = (TestObservableId) obj;
		return Objects.Equals(id, that.id);
	  }

	  public override int GetHashCode()
	  {
		return Objects.hash(id);
	  }

	  public override string ToString()
	  {
		return "TestObservableId [id=" + id + ", observableSource=" + observableSource + "]";
	  }
	}

}