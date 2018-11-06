/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using EtdOptionType = com.opengamma.strata.product.etd.EtdOptionType;
	using EtdSettlementType = com.opengamma.strata.product.etd.EtdSettlementType;

	/// <summary>
	/// Test <seealso cref="CsvLoaderUtils"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CsvLoaderUtilsTest
	public class CsvLoaderUtilsTest
	{

	  public virtual void test_parseEtdSettlementType()
	  {
		assertEquals(CsvLoaderUtils.parseEtdSettlementType("C"), EtdSettlementType.CASH);
		assertEquals(CsvLoaderUtils.parseEtdSettlementType("CASH"), EtdSettlementType.CASH);
		assertEquals(CsvLoaderUtils.parseEtdSettlementType("c"), EtdSettlementType.CASH);
		assertEquals(CsvLoaderUtils.parseEtdSettlementType("E"), EtdSettlementType.PHYSICAL);
		assertEquals(CsvLoaderUtils.parseEtdSettlementType("PHYSICAL"), EtdSettlementType.PHYSICAL);
		assertEquals(CsvLoaderUtils.parseEtdSettlementType("e"), EtdSettlementType.PHYSICAL);
		assertThrowsIllegalArg(() => CsvLoaderUtils.parseEtdSettlementType(""));
	  }

	  public virtual void test_parseEtdOptionType()
	  {
		assertEquals(CsvLoaderUtils.parseEtdOptionType("A"), EtdOptionType.AMERICAN);
		assertEquals(CsvLoaderUtils.parseEtdOptionType("AMERICAN"), EtdOptionType.AMERICAN);
		assertEquals(CsvLoaderUtils.parseEtdOptionType("a"), EtdOptionType.AMERICAN);
		assertEquals(CsvLoaderUtils.parseEtdOptionType("E"), EtdOptionType.EUROPEAN);
		assertEquals(CsvLoaderUtils.parseEtdOptionType("EUROPEAN"), EtdOptionType.EUROPEAN);
		assertEquals(CsvLoaderUtils.parseEtdOptionType("e"), EtdOptionType.EUROPEAN);
		assertThrowsIllegalArg(() => CsvLoaderUtils.parseEtdOptionType(""));
	  }

	}

}