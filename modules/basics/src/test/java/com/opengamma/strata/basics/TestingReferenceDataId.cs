using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics
{

	/// <summary>
	/// ReferenceDataId implementation used in tests.
	/// </summary>
	[Serializable]
	public class TestingReferenceDataId : ReferenceDataId<Number>
	{

	  private const long serialVersionUID = 1L;

	  private readonly string id;

	  public TestingReferenceDataId(string id)
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

	  public virtual Type<Number> ReferenceDataType
	  {
		  get
		  {
			return typeof(Number);
		  }
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
		TestingReferenceDataId that = (TestingReferenceDataId) obj;
		return Objects.Equals(id, that.id);
	  }

	  public override int GetHashCode()
	  {
		return Objects.hash(id);
	  }

	  public override string ToString()
	  {
		return "TestingReferenceDataId [id=" + id + "]";
	  }

	}

}