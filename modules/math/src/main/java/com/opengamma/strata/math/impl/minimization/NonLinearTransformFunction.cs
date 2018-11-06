/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;

	/// 
	//CSOFF: JavadocMethod
	public class NonLinearTransformFunction
	{

	  private static readonly MatrixAlgebra MA = new OGMatrixAlgebra();

	  private readonly NonLinearParameterTransforms _transform;
	  private readonly System.Func<DoubleArray, DoubleArray> _func;
	  private readonly System.Func<DoubleArray, DoubleMatrix> _jac;

	  public NonLinearTransformFunction(System.Func<DoubleArray, DoubleArray> func, System.Func<DoubleArray, DoubleMatrix> jac, NonLinearParameterTransforms transform)
	  {

		_transform = transform;

		_func = new FuncAnonymousInnerClass(this, func);

		_jac = new FuncAnonymousInnerClass2(this, jac);

	  }

	  private class FuncAnonymousInnerClass : System.Func<DoubleArray, DoubleArray>
	  {
		  private readonly NonLinearTransformFunction outerInstance;

		  private System.Func<DoubleArray, DoubleArray> func;

		  public FuncAnonymousInnerClass(NonLinearTransformFunction outerInstance, System.Func<DoubleArray, DoubleArray> func)
		  {
			  this.outerInstance = outerInstance;
			  this.func = func;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleArray apply(com.opengamma.strata.collect.array.DoubleArray yStar)
		  public override DoubleArray apply(DoubleArray yStar)
		  {
			DoubleArray y = outerInstance._transform.inverseTransform(yStar);
			return func(y);
		  }
	  }

	  private class FuncAnonymousInnerClass2 : System.Func<DoubleArray, DoubleMatrix>
	  {
		  private readonly NonLinearTransformFunction outerInstance;

		  private System.Func<DoubleArray, DoubleMatrix> jac;

		  public FuncAnonymousInnerClass2(NonLinearTransformFunction outerInstance, System.Func<DoubleArray, DoubleMatrix> jac)
		  {
			  this.outerInstance = outerInstance;
			  this.jac = jac;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleMatrix apply(com.opengamma.strata.collect.array.DoubleArray yStar)
		  public override DoubleMatrix apply(DoubleArray yStar)
		  {
			DoubleArray y = outerInstance._transform.inverseTransform(yStar);
			DoubleMatrix h = jac(y);
			DoubleMatrix invJ = outerInstance._transform.inverseJacobian(yStar);
			return (DoubleMatrix) MA.multiply(h, invJ);
		  }
	  }

	  public virtual System.Func<DoubleArray, DoubleArray> FittingFunction
	  {
		  get
		  {
			return _func;
		  }
	  }

	  public virtual System.Func<DoubleArray, DoubleMatrix> FittingJacobian
	  {
		  get
		  {
			return _jac;
		  }
	  }

	}

}