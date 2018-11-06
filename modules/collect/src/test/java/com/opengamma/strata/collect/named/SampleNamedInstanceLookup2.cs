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
	public class SampleNamedInstanceLookup2
	{

	  public static readonly SampleNamed ANOTHER2 = new SampleNamedAnonymousInnerClass();

	  private class SampleNamedAnonymousInnerClass : SampleNamed
	  {
		  public SampleNamedAnonymousInnerClass()
		  {
		  }

		  public override string Name
		  {
			  get
			  {
				return "Another2";
			  }
		  }
	  }

	  // public scoped
	  public static readonly NamedLookup<SampleNamed> INSTANCE = new NamedLookupAnonymousInnerClass();

	  private class NamedLookupAnonymousInnerClass : NamedLookup<SampleNamed>
	  {
		  public NamedLookupAnonymousInnerClass()
		  {
		  }

		  public override ImmutableMap<string, SampleNamed> lookupAll()
		  {
			return ImmutableMap.of("Another2", ANOTHER2, "ANOTHER2", ANOTHER2);
		  }
	  }

	}

}