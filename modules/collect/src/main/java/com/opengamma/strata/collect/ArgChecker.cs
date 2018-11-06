using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{

	using CharMatcher = com.google.common.@base.CharMatcher;
	using DoubleMath = com.google.common.math.DoubleMath;

	/// <summary>
	/// Contains utility methods for checking inputs to methods.
	/// <para>
	/// This utility is used throughout the system to validate inputs to methods.
	/// Most of the methods return their validated input, allowing patterns like this:
	/// <pre>
	///  // constructor
	///  public Person(String name, int age) {
	///    this.name = ArgChecker.notBlank(name, "name");
	///    this.age = ArgChecker.notNegative(age, "age");
	///  }
	/// </pre>
	/// </para>
	/// </summary>
	public sealed class ArgChecker
	{

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private ArgChecker()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks that the specified boolean is true.
	  /// <para>
	  /// Given the input argument, this returns normally only if it is true.
	  /// This will typically be the result of a caller-specific check.
	  /// For example:
	  /// <pre>
	  ///  ArgChecker.isTrue(collection.contains("value"));
	  /// </pre>
	  /// </para>
	  /// <para>
	  /// It is strongly recommended to pass an additional message argument using
	  /// <seealso cref="#isTrue(boolean, String)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="validIfTrue">  a boolean resulting from testing an argument </param>
	  /// <exception cref="IllegalArgumentException"> if the test value is false </exception>
	  public static void isTrue(bool validIfTrue)
	  {
		// return void, not the argument, as no need to check a boolean method argument
		if (!validIfTrue)
		{
		  throw new System.ArgumentException("Invalid argument, expression must be true");
		}
	  }

	  /// <summary>
	  /// Checks that the specified boolean is true.
	  /// <para>
	  /// Given the input argument, this returns normally only if it is true.
	  /// This will typically be the result of a caller-specific check.
	  /// For example:
	  /// <pre>
	  ///  ArgChecker.isTrue(collection.contains("value"), "Collection must contain 'value'");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="validIfTrue">  a boolean resulting from testing an argument </param>
	  /// <param name="message">  the error message, not null </param>
	  /// <exception cref="IllegalArgumentException"> if the test value is false </exception>
	  public static void isTrue(bool validIfTrue, string message)
	  {
		// return void, not the argument, as no need to check a boolean method argument
		if (!validIfTrue)
		{
		  throw new System.ArgumentException(message);
		}
	  }

	  /// <summary>
	  /// Checks that the specified boolean is true.
	  /// <para>
	  /// Given the input argument, this returns normally only if it is true.
	  /// This will typically be the result of a caller-specific check.
	  /// For example:
	  /// <pre>
	  ///  ArgChecker.isTrue(collection.contains("value"), "Collection must contain 'value': {}", collection);
	  /// </pre>
	  /// </para>
	  /// <para>
	  /// This returns {@code void}, and not the value being checked, as there is
	  /// never a good reason to validate a boolean argument value.
	  /// </para>
	  /// <para>
	  /// The message is produced using a template that contains zero to many "{}" placeholders.
	  /// Each placeholder is replaced by the next available argument.
	  /// If there are too few arguments, then the message will be left with placeholders.
	  /// If there are too many arguments, then the excess arguments are appended to the
	  /// end of the message. No attempt is made to format the arguments.
	  /// See <seealso cref="Messages#format(String, Object...)"/> for more details.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="validIfTrue">  a boolean resulting from testing an argument </param>
	  /// <param name="message">  the error message with {} placeholders, not null </param>
	  /// <param name="arg">  the message arguments </param>
	  /// <exception cref="IllegalArgumentException"> if the test value is false </exception>
	  public static void isTrue(bool validIfTrue, string message, params object[] arg)
	  {
		// return void, not the argument, as no need to check a boolean method argument
		if (!validIfTrue)
		{
		  throw new System.ArgumentException(Messages.format(message, arg));
		}
	  }

	  /// <summary>
	  /// Checks that the specified boolean is true.
	  /// <para>
	  /// Given the input argument, this returns normally only if it is true.
	  /// This will typically be the result of a caller-specific check.
	  /// For example:
	  /// <pre>
	  ///  ArgChecker.isTrue(value &gt; check, "Value must be greater than check: {}", value);
	  /// </pre>
	  /// </para>
	  /// <para>
	  /// This returns {@code void}, and not the value being checked, as there is
	  /// never a good reason to validate a boolean argument value.
	  /// </para>
	  /// <para>
	  /// The message is produced using a template that contains zero or one "{}" placeholders.
	  /// The placeholder, if present, is replaced by the argument.
	  /// If there is no placeholder, the argument is appended to the end of the message.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="validIfTrue">  a boolean resulting from testing an argument </param>
	  /// <param name="message">  the error message with {} placeholders, not null </param>
	  /// <param name="arg">  the message argument </param>
	  /// <exception cref="IllegalArgumentException"> if the test value is false </exception>
	  public static void isTrue(bool validIfTrue, string message, long arg)
	  {
		// return void, not the argument, as no need to check a boolean method argument
		if (!validIfTrue)
		{
		  throw new System.ArgumentException(Messages.format(message, arg));
		}
	  }

	  /// <summary>
	  /// Checks that the specified boolean is true.
	  /// <para>
	  /// Given the input argument, this returns normally only if it is true.
	  /// This will typically be the result of a caller-specific check.
	  /// For example:
	  /// <pre>
	  ///  ArgChecker.isTrue(value &gt; check, "Value must be greater than check: {}", value);
	  /// </pre>
	  /// </para>
	  /// <para>
	  /// This returns {@code void}, and not the value being checked, as there is
	  /// never a good reason to validate a boolean argument value.
	  /// </para>
	  /// <para>
	  /// The message is produced using a template that contains zero or one "{}" placeholders.
	  /// The placeholder, if present, is replaced by the argument.
	  /// If there is no placeholder, the argument is appended to the end of the message.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="validIfTrue">  a boolean resulting from testing an argument </param>
	  /// <param name="message">  the error message with {} placeholders, not null </param>
	  /// <param name="arg">  the message argument </param>
	  /// <exception cref="IllegalArgumentException"> if the test value is false </exception>
	  public static void isTrue(bool validIfTrue, string message, double arg)
	  {
		// return void, not the argument, as no need to check a boolean method argument
		if (!validIfTrue)
		{
		  throw new System.ArgumentException(Messages.format(message, arg));
		}
	  }

	  /// <summary>
	  /// Checks that the specified boolean is false.
	  /// <para>
	  /// Given the input argument, this returns normally only if it is false.
	  /// This will typically be the result of a caller-specific check.
	  /// For example:
	  /// <pre>
	  ///  ArgChecker.isFalse(collection.contains("value"), "Collection must not contain 'value'");
	  /// </pre>
	  /// </para>
	  /// <para>
	  /// This returns {@code void}, and not the value being checked, as there is
	  /// never a good reason to validate a boolean argument value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="validIfFalse">  a boolean resulting from testing an argument </param>
	  /// <param name="message">  the error message, not null </param>
	  /// <exception cref="IllegalArgumentException"> if the test value is true </exception>
	  public static void isFalse(bool validIfFalse, string message)
	  {
		// return void, not the argument, as no need to check a boolean method argument
		if (validIfFalse)
		{
		  throw new System.ArgumentException(message);
		}
	  }

	  /// <summary>
	  /// Checks that the specified boolean is false.
	  /// <para>
	  /// Given the input argument, this returns normally only if it is false.
	  /// This will typically be the result of a caller-specific check.
	  /// For example:
	  /// <pre>
	  ///  ArgChecker.isFalse(collection.contains("value"), "Collection must not contain 'value': {}", collection);
	  /// </pre>
	  /// </para>
	  /// <para>
	  /// This returns {@code void}, and not the value being checked, as there is
	  /// never a good reason to validate a boolean argument value.
	  /// </para>
	  /// <para>
	  /// The message is produced using a template that contains zero to many "{}" placeholders.
	  /// Each placeholder is replaced by the next available argument.
	  /// If there are too few arguments, then the message will be left with placeholders.
	  /// If there are too many arguments, then the excess arguments are appended to the
	  /// end of the message. No attempt is made to format the arguments.
	  /// See <seealso cref="Messages#format(String, Object...)"/> for more details.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="validIfFalse">  a boolean resulting from testing an argument </param>
	  /// <param name="message">  the error message with {} placeholders, not null </param>
	  /// <param name="arg">  the message arguments, not null </param>
	  /// <exception cref="IllegalArgumentException"> if the test value is true </exception>
	  public static void isFalse(bool validIfFalse, string message, params object[] arg)
	  {
		// return void, not the argument, as no need to check a boolean method argument
		if (validIfFalse)
		{
		  throw new System.ArgumentException(Messages.format(message, arg));
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks that the specified argument is non-null.
	  /// <para>
	  /// Given the input argument, this returns only if it is non-null.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.name = ArgChecker.notNull(name, "name");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the input argument reflected in the result </param>
	  /// <param name="argument">  the argument to check, null throws an exception </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument}, not null </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is null </exception>
	  public static T notNull<T>(T argument, string name)
	  {
		if (argument == default(T))
		{
		  throw new System.ArgumentException(notNullMsg(name));
		}
		return argument;
	  }

	  // extracted to aid inlining performance
	  private static string notNullMsg(string name)
	  {
		return "Argument '" + name + "' must not be null";
	  }

	  /// <summary>
	  /// Checks that the specified item is non-null.
	  /// <para>
	  /// Given the input argument, this returns only if it is non-null.
	  /// One use for this method is in a stream:
	  /// <pre>
	  ///  ArgChecker.notNull(coll, "coll")
	  ///  coll.stream()
	  ///    .map(ArgChecker::notNullItem)
	  ///    ...
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the input argument reflected in the result </param>
	  /// <param name="argument">  the argument to check, null throws an exception </param>
	  /// <returns> the input {@code argument}, not null </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is null </exception>
	  public static T notNullItem<T>(T argument)
	  {
		if (argument == default(T))
		{
		  throw new System.ArgumentException("Argument array/collection/map must not contain null");
		}
		return argument;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks that the specified argument is non-null and matches the specified pattern.
	  /// <para>
	  /// Given the input argument, this returns only if it is non-null and matches
	  /// the regular expression pattern specified.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.name = ArgChecker.matches(REGEX_NAME, name, "name");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pattern">  the pattern to check against, not null </param>
	  /// <param name="argument">  the argument to check, null throws an exception </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument}, not null </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is null or empty </exception>
	  public static string matches(Pattern pattern, string argument, string name)
	  {
		notNull(pattern, "pattern");
		notNull(argument, name);
		if (!pattern.matcher(argument).matches())
		{
		  throw new System.ArgumentException(matchesMsg(pattern, name, argument));
		}
		return argument;
	  }

	  // extracted to aid inlining performance
	  private static string matchesMsg(Pattern pattern, string name, string value)
	  {
		return "Argument '" + name + "' with value '" + value + "' must match pattern: " + pattern;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks that the specified argument is non-null and only contains the specified characters.
	  /// <para>
	  /// Given the input argument, this returns only if it is non-null and matches
	  /// the <seealso cref="CharMatcher"/> specified.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.name = ArgChecker.matches(REGEX_NAME, 1, Integer.MAX_VALUE, name, "name", "[A-Z]+");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="matcher">  the matcher to check against, not null </param>
	  /// <param name="minLength">  the minimum length to allow </param>
	  /// <param name="maxLength">  the minimum length to allow </param>
	  /// <param name="argument">  the argument to check, null throws an exception </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <param name="equivalentRegex">  the equivalent regular expression pattern </param>
	  /// <returns> the input {@code argument}, not null </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is null or empty </exception>
	  public static string matches(CharMatcher matcher, int minLength, int maxLength, string argument, string name, string equivalentRegex)
	  {

		notNull(matcher, "pattern");
		notNull(argument, name);
		if (argument.Length < minLength || argument.Length > maxLength || !matcher.matchesAllOf(argument))
		{
		  throw new System.ArgumentException(matchesMsg(matcher, name, argument, equivalentRegex));
		}
		return argument;
	  }

	  // extracted to aid inlining performance
	  private static string matchesMsg(CharMatcher matcher, string name, string value, string equivalentRegex)
	  {
		return "Argument '" + name + "' with value '" + value + "' must match pattern: " + equivalentRegex;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks that the specified argument is non-null and not blank.
	  /// <para>
	  /// Given the input argument, this returns the input only if it is non-null
	  /// and contains at least one non whitespace character.
	  /// This is often linked with a call to {@code trim()}.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.name = ArgChecker.notBlank(name, "name").trim();
	  /// </pre>
	  /// </para>
	  /// <para>
	  /// The argument is trimmed using <seealso cref="String#trim()"/> to determine if it is empty.
	  /// The result is the original argument, not the trimmed one.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the argument to check, null or blank throws an exception </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument}, not null </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is null or blank </exception>
	  public static string notBlank(string argument, string name)
	  {
		notNull(argument, name);
		if (argument.Trim().Length == 0)
		{
		  throw new System.ArgumentException(notBlankMsg(name));
		}
		return argument;
	  }

	  // extracted to aid inlining performance
	  private static string notBlankMsg(string name)
	  {
		return "Argument '" + name + "' must not be blank";
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks that the specified argument is non-null and not empty.
	  /// <para>
	  /// Given the input argument, this returns only if it is non-null and contains
	  /// at least one character, which may be a whitespace character.
	  /// See also <seealso cref="#notBlank(String, String)"/>.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.name = ArgChecker.notEmpty(name, "name");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the argument to check, null or empty throws an exception </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument}, not null </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is null or empty </exception>
	  public static string notEmpty(string argument, string name)
	  {
		notNull(argument, name);
		if (argument.Length == 0)
		{
		  throw new System.ArgumentException(notEmptyMsg(name));
		}
		return argument;
	  }

	  // extracted to aid inlining performance
	  private static string notEmptyMsg(string name)
	  {
		return "Argument '" + name + "' must not be empty";
	  }

	  /// <summary>
	  /// Checks that the specified argument array is non-null and not empty.
	  /// <para>
	  /// Given the input argument, this returns only if it is non-null and contains
	  /// at least one element. The element is not validated and may be null.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.names = ArgChecker.notEmpty(names, "names");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the input array reflected in the result </param>
	  /// <param name="argument">  the argument to check, null or empty throws an exception </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument}, not null </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is null or empty </exception>
	  public static T[] notEmpty<T>(T[] argument, string name)
	  {
		notNull(argument, name);
		if (argument.Length == 0)
		{
		  throw new System.ArgumentException(notEmptyArrayMsg(name));
		}
		return argument;
	  }

	  // extracted to aid inlining performance
	  private static string notEmptyArrayMsg(string name)
	  {
		return "Argument array '" + name + "' must not be empty";
	  }

	  /// <summary>
	  /// Checks that the specified argument array is non-null and not empty.
	  /// <para>
	  /// Given the input argument, this returns only if it is non-null and contains
	  /// at least one element.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.values = ArgChecker.notEmpty(values, "values");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the argument to check, null or empty throws an exception </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument}, not null </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is null or empty </exception>
	  public static int[] notEmpty(int[] argument, string name)
	  {
		notNull(argument, name);
		if (argument.Length == 0)
		{
		  throw new System.ArgumentException(notEmptyArrayMsg(name));
		}
		return argument;
	  }

	  /// <summary>
	  /// Checks that the specified argument array is non-null and not empty.
	  /// <para>
	  /// Given the input argument, this returns only if it is non-null and contains
	  /// at least one element.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.values = ArgChecker.notEmpty(values, "values");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the argument to check, null or empty throws an exception </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument}, not null </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is null or empty </exception>
	  public static long[] notEmpty(long[] argument, string name)
	  {
		notNull(argument, name);
		if (argument.Length == 0)
		{
		  throw new System.ArgumentException(notEmptyArrayMsg(name));
		}
		return argument;
	  }

	  /// <summary>
	  /// Checks that the specified argument array is non-null and not empty.
	  /// <para>
	  /// Given the input argument, this returns only if it is non-null and contains
	  /// at least one element.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.values = ArgChecker.notEmpty(values, "values");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the argument to check, null or empty throws an exception </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument}, not null </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is null or empty </exception>
	  public static double[] notEmpty(double[] argument, string name)
	  {
		notNull(argument, name);
		if (argument.Length == 0)
		{
		  throw new System.ArgumentException(notEmptyArrayMsg(name));
		}
		return argument;
	  }

	  /// <summary>
	  /// Checks that the specified argument iterable is non-null and not empty.
	  /// <para>
	  /// Given the input argument, this returns only if it is non-null and contains
	  /// at least one element. The element is not validated and may be null.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.values = ArgChecker.notEmpty(values, "values");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the element type of the input iterable reflected in the result </param>
	  /// @param <I>  the type of the input iterable, reflected in the result </param>
	  /// <param name="argument">  the argument to check, null or empty throws an exception </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument}, not null </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is null or empty </exception>
	  public static I notEmpty<T, I>(I argument, string name) where I : IEnumerable<T>
	  {
		notNull(argument, name);
		if (!argument.GetEnumerator().hasNext())
		{
		  throw new System.ArgumentException(notEmptyIterableMsg(name));
		}
		return argument;
	  }

	  // extracted to aid inlining performance
	  private static string notEmptyIterableMsg(string name)
	  {
		return "Argument iterable '" + name + "' must not be empty";
	  }

	  /// <summary>
	  /// Checks that the specified argument collection is non-null and not empty.
	  /// <para>
	  /// Given the input argument, this returns only if it is non-null and contains at least one element.
	  /// The element is not validated and may contain nulls if the collection allows nulls.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.values = ArgChecker.notEmpty(values, "values");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the element type of the input collection reflected in the result </param>
	  /// @param <C>  the type of the input collection, reflected in the result </param>
	  /// <param name="argument">  the argument to check, null or empty throws an exception </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument}, not null </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is null or empty </exception>
	  public static C notEmpty<T, C>(C argument, string name) where C : ICollection<T>
	  {
		notNull(argument, name);
		if (argument.Count == 0)
		{
		  throw new System.ArgumentException(notEmptyCollectionMsg(name));
		}
		return argument;
	  }

	  // extracted to aid inlining performance
	  private static string notEmptyCollectionMsg(string name)
	  {
		return "Argument collection '" + name + "' must not be empty";
	  }

	  /// <summary>
	  /// Checks that the specified argument map is non-null and not empty.
	  /// <para>
	  /// Given the input argument, this returns only if it is non-null and contains at least one mapping.
	  /// The element is not validated and may contain nulls if the collection allows nulls.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.keyValues = ArgChecker.notEmpty(keyValues, "keyValues");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <K>  the key type of the input map key, reflected in the result </param>
	  /// @param <V>  the value type of the input map value, reflected in the result </param>
	  /// @param <M>  the type of the input map, reflected in the result </param>
	  /// <param name="argument">  the argument to check, null or empty throws an exception </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument}, not null </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is null or empty </exception>
	  public static M notEmpty<K, V, M>(M argument, string name) where M : IDictionary<K, V>
	  {
		notNull(argument, name);
		if (argument.Count == 0)
		{
		  throw new System.ArgumentException(notEmptyMapMsg(name));
		}
		return argument;
	  }

	  // extracted to aid inlining performance
	  private static string notEmptyMapMsg(string name)
	  {
		return "Argument map '" + name + "' must not be empty";
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks that the specified argument array is non-null and contains no nulls.
	  /// <para>
	  /// Given the input argument, this returns only if it is non-null and contains no nulls.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.values = ArgChecker.noNulls(values, "values");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the input array reflected in the result </param>
	  /// <param name="argument">  the argument to check, null or contains null throws an exception </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument}, not null </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is null or contains nulls </exception>
	  public static T[] noNulls<T>(T[] argument, string name)
	  {
		notNull(argument, name);
		for (int i = 0; i < argument.Length; i++)
		{
		  if (argument[i] == default(T))
		  {
			throw new System.ArgumentException("Argument array '" + name + "' must not contain null at index " + i);
		  }
		}
		return argument;
	  }

	  /// <summary>
	  /// Checks that the specified argument collection is non-null and contains no nulls.
	  /// <para>
	  /// Given the input argument, this returns only if it is non-null and contains no nulls.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.values = ArgChecker.noNulls(values, "values");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the element type of the input iterable reflected in the result </param>
	  /// @param <I>  the type of the input iterable, reflected in the result </param>
	  /// <param name="argument">  the argument to check, null or contains null throws an exception </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument}, not null </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is null or contains nulls </exception>
	  public static I noNulls<T, I>(I argument, string name) where I : IEnumerable<T>
	  {
		notNull(argument, name);
		foreach (object obj in argument)
		{
		  if (obj == null)
		  {
			throw new System.ArgumentException("Argument iterable '" + name + "' must not contain null");
		  }
		}
		return argument;
	  }

	  /// <summary>
	  /// Checks that the specified argument map is non-null and contains no nulls.
	  /// <para>
	  /// Given the input argument, this returns only if it is non-null and contains no nulls.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.keyValues = ArgChecker.noNulls(keyValues, "keyValues");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <K>  the key type of the input map key, reflected in the result </param>
	  /// @param <V>  the value type of the input map value, reflected in the result </param>
	  /// @param <M>  the type of the input map, reflected in the result </param>
	  /// <param name="argument">  the argument to check, null or contains null throws an exception </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument}, not null </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is null or contains nulls </exception>
	  public static M noNulls<K, V, M>(M argument, string name) where M : IDictionary<K, V>
	  {
		notNull(argument, name);
		foreach (KeyValuePair<K, V> entry in argument.SetOfKeyValuePairs())
		{
		  if (entry.Key == null)
		  {
			throw new System.ArgumentException("Argument map '" + name + "' must not contain a null key");
		  }
		  if (entry.Value == null)
		  {
			throw new System.ArgumentException("Argument map '" + name + "' must not contain a null value");
		  }
		}
		return argument;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks that the argument is not negative.
	  /// <para>
	  /// Given the input argument, this returns only if it is zero or greater.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.amount = ArgChecker.notNegative(amount, "amount");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the argument to check </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument} </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is negative </exception>
	  public static int notNegative(int argument, string name)
	  {
		if (argument < 0)
		{
		  throw new System.ArgumentException(notNegativeMsg(name));
		}
		return argument;
	  }

	  // extracted to aid inlining performance
	  private static string notNegativeMsg(string name)
	  {
		return "Argument '" + name + "' must not be negative";
	  }

	  /// <summary>
	  /// Checks that the argument is not negative.
	  /// <para>
	  /// Given the input argument, this returns only if it is zero or greater.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.amount = ArgChecker.notNegative(amount, "amount");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the argument to check </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument} </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is negative </exception>
	  public static long notNegative(long argument, string name)
	  {
		if (argument < 0)
		{
		  throw new System.ArgumentException(notNegativeMsg(name));
		}
		return argument;
	  }

	  /// <summary>
	  /// Checks that the argument is not negative.
	  /// <para>
	  /// Given the input argument, this returns only if it is zero or greater.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.amount = ArgChecker.notNegative(amount, "amount");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the argument to check </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument} </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is negative </exception>
	  public static double notNegative(double argument, string name)
	  {
		if (argument < 0)
		{
		  throw new System.ArgumentException(notNegativeMsg(name));
		}
		return argument;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks that the argument is not negative or zero.
	  /// <para>
	  /// Given the input argument, this returns only if it is greater than zero.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.amount = ArgChecker.notNegativeOrZero(amount, "amount");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the argument to check </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument} </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is negative or zero </exception>
	  public static int notNegativeOrZero(int argument, string name)
	  {
		if (argument <= 0)
		{
		  throw new System.ArgumentException(notNegativeOrZeroMsg(name, argument));
		}
		return argument;
	  }

	  // extracted to aid inlining performance
	  private static string notNegativeOrZeroMsg(string name, double argument)
	  {
		return "Argument '" + name + "' must not be negative or zero but has value " + argument;
	  }

	  /// <summary>
	  /// Checks that the argument is not negative or zero.
	  /// <para>
	  /// Given the input argument, this returns only if it is greater than zero.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.amount = ArgChecker.notNegativeOrZero(amount, "amount");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the argument to check </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument} </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is negative or zero </exception>
	  public static long notNegativeOrZero(long argument, string name)
	  {
		if (argument <= 0)
		{
		  throw new System.ArgumentException(notNegativeOrZeroMsg(name, argument));
		}
		return argument;
	  }

	  /// <summary>
	  /// Checks that the argument is not negative or zero.
	  /// <para>
	  /// Given the input argument, this returns only if it is greater than zero.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.amount = ArgChecker.notNegativeOrZero(amount, "amount");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the argument to check </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument} </returns>
	  /// <exception cref="IllegalArgumentException"> if the input is negative or zero </exception>
	  public static double notNegativeOrZero(double argument, string name)
	  {
		if (argument <= 0)
		{
		  throw new System.ArgumentException(notNegativeOrZeroMsg(name, argument));
		}
		return argument;
	  }

	  /// <summary>
	  /// Checks that the argument is greater than zero to within a given accuracy.
	  /// <para>
	  /// Given the input argument, this returns only if it is greater than zero
	  /// using the {@code eps} accuracy for zero.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.amount = ArgChecker.notNegativeOrZero(amount, 0.0001d, "amount");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the value to check </param>
	  /// <param name="tolerance">  the tolerance to use for zero </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument} </returns>
	  /// <exception cref="IllegalArgumentException"> if the absolute value of the argument is less than eps </exception>
	  public static double notNegativeOrZero(double argument, double tolerance, string name)
	  {
		if (DoubleMath.fuzzyEquals(argument, 0, tolerance))
		{
		  throw new System.ArgumentException("Argument '" + name + "' must not be zero");
		}
		if (argument < 0)
		{
		  throw new System.ArgumentException("Argument '" + name + "' must be greater than zero but has value " + argument);
		}
		return argument;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks that the argument is not equal to zero.
	  /// <para>
	  /// Given the input argument, this returns only if it is not zero.
	  /// Both positive and negative zero are checked.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.amount = ArgChecker.notZero(amount, "amount");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the value to check </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument} </returns>
	  /// <exception cref="IllegalArgumentException"> if the argument is zero </exception>
	  public static double notZero(double argument, string name)
	  {
		if (argument == 0d || argument == -0d)
		{
		  throw new System.ArgumentException("Argument '" + name + "' must not be zero");
		}
		return argument;
	  }

	  /// <summary>
	  /// Checks that the argument is not equal to zero to within a given accuracy.
	  /// <para>
	  /// Given the input argument, this returns only if it is not zero comparing
	  /// using the {@code eps} accuracy.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.amount = ArgChecker.notZero(amount, 0.0001d, "amount");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the value to check </param>
	  /// <param name="tolerance">  the tolerance to use for zero </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument} </returns>
	  /// <exception cref="IllegalArgumentException"> if the absolute value of the argument is less than the tolerance </exception>
	  public static double notZero(double argument, double tolerance, string name)
	  {
		if (DoubleMath.fuzzyEquals(argument, 0d, tolerance))
		{
		  throw new System.ArgumentException("Argument '" + name + "' must not be zero");
		}
		return argument;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks that the argument is within the range defined by {@code low <= x < high}.
	  /// <para>
	  /// Given a value, this returns true if it is within the specified range including the
	  /// lower boundary but excluding the upper boundary.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.amount = ArgChecker.inRange(amount, 0d, 1d, "amount");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the value to check </param>
	  /// <param name="lowInclusive">  the low value of the range </param>
	  /// <param name="highExclusive">  the high value of the range </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument} </returns>
	  /// <exception cref="IllegalArgumentException"> if the argument is outside the valid range </exception>
	  public static double inRange(double argument, double lowInclusive, double highExclusive, string name)
	  {
		if (argument < lowInclusive || argument >= highExclusive)
		{
		  throw new System.ArgumentException(Messages.format("Expected {} <= '{}' < {}, but found {}", lowInclusive, name, highExclusive, argument));
		}
		return argument;
	  }

	  /// <summary>
	  /// Checks that the argument is within the range defined by {@code low <= x <= high}.
	  /// <para>
	  /// Given a value, this returns true if it is within the specified range including both boundaries.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.amount = ArgChecker.inRangeInclusive(amount, 0d, 1d, "amount");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the value to check </param>
	  /// <param name="lowInclusive">  the low value of the range </param>
	  /// <param name="highInclusive">  the high value of the range </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument} </returns>
	  /// <exception cref="IllegalArgumentException"> if the argument is outside the valid range </exception>
	  public static double inRangeInclusive(double argument, double lowInclusive, double highInclusive, string name)
	  {
		if (argument < lowInclusive || argument > highInclusive)
		{
		  throw new System.ArgumentException(Messages.format("Expected {} <= '{}' <= {}, but found {}", lowInclusive, name, highInclusive, argument));
		}
		return argument;
	  }

	  /// <summary>
	  /// Checks that the argument is within the range defined by {@code low < x < high}.
	  /// <para>
	  /// Given a value, this returns true if it is within the specified range excluding both boundaries.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.amount = ArgChecker.inRangeExclusive(amount, 0d, 1d, "amount");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the value to check </param>
	  /// <param name="lowExclusive">  the low value of the range </param>
	  /// <param name="highExclusive">  the high value of the range </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument} </returns>
	  /// <exception cref="IllegalArgumentException"> if the argument is outside the valid range </exception>
	  public static double inRangeExclusive(double argument, double lowExclusive, double highExclusive, string name)
	  {
		if (argument <= lowExclusive || argument >= highExclusive)
		{
		  throw new System.ArgumentException(Messages.format("Expected {} < '{}' < {}, but found {}", lowExclusive, name, highExclusive, argument));
		}
		return argument;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks that the argument is within the range defined by {@code low <= x < high}.
	  /// <para>
	  /// Given a value, this returns true if it is within the specified range including the
	  /// lower boundary but excluding the upper boundary.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.amount = ArgChecker.inRange(amount, 0d, 1d, "amount");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the value to check </param>
	  /// <param name="lowInclusive">  the low value of the range </param>
	  /// <param name="highExclusive">  the high value of the range </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument} </returns>
	  /// <exception cref="IllegalArgumentException"> if the argument is outside the valid range </exception>
	  public static int inRange(int argument, int lowInclusive, int highExclusive, string name)
	  {
		if (argument < lowInclusive || argument >= highExclusive)
		{
		  throw new System.ArgumentException(Messages.format("Expected {} <= '{}' < {}, but found {}", lowInclusive, name, highExclusive, argument));
		}
		return argument;
	  }

	  /// <summary>
	  /// Checks that the argument is within the range defined by {@code low <= x <= high}.
	  /// <para>
	  /// Given a value, this returns true if it is within the specified range including both boundaries.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.amount = ArgChecker.inRangeInclusive(amount, 0d, 1d, "amount");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the value to check </param>
	  /// <param name="lowInclusive">  the low value of the range </param>
	  /// <param name="highInclusive">  the high value of the range </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument} </returns>
	  /// <exception cref="IllegalArgumentException"> if the argument is outside the valid range </exception>
	  public static int inRangeInclusive(int argument, int lowInclusive, int highInclusive, string name)
	  {
		if (argument < lowInclusive || argument > highInclusive)
		{
		  throw new System.ArgumentException(Messages.format("Expected {} <= '{}' <= {}, but found {}", lowInclusive, name, highInclusive, argument));
		}
		return argument;
	  }

	  /// <summary>
	  /// Checks that the argument is within the range defined by {@code low < x < high}.
	  /// <para>
	  /// Given a value, this returns true if it is within the specified range excluding both boundaries.
	  /// For example, in a constructor:
	  /// <pre>
	  ///  this.amount = ArgChecker.inRangeExclusive(amount, 0d, 1d, "amount");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="argument">  the value to check </param>
	  /// <param name="lowExclusive">  the low value of the range </param>
	  /// <param name="highExclusive">  the high value of the range </param>
	  /// <param name="name">  the name of the argument to use in the error message, not null </param>
	  /// <returns> the input {@code argument} </returns>
	  /// <exception cref="IllegalArgumentException"> if the argument is outside the valid range </exception>
	  public static int inRangeExclusive(int argument, int lowExclusive, int highExclusive, string name)
	  {
		if (argument <= lowExclusive || argument >= highExclusive)
		{
		  throw new System.ArgumentException(Messages.format("Expected {} < '{}' < {}, but found {}", lowExclusive, name, highExclusive, argument));
		}
		return argument;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks that the two values are in order and not equal.
	  /// <para>
	  /// Given two comparable instances, this checks that the first is "less than" the second.
	  /// Two equal values also throw the exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type </param>
	  /// <param name="obj1">  the first object, null throws an exception </param>
	  /// <param name="obj2">  the second object, null throws an exception </param>
	  /// <param name="name1">  the first argument name, not null </param>
	  /// <param name="name2">  the second argument name, not null </param>
	  /// <exception cref="IllegalArgumentException"> if either input is null or they are not in order </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public static <T> void inOrderNotEqual(Comparable<? super T> obj1, T obj2, String name1, String name2)
	  public static void inOrderNotEqual<T, T1>(IComparable<T1> obj1, T obj2, string name1, string name2)
	  {
		notNull(obj1, name1);
		notNull(obj2, name2);
		if (obj1.CompareTo(obj2) >= 0)
		{
		  throw new System.ArgumentException(Messages.format("Invalid order: Expected '{}' < '{}', but found: '{}' >= '{}", name1, name2, obj1, obj2));
		}
	  }

	  /// <summary>
	  /// Checks that the two values are in order or equal.
	  /// <para>
	  /// Given two comparable instances, this checks that the first is "less than" or "equal to" the second.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type </param>
	  /// <param name="obj1">  the first object, null throws an exception </param>
	  /// <param name="obj2">  the second object, null throws an exception </param>
	  /// <param name="name1">  the first argument name, not null </param>
	  /// <param name="name2">  the second argument name, not null </param>
	  /// <exception cref="IllegalArgumentException"> if either input is null or they are not in order </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public static <T> void inOrderOrEqual(Comparable<? super T> obj1, T obj2, String name1, String name2)
	  public static void inOrderOrEqual<T, T1>(IComparable<T1> obj1, T obj2, string name1, string name2)
	  {
		notNull(obj1, name1);
		notNull(obj2, name2);
		if (obj1.CompareTo(obj2) > 0)
		{
		  throw new System.ArgumentException(Messages.format("Invalid order: Expected '{}' <= '{}', but found: '{}' > '{}", name1, name2, obj1, obj2));
		}
	  }

	}

}