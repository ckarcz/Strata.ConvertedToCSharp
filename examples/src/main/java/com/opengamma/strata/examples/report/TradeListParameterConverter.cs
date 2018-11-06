using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.report
{
	/// <summary>
	/// Parameter converter for <seealso cref="TradeList"/>.
	/// </summary>
	public class TradeListParameterConverter : JodaBeanParameterConverter<TradeList>
	{

	  protected internal override Type<TradeList> ExpectedType
	  {
		  get
		  {
			return typeof(TradeList);
		  }
	  }

	}

}