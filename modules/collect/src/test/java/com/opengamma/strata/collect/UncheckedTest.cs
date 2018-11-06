using System;
using System.Threading;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertUtilityClass;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.fail;


	using Test = org.testng.annotations.Test;

	using CheckedRunnable = com.opengamma.strata.collect.function.CheckedRunnable;
	using CheckedSupplier = com.opengamma.strata.collect.function.CheckedSupplier;

	/// <summary>
	/// Test Unchecked.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class UncheckedTest
	public class UncheckedTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_wrap_runnable1()
	  {
		// cannot use assertThrows() here
		try
		{
		  Unchecked.wrap((CheckedRunnable)() =>
		  {
		  throw new IOException();
		  });
		  fail();
		}
		catch (UncheckedIOException)
		{
		  // success
		}
	  }

	  public virtual void test_wrap_runnable2()
	  {
		// cannot use assertThrows() here
		try
		{
		  Unchecked.wrap((CheckedRunnable)() =>
		  {
		  throw new Exception();
		  });
		  fail();
		}
		catch (Exception)
		{
		  // success
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_wrap_supplier()
	  {
		// cannot use assertThrows() here
		try
		{
		  Unchecked.wrap((CheckedSupplier<string>)() =>
		  {
		  throw new IOException();
		  });
		  fail();
		}
		catch (UncheckedIOException)
		{
		  // success
		}
	  }

	  public virtual void test_wrap_supplier2()
	  {
		// cannot use assertThrows() here
		try
		{
		  Unchecked.wrap((CheckedSupplier<string>)() =>
		  {
		  throw new Exception();
		  });
		  fail();
		}
		catch (Exception)
		{
		  // success
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_runnable_fail1()
	  {
		ThreadStart a = Unchecked.runnable(() =>
		{
		throw new IOException();
		});
		assertThrows(() => a.run(), typeof(UncheckedIOException));
	  }

	  public virtual void test_runnable_fail2()
	  {
		ThreadStart a = Unchecked.runnable(() =>
		{
		throw new Exception();
		});
		assertThrows(() => a.run(), typeof(Exception));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_function_success()
	  {
		System.Func<string, string> a = Unchecked.function((t) => t);
		assertEquals(a("A"), "A");
	  }

	  public virtual void test_function_fail1()
	  {
		System.Func<string, string> a = Unchecked.function((t) =>
		{
		throw new IOException();
		});
		assertThrows(() => a("A"), typeof(UncheckedIOException));
	  }

	  public virtual void test_function_fail2()
	  {
		System.Func<string, string> a = Unchecked.function((t) =>
		{
		throw new Exception();
		});
		assertThrows(() => a("A"), typeof(Exception));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_biFunction_success()
	  {
		System.Func<string, string, string> a = Unchecked.biFunction((t, u) => t + u);
		assertEquals(a("A", "B"), "AB");
	  }

	  public virtual void test_biFunction_fail1()
	  {
		System.Func<string, string, string> a = Unchecked.biFunction((t, u) =>
		{
		throw new IOException();
		});
		assertThrows(() => a("A", "B"), typeof(UncheckedIOException));
	  }

	  public virtual void test_biFunction_fail2()
	  {
		System.Func<string, string, string> a = Unchecked.biFunction((t, u) =>
		{
		throw new Exception();
		});
		assertThrows(() => a("A", "B"), typeof(Exception));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_unaryOperator_success()
	  {
		System.Func<string, string> a = Unchecked.unaryOperator((t) => t);
		assertEquals(a("A"), "A");
	  }

	  public virtual void test_unaryOperator_fail1()
	  {
		System.Func<string, string> a = Unchecked.unaryOperator((t) =>
		{
		throw new IOException();
		});
		assertThrows(() => a("A"), typeof(UncheckedIOException));
	  }

	  public virtual void test_unaryOperator_fail2()
	  {
		System.Func<string, string> a = Unchecked.unaryOperator((t) =>
		{
		throw new Exception();
		});
		assertThrows(() => a("A"), typeof(Exception));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_binaryOperator_success()
	  {
		System.Func<string, string, string> a = Unchecked.binaryOperator((t, u) => t + u);
		assertEquals(a("A", "B"), "AB");
	  }

	  public virtual void test_binaryOperator_fail1()
	  {
		System.Func<string, string, string> a = Unchecked.binaryOperator((t, u) =>
		{
		throw new IOException();
		});
		assertThrows(() => a("A", "B"), typeof(UncheckedIOException));
	  }

	  public virtual void test_binaryOperator_fail2()
	  {
		System.Func<string, string, string> a = Unchecked.binaryOperator((t, u) =>
		{
		throw new Exception();
		});
		assertThrows(() => a("A", "B"), typeof(Exception));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_predicate_success()
	  {
		System.Predicate<string> a = Unchecked.predicate((t) => true);
		assertEquals(a("A"), true);
	  }

	  public virtual void test_predicate_fail1()
	  {
		System.Predicate<string> a = Unchecked.predicate((t) =>
		{
		throw new IOException();
		});
		assertThrows(() => a("A"), typeof(UncheckedIOException));
	  }

	  public virtual void test_predicate_fail2()
	  {
		System.Predicate<string> a = Unchecked.predicate((t) =>
		{
		throw new Exception();
		});
		assertThrows(() => a("A"), typeof(Exception));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_biPredicate_success()
	  {
		System.Func<string, string, bool> a = Unchecked.biPredicate((t, u) => true);
		assertEquals(a("A", "B"), true);
	  }

	  public virtual void test_biPredicate_fail1()
	  {
		System.Func<string, string, bool> a = Unchecked.biPredicate((t, u) =>
		{
		throw new IOException();
		});
		assertThrows(() => a("A", "B"), typeof(UncheckedIOException));
	  }

	  public virtual void test_biPredicate_fail2()
	  {
		System.Func<string, string, bool> a = Unchecked.biPredicate((t, u) =>
		{
		throw new Exception();
		});
		assertThrows(() => a("A", "B"), typeof(Exception));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_consumer_success()
	  {
		System.Action<string> a = Unchecked.consumer((t) =>
		{
		});
		a("A");
	  }

	  public virtual void test_consumer_fail1()
	  {
		System.Action<string> a = Unchecked.consumer((t) =>
		{
		throw new IOException();
		});
		assertThrows(() => a("A"), typeof(UncheckedIOException));
	  }

	  public virtual void test_consumer_fail2()
	  {
		System.Action<string> a = Unchecked.consumer((t) =>
		{
		throw new Exception();
		});
		assertThrows(() => a("A"), typeof(Exception));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_biConsumer_success()
	  {
		System.Action<string, string> a = Unchecked.biConsumer((t, u) =>
		{
		});
		a("A", "B");
	  }

	  public virtual void test_biConsumer_fail1()
	  {
		System.Action<string, string> a = Unchecked.biConsumer((t, u) =>
		{
		throw new IOException();
		});
		assertThrows(() => a("A", "B"), typeof(UncheckedIOException));
	  }

	  public virtual void test_biConsumer_fail2()
	  {
		System.Action<string, string> a = Unchecked.biConsumer((t, u) =>
		{
		throw new Exception();
		});
		assertThrows(() => a("A", "B"), typeof(Exception));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_supplier_success()
	  {
		System.Func<string> a = Unchecked.supplier(() => "A");
		assertEquals(a(), "A");
	  }

	  public virtual void test_supplier_fail1()
	  {
		System.Func<string> a = Unchecked.supplier(() =>
		{
		throw new IOException();
		});
		assertThrows(() => a(), typeof(UncheckedIOException));
	  }

	  public virtual void test_supplier_fail2()
	  {
		System.Func<string> a = Unchecked.supplier(() =>
		{
		throw new Exception();
		});
		assertThrows(() => a(), typeof(Exception));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_validUtilityClass()
	  {
		assertUtilityClass(typeof(Unchecked));
	  }

	  public virtual void test_propagate()
	  {
		Exception error = new Exception("a");
		System.ArgumentException argEx = new System.ArgumentException("b");
		IOException ioEx = new IOException("c");
		URISyntaxException namingEx = new URISyntaxException("d", "e");

		// use old-style try-catch to ensure test really working
		try
		{
		  Unchecked.propagate(error);
		  fail();
		}
		catch (Exception ex)
		{
		  assertSame(ex, error);
		}
		try
		{
		  Unchecked.propagate(argEx);
		  fail();
		}
		catch (System.ArgumentException ex)
		{
		  assertSame(ex, argEx);
		}
		try
		{
		  Unchecked.propagate(ioEx);
		  fail();
		}
		catch (UncheckedIOException ex)
		{
		  assertEquals(ex.GetType(), typeof(UncheckedIOException));
		  assertSame(ex.InnerException, ioEx);
		}
		try
		{
		  Unchecked.propagate(namingEx);
		  fail();
		}
		catch (Exception ex)
		{
		  assertEquals(ex.GetType(), typeof(Exception));
		  assertSame(ex.InnerException, namingEx);
		}

		try
		{
		  Unchecked.propagate(new InvocationTargetException(error));
		  fail();
		}
		catch (Exception ex)
		{
		  assertSame(ex, error);
		}
		try
		{
		  Unchecked.propagate(new InvocationTargetException(argEx));
		  fail();
		}
		catch (System.ArgumentException ex)
		{
		  assertSame(ex, argEx);
		}
		try
		{
		  Unchecked.propagate(new InvocationTargetException(ioEx));
		  fail();
		}
		catch (UncheckedIOException ex)
		{
		  assertEquals(ex.GetType(), typeof(UncheckedIOException));
		  assertSame(ex.InnerException, ioEx);
		}
		try
		{
		  Unchecked.propagate(new InvocationTargetException(namingEx));
		  fail();
		}
		catch (Exception ex)
		{
		  assertEquals(ex.GetType(), typeof(Exception));
		  assertSame(ex.InnerException, namingEx);
		}
	  }

	}

}