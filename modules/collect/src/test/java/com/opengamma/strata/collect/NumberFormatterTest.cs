using System;
using System.Threading;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="NumberFormatter"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NumberFormatterTest
	public class NumberFormatterTest
	{

	  private static readonly string NAN = DecimalFormatSymbols.getInstance(Locale.ENGLISH).NaN;
	  private static readonly string INF = DecimalFormatSymbols.getInstance(Locale.ENGLISH).Infinity;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "standard") Object[][] data_standard()
	  internal virtual object[][] data_standard()
	  {
		return new object[][]
		{
			new object[] {true, 0, 0, 123, "123", 123d},
			new object[] {true, 0, 0, 12345.678, "12,346", 12346d},
			new object[] {true, 0, 0, 12345678.9, "12,345,679", 12345679d},
			new object[] {false, 0, 0, 123, "123", 123},
			new object[] {false, 0, 0, 12345.678, "12346", 12346d},
			new object[] {false, 0, 0, 12345678.9, "12345679", 12345679d},
			new object[] {true, 1, 1, 123, "123.0", 123d},
			new object[] {true, 1, 1, 12345.678, "12,345.7", 12345.7d},
			new object[] {true, 1, 1, 12345678.9, "12,345,678.9", 12345678.9d},
			new object[] {true, 1, 3, 123, "123.0", 123d},
			new object[] {true, 1, 3, 12345.678, "12,345.678", 12345.678d},
			new object[] {true, 1, 3, 12345678.9, "12,345,678.9", 12345678.9d},
			new object[] {true, 1, 3, 12345678.91, "12,345,678.91", 12345678.91d},
			new object[] {true, 0, 3, -12345.67d, "-12,345.67", -12345.67d},
			new object[] {true, 0, 3, -12345.67e30, "-12,345,670,000,000,000,000,000,000,000,000,000", -12345.67e30},
			new object[] {true, 0, 3, -0d, "-0", -0d},
			new object[] {true, 0, 3, Double.NaN, NAN, Double.NaN},
			new object[] {true, 0, 3, double.PositiveInfinity, INF, double.PositiveInfinity},
			new object[] {true, 0, 3, double.NegativeInfinity, "-" + INF, double.NegativeInfinity}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "standard") public void test_of_3arg(boolean grouping, int minDp, int maxDp, double value, String expected, double parsed)
	  public virtual void test_of_3arg(bool grouping, int minDp, int maxDp, double value, string expected, double parsed)
	  {
		string text = NumberFormatter.of(grouping, minDp, maxDp).format(value);
		assertEquals(text, expected);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "standard") public void test_of_2arg(boolean grouping, int minDp, int maxDp, double value, String expected, double parsed)
	  public virtual void test_of_2arg(bool grouping, int minDp, int maxDp, double value, string expected, double parsed)
	  {
		if (minDp == maxDp)
		{
		  string text = NumberFormatter.of(grouping, minDp).format(value);
		  assertEquals(text, expected);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "standard") public void test_parse(boolean grouping, int minDp, int maxDp, double value, String expected, double parsed)
	  public virtual void test_parse(bool grouping, int minDp, int maxDp, double value, string expected, double parsed)
	  {
		NumberFormatter formatter = NumberFormatter.of(grouping, minDp, maxDp);
		string text = formatter.format(value);
		double actual = formatter.parse(text);
		assertEquals(actual, parsed, 0d);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "percentage") Object[][] data_percentage()
	  internal virtual object[][] data_percentage()
	  {
		return new object[][]
		{
			new object[] {true, 0, 0, 1.23, "123%"},
			new object[] {true, 0, 0, 123.45678, "12,346%"},
			new object[] {true, 0, 0, 123456.789, "12,345,679%"},
			new object[] {false, 0, 0, 1.23, "123%"},
			new object[] {false, 0, 0, 123.4578, "12346%"},
			new object[] {false, 0, 0, 123456.789, "12345679%"},
			new object[] {true, 1, 1, 1.23, "123.0%"},
			new object[] {true, 1, 1, 123.45678, "12,345.7%"},
			new object[] {true, 1, 1, 123456.789, "12,345,678.9%"},
			new object[] {true, 1, 3, 1.23, "123.0%"},
			new object[] {true, 1, 3, 123.45678, "12,345.678%"},
			new object[] {true, 1, 3, 123456.789, "12,345,678.9%"},
			new object[] {true, 1, 3, 123456.7891, "12,345,678.91%"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "percentage") public void test_ofPercentage(boolean grouping, int minDp, int maxDp, double value, String expected)
	  public virtual void test_ofPercentage(bool grouping, int minDp, int maxDp, double value, string expected)
	  {
		string text = NumberFormatter.ofPercentage(grouping, minDp, maxDp).format(value);
		assertEquals(text, expected);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "patterns") Object[][] data_patterns()
	  internal virtual object[][] data_patterns()
	  {
		return new object[][]
		{
			new object[] {"0", 12345.678, "12346"},
			new object[] {"00", 12345.678, "12346"},
			new object[] {"#,##0", 12345.678, "12,346"},
			new object[] {"#,##0.00", 12345, "12,345.00"},
			new object[] {"#,##0.00", 12345.6, "12,345.60"},
			new object[] {"#,##0.00", 12345.678, "12,345.68"},
			new object[] {"#,##0.##", 12345, "12,345"},
			new object[] {"#,##0.##", 12345.6, "12,345.6"},
			new object[] {"#,##0.##", 12345.678, "12,345.68"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "patterns") public void test_ofPattern(String pattern, double value, String expected)
	  public virtual void test_ofPattern(string pattern, double value, string expected)
	  {
		string java = (new DecimalFormat(pattern, DecimalFormatSymbols.getInstance(Locale.ENGLISH))).format(value);
		string strata = NumberFormatter.ofPattern(pattern, Locale.ENGLISH).format(value);
		assertEquals(strata, java);
		assertEquals(strata, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_ofLocalizedNumber()
	  {
		string text = NumberFormatter.ofLocalizedNumber(Locale.ENGLISH).format(12345.678);
		assertEquals(text, "12,345.678");
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(enabled = false) public void test_javaBroken() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void test_javaBroken()
	  {
		// uncomment system out to see how broken it is
		// very specific format instance needed
		DecimalFormat format = new DecimalFormat("#,##0.###", new DecimalFormatSymbols(Locale.ENGLISH));
		Random random = new Random(1);
		System.Threading.CountdownEvent latch = new System.Threading.CountdownEvent(1);
		AtomicInteger broken = new AtomicInteger();
		int threadCount = 15;
		for (int i = 0; i < threadCount; i++)
		{
		  ThreadStart runner = () =>
		  {
		try
		{
		  latch.await();
		  int val = random.Next(999);
		  string a = format.format((double) val);
		  string b = Convert.ToInt32(val).ToString();
		  Console.WriteLine(a + " " + b);
		  if (!a.Equals(b))
		  {
			broken.incrementAndGet();
		  }
		}
		catch (Exception ex)
		{
		  Console.WriteLine("Exception: " + ex.Message);
		}
		  };
		  (new Thread(runner, "TestThread" + i)).Start();
		}
		// start all threads together
		latch.Signal();
		Thread.Sleep(1000);
		Console.WriteLine("Broken: " + broken.get());
		assertTrue(broken.get() > 0);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(enabled = false) public void test_performance() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void test_performance()
	  {
		ThreadLocal<DecimalFormat> thread = ThreadLocal.withInitial(() => new DecimalFormat("#,##0.###", new DecimalFormatSymbols(Locale.ENGLISH)));
		DecimalFormat java = new DecimalFormat("#,##0.###", new DecimalFormatSymbols(Locale.ENGLISH));
		NumberFormatter strata = NumberFormatter.of(true, 0, 3);
		Random random = new Random(1);

		for (int i = 0; i < 20; i++)
		{
		  long start0 = System.nanoTime();
		  for (int j = 0; j < 100_000; j++)
		  {
			double val = random.NextDouble();
			string str = java.format(val);
			if (str.Length == 0)
			{
			  throw new System.InvalidOperationException("Just to avoid dead code elimination: " + str);
			}
		  }
		  long end0 = System.nanoTime();
		  Console.WriteLine("  Java: " + ((end0 - start0) / 1_000_000d) + "ms");

		  long start1 = System.nanoTime();
		  for (int j = 0; j < 100_000; j++)
		  {
			double val = random.NextDouble();
			string str = thread.get().format(val);
			if (str.Length == 0)
			{
			  throw new System.InvalidOperationException("Just to avoid dead code elimination: " + str);
			}
		  }
		  long end1 = System.nanoTime();
		  Console.WriteLine("JavaTL: " + ((end1 - start1) / 1_000_000d) + "ms");

		  long start1b = System.nanoTime();
		  for (int j = 0; j < 100_000; j++)
		  {
			double val = random.NextDouble();
			string str = ((NumberFormat) java.clone()).format(val);
			if (str.Length == 0)
			{
			  throw new System.InvalidOperationException("Just to avoid dead code elimination: " + str);
			}
		  }
		  long end1b = System.nanoTime();
		  Console.WriteLine("JavaCl: " + ((end1b - start1b) / 1_000_000d) + "ms");

		  long start2 = System.nanoTime();
		  for (int j = 0; j < 100_000; j++)
		  {
			double val = random.NextDouble();
			string str = strata.format(val);
			if (str.Length == 0)
			{
			  throw new System.InvalidOperationException("Just to avoid dead code elimination: " + str);
			}
		  }
		  long end2 = System.nanoTime();
		  Console.WriteLine("Strata: " + ((end2 - start2) / 1_000_000d) + "ms");
		}
	  }

	}

}