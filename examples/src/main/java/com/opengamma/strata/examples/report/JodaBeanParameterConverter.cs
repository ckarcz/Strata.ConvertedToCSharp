using System;
using System.Text;
using System.IO;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.report
{

	using JodaBeanSer = org.joda.beans.ser.JodaBeanSer;

	using IStringConverter = com.beust.jcommander.IStringConverter;
	using ParameterException = com.beust.jcommander.ParameterException;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// Abstract parameter converter for bean types.
	/// </summary>
	/// @param <T>  the type of the converter </param>
	public abstract class JodaBeanParameterConverter<T> : IStringConverter<T>
	{

	  /// <summary>
	  /// The Joda-Bean type to parse.
	  /// </summary>
	  /// <returns> the type </returns>
	  protected internal abstract Type<T> ExpectedType {get;}

	  public override T convert(string fileName)
	  {
		try
		{
		  File f = new File(fileName);
		  using (Reader reader = new StreamReader(new FileStream(f, FileMode.Open, FileAccess.Read), Encoding.UTF8))
		  {
			return JodaBeanSer.PRETTY.xmlReader().read(reader, ExpectedType);
		  }
		}
		catch (Exception ex) when (ex is Exception || ex is IOException)
		{
		  if (ex is FileNotFoundException || ex.Cause is FileNotFoundException)
		  {
			throw new ParameterException(Messages.format("File not found: {}", fileName));
		  }
		  throw new ParameterException(Messages.format("Invalid file: {}" + Environment.NewLine + "Exception: {}", fileName, ex.Message));
		}
	  }

	}

}