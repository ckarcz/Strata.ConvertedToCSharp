/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.named
{
	using ImmutableMap = com.google.common.collect.ImmutableMap;

	/// <summary>
	/// Mock named object.
	/// </summary>
	public class SampleNamedInstanceLookup1
	{

	  public static readonly SampleNamed ANOTHER1 = new SampleNamedAnonymousInnerClass();

	  private class SampleNamedAnonymousInnerClass : SampleNamed
	  {
		  public SampleNamedAnonymousInnerClass()
		  {
		  }

		  public override string Name
		  {
			  get
			  {
				return "Another1";
			  }
		  }
	  }

	  // package scoped
	  internal static readonly NamedLookup<SampleNamed> INSTANCE = new NamedLookupAnonymousInnerClass();

	  private class NamedLookupAnonymousInnerClass : NamedLookup<SampleNamed>
	  {
		  public NamedLookupAnonymousInnerClass()
		  {
		  }

		  public override ImmutableMap<string, SampleNamed> lookupAll()
		  {
			return ImmutableMap.of("Another1", ANOTHER1, "ANOTHER1", ANOTHER1);
		  }
	  }

	}

}