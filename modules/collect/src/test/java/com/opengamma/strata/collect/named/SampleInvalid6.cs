/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.named
{
	/// <summary>
	/// Mock named object.
	/// </summary>
	public class SampleInvalid6 : Named
	{

	  /// <summary>
	  /// Non-static - Error.
	  /// </summary>
	  internal NamedLookup<SampleInvalid6> INSTANCE = null;

	  public virtual string Name
	  {
		  get
		  {
			return null;
		  }
	  }

	}

}