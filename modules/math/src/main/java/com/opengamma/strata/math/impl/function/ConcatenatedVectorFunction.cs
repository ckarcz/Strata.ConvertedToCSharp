using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// For the set of $k$ vector functions $f_i: \mathbb{R}^{m_i} \to \mathbb{R}^{n_i} \quad x_i \mapsto f_i(x_i) = y_i$ 
	/// this forms the function 
	/// $f: \mathbb{R}^{m} \to \mathbb{R}^{n} \quad x_i \mapsto f(x) = y$ where $n = \sum_{i=1}^k n_i$ and  
	/// $m = \sum_{i=1}^k m_i$ and $x = (x_1,x_2,\dots,x_k)$ \& $y = (y_1,y_2,\dots,y_k)$.
	/// 
	/// </summary>
	public class ConcatenatedVectorFunction : VectorFunction
	{

	  private readonly int[] xPartition;
	  private readonly int[] yPartition;
	  private readonly int nPartitions;
	  private readonly VectorFunction[] functions;
	  private readonly int sizeDom;
	  private readonly int sizeRange;

	  /// <summary>
	  /// Creates an instance.
	  /// <para>
	  /// This creates the concatenated function, in the order that the sub functions are given.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="functions">  the sub functions  </param>
	  public ConcatenatedVectorFunction(VectorFunction[] functions)
	  {
		ArgChecker.noNulls(functions, "functions");
		this.functions = functions;
		this.nPartitions = functions.Length;
		this.xPartition = new int[nPartitions];
		this.yPartition = new int[nPartitions];
		int m = 0;
		int n = 0;
		for (int i = 0; i < nPartitions; i++)
		{
		  xPartition[i] = functions[i].LengthOfDomain;
		  yPartition[i] = functions[i].LengthOfRange;
		  m += xPartition[i];
		  n += yPartition[i];
		}
		this.sizeDom = m;
		this.sizeRange = n;
	  }

	  //-------------------------------------------------------------------------
	  public override DoubleMatrix calculateJacobian(DoubleArray x)
	  {
		ArgChecker.notNull(x, "x");
		ArgChecker.isTrue(x.size() == LengthOfDomain, "Incorrect length of x. Is {} but should be {}", x.size(), LengthOfDomain);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] jac = new double[LengthOfRange][LengthOfDomain];
		double[][] jac = RectangularArrays.ReturnRectangularDoubleArray(LengthOfRange, LengthOfDomain);

		int posInput = 0;
		int pos1 = 0;
		int pos2 = 0;
		for (int i = 0; i < nPartitions; i++)
		{
		  int nRows = yPartition[i];
		  int nCols = xPartition[i];
		  DoubleArray sub = x.subArray(posInput, posInput + nCols);
		  DoubleMatrix subJac = functions[i].calculateJacobian(sub);
		  if (nCols > 0)
		  {
			for (int r = 0; r < nRows; r++)
			{
			  Array.Copy(subJac.toArrayUnsafe()[r], 0, jac[pos1++], pos2, nCols);
			}
			pos2 += nCols;
		  }
		  else
		  {
			pos1 += nRows;
		  }
		  posInput += nCols;
		}
		return DoubleMatrix.copyOf(jac);
	  }

	  public override DoubleArray apply(DoubleArray x)
	  {
		ArgChecker.notNull(x, "x");
		ArgChecker.isTrue(x.size() == LengthOfDomain, "Incorrect length of x. Is {} but should be {}", x.size(), LengthOfDomain);
		double[] y = new double[LengthOfRange];
		int posInput = 0;
		int posOutput = 0;
		//evaluate each function (with the appropriate sub vector) and concatenate the results 
		for (int i = 0; i < nPartitions; i++)
		{
		  int length = xPartition[i];
		  DoubleArray sub = x.subArray(posInput, posInput + length);
		  DoubleArray eval = functions[i].apply(sub);
		  eval.copyInto(y, posOutput);
		  posInput += length;
		  posOutput += eval.size();
		}
		return DoubleArray.copyOf(y);
	  }

	  public override int LengthOfDomain
	  {
		  get
		  {
			return sizeDom;
		  }
	  }

	  public override int LengthOfRange
	  {
		  get
		  {
			return sizeRange;
		  }
	  }

	}

}