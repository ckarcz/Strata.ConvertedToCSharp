/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.named
{
	/// <summary>
	/// Mock named object.
	/// </summary>
	internal class OtherSampleNameds : SampleNamed
	{

	  public static readonly OtherSampleNameds OTHER = new OtherSampleNameds();

	  public virtual string Name
	  {
		  get
		  {
			return "Other";
		  }
	  }

	}

}