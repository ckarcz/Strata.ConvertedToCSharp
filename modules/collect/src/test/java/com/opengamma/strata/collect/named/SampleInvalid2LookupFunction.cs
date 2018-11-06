/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.named
{
	using ImmutableMap = com.google.common.collect.ImmutableMap;

	/// <summary>
	/// Mock named object function.
	/// </summary>
	internal class SampleInvalid2LookupFunction : NamedLookup<SampleInvalid2>
	{

	  internal SampleInvalid2LookupFunction(string badConstrucor)
	  {
	  }

	  public virtual ImmutableMap<string, SampleInvalid2> lookupAll()
	  {
		return ImmutableMap.of();
	  }

	}

}