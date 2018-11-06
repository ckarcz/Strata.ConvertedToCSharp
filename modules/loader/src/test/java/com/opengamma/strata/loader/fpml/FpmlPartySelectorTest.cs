/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.fpml
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableListMultimap = com.google.common.collect.ImmutableListMultimap;
	using ListMultimap = com.google.common.collect.ListMultimap;

	/// <summary>
	/// Test <seealso cref="FpmlPartySelector"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FpmlPartySelectorTest
	public class FpmlPartySelectorTest
	{

	  private static readonly ListMultimap<string, string> MAP = ImmutableListMultimap.of("A", "a1", "A", "a2", "B", "b", "C1", "c1", "C2", "c2");

	  //-------------------------------------------------------------------------
	  public virtual void test_any()
	  {
		assertEquals(FpmlPartySelector.any().selectParties(MAP), ImmutableList.of());
	  }

	  public virtual void test_matching()
	  {
		assertEquals(FpmlPartySelector.matching("a1").selectParties(MAP), ImmutableList.of("A"));
		assertEquals(FpmlPartySelector.matching("a2").selectParties(MAP), ImmutableList.of("A"));
		assertEquals(FpmlPartySelector.matching("b").selectParties(MAP), ImmutableList.of("B"));
		assertEquals(FpmlPartySelector.matching("c").selectParties(MAP), ImmutableList.of());
	  }

	  public virtual void test_matchingRegex()
	  {
		assertEquals(FpmlPartySelector.matchingRegex(Pattern.compile("a[12]")).selectParties(MAP), ImmutableList.of("A"));
		assertEquals(FpmlPartySelector.matchingRegex(Pattern.compile("b")).selectParties(MAP), ImmutableList.of("B"));
		assertEquals(FpmlPartySelector.matchingRegex(Pattern.compile("c[0-9]")).selectParties(MAP), ImmutableList.of("C1", "C2"));
		assertEquals(FpmlPartySelector.matchingRegex(Pattern.compile("d")).selectParties(MAP), ImmutableList.of());
	  }

	}

}