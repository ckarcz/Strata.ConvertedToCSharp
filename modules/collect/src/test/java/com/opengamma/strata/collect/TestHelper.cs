using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.IO;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertNotNull;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.fail;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using BeanAssert = org.joda.beans.test.BeanAssert;
	using JodaBeanTests = org.joda.beans.test.JodaBeanTests;
	using StringConvert = org.joda.convert.StringConvert;

	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// Test helper.
	/// <para>
	/// Provides additional classes to help with testing.
	/// </para>
	/// </summary>
	public class TestHelper
	{

	  /// <summary>
	  /// UTF-8 encoding name.
	  /// </summary>
	  private const string UTF_8 = "UTF-8";

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an empty {@code ImmutableList}, intended for static import.
	  /// </summary>
	  /// <returns> the list </returns>
	  public static ImmutableList<T> list<T>()
	  {
		return ImmutableList.of();
	  }

	  /// <summary>
	  /// Creates an {@code ImmutableList}, intended for static import.
	  /// </summary>
	  /// <param name="item0">  the item </param>
	  /// <returns> the list </returns>
	  public static ImmutableList<T> list<T>(T item0)
	  {
		return ImmutableList.of(item0);
	  }

	  /// <summary>
	  /// Creates an {@code ImmutableList}, intended for static import.
	  /// </summary>
	  /// <param name="item0">  the item </param>
	  /// <param name="item1">  the item </param>
	  /// <returns> the list </returns>
	  public static ImmutableList<T> list<T>(T item0, T item1)
	  {
		return ImmutableList.of(item0, item1);
	  }

	  /// <summary>
	  /// Creates an {@code ImmutableList}, intended for static import.
	  /// </summary>
	  /// <param name="item0">  the item </param>
	  /// <param name="item1">  the item </param>
	  /// <param name="item2">  the item </param>
	  /// <returns> the list </returns>
	  public static ImmutableList<T> list<T>(T item0, T item1, T item2)
	  {
		return ImmutableList.of(item0, item1, item2);
	  }

	  /// <summary>
	  /// Creates an {@code ImmutableList}, intended for static import.
	  /// </summary>
	  /// <param name="item0">  the item </param>
	  /// <param name="item1">  the item </param>
	  /// <param name="item2">  the item </param>
	  /// <param name="item3">  the item </param>
	  /// <returns> the list </returns>
	  public static ImmutableList<T> list<T>(T item0, T item1, T item2, T item3)
	  {
		return ImmutableList.of(item0, item1, item2, item3);
	  }

	  /// <summary>
	  /// Creates an {@code ImmutableList}, intended for static import.
	  /// </summary>
	  /// <param name="item0">  the item </param>
	  /// <param name="item1">  the item </param>
	  /// <param name="item2">  the item </param>
	  /// <param name="item3">  the item </param>
	  /// <param name="item4">  the item </param>
	  /// <returns> the list </returns>
	  public static ImmutableList<T> list<T>(T item0, T item1, T item2, T item3, T item4)
	  {
		return ImmutableList.of(item0, item1, item2, item3, item4);
	  }

	  /// <summary>
	  /// Creates an {@code ImmutableList}, intended for static import.
	  /// </summary>
	  /// <param name="list">  the list </param>
	  /// <returns> the list </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SafeVarargs public static <T> com.google.common.collect.ImmutableList<T> list(T... list)
	  public static ImmutableList<T> list<T>(params T[] list)
	  {
		return ImmutableList.copyOf(list);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a {@code LocalDate}, intended for static import.
	  /// </summary>
	  /// <param name="year">  the year </param>
	  /// <param name="month">  the month </param>
	  /// <param name="dayOfMonth">  the day of month </param>
	  /// <returns> the date </returns>
	  public static LocalDate date(int year, int month, int dayOfMonth)
	  {
		return LocalDate.of(year, month, dayOfMonth);
	  }

	  /// <summary>
	  /// Creates a {@code LocalDate}, intended for static import.
	  /// </summary>
	  /// <param name="year">  the year </param>
	  /// <param name="month">  the month </param>
	  /// <param name="dayOfMonth">  the day of month </param>
	  /// <returns> the date </returns>
	  public static LocalDate date(int year, Month month, int dayOfMonth)
	  {
		return LocalDate.of(year, month, dayOfMonth);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a {@code ZonedDateTime} from the date.
	  /// <para>
	  /// The time is start of day and the zone is UTC.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="year">  the year </param>
	  /// <param name="month">  the month </param>
	  /// <param name="dayOfMonth">  the day of month </param>
	  /// <returns> the date-time, representing the date at midnight UTC </returns>
	  public static ZonedDateTime dateUtc(int year, int month, int dayOfMonth)
	  {
		return LocalDate.of(year, month, dayOfMonth).atStartOfDay(ZoneOffset.UTC);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Asserts that two beans are equal.
	  /// Provides better error messages than a normal {@code assertEquals} comparison.
	  /// </summary>
	  /// <param name="actual">  the actual bean under test </param>
	  /// <param name="expected">  the expected bean </param>
	  public static void assertEqualsBean(Bean actual, Bean expected)
	  {
		BeanAssert.assertBeanEquals(expected, actual);
	  }

	  /// <summary>
	  /// Asserts that two beans are equal.
	  /// Provides better error messages than a normal {@code assertEquals} comparison.
	  /// <para>
	  /// This provides extra detail used when debugging an issue.
	  /// Normal use should be to call <seealso cref="#assertEqualsBean(Bean, Bean)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="actual">  the actual bean under test </param>
	  /// <param name="expected">  the expected bean </param>
	  public static void assertEqualsBeanDetailed(Bean actual, Bean expected)
	  {
		BeanAssert.assertBeanEqualsFullDetail(expected, actual);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Asserts that the object can be serialized and deserialized to an equal form.
	  /// </summary>
	  /// <param name="base">  the object to be tested </param>
	  public static void assertSerialization(object @base)
	  {
		assertNotNull(@base);
		try
		{
		  using (MemoryStream baos = new MemoryStream())
		  {
			using (ObjectOutputStream oos = new ObjectOutputStream(baos))
			{
			  oos.writeObject(@base);
			  oos.close();
			  using (MemoryStream bais = new MemoryStream(baos.toByteArray()))
			  {
				using (ObjectInputStream ois = new ObjectInputStream(bais))
				{
				  assertEquals(ois.readObject(), @base);
				}
			  }
			}
		  }
		}
		catch (Exception ex) when (ex is IOException || ex is ClassNotFoundException)
		{
		  throw new Exception(ex);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Asserts that the object can be serialized and deserialized via a string using Joda-Convert.
	  /// </summary>
	  /// @param <T>  the type </param>
	  /// <param name="cls">  the effective type </param>
	  /// <param name="base">  the object to be tested </param>
	  public static void assertJodaConvert<T>(Type<T> cls, object @base)
	  {
		assertNotNull(@base);
		StringConvert convert = StringConvert.create();
		string str = convert.convertToString(@base);
		T result = convert.convertFromString(cls, str);
		assertEquals(result, @base);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Asserts that the lambda-based code throws the specified exception.
	  /// <para>
	  /// For example:
	  /// <pre>
	  ///  assertThrows(() -> bean.property(""), NoSuchElementException.class);
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="runner">  the lambda containing the code to test </param>
	  /// <param name="expected">  the expected exception </param>
	  public static void assertThrows(AssertRunnable runner, Type expected)
	  {
		assertThrowsImpl(runner, expected, null);
	  }

	  /// <summary>
	  /// Asserts that the lambda-based code throws the specified exception
	  /// and that the exception message matches the supplied regular
	  /// expression.
	  /// <para>
	  /// For example:
	  /// <pre>
	  ///  assertThrows(() -> bean.property(""), NoSuchElementException.class, "Unknown property.*");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="runner">  the lambda containing the code to test </param>
	  /// <param name="expected">  the expected exception </param>
	  /// <param name="regex">  the regex that the exception message is expected to match </param>
	  public static void assertThrows(AssertRunnable runner, Type expected, string regex)
	  {
		assertNotNull(regex, "assertThrows() called with null regex");
		assertThrowsImpl(runner, expected, regex);
	  }

	  private static void assertThrowsImpl(AssertRunnable runner, Type expected, string regex)
	  {

		assertNotNull(runner, "assertThrows() called with null AssertRunnable");
		assertNotNull(expected, "assertThrows() called with null expected Class");

		try
		{
		  runner();
		  fail("Expected " + expected.Name + " but code succeeded normally");
		}
		catch (AssertionError ex)
		{
		  throw ex;
		}
		catch (Exception ex)
		{
		  if (expected.IsInstanceOfType(ex))
		  {
			string message = ex.Message;
			if (string.ReferenceEquals(regex, null) || message.matches(regex))
			{
			  return;
			}
			else
			{
			  fail("Expected exception message to match: [" + regex + "] but received: " + message);
			}
		  }
		  fail("Expected " + expected.Name + " but received " + ex.GetType().Name, ex);
		}
	  }

	  /// <summary>
	  /// Asserts that the lambda-based code throws an {@code RuntimeException}.
	  /// <para>
	  /// For example:
	  /// <pre>
	  ///  assertThrowsRuntime(() -> new Foo(null));
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="runner">  the lambda containing the code to test </param>
	  public static void assertThrowsRuntime(AssertRunnable runner)
	  {
		assertThrows(runner, typeof(Exception));
	  }

	  /// <summary>
	  /// Asserts that the lambda-based code throws an {@code IllegalArgumentException}.
	  /// <para>
	  /// For example:
	  /// <pre>
	  ///  assertThrowsIllegalArg(() -> new Foo(null));
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="runner">  the lambda containing the code to test </param>
	  public static void assertThrowsIllegalArg(AssertRunnable runner)
	  {
		assertThrows(runner, typeof(System.ArgumentException));
	  }

	  /// <summary>
	  /// Asserts that the lambda-based code throws an {@code IllegalArgumentException} and checks the message
	  /// matches an regex.
	  /// <para>
	  /// For example:
	  /// <pre>
	  ///  assertThrowsIllegalArg(() -> new Foo(null), "Foo constructor argument must not be null");
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="runner">  the lambda containing the code to test </param>
	  /// <param name="regex">  regular expression that must match the exception message </param>
	  public static void assertThrowsIllegalArg(AssertRunnable runner, string regex)
	  {
		assertThrows(runner, typeof(System.ArgumentException), regex);
	  }

	  /// <summary>
	  /// Asserts that the lambda-based code throws an exception
	  /// and that the cause of the exception is the supplied cause.
	  /// <para>
	  /// For example:
	  /// <pre>
	  ///  assertThrowsWithCause(() ->
	  ///    executeSql("INSERT DATA THAT ALREADY EXISTS"), SQLIntegrityConstraintViolationException.class);
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="runner">  the lambda containing the code to test </param>
	  /// <param name="cause">  the expected cause of the exception thrown </param>
	  public static void assertThrowsWithCause(AssertRunnable runner, Type cause)
	  {
		assertNotNull(runner, "assertThrowsWithCause() called with null AssertRunnable");
		assertNotNull(cause, "assertThrowsWithCause() called with null expected cause");

		try
		{
		  runner();
		  fail("Expected " + cause.Name + " but code succeeded normally");
		}
		catch (AssertionError ex)
		{
		  throw ex;
		}
		catch (Exception ex)
		{
		  Exception ex2 = ex;
		  while (ex2 != null && !cause.IsInstanceOfType(ex2))
		  {
			ex2 = ex2.InnerException;
		  }

		  if (ex2 == null)
		  {
			fail("Expected cause of exception to be: " + cause.Name + " but got different exception", ex);
		  }
		}
	  }

	  /// <summary>
	  /// Ignore any exception thrown by the lambda-based code.
	  /// <para>
	  /// For example:
	  /// <pre>
	  ///  ignoreThrows(() -> bean.property(""));
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="runner">  the lambda containing the code to test </param>
	  public static void ignoreThrows(AssertRunnable runner)
	  {
		assertNotNull(runner, "ignoreThrows() called with null AssertRunnable");
		try
		{
		  runner();
		}
		catch (Exception)
		{
		  // ignore
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Capture system out for testing.
	  /// <para>
	  /// This returns the output from calls to {@code System.out}.
	  /// This is thread-safe, providing that no other utility alters system out.
	  /// </para>
	  /// <para>
	  /// For example:
	  /// <pre>
	  ///  String sysOut = captureSystemOut(() -> myCode);
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="runner">  the lambda containing the code to test </param>
	  /// <returns> the captured output </returns>
	  public static string caputureSystemOut(ThreadStart runner)
	  {
		  lock (typeof(TestHelper))
		  {
			// it would be possible to use some form of thread-local PrintStream to increase concurrency,
			// but that should be done only if synchronized is insufficient
			assertNotNull(runner, "caputureSystemOut() called with null Runnable");
			MemoryStream baos = new MemoryStream(1024);
			PrintStream ps = Unchecked.wrap(() => new PrintStream(baos, false, UTF_8));
			PrintStream old = System.out;
			try
			{
			  System.Out = ps;
			  runner.run();
			  System.out.flush();
			}
			finally
			{
			  System.Out = old;
			}
			return Unchecked.wrap(() => baos.ToString(UTF_8));
		  }
	  }

	  /// <summary>
	  /// Capture system err for testing.
	  /// <para>
	  /// This returns the output from calls to {@code System.err}.
	  /// This is thread-safe, providing that no other utility alters system out.
	  /// </para>
	  /// <para>
	  /// For example:
	  /// <pre>
	  ///  String sysErr = captureSystemErr(() -> myCode);
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="runner">  the lambda containing the code to test </param>
	  /// <returns> the captured output </returns>
	  public static string caputureSystemErr(ThreadStart runner)
	  {
		  lock (typeof(TestHelper))
		  {
			// it would be possible to use some form of thread-local PrintStream to increase concurrency,
			// but that should be done only if synchronized is insufficient
			assertNotNull(runner, "caputureSystemErr() called with null Runnable");
			MemoryStream baos = new MemoryStream(1024);
			PrintStream ps = Unchecked.wrap(() => new PrintStream(baos, false, UTF_8));
			PrintStream old = System.err;
			try
			{
			  System.Err = ps;
			  runner.run();
			  System.err.flush();
			}
			finally
			{
			  System.Err = old;
			}
			return Unchecked.wrap(() => baos.ToString(UTF_8));
		  }
	  }

	  /// <summary>
	  /// Capture log for testing.
	  /// <para>
	  /// This returns the output from calls to the java logger.
	  /// This is thread-safe, providing that no other utility alters the logger.
	  /// </para>
	  /// <para>
	  /// For example:
	  /// <pre>
	  ///  String log = captureLog(Foo.class, () -> myCode);
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="loggerClass">  the class defining the logger to trap </param>
	  /// <param name="runner">  the lambda containing the code to test </param>
	  /// <returns> the captured output </returns>
	  public static IList<LogRecord> caputureLog(Type loggerClass, ThreadStart runner)
	  {
		  lock (typeof(TestHelper))
		  {
			assertNotNull(loggerClass, "caputureLog() called with null Class");
			assertNotNull(runner, "caputureLog() called with null Runnable");
        
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
			Logger logger = Logger.getLogger(loggerClass.FullName);
			LogHandler handler = new LogHandler();
			try
			{
			  handler.Level = Level.ALL;
			  logger.UseParentHandlers = false;
			  logger.addHandler(handler);
			  runner.run();
			  return handler.records;
			}
			finally
			{
			  logger.removeHandler(handler);
			}
		  }
	  }

	  private class LogHandler : Handler
	  {
		internal IList<LogRecord> records = new List<LogRecord>();

		public override void publish(LogRecord record)
		{
		  records.Add(record);
		}

		public override void close()
		{
		}

		public override void flush()
		{
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Asserts that a class is a well-defined utility class.
	  /// <para>
	  /// Must be final and with one zero-arg private constructor.
	  /// All public methods must be static.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="clazz">  the class to test </param>
	  public static void assertUtilityClass(Type clazz)
	  {
		assertNotNull(clazz, "assertUtilityClass() called with null class");
		assertTrue(Modifier.isFinal(clazz.Modifiers), "Utility class must be final");
		assertEquals(clazz.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).length, 1, "Utility class must have one constructor");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: Constructor<?> con = clazz.getDeclaredConstructors()[0];
		System.Reflection.ConstructorInfo<object> con = clazz.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)[0];
		assertEquals(con.ParameterTypes.length, 0, "Utility class must have zero-arg constructor");
		assertTrue(Modifier.isPrivate(con.Modifiers), "Utility class must have private constructor");
		foreach (System.Reflection.MethodInfo method in clazz.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
		{
		  if (Modifier.isPublic(method.Modifiers))
		  {
			assertTrue(Modifier.isStatic(method.Modifiers), "Utility class public methods must be static");
		  }
		}
		// coverage
		ignoreThrows(() =>
		{
		con.Accessible = true;
		con.newInstance();
		});
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Test a private no-arg constructor the primary purpose of increasing test coverage.
	  /// </summary>
	  /// <param name="clazz">  the class to test </param>
	  public static void coverPrivateConstructor(Type clazz)
	  {
		assertNotNull(clazz, "coverPrivateConstructor() called with null class");
		AtomicBoolean isPrivate = new AtomicBoolean(false);
		ignoreThrows(() =>
		{
		System.Reflection.ConstructorInfo<object> con = clazz.DeclaredConstructor;
		isPrivate.set(Modifier.isPrivate(con.Modifiers));
		con.Accessible = true;
		con.newInstance();
		});
		assertTrue(isPrivate.get(), "No-arg constructor must be private");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Test an enum for the primary purpose of increasing test coverage.
	  /// </summary>
	  /// @param <E>  the enum type </param>
	  /// <param name="clazz">  the class to test </param>
	  public static void coverEnum<E>(Type<E> clazz) where E : Enum<E>
	  {
		assertNotNull(clazz, "coverEnum() called with null class");
		ignoreThrows(() =>
		{
		System.Reflection.MethodInfo method = clazz.getDeclaredMethod("values");
		method.Accessible = true;
		method.invoke(null);
		});
		foreach (E val in clazz.EnumConstants)
		{
		  ignoreThrows(() =>
		  {
		  System.Reflection.MethodInfo method = clazz.getDeclaredMethod("valueOf", typeof(string));
		  method.Accessible = true;
		  method.invoke(null, val.name());
		  });
		}
		ignoreThrows(() =>
		{
		System.Reflection.MethodInfo method = clazz.getDeclaredMethod("valueOf", typeof(string));
		method.Accessible = true;
		method.invoke(null, "");
		});
		ignoreThrows(() =>
		{
		System.Reflection.MethodInfo method = clazz.getDeclaredMethod("valueOf", typeof(string));
		method.Accessible = true;
		method.invoke(null, (object) null);
		});
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Test a mutable bean for the primary purpose of increasing test coverage.
	  /// </summary>
	  /// <param name="bean">  the bean to test </param>
	  public static void coverMutableBean(Bean bean)
	  {
		JodaBeanTests.coverMutableBean(bean);
	  }

	  /// <summary>
	  /// Test an immutable bean for the primary purpose of increasing test coverage.
	  /// </summary>
	  /// <param name="bean">  the bean to test </param>
	  public static void coverImmutableBean(ImmutableBean bean)
	  {
		JodaBeanTests.coverImmutableBean(bean);
	  }

	  /// <summary>
	  /// Test a bean equals method for the primary purpose of increasing test coverage.
	  /// <para>
	  /// The two beans passed in should contain a different value for each property.
	  /// The method creates a cross-product to ensure test coverage of equals.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bean1">  the first bean to test </param>
	  /// <param name="bean2">  the second bean to test </param>
	  public static void coverBeanEquals(Bean bean1, Bean bean2)
	  {
		JodaBeanTests.coverBeanEquals(bean1, bean2);
	  }

	}

}