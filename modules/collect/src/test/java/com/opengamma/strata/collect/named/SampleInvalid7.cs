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
	public class SampleInvalid7 : Named
	{

	  /// <summary>
	  /// Not-NamedLookup - Error.
	  /// </summary>
	  internal static object INSTANCE = null;

	  public virtual string Name
	  {
		  get
		  {
			return null;
		  }
	  }

	}

}