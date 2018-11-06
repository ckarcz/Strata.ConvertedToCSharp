/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.report
{

	using IValueValidator = com.beust.jcommander.IValueValidator;
	using ParameterException = com.beust.jcommander.ParameterException;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// Value validator for the market data root directory.
	/// </summary>
	public class MarketDataRootValidator : IValueValidator<File>
	{

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void validate(String name, java.io.File value) throws com.beust.jcommander.ParameterException
	  public override void validate(string name, File value)
	  {
		if (!value.exists())
		{
		  throw new ParameterException(Messages.format("Invalid market data root directory: {}", value.AbsolutePath));
		}
		if (!value.Directory)
		{
		  throw new ParameterException(Messages.format("Market data root must be a directory: {}", value.AbsolutePath));
		}
	  }

	}

}