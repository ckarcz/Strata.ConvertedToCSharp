/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="Version"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class VersionTest
	public class VersionTest
	{

	  public virtual void test_version()
	  {
		assertEquals(Version.VersionString.Length == 0, false);
		// this line fails when tests are run in IntelliJ (works in Eclipse)
		// assertEquals(Version.getVersionString().contains("$"), false);
	  }

	}

}