/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.schedule
{

	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// Exception thrown when a schedule cannot be calculated.
	/// </summary>
	public sealed class ScheduleException : System.ArgumentException
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The invalid schedule definition.
	  /// </summary>
	  private readonly PeriodicSchedule definition;

	  /// <summary>
	  /// Creates an instance.
	  /// <para>
	  /// The message is formatted using <seealso cref="Messages#format(String, Object...)"/>.
	  /// Message formatting is null tolerant to avoid hiding this exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="msgTemplate">  the message template, null tolerant </param>
	  /// <param name="msgArguments">  the message arguments, null tolerant </param>
	  public ScheduleException(string msgTemplate, params object[] msgArguments) : this(null, msgTemplate, msgArguments)
	  {
	  }

	  /// <summary>
	  /// Creates an instance, specifying the definition that caused the problem.
	  /// <para>
	  /// The message is formatted using <seealso cref="Messages#format(String, Object...)"/>.
	  /// Message formatting is null tolerant to avoid hiding this exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="definition">  the invalid schedule definition, null tolerant </param>
	  /// <param name="msgTemplate">  the message template, null tolerant </param>
	  /// <param name="msgArguments">  the message arguments, null tolerant </param>
	  public ScheduleException(PeriodicSchedule definition, string msgTemplate, params object[] msgArguments) : base(Messages.format(msgTemplate, msgArguments))
	  {
		this.definition = definition;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the invalid schedule definition.
	  /// </summary>
	  /// <returns> the optional definition </returns>
	  public Optional<PeriodicSchedule> Definition
	  {
		  get
		  {
			return Optional.ofNullable(definition);
		  }
	  }

	}

}