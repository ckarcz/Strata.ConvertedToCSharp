/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertNotNull;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using TestTarget = com.opengamma.strata.calc.runner.CalculationTaskTest.TestTarget;

	/// <summary>
	/// Test <seealso cref="CalculationParameters"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalculationParametersTest
	public class CalculationParametersTest
	{

	  private static readonly CalculationParameter PARAM = new TestParameter();
	  private static readonly CalculationParameter PARAM2 = new TestParameter2();

	  //-------------------------------------------------------------------------
	  public virtual void of()
	  {
		CalculationParameters test = CalculationParameters.of(PARAM);
		assertEquals(test.Parameters.size(), 1);
		assertEquals(test.findParameter(typeof(TestParameter)), PARAM);
	  }

	  public virtual void of_empty()
	  {
		CalculationParameters test = CalculationParameters.of();
		assertEquals(test.Parameters.size(), 0);
	  }

	  public virtual void of_list()
	  {
		CalculationParameters test = CalculationParameters.of(ImmutableList.of(PARAM));
		assertEquals(test.Parameters.size(), 1);
		assertEquals(test.findParameter(typeof(TestParameter)), PARAM);
	  }

	  public virtual void of_list_empty()
	  {
		CalculationParameters test = CalculationParameters.of(ImmutableList.of());
		assertEquals(test.Parameters.size(), 0);
	  }

	  public virtual void getParameter1()
	  {
		CalculationParameters test = CalculationParameters.of(ImmutableList.of(PARAM));
		assertEquals(test.getParameter(typeof(TestParameter)), PARAM);
		assertThrowsIllegalArg(() => test.getParameter(typeof(TestParameter2)));
		assertThrowsIllegalArg(() => test.getParameter(typeof(TestInterfaceParameter)));
		assertEquals(test.findParameter(typeof(TestParameter)), PARAM);
		assertEquals(test.findParameter(typeof(TestParameter2)), null);
		assertEquals(test.findParameter(typeof(TestInterfaceParameter)), null);
	  }

	  public virtual void getParameter2()
	  {
		CalculationParameters test = CalculationParameters.of(ImmutableList.of(PARAM2));
		assertEquals(test.getParameter(typeof(TestParameter2)), PARAM2);
		assertEquals(test.getParameter(typeof(TestInterfaceParameter)), PARAM2);
		assertThrowsIllegalArg(() => test.getParameter(typeof(TestParameter)));
		assertEquals(test.findParameter(typeof(TestParameter2)), PARAM2);
		assertEquals(test.findParameter(typeof(TestInterfaceParameter)), PARAM2);
		assertEquals(test.findParameter(typeof(TestParameter)), null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith()
	  {
		CalculationParameters test1 = CalculationParameters.of(PARAM);
		CalculationParameters test2 = CalculationParameters.of(ImmutableList.of());

		assertEquals(test1.combinedWith(test2).Parameters.size(), 1);
		assertEquals(test1.combinedWith(test2).Parameters.get(typeof(TestParameter)), PARAM);

		assertEquals(test2.combinedWith(test1).Parameters.size(), 1);
		assertEquals(test2.combinedWith(test1).Parameters.get(typeof(TestParameter)), PARAM);

		assertEquals(test1.combinedWith(test1).Parameters.size(), 1);
		assertEquals(test1.combinedWith(test1).Parameters.get(typeof(TestParameter)), PARAM);
	  }

	  public virtual void test_with_add()
	  {
		CalculationParameters test = CalculationParameters.of(PARAM).with(PARAM2);
		assertEquals(test.Parameters.size(), 2);
	  }

	  public virtual void test_with_replace()
	  {
		CalculationParameters test = CalculationParameters.of(PARAM).with(PARAM);
		assertEquals(test.Parameters.size(), 1);
	  }

	  public virtual void test_without_typeFound()
	  {
		CalculationParameters test = CalculationParameters.of(PARAM);
		CalculationParameters filtered1 = test.without(typeof(TestParameter));
		assertEquals(filtered1.Parameters.size(), 0);
	  }

	  public virtual void test_without_typeNotFound()
	  {
		CalculationParameters test = CalculationParameters.empty();
		CalculationParameters filtered1 = test.without(typeof(TestParameter));
		assertEquals(filtered1.Parameters.size(), 0);
	  }

	  public virtual void test_filter()
	  {
		CalculationParameters test = CalculationParameters.of(PARAM);
		TestTarget target = new TestTarget();

		CalculationParameters filtered1 = test.filter(target, TestingMeasures.PRESENT_VALUE);
		assertEquals(filtered1.Parameters.size(), 1);
		assertEquals(filtered1.Parameters.get(typeof(TestParameter)), PARAM);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CalculationParameters test = CalculationParameters.of(PARAM);
		coverImmutableBean(test);
		CalculationParameters test2 = CalculationParameters.empty();
		coverBeanEquals(test, test2);
		assertNotNull(CalculationParameters.meta());
	  }

	}

}