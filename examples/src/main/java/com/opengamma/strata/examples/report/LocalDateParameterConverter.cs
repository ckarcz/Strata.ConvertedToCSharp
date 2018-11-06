using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.report
{

	using IStringConverter = com.beust.jcommander.IStringConverter;
	using ParameterException = com.beust.jcommander.ParameterException;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// Parameter converter for <seealso cref="LocalDate"/>.
	/// </summary>
	public class LocalDateParameterConverter : IStringConverter<LocalDate>
	{

	  public override LocalDate convert(string value)
	  {
		try
		{
		  return LocalDate.parse(value);

		}
		catch (DateTimeParseException)
		{
		  throw new ParameterException(Messages.format("Invalid valuation date: {}", value));
		}
		catch (Exception ex)
		{
		  throw new ParameterException(Messages.format("Invalid valuation date: {}" + Environment.NewLine + "Exception: {}", value, ex.Message));
		}
	  }

	}

}