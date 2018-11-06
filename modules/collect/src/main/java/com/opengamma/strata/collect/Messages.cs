using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Pair = com.opengamma.strata.collect.tuple.Pair;

	/// <summary>
	/// Contains utility methods for managing messages.
	/// </summary>
	public sealed class Messages
	{

	  private static readonly Pattern REGEX_PATTERN = Pattern.compile("\\{(\\w*)\\}"); //This will match both {}, and {anything}

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private Messages()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Formats a templated message inserting a single argument.
	  /// <para>
	  /// This method combines a template message with a single argument.
	  /// It can be useful to delay string concatenation, which is sometimes a performance issue.
	  /// The approach is similar to SLF4J MessageFormat, Guava Preconditions and String format().
	  /// </para>
	  /// <para>
	  /// The message template contains zero to many "{}" placeholders.
	  /// The first placeholder is replaced by the string form of the argument.
	  /// Subsequent placeholders are not replaced.
	  /// If there is no placeholder, then the argument is appended to the end of the message.
	  /// No attempt is made to format the argument.
	  /// </para>
	  /// <para>
	  /// This method is null tolerant to ensure that use in exception construction will
	  /// not throw another exception, which might hide the intended exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="messageTemplate">  the message template with "{}" placeholders, null returns empty string </param>
	  /// <param name="arg">  the message argument, null treated as string "null" </param>
	  /// <returns> the formatted message </returns>
	  public static string format(string messageTemplate, object arg)
	  {
		if (string.ReferenceEquals(messageTemplate, null))
		{
		  return format("", arg);
		}
		int placeholderPos = messageTemplate.IndexOf("{}", 0, StringComparison.Ordinal);
		string argStr = arg.ToString();
		StringBuilder builder = new StringBuilder(messageTemplate.Length + argStr.Length + 3);
		if (placeholderPos >= 0)
		{
		  builder.Append(messageTemplate.Substring(0, placeholderPos)).Append(argStr).Append(messageTemplate.Substring(placeholderPos + 2));
		}
		else
		{
		  builder.Append(messageTemplate).Append(" - [").Append(argStr).Append(']');
		}
		return builder.ToString();
	  }

	  /// <summary>
	  /// Formats a templated message inserting arguments.
	  /// <para>
	  /// This method combines a template message with a list of specific arguments.
	  /// It can be useful to delay string concatenation, which is sometimes a performance issue.
	  /// The approach is similar to SLF4J MessageFormat, Guava Preconditions and String format().
	  /// </para>
	  /// <para>
	  /// The message template contains zero to many "{}" placeholders.
	  /// Each placeholder is replaced by the next available argument.
	  /// If there are too few arguments, then the message will be left with placeholders.
	  /// If there are too many arguments, then the excess arguments are appended to the end of the message.
	  /// No attempt is made to format the arguments.
	  /// </para>
	  /// <para>
	  /// This method is null tolerant to ensure that use in exception construction will
	  /// not throw another exception, which might hide the intended exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="messageTemplate">  the message template with "{}" placeholders, null returns empty string </param>
	  /// <param name="args">  the message arguments, null treated as empty array </param>
	  /// <returns> the formatted message </returns>
	  public static string format(string messageTemplate, params object[] args)
	  {
		if (string.ReferenceEquals(messageTemplate, null))
		{
		  return format("", args);
		}
		if (args == null)
		{
		  return format(messageTemplate, new object[0]);
		}
		// try to make builder big enough for the message and the args
		StringBuilder builder = new StringBuilder(messageTemplate.Length + args.Length * 20);
		// insert placeholders
		int argIndex = 0;
		int curPos = 0;
		int nextPlaceholderPos = messageTemplate.IndexOf("{}", curPos, StringComparison.Ordinal);
		while (nextPlaceholderPos >= 0 && argIndex < args.Length)
		{
		  builder.Append(messageTemplate.Substring(curPos, nextPlaceholderPos - curPos)).Append(args[argIndex]);
		  argIndex++;
		  curPos = nextPlaceholderPos + 2;
		  nextPlaceholderPos = messageTemplate.IndexOf("{}", curPos, StringComparison.Ordinal);
		}
		// append remainder of message template
		builder.Append(messageTemplate.Substring(curPos));
		// append remaining args
		if (argIndex < args.Length)
		{
		  builder.Append(" - [");
		  for (int i = argIndex; i < args.Length; i++)
		  {
			if (i > argIndex)
			{
			  builder.Append(", ");
			}
			builder.Append(args[i]);
		  }
		  builder.Append(']');
		}
		return builder.ToString();
	  }

	  /// <summary>
	  /// Formats a templated message inserting named arguments.
	  /// <para>
	  /// A typical template would look like:
	  /// <pre>
	  /// Messages.formatWithAttributes("Foo={foo}, Bar={}", "abc", 123)
	  /// </pre>
	  /// This will return a <seealso cref="Pair"/> with a String and a Map.
	  /// The String will be the formatted message: {@code "Foo=abc, Bar=123"}.
	  /// The Map will look like: <code>{"foo": "123"}</code>.
	  /// </para>
	  /// <para>
	  /// This method combines a template message with a list of specific arguments.
	  /// It can be useful to delay string concatenation, which is sometimes a performance issue.
	  /// The approach is similar to SLF4J MessageFormat, Guava Preconditions and String format().
	  /// </para>
	  /// <para>
	  /// The message template contains zero to many "{name}" placeholders.
	  /// Each placeholder is replaced by the next available argument.
	  /// If there are too few arguments, then the message will be left with placeholders.
	  /// If there are too many arguments, then the excess arguments are appended to the message.
	  /// No attempt is made to format the arguments.
	  /// </para>
	  /// <para>
	  /// This method is null tolerant to ensure that use in exception construction will
	  /// not throw another exception, which might hide the intended exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="messageTemplate">  the message template with "{}" placeholders, null returns empty string </param>
	  /// <param name="args">  the message arguments, null treated as empty array </param>
	  /// <returns> the formatted message </returns>
	  public static Pair<string, IDictionary<string, string>> formatWithAttributes(string messageTemplate, params object[] args)
	  {
		if (string.ReferenceEquals(messageTemplate, null))
		{
		  return formatWithAttributes("", args);
		}
		if (args == null)
		{
		  return formatWithAttributes(messageTemplate);
		}

		// do not use an ImmutableMap, as we avoid throwing exceptions in case of duplicate keys.
		IDictionary<string, string> attributes = new Dictionary<string, string>();
		Matcher matcher = REGEX_PATTERN.matcher(messageTemplate);
		int argIndex = 0;

		StringBuilder outputMessageBuffer = new StringBuilder();
		while (matcher.find())
		{
		  // if the number of placeholders is greater than the number of arguments, then not all placeholders are replaced.
		  if (argIndex >= args.Length)
		  {
			continue;
		  }

		  string attributeName = matcher.group(1); // extract the attribute name
		  string replacement = args[argIndex].ToString().Replace("$", "\\$");
		  matcher.appendReplacement(outputMessageBuffer, replacement);
		  if (attributeName.Length > 0)
		  {
			attributes[attributeName] = replacement;
		  }
		  argIndex++;
		}
		matcher.appendTail(outputMessageBuffer);

		// append remaining args
		if (argIndex < args.Length)
		{
		  outputMessageBuffer.Append(" - [");
		  for (int i = argIndex; i < args.Length; i++)
		  {
			if (i > argIndex)
			{
			  outputMessageBuffer.Append(", ");
			}
			outputMessageBuffer.Append(args[i]);
		  }
		  outputMessageBuffer.Append(']');
		}

		return Pair.of(outputMessageBuffer.ToString(), ImmutableMap.copyOf(attributes));
	  }

	}

}