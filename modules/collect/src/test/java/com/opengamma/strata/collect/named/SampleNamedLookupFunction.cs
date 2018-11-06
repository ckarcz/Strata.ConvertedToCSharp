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
	internal class SampleNamedLookupFunction : NamedLookup<SampleNamed>
	{

	  internal SampleNamedLookupFunction()
	  {
	  }

	  public virtual ImmutableMap<string, SampleNamed> lookupAll()
	  {
		return ImmutableMap.of("Other", OtherSampleNameds.OTHER);
	  }

	}

}