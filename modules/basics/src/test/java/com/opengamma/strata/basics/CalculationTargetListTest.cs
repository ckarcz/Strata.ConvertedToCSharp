using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// Test <seealso cref="CalculationTargetList"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalculationTargetListTest
	public class CalculationTargetListTest
	{

	  private static readonly CalculationTarget TARGET1 = new TestTarget(1);
	  private static readonly CalculationTarget TARGET2 = new TestTarget(2);

	  //-------------------------------------------------------------------------
	  public virtual void test_array0()
	  {
		CalculationTargetList test = CalculationTargetList.of();
		assertEquals(test.Targets, ImmutableList.of());
	  }

	  public virtual void test_array2()
	  {
		CalculationTargetList test = CalculationTargetList.of(TARGET1, TARGET2);
		assertEquals(test.Targets, ImmutableList.of(TARGET1, TARGET2));
	  }

	  public virtual void test_collection1()
	  {
		CalculationTargetList test = CalculationTargetList.of(ImmutableList.of(TARGET1));
		assertEquals(test.Targets, ImmutableList.of(TARGET1));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CalculationTargetList test = CalculationTargetList.of(TARGET1, TARGET2);
		coverImmutableBean(test);
	  }

	  public virtual void test_serialization()
	  {
		CalculationTargetList test = CalculationTargetList.of(TARGET1, TARGET2);
		assertSerialization(test);
	  }

	  //-------------------------------------------------------------------------
	  [Serializable]
	  internal class TestTarget : CalculationTarget
	  {
		internal const long serialVersionUID = 1L;
		internal readonly int value;

		public TestTarget(int value)
		{
		  this.value = value;
		}

		public override bool Equals(object obj)
		{
		  return obj is TestTarget && value == ((TestTarget) obj).value;
		}

		public override int GetHashCode()
		{
		  return value;
		}
	  }

	}

}