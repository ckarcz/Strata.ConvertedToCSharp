/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
	using Test = org.testng.annotations.Test;

	using CommonsMatrixAlgebra = com.opengamma.strata.math.impl.matrix.CommonsMatrixAlgebra;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SVDecompositionCommonsTest extends SVDecompositionCalculationTestCase
	public class SVDecompositionCommonsTest : SVDecompositionCalculationTestCase
	{
	  private static readonly MatrixAlgebra ALGEBRA = new CommonsMatrixAlgebra();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private static readonly Decomposition<SVDecompositionResult> SVD_Renamed = new SVDecompositionCommons();

	  protected internal override MatrixAlgebra Algebra
	  {
		  get
		  {
			return ALGEBRA;
		  }
	  }

	  protected internal override Decomposition<SVDecompositionResult> SVD
	  {
		  get
		  {
			return SVD_Renamed;
		  }
	  }
	}

}