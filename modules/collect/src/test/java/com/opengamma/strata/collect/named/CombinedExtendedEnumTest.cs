/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.named
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertThrows;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="CombinedExtendedEnum"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CombinedExtendedEnumTest
	public class CombinedExtendedEnumTest
	{

	  public virtual void test_lookup()
	  {
		assertEquals(UberNamed.of("Standard"), SampleNameds.STANDARD);
		assertEquals(UberNamed.of("More"), MoreSampleNameds.MORE);
		CombinedExtendedEnum<UberNamed> combined = CombinedExtendedEnum.of(typeof(UberNamed));
		assertEquals(combined.find("Rubbish"), null);
		assertThrows(typeof(System.ArgumentException), () => combined.lookup("Rubbish"));
		assertEquals(combined.ToString(), "CombinedExtendedEnum[UberNamed]");
	  }

	}

}