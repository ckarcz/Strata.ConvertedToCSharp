/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;

	using Test = org.testng.annotations.Test;

	using Country = com.opengamma.strata.basics.location.Country;

	/// <summary>
	/// Test <seealso cref="SimpleLegalEntity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SimpleLegalEntityTest
	public class SimpleLegalEntityTest
	{

	  private static readonly LegalEntityId LEI = LegalEntityId.of("LEI", "A");
	  private static readonly LegalEntityId LEI2 = LegalEntityId.of("LEI", "B");

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SimpleLegalEntity test = SimpleLegalEntity.of(LEI, "US GOV", Country.US);
		coverImmutableBean(test);
		SimpleLegalEntity test2 = SimpleLegalEntity.of(LEI2, "GB GOV", Country.GB);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		SimpleLegalEntity test = SimpleLegalEntity.of(LEI, "US GOV", Country.US);
		assertSerialization(test);
	  }

	}

}