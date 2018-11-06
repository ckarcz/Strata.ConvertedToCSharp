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
	public class SampleNameds : SampleNamed
	{

	  public static readonly SampleNameds STANDARD = new SampleNameds();

	  public virtual string Name
	  {
		  get
		  {
			return "Standard";
		  }
	  }

	}

}