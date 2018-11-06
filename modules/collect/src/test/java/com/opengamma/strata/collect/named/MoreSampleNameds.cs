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
	internal class MoreSampleNameds : SampleNamed
	{

	  public static readonly MoreSampleNameds MORE = new MoreSampleNameds();
	  public const string TEXT = "Not a constant";
	  internal const MoreSampleNameds NOT_PUBLIC = null;
	  public readonly MoreSampleNameds NOT_STATIC = null;
	  public static MoreSampleNameds NOT_FINAL = null;

	  public virtual string Name
	  {
		  get
		  {
			return "More";
		  }
	  }

	}

}